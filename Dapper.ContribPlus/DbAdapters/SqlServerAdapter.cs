﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.ContribPlus.DbAdapters
{
    /// <summary>
    /// The SQL Server database adapter.
    /// </summary>
    public class SqlServerAdapter : ISqlAdapter
    {
        /// <summary>
        /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
        /// </summary>
        /// <param name="connection">The connection to use.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="commandTimeout">The command timeout to use.</param>
        /// <param name="tableName">The table to insert into.</param>
        /// <param name="columnList">The columns to set with this insert.</param>
        /// <param name="parameterList">The parameters to set for this insert.</param>
        /// <param name="keyProperties">The key columns in this table.</param>
        /// <param name="entityToInsert">The entity to insert.</param>
        /// <returns>The Id of the row created.</returns>
        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            var cmd = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList});SELECT SCOPE_IDENTITY() id";
            var multi = connection.QueryMultiple(cmd, entityToInsert, transaction, commandTimeout);

            var first = multi.Read().FirstOrDefault();
            if (first == null || first.id == null) return 0;

            var id = (int)first.id;
            var propertyInfos = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
            if (propertyInfos.Length == 0) return id;

            var idProperty = propertyInfos[0];
            idProperty.SetValue(entityToInsert, Convert.ChangeType(id, idProperty.PropertyType), null);

            return id;
        }

        /// <summary>
        /// Inserts <paramref name="entityToInsert"/> into the database, returning the Id of the row created.
        /// </summary>
        /// <param name="connection">The connection to use.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="commandTimeout">The command timeout to use.</param>
        /// <param name="tableName">The table to insert into.</param>
        /// <param name="columnList">The columns to set with this insert.</param>
        /// <param name="parameterList">The parameters to set for this insert.</param>
        /// <param name="keyProperties">The key columns in this table.</param>
        /// <param name="entityToInsert">The entity to insert.</param>
        /// <returns>The Id of the row created.</returns>
        public async Task<int> InsertAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            var cmd = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList}); SELECT SCOPE_IDENTITY() id";
            var multi = await connection.QueryMultipleAsync(cmd, entityToInsert, transaction, commandTimeout).ConfigureAwait(false);

            var first = multi.Read().FirstOrDefault();
            if (first == null || first.id == null) return 0;

            var id = (int)first.id;
            var pi = keyProperties as PropertyInfo[] ?? keyProperties.ToArray();
            if (pi.Length == 0) return id;

            var idp = pi[0];
            idp.SetValue(entityToInsert, Convert.ChangeType(id, idp.PropertyType), null);

            return id;
        }




        /// <summary>
        /// Adds the name of a column.
        /// </summary>
        /// <param name="sb">The string builder  to append to.</param>
        /// <param name="columnName">The column name.</param>
        public void AppendColumnName(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}]", columnName);
        }

        /// <summary>
        /// Adds a column equality to a parameter.
        /// </summary>
        /// <param name="sb">The string builder  to append to.</param>
        /// <param name="columnName">The column name.</param>
        public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
        {
            sb.AppendFormat("[{0}] = @{1}", columnName, columnName);
        }


        public string GetPaginatedCmd(string tableName, string orderBy, int currentPage, int itemsPerPage, bool isDesc)
        {
            string desc = isDesc ? "DESC" : "ASC";
                int totalItems = (currentPage - 1) * itemsPerPage;
                return $"SELECT * FROM {tableName} ORDER BY {orderBy} {desc} OFFSET {totalItems} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY ";
        }

        public void BulkInsert<T>(IDbConnection connection, IEnumerable<T> data, IDbTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30)
        {
                var type = typeof (T);
                var tableName = SqlMapperExtensions.GetTableName(type);
                DataTable dataTables = data.ToDataTable();
                using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction))
                {
                    bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                    bulkCopy.BatchSize = batchSize;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.ToColumnMapping<T>();
                    bulkCopy.WriteToServer(dataTables);
                }
        }

        public async Task BulkInsertAsync<T>(IDbConnection connection, IEnumerable<T> data, IDbTransaction transaction = null, int batchSize = 0, int bulkCopyTimeout = 30)
        {
             var type = typeof (T);
                var tableName = SqlMapperExtensions.GetTableName(type);
                DataTable dataTables = data.ToDataTable();
                using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction))
                {
                    bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                    bulkCopy.BatchSize = batchSize;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.ToColumnMapping<T>();
                    await bulkCopy.WriteToServerAsync(dataTables);
                }
        }
    }
}
