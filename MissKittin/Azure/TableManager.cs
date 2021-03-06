﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MissKittin.Azure
{
    public static class TableManager
    {
        public static IList<T> GetAll<T>(string tableName) where T : ITableEntity, new()
        {
            try
            {
                var table = GetTable(tableName);
                return table.ExecuteQuery(new TableQuery<T>()).ToList();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public static void Insert<T>(string tableName, ITableEntity entity) where T : ITableEntity, new()
        {
            var table = GetTable(tableName);
            var op = TableOperation.InsertOrReplace(entity);
            table.Execute(op);
        }

        private static CloudTable GetTable(string tableName)
        {
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}