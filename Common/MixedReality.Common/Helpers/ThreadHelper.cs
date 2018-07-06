#region Using

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

#endregion

namespace MixedReality.Common.Helpers
{
    public static class ThreadHelper
    {
        #region Properties

        public static CoreDispatcher Dispatcher { get; private set; }

        #endregion

        #region Constructor

        static ThreadHelper()
        {
            Dispatcher = CoreApplication.MainView.Dispatcher;
        }

        #endregion

        #region Methods (Public)

        public static async Task InvokeOnMainThread(Action action)
        {
            if (Dispatcher.HasThreadAccess)
            {
                action?.Invoke();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await InvokeOnMainThread(action);
                });
            }
        }

        #endregion
    }
}
