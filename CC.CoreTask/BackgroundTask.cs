using System;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using CC.Common.Models;

namespace CC.CoreTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ToastTileManager.UpdatePrimaryTile(true);
            ToastTileManager.MakeAToast();
        }
    }
}
