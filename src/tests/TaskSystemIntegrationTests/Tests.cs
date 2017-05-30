﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using GitHub.Unity;
using System.Threading.Tasks.Schedulers;
using System.IO;
using NSubstitute;

namespace IntegrationTests
{
    class BaseTest
    {
        protected ITaskManager TaskManager { get; set; }
        protected IProcessManager ProcessManager { get; set; }
        protected NPath TestBasePath { get; private set; }
        protected IFileSystem FileSystem { get; private set; }
        protected CancellationToken Token => TaskManager.Token;
        protected NPath TestApp => System.Reflection.Assembly.GetExecutingAssembly().Location.ToNPath().Combine("TestApp.exe");

        [TestFixtureSetUp]
        public void OneTimeSetup()
        {
            Logging.LoggerFactory = context => new ConsoleLogAdapter(context);
            //Logging.TracingEnabled = true;
            TaskManager = new TaskManager();
            var syncContext = new ThreadSynchronizationContext(Token);
            TaskManager.UIScheduler = new SynchronizationContextTaskScheduler(syncContext);

            FileSystem = new FileSystem();
            NPath.FileSystem = FileSystem;
            TestBasePath = NPath.CreateTempDirectory("integration-tests");
            FileSystem.SetCurrentDirectory(TestBasePath);

            var env = new DefaultEnvironment();
            var repo = Substitute.For<IRepository>();
            repo.LocalPath.Returns(TestBasePath);
            env.Repository = repo;

            var platform = new Platform(env, FileSystem);
            ProcessManager = new ProcessManager(env, platform.GitEnvironment, Token);
            var processEnv = platform.GitEnvironment;
            var path = new ProcessTask<NPath>(TaskManager.Token, new FirstLineIsPathOutputProcessor())
                .Configure(ProcessManager, env.IsWindows ? "where" : "which", "git")
                .Start().Result;
            env.GitExecutablePath = path ?? "git".ToNPath();
        }

        [TestFixtureTearDown]
        public void OneTimeTearDown()
        {
            TaskManager?.Stop();
            try
            {
                TestBasePath.DeleteIfExists();
            }
            catch { }
        }

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
        }
    }

    [TestFixture]
    class ProcessTaskTests : BaseTest
    {
        [Test]
        public async Task ProcessReturningErrorThrowsException()
        {
            var success = false;
            Exception thrown = null;
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name" };

            var task = new SimpleProcessTask(TestApp, @"-s 100 -d ""one name""", Token).Configure(ProcessManager)
                .Catch(ex => thrown = ex)
                .Then((s, d) => output.Add(d))
                .Then(new SimpleProcessTask(TestApp, @"-e kaboom -r -1", Token).Configure(ProcessManager))
                .Catch(ex => thrown = ex)
                .Then((s, d) => output.Add(d))
                .Finally((s, e) => success = s);

            await task.StartAwait();

            Assert.IsFalse(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNotNull(thrown);
            Assert.AreEqual("kaboom", thrown.Message);
        }
    }

    [TestFixture]
    class SchedulerTests : BaseTest
    {
        private ActionTask GetTask(TaskAffinity affinity, int id, Action<int> body, ActionTask dependsOn = null)
        {
            return new ActionTask(Token, _ => body(id), dependsOn) { Affinity = affinity };
        }

        /// <summary>
        /// This exemplifies that running a bunch of tasks that don't depend on anything on the concurrent (default) scheduler
        /// run in any order
        /// </summary>
        [Test]
        public void ConcurrentSchedulerDoesNotGuaranteeOrdering()
        {
            var runningOrder = new List<int>();
            var rand = new Randomizer();
            var tasks = new List<ActionTask>();
            for (int i = 1; i < 11; i++)
            {
                tasks.Add(GetTask(TaskAffinity.Concurrent, i, id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); }));
            }

            TaskManager.Schedule(tasks.Cast<ITask>().ToArray());
            Task.WaitAll(tasks.Select(x => x.Task).ToArray());
            Console.WriteLine(String.Join(",", runningOrder.Select(x => x.ToString()).ToArray()));
            Assert.AreEqual(10, runningOrder.Count);
        }

        /// <summary>
        /// This exemplifies that running a bunch of tasks that depend on other things on the concurrent (default) scheduler
        /// run in dependency order. Each group of tasks depends on a task on the previous group, so the first group
        /// runs first, then the second group of tasks, then the third. Run order within each group is not guaranteed
        /// </summary>
        [Test]
        public void ConcurrentSchedulerWithDependencyOrdering()
        {
            var count = 3;
            var runningOrder = new List<int>();
            var rand = new Randomizer();
            var startTasks = new List<ActionTask>();
            var i = 1;
            for (var start = i; i < start + count; i++)
            {
                startTasks.Add(GetTask(TaskAffinity.Concurrent, i,
                    id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); }));
            }

            var midTasks = new List<ActionTask>();
            for (var start = i; i < start + count; i++)
            {
                midTasks.Add(GetTask(TaskAffinity.Concurrent, i,
                    id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); },
                    startTasks[i - 4]));
            }

            var endTasks = new List<ActionTask>();
            for (var start = i; i < start + count; i++)
            {
                endTasks.Add(GetTask(TaskAffinity.Concurrent, i,
                    id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); },
                    midTasks[i - 7]));
            }

            foreach (var t in endTasks)
                t.Start();
            Task.WaitAll(endTasks.Select(x => x.Task).ToArray());

            CollectionAssert.AreEquivalent(Enumerable.Range(1, 3), runningOrder.Take(3));
            CollectionAssert.AreEquivalent(Enumerable.Range(4, 3), runningOrder.Skip(3).Take(3));
            CollectionAssert.AreEquivalent(Enumerable.Range(7, 3), runningOrder.Skip(6).Take(3));
        }

        [Test]
        public void ExclusiveSchedulerGuaranteesOrdering()
        {
            var runningOrder = new List<int>();
            var tasks = new List<ActionTask>();
            var rand = new Randomizer();
            for (int i = 1; i < 11; i++)
            {
                tasks.Add(GetTask(TaskAffinity.Exclusive, i, id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); }));
            }

            TaskManager.Schedule(tasks.Cast<ITask>().ToArray());
            Task.WaitAll(tasks.Select(x => x.Task).ToArray());
            Assert.AreEqual(Enumerable.Range(1, 10), runningOrder);
        }

        [Test]
        public void UISchedulerGuaranteesOrdering()
        {
            var runningOrder = new List<int>();
            var tasks = new List<ActionTask>();
            var rand = new Randomizer();
            for (int i = 1; i < 11; i++)
            {
                tasks.Add(GetTask(TaskAffinity.UI, i, id => { new ManualResetEventSlim().Wait(rand.Next(100, 200)); lock (runningOrder) runningOrder.Add(id); }));
            }

            TaskManager.Schedule(tasks.Cast<ITask>().ToArray());
            Task.WaitAll(tasks.Select(x => x.Task).ToArray());
            Assert.AreEqual(Enumerable.Range(1, 10), runningOrder);
        }

        [Test]
        public async void NonUITasksAlwaysRunOnDifferentThreadFromUITasks()
        {
            var output = new Dictionary<int, int>();
            var tasks = new List<ITask>();
            var seed = Randomizer.RandomSeed;
            var rand = new Randomizer(seed);

            var uiThread = 0;
            await new ActionTask(Token, _ => uiThread = Thread.CurrentThread.ManagedThreadId) { Affinity = TaskAffinity.UI }.StartAwait();

            for (int i = 1; i < 100; i++)
            {
                tasks.Add(GetTask(i % 2 == 0 ? TaskAffinity.Concurrent : TaskAffinity.Exclusive, i,
                    id => { lock (output) output.Add(id, Thread.CurrentThread.ManagedThreadId); })
                    .Start());
            }

            Task.WaitAll(tasks.Select(x => x.Task).ToArray());
            CollectionAssert.DoesNotContain(output.Values, uiThread);
        }


        [Test]
        public async void ChainingOnDifferentSchedulers()
        {
            var output = new Dictionary<int, KeyValuePair<int, int>>();
            var tasks = new List<ITask>();
            var seed = Randomizer.RandomSeed;
            var rand = new Randomizer(seed);

            var uiThread = 0;
            await new ActionTask(Token, _ => uiThread = Thread.CurrentThread.ManagedThreadId) { Affinity = TaskAffinity.UI }.StartAwait();

            for (int i = 1; i < 100; i++)
            {
                tasks.Add(
                    GetTask(TaskAffinity.UI, i,
                        id => { lock (output) output.Add(id, KeyValuePair.Create(Thread.CurrentThread.ManagedThreadId, -1)); })
                    .Then(
                    GetTask(i % 2 == 0 ? TaskAffinity.Concurrent : TaskAffinity.Exclusive, i,
                        id => { lock (output) output[id] = KeyValuePair.Create(output[id].Key, Thread.CurrentThread.ManagedThreadId); })
                    ).
                    Start());
            }

            Task.WaitAll(tasks.Select(x => x.Task).ToArray());
            Console.WriteLine(String.Join(",", output.Select(x => x.Key.ToString()).ToArray()));
            foreach (var t in output)
            {
                Assert.AreEqual(uiThread, t.Value.Key, $"Task {t.Key} pass 1 should have been on ui thread {uiThread} but ran instead on {t.Value.Key}");
                Assert.AreNotEqual(t.Value.Key, t.Value.Value, $"Task {t.Key} pass 2 should not have been on ui thread {uiThread}");
            }
        }
    }

    [TestFixture]
    class Chains : BaseTest
    {
        [Test]
        public async Task ThrowingInterruptsTaskChainButAlwaysRunsFinallyAndCatch()
        {
            var success = false;
            string thrown = "";
            Exception finallyException = null;
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(_ => { throw new Exception("an exception"); })
                .Catch(ex => thrown = ex.Message)
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .ThenInUI((s, d) => output.Add(d))
                .Finally((s, e) =>
                {
                    success = s;
                    finallyException = e;
                });

            await task.StartAwait();

            Assert.IsFalse(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNotNull(finallyException);
        }

        [Test]
        public async Task FinallyReportsException()
        {
            var success = false;
            Exception finallyException = null;
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(_ => { throw new Exception("an exception"); })
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .ThenInUI((s, d) => output.Add(d))
                .Finally((s, e) =>
                {
                    success = s;
                    finallyException = e;
                });

            await task.StartAwait();

            Assert.IsFalse(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNotNull(finallyException);
            Assert.AreEqual("an exception", finallyException.Message);
        }

        [Test]
        public async Task CatchAlwaysRunsBeforeFinally()
        {
            var success = false;
            Exception exception = null;
            Exception finallyException = null;
            var runOrder = new List<string>();
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(_ => { throw new Exception("an exception"); })
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .Then((s, d) =>
                {
                    output.Add(d);
                    return "done";
                })
                .Catch(ex =>
                {
                    Thread.Sleep(300);
                    lock (runOrder)
                    {
                        exception = ex;
                        runOrder.Add("catch");
                    }
                })
                .Finally((s, e, d) =>
                {
                    Thread.Sleep(300);
                    lock (runOrder)
                    {
                        success = s;
                        finallyException = e;
                        runOrder.Add("finally");
                    }
                    return d;
                });

            await task.StartAwait();

            Assert.IsFalse(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNotNull(exception);
            Assert.IsNotNull(finallyException);
            Assert.AreEqual("an exception", exception.Message);
            Assert.AreEqual("an exception", finallyException.Message);
            CollectionAssert.AreEqual(new List<string> { "catch", "finally" }, runOrder);
        }

        [Test]
        public async Task DoNotUseCatchAtTheEndOfAChain()
        {
            var success = false;
            Exception exception = null;
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(_ => { throw new Exception("an exception"); })
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .ThenInUI((s, d) => output.Add(d))
                .Finally((_, __) => { })
                .Catch(ex => { Thread.Sleep(20); exception = ex; });

            await task.StartAwait();

            Assert.IsFalse(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNull(exception);
        }

        [Test]
        public async Task FinallyCanReturnData()
        {
            var success = false;
            Exception exception = null;
            Exception finallyException = null;
            var runOrder = new List<string>();
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name", "another name", "done" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .Then((s, d) =>
                {
                    output.Add(d);
                    return "done";
                })
                .Catch(ex =>
                {
                    lock (runOrder)
                    {
                        exception = ex;
                        runOrder.Add("catch");
                    }
                })
                .Finally((s, e, d) =>
                {
                    lock (runOrder)
                    {
                        success = s;
                        output.Add(d);
                        finallyException = e;
                        runOrder.Add("finally");
                    }
                    return d;
                })
                .ThenInUI((s, d) =>
                {
                    lock (runOrder)
                    {
                        runOrder.Add("boo");
                    }
                    return d;
                });

            var ret = await task.StartAwait();
            Assert.AreEqual("done", ret);
            Assert.IsTrue(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNull(exception);
            Assert.IsNull(finallyException);
            CollectionAssert.AreEqual(new List<string> { "finally", "boo" }, runOrder);
        }

        [Test]
        public void ConditionalChaining()
        {
            var success = false;
            Exception exception = null;
            Exception finallyException = null;
            var runOrder = new List<string>();
            var output = new List<string>();
            var bools = new List<bool>();
            for (int i = 0; i < 10; i++)
            {
                bools.Add(i % 2 == 0);
            }
            var expectedOutput = bools.SelectMany(x => new List<string> { x.ToString().ToLower(), x ? "something" : "nothing" }).ToList();

            var tasks = new List<ITask>();
            foreach (var b in bools)
            {
                var task =
                    new FuncTask<bool>(Token, _ => b)
                    .ThenIf(go =>
                    {
                        output.Add(go.ToString().ToLower());
                        if (go)
                            return new FuncTask<string>(Token, _ => "something");
                        else
                            return new FuncTask<string>(Token, _ => "nothing");
                    })
                    .Finally((s, e, d) =>
                    {
                        lock (runOrder)
                        {
                            success = s;
                            output.Add(d);
                            finallyException = e;
                        }
                        return d;
                    });
                tasks.Add(task.Start());
            }

            Task.WaitAll(tasks.Select(x => x.Task).ToArray());

            Assert.IsTrue(success);
            CollectionAssert.AreEquivalent(expectedOutput, output);
            Assert.IsNull(exception);
            Assert.IsNull(finallyException);
        }

        [Test]
        public async Task FinallyCanAlsoNotReturnData()
        {
            var success = false;
            Exception exception = null;
            Exception finallyException = null;
            var runOrder = new List<string>();
            var output = new List<string>();
            var expectedOutput = new List<string> { "one name", "another name", "done" };

            var task =
                new FuncTask<string>(Token, _ => "one name") { Affinity = TaskAffinity.UI }
                .Then((s, d) => output.Add(d))
                .Then(new FuncTask<string>(Token, _ => "another name") { Affinity = TaskAffinity.Exclusive })
                .Then((s, d) =>
                {
                    output.Add(d);
                    return "done";
                })
                .Finally((s, e, d) =>
                {
                    lock (runOrder)
                    {
                        success = s;
                        output.Add(d);
                        finallyException = e;
                        runOrder.Add("finally");
                    }
                });

            await task.StartAwait();

            Assert.IsTrue(success);
            CollectionAssert.AreEqual(expectedOutput, output);
            Assert.IsNull(exception);
            Assert.IsNull(finallyException);
            CollectionAssert.AreEqual(new List<string> { "finally" }, runOrder);
        }
    }

    [TestFixture]
    class Exceptions : BaseTest
    {
        [Test]
        public async Task StartAndEndAreAlwaysRaised()
        {
            var runOrder = new List<string>();
            var task = new ActionTask(Token, _ => { throw new Exception(); })
                .Finally((s, d) => { });
            task.OnStart += _ => runOrder.Add("start");
            task.OnEnd += _ => runOrder.Add("end");
            await task.Start().Task;
            CollectionAssert.AreEqual(new string[] { "start", "end" }, runOrder);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ExceptionPropagatesOutIfNoFinally()
        {
            var runOrder = new List<string>();
            var task = new ActionTask(Token, _ => { throw new InvalidOperationException(); })
                .Catch(_ => { });
            await task.Start().Task;
        }

        [Test]
        public async Task StartAwaitSafelyAwaits()
        {
            var runOrder = new List<string>();
            var task = new ActionTask(Token, _ => { throw new InvalidOperationException(); })
                .Catch(_ => { });
            await task.StartAwait();
        }
    }

    [TestFixture]
    class TaskToActionTask : BaseTest
    {
        [Test]
        public async Task CanWrapATask()
        {
            var uiThread = 0;
            await new ActionTask(Token, _ => uiThread = Thread.CurrentThread.ManagedThreadId) { Affinity = TaskAffinity.UI }.StartAwait();

            var runOrder = new List<string>();
            var task = new Task(() => runOrder.Add($"ran {Thread.CurrentThread.ManagedThreadId}"));
            var act = new ActionTask(task) { Affinity = TaskAffinity.UI };
            await act.StartAwait();
            CollectionAssert.AreEqual(new string[] { $"ran {uiThread}" }, runOrder);
        }

        /// <summary>
        /// Always call Then or another non-Defer variant after calling Defer
        /// </summary>
        [Test]
        public async Task AlwaysChainAsyncBodiesWithNonAsync()
        {
            var runOrder = new List<int>();
            var act = new FuncTask<List<int>>(Token, _ => runOrder) { Name = "First" }
                .Defer(GetData)
                .Then((_, v) =>
                {
                    v.Add(2);
                    return v;
                })
                .Defer(GetData2)
                .Then((_, v) =>
                {
                    v.Add(4);
                    return v;
                })
                .Defer(async v =>
                {
                    await TaskEx.Delay(10);
                    v.Add(5);
                    return v;
                })
                .Then((_, v) =>
                {
                    v.Add(6);
                    return v;
                })
                .Defer(v => new Task<List<int>>(() =>
                {
                    v.Add(7);
                    return v;
                }), TaskAffinity.Concurrent)
                .Finally((_, e, v) => v);
            ;
            var ret = await act.StartAwait();
            CollectionAssert.AreEqual(Enumerable.Range(1, 7), runOrder);
        }

        /// <summary>
        /// Always call Then or another non-Defer variant after calling Defer
        /// </summary>
        [Test]
        public async Task TwoDefersInARowWillNotWork()
        {
            var runOrder = new List<int>();
            var act = new FuncTask<List<int>>(Token, _ => runOrder) { Name = "First" }
                .Defer(GetData)
                .Defer(GetData2)
                .Finally((_, e, v) => v);
            ;
            var ret = await act.StartAwait();
            Assert.IsNull(ret);
        }

        [Test]
        public async Task DoNotEndChainsWithDefer()
        {
            var runOrder = new List<int>();
            var act = new FuncTask<List<int>>(Token, _ => runOrder) { Name = "First" }
                .Defer(GetData)
                .Then((_, v) =>
                {
                    v.Add(2);
                    return v;
                })
                .Defer(GetData2)
                .Then((_, v) =>
                {
                    v.Add(4);
                    return v;
                })
                .Defer(async v =>
                {
                    await TaskEx.Delay(10);
                    v.Add(5);
                    return v;
                })
                .Then((_, v) =>
                {
                    v.Add(6);
                    return v;
                })
                .Defer(v => new Task<List<int>>(() =>
                {
                    v.Add(7);
                    return v;
                }), TaskAffinity.Concurrent);
            ;
            var ret = await act.StartAwait();
            // the last one hasn't finished before await is done
            CollectionAssert.AreEqual(Enumerable.Range(1, 6), runOrder);
        }

        private async Task<List<int>> GetData(List<int> v)
        {
            await TaskEx.Delay(10);
            v.Add(1);
            return v;
        }

        private async Task<List<int>> GetData2(List<int> v)
        {
            await TaskEx.Delay(10);
            v.Add(3);
            return v;
        }

        [Test]
        public async Task Inlining()
        {
            var runOrder = new List<string>();
            var act = new ActionTask(Token, _ => runOrder.Add($"started"))
                .Then(TaskEx.FromResult(1), TaskAffinity.Exclusive)
                .Then((_, n) => n + 1)
                .Then((_, n) => runOrder.Add(n.ToString()))
                .Then(TaskEx.FromResult(20f), TaskAffinity.Exclusive)
                .Then((_, n) => n + 1)
                .Then((_, n) => runOrder.Add(n.ToString()))
                .Finally((_, t) => runOrder.Add("done"))
            ;
            await act.StartAwait();
            CollectionAssert.AreEqual(new string[] { "started", "2", "21", "done" }, runOrder);
        }
    }

    static class KeyValuePair
    {
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
