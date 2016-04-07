using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using System.Data.Services.Client;
using System.Reflection;

namespace TableOperatorClassNS
{
    public class TableOperatorClass
    {
        private CloudStorageAccount cloudStorageAccount = null;
        private CloudTableClient TableClient = null;
        private string DefaultTableName = "";

        private TableOperatorClass()
        {

        }

        public TableOperatorClass(string CloudStorageConnectionString, string DefaultTable)
        {
            DefaultTableName = DefaultTable;
            cloudStorageAccount = CloudStorageAccount.Parse(CloudStorageConnectionString);
            TableClient = cloudStorageAccount.CreateCloudTableClient();
            TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero);
        }

        public TableOperatorClass(string CloudStorageConnectionString)
        {
            cloudStorageAccount = CloudStorageAccount.Parse(CloudStorageConnectionString);
            TableClient = cloudStorageAccount.CreateCloudTableClient();
            TableClient.RetryPolicy = RetryPolicies.Retry(4, TimeSpan.Zero);
        }

        public bool CreateTable()
        {
            return CreateTable(DefaultTableName);
        }
        public bool CreateTable(string tableName)
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (tableName.CompareTo("") == 0)
            {
                return false;
            }

            try
            {
                TableClient.CreateTableIfNotExist(tableName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteTable()
        {
            return DeleteTable(DefaultTableName);
        }
        public bool DeleteTable(string tableName)
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (tableName.CompareTo("") == 0)
            {
                return false;
            }

            try
            {
                TableClient.DeleteTableIfExist(tableName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool InsertEntity<T>(T EDE) where T : TableServiceEntity
        {
            if (DefaultTableName != "")

                return InsertEntity(DefaultTableName, EDE);
            else
                return false;
        }
        public bool InsertEntity<T>(string tableName, T EDE) where T : TableServiceEntity
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (!CreateTable(tableName))
            {
                return false;
            }

            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                tableServiceContext.AddObject(tableName, EDE);
                tableServiceContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool GetEntity<T>(string partitionKey, string rowKey, out IQueryable<T> EntityIQ) where T : TableServiceEntity
        {
            EntityIQ = null;
            return this.GetEntity(DefaultTableName, partitionKey, rowKey, out EntityIQ);
        }
        public bool GetEntity<T>(string tableName, string partitionKey, string rowKey, out IQueryable<T> EntityIQ) where T : TableServiceEntity
        {
            EntityIQ = null;

            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (!CreateTable(tableName))
            {
                return false;
            }

            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                EntityIQ = (from e in tableServiceContext.CreateQuery<T>(tableName)
                            where e.PartitionKey == partitionKey && e.RowKey == rowKey
                            select e);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool GetEntity<T>(string partitionKey, string rowKey, out List<T> EntityList) where T : TableServiceEntity
        {
            EntityList = null;
            return this.GetEntity(DefaultTableName, partitionKey, rowKey, out EntityList);
        }
        public bool GetEntity<T>(string tableName, string partitionKey, string rowKey, out List<T> EntityList) where T : TableServiceEntity
        {
            EntityList = null;
            try
            {
                IQueryable<T> EntityIQ = null;
                this.GetEntity(tableName, partitionKey, rowKey, out EntityIQ);
                EntityList = EntityIQ.ToList();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool GetEntity<T>(string partitionKey, string rowKey, out T entity) where T : TableServiceEntity
        {
            entity = null;
            return this.GetEntity(DefaultTableName, partitionKey, rowKey, out entity);
        }
        public bool GetEntity<T>(string tableName, string partitionKey, string rowKey, out T entity) where T : TableServiceEntity
        {
            entity = null;
            try
            {
                IQueryable<T> EntityIQ = null;
                this.GetEntity(tableName, partitionKey, rowKey, out EntityIQ);
                entity = EntityIQ.FirstOrDefault();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataServiceQuery<T> GetQueryEntities<T>() where T : TableServiceEntity
        {
            return this.GetQueryEntities<T>(DefaultTableName);
        }
        public DataServiceQuery<T> GetQueryEntities<T>(string tableName) where T : TableServiceEntity
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return null;
            }

            if (!CreateTable(tableName))
            {
                return null;
            }

            TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
            return tableServiceContext.CreateQuery<T>(tableName);
        }

        public bool UpdateEntity<T>(bool ReplaceMode, string partitionKey, string rowKey, T obj) where T : TableServiceEntity
        {
            return UpdateEntity<T>(DefaultTableName, ReplaceMode, partitionKey, rowKey, obj);
        }
        public bool UpdateEntity<T>(string tableName, bool ReplaceMode, string partitionKey, string rowKey, T obj) where T : TableServiceEntity
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (!CreateTable(tableName))
            {
                return false;
            }

            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                IQueryable<T> entities = (from e in tableServiceContext.CreateQuery<T>(tableName)
                                          where e.PartitionKey == partitionKey && e.RowKey == rowKey
                                          select e);

                T entity = entities.FirstOrDefault();

                Type t = obj.GetType();
                PropertyInfo[] pi = t.GetProperties();

                foreach (PropertyInfo p in pi)
                {
                    p.SetValue(entity, p.GetValue(obj, null), null);
                }

                tableServiceContext.UpdateObject(entity);

                if (ReplaceMode)
                    tableServiceContext.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
                else
                    tableServiceContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteEntity<T>(string partitionKey, string rowKey) where T : TableServiceEntity
        {
            return DeleteEntity<T>(DefaultTableName, partitionKey, rowKey);
        }
        public bool DeleteEntity<T>(string tableName, string partitionKey, string rowKey) where T : TableServiceEntity
        {
            if (cloudStorageAccount == null || TableClient == null)
            {
                return false;
            }

            if (!CreateTable(tableName))
            {
                return false;
            }

            try
            {
                TableServiceContext tableServiceContext = TableClient.GetDataServiceContext();
                IQueryable<T> entities = (from e in tableServiceContext.CreateQuery<T>(tableName)
                                          where e.PartitionKey == partitionKey && e.RowKey == rowKey
                                          select e);

                T entity = entities.FirstOrDefault();

                if (entities != null)
                {
                    tableServiceContext.DeleteObject(entity);
                    tableServiceContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class DataEntity_LogEvent : TableServiceEntity
    {
        public DataEntity_LogEvent(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }
        public DataEntity_LogEvent() : this(Guid.NewGuid().ToString(), String.Empty) { } // 這個一定要有，否則無法Query
        public string URL { get; set; }
        public string Time { get; set; }
        public string Catalog { get; set; }
        public string Message { get; set; }
    }

    public class DataEntity_JobEvent : TableServiceEntity
    {
        public DataEntity_JobEvent(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }
        public DataEntity_JobEvent() : this(Guid.NewGuid().ToString(), String.Empty) { } // 這個一定要有，否則無法Query
        public string Name { get; set; }
        public string State { get; set; }
        public string Steps { get; set; }
        public string Message { get; set; }
    }
}

