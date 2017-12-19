﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace GitHub.Unity
{
    public class Utils
    {
        public static bool Copy(Stream source, Stream destination, int chunkSize)
        {
            return Copy(source, destination, chunkSize, 0, null, 1000);
        }

        public static bool Copy(Stream source, Stream destination, int chunkSize, long totalSize,
            Func<long, long, bool> progress, int progressUpdateRate)
        {
            byte[] buffer = new byte[chunkSize];
            int bytesRead = 0;
            long totalRead = 0;
            float averageSpeed = -1f;
            float lastSpeed = 0f;
            float smoothing = 0.005f;
            long readLastSecond = 0;
            long timeToFinish = 0;
            Stopwatch watch = null;
            bool success = true;

            bool trackProgress = totalSize > 0 && progress != null;
            if (trackProgress)
                watch = new Stopwatch();

            do
            {
                if (trackProgress)
                    watch.Start();

                bytesRead = source.Read(buffer, 0, chunkSize);

                if (trackProgress)
                    watch.Stop();

                totalRead += bytesRead;

                if (bytesRead > 0)
                {
                    destination.Write(buffer, 0, bytesRead);
                    if (trackProgress)
                    {
                        readLastSecond += bytesRead;
                        if (watch.ElapsedMilliseconds >= progressUpdateRate || totalRead == totalSize)
                        {
                            watch.Reset();
                            lastSpeed = readLastSecond;
                            readLastSecond = 0;
                            averageSpeed = averageSpeed < 0f
                                ? lastSpeed
                                : smoothing * lastSpeed + (1f - smoothing) * averageSpeed;
                            timeToFinish = Math.Max(1L,
                                (long)((totalSize - totalRead) / (averageSpeed / progressUpdateRate)));

                            if (!progress(totalRead, timeToFinish))
                                break;
                        }
                    }
                }
            } while (bytesRead > 0);

            if (totalRead > 0)
                destination.Flush();

            return success;
        }
    }

    public class DownloadResult
    {

    }

    public static class WebRequestExtensions
    {
        public static WebResponse GetResponseWithoutException(this WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
        }
    }

    class DownloadTask: TaskBase<DownloadResult>
    {
        private long bytes;
        private WebRequest webRequest;
        private bool restarted;

        public float Progress { get; set; }

        public DownloadTask(CancellationToken token, string url, string destination)
            : base(token)
        {
            Url = url;
            Destination = destination;
            Name = "DownloadTask";
        }

        protected override DownloadResult RunWithReturn(bool success)
        {
            DownloadResult result = base.RunWithReturn(success);

            RaiseOnStart();

            try
            {
                Logger.Trace("Downloading");
                RunDownload();

                Logger.Trace("Downloaded");
            }
            catch (Exception ex)
            {
                Errors = ex.Message;
                if (!RaiseFaultHandlers(ex))
                    throw;
            }
            finally
            {
                RaiseOnEnd(result);
            }

            return result;
        }

        protected virtual void UpdateProgress(float progress)
        {
            Progress = progress;
        }

        public bool RunDownload()
        {
            var fileInfo = new FileInfo(Destination);
            if (fileInfo.Exists)
            {
                if (fileInfo.Length > 0)
                {
                    bytes = fileInfo.Length;
                    restarted = true;
                }
                else if (fileInfo.Length == 0)
                {
                    fileInfo.Delete();
                }
            }

            webRequest = WebRequest.Create(Url);
            var httpWebRequest = webRequest as HttpWebRequest;
            if (httpWebRequest != null)
            {
                if (bytes > 0)
                {
                    // TODO: fix classlibs to take long overloads
                    httpWebRequest.AddRange((int)bytes);
                }
            }

            webRequest.Method = "GET";
            webRequest.Timeout = 3000;

            if (restarted && bytes > 0)
                Logger.Trace($"Resuming download of {Url} to {Destination}");
            else
                Logger.Trace($"Downloading {Url} to {Destination}");

            using (var webResponse = webRequest.GetResponseWithoutException())
            {
                if (webResponse == null)
                    return false;

                if (restarted && bytes > 0)
                {
                    var httpWebResponse = webResponse as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        var httpStatusCode = httpWebResponse.StatusCode;
                        if (httpStatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
                        {
                            UpdateProgress(1);
                            return true;
                        }

                        if (!(httpStatusCode == HttpStatusCode.OK || httpStatusCode == HttpStatusCode.PartialContent))
                        {
                            return false;
                        }
                    }
                }

                var responseLength =  webResponse.ContentLength;
                if (restarted && bytes > 0)
                {
                    UpdateProgress(bytes / (float) responseLength);
                }

                using (var responseStream = webResponse.GetResponseStream())
                {
                    using (Stream destinationStream = new FileStream(Destination, FileMode.Append))
                    {
                        if (Token.IsCancellationRequested)
                            return false;

                        return Utils.Copy(responseStream, destinationStream, 8192, responseLength, null, 100);
                    }
                }
            }
        }

        protected string Url { get; }

        protected string Destination { get; }
    }
}
