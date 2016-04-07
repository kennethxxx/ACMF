using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WorkerRoleController.LogManagement
{
    public class LogController
    {

        public const String LogTitleInformation = "Information";
        public const String LogTitleError = "Error";
        public const String LogTitleWarning = "Warning";

        private static String scalingConfigurationStorageConnectionStringTag;
        private static String logTableName;
        private static String scalingLogTableEntitiesPartionKey;


        public static bool PublishFirstMessage(String eventTitle, String eventMessage)
        {
            logTableName = Settings.GetSettingString(Settings.Setting.ScalingLogTableName);
            scalingConfigurationStorageConnectionStringTag = Settings.GetConfigurationTag(Settings.Setting.ScalingConfigurationStorageConnectionString);
            scalingLogTableEntitiesPartionKey = Settings.GetSettingString(Settings.Setting.ScalingLogTableEntitiesPartionKey);
           return PublishMessage(eventTitle, eventMessage);
        }

        public static bool PublishMessage(String eventTitle, String eventMessage)
        {
            if (String.IsNullOrEmpty(scalingConfigurationStorageConnectionStringTag) || String.IsNullOrEmpty(logTableName) || String.IsNullOrEmpty(scalingLogTableEntitiesPartionKey))
            {
                throw new Exception();
            }

            try
            {
                CloudStorageAccount acct = Utils.GetStorageAccount(scalingConfigurationStorageConnectionStringTag);
                CloudTableClient tableClient = acct.CreateCloudTableClient();
                LogMessageServiceContext context = new LogMessageServiceContext(tableClient, logTableName);
                if (eventTitle == "Warning" || eventTitle == "ERROR")
                {
                    context.Insert(new LogMessageEntity(scalingLogTableEntitiesPartionKey, eventTitle, eventMessage));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private class LogMessageServiceContext : TableServiceContext
        {
            private readonly String partitionKey;

            public LogMessageServiceContext(CloudTableClient tableClient, String partionKey) : base(tableClient)
            {
                partitionKey = partionKey;
            }

            public void delete(LogMessageEntity message)
            {
                this.AttachObject(message);
                this.DeleteObject(message);
                this.SaveChanges();
            }

            private void AttachObject(LogMessageEntity message)
            {
                this.AttachTo(partitionKey, message, "*");
            }

            public void update(LogMessageEntity message)
            {
                this.AttachObject(message);
                this.UpdateObject(message);
                this.SaveChanges();
            }

            public IQueryable<LogMessageEntity> Accounts
            {
                get { return this.CreateQuery<LogMessageEntity>(partitionKey); }
            }

            public void Insert(LogMessageEntity message)
            {
                this.AddObject(partitionKey, message);
                this.SaveChanges();
            }
        }

        private class LogMessageEntity : TableServiceEntity
        {
            public LogMessageEntity(String partitionKey, String title, String message)
            {
                DateTime now = Utils.Now;
                PartitionKey = partitionKey;
                RowKey = String.Format("{0:10}_{1}", (DateTime.MaxValue.Ticks - now.Ticks), Guid.NewGuid());
                Timestamp = now;
                Title = title;
                Message = message;
            }

            public String Title { get; set; }
            public String Message { get; set; }

        }

    }
}
