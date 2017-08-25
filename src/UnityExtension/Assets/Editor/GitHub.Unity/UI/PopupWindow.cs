using System;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    [Serializable]
    class PopupWindow : BaseWindow
    {
        public enum PopupViewType
        {
            PublishView,
            AuthenticationView
        }

        [NonSerialized] private Subview activeView;

        [SerializeField] private PopupViewType activeViewType;
        [SerializeField] private AuthenticationView authenticationView;
        [SerializeField] private PublishView publishView;

        public event Action<bool> OnClose;

        [MenuItem("GitHub/Authenticate")]
        public static void Launch()
        {
            Open(PopupViewType.AuthenticationView);
        }

        public static PopupWindow Open(PopupViewType popupViewType, Action<bool> onClose = null)
        {
            var popupWindow = GetWindow<PopupWindow>(true);

            popupWindow.OnClose.SafeInvoke(false);

            if (onClose != null)
            {
                popupWindow.OnClose += onClose;
            }

            popupWindow.ActiveViewType = popupViewType;
            popupWindow.titleContent = new GUIContent(popupWindow.ActiveView.Title, Styles.SmallLogo);

            popupWindow.InitializeWindow(EntryPoint.ApplicationManager);
            popupWindow.Show();

            return popupWindow;
        }

        public override void Initialize(IApplicationManager applicationManager)
        {
            base.Initialize(applicationManager);

            publishView = publishView ?? new PublishView();
            authenticationView = authenticationView ?? new AuthenticationView();

            publishView.InitializeView(this);
            authenticationView.InitializeView(this);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (ActiveView != null)
            {
                minSize = maxSize = ActiveView.Size;
                ActiveView.OnEnable();
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (ActiveView != null)
            {
                ActiveView.OnDisable();
            }
        }

        public override void OnUI()
        {
            base.OnUI();

            if (ActiveView != null)
            {
                ActiveView.OnGUI();
            }
        }

        public override void Refresh()
        {
            base.Refresh();

            if (ActiveView != null)
            {
                ActiveView.Refresh();
            }
        }

        public override void OnSelectionChange()
        {
            base.OnSelectionChange();
            ActiveView.OnSelectionChange();
        }

        public override void Finish(bool result)
        {
            OnClose.SafeInvoke(result);
            OnClose = null;
            Close();
            base.Finish(result);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            OnClose.SafeInvoke(false);
            OnClose = null;
        }

        private Subview ActiveView
        {
            get { return activeView; }
        }

        private PopupViewType ActiveViewType
        {
            get { return activeViewType; }
            set
            {
                var valueChanged = false;
                if (activeViewType != value)
                {
                    valueChanged = true;
                    activeViewType = value;
                }

                if (activeView == null || valueChanged)
                {
                    switch (activeViewType)
                    {
                        case PopupViewType.PublishView:
                            activeView = publishView;
                            break;

                        case PopupViewType.AuthenticationView:
                            activeView = authenticationView;
                            break;

                        default: throw new ArgumentOutOfRangeException("value", value, null);
                    }
                }
            }
        }
    }
}
