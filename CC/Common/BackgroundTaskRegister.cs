using CC.CoreTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace CC.Common
{
    class BackgroundTaskRegister
    {

        public static void RegisterBackgroundTask()
        {
            var x = RegisterBackgroundTask(typeof(BackgroundTask), "BackgroundTask", new TimeTrigger(60 * 12, false), null);
        }

        private async static Task<BackgroundTaskRegistration> RegisterBackgroundTask(Type taskEntryPoint, string taskName, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.Denied)
            {
                return null;
            }

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == taskName)
                {
                    cur.Value.Unregister(true);
                }
            }

            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = taskEntryPoint.FullName
            };

            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            return builder.Register(); ;
        }
    }
}
