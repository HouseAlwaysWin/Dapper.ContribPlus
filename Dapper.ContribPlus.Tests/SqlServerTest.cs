using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper.ContribPlus;
using Dapper.ContribPlus.Tests.Models;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dapper.ContribPlus.Tests
{
    public class SqlServerTest
    {
        private string currentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\testDB.mdf";
        private string connectionString;
        [SetUp]
        public void Setup()
        {
            connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={currentPath};Integrated Security=True";
        }

        private void InitialData(string tableName)
        {
            string sql = @$"
                CREATE TABLE [dbo].[{tableName}]
                (
                    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
                    [Name] NVARCHAR NULL,
                    [CreatedDate] datetime NOT NULL DEFAULT  GETDATE()
                )

               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-04-30 13:10:02')
               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-05-31 13:10:02')
               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-01-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-02-28 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-03-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('a','2007-05-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('b','2007-07-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('b','2007-06-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('b','2013-08-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('b','2015-09-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('b','2007-07-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('c','2007-08-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('c','2007-03-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('c','2007-02-01 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('c','2007-01-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('d','2007-10-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('d','2007-11-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('d','2007-12-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('d','2008-09-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('e','2018-02-28 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('e','2007-03-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('e','2007-04-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('e','2007-05-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('f','2007-06-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('f','2017-07-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('f','2019-08-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('f','2007-06-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('g','2007-09-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('h','2007-03-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('i','2018-05-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('j','2002-07-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('k','2003-03-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('l','2005-04-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('m','2008-01-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('n','2000-02-28 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('o','2001-06-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('p','2007-09-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('q','2002-10-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('r','2007-11-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('s','2020-05-30 11:11:11')
               INSERT INTO  [dbo].[{tableName}] VALUES ('t','2010-06-30 11:11:11')
            ";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Execute($"IF OBJECT_ID('[dbo].[{tableName}]', 'U') IS NOT NULL DROP TABLE[dbo].[{tableName}]");
                conn.Execute(sql);
            }
        }


        [Test]
        public void IsValidGetPagingTotalCount_CorrectListAndTotalCount_10ItemAnd20TotalCount()
        {
            InitialData("Test");

            using (var conn = new SqlConnection(connectionString))
            {
                var result = conn.GetListByPaging<Test>(2, 10);
                Assert.AreEqual(10, result.data.Count());
                Assert.AreEqual(41, result.totalCount);

                result = conn.GetListByPaging<Test>(2, 3, new { Name = "a" });
                Assert.AreEqual(3, result.data.Count());
                Assert.AreEqual(6, result.totalCount);

                conn.Execute("DROP TABLE [dbo].[Test]");
                Assert.Pass();
            }
        }

        [Test]
        public void IsValidGetPagingTotalCount_CorrectListAndTotalCount_10ItemAnd20TotalCount_Acync()
        {
            InitialData("Test");

            using (var conn = new SqlConnection(connectionString))
            {

                Task.Run(async () =>
                {
                    var result = conn.GetListByPagingAsync<Test>(2, 10).Result;
                    Assert.AreEqual(10, result.data.Count());
                    Assert.AreEqual(41, result.totalCount);

                    result = conn.GetListByPaging<Test>(2, 3, new { Name = "a" });
                    Assert.AreEqual(3, result.data.Count());
                    Assert.AreEqual(6, result.totalCount);

                    conn.Execute("DROP TABLE [dbo].[Test]");
                    Assert.Pass();
                }).GetAwaiter().GetResult();

            }
        }

        [Test]
        public void IsValidItemsPerPage_CorrectItemContent()
        {
            InitialData("Test");

            using (var conn = new SqlConnection(connectionString))
            {
                var result = conn.GetListByPaging<Test>(1, 10);
                Assert.AreEqual("a", result.data.ToList()[0].Name);
                Assert.AreEqual("a", result.data.ToList()[1].Name);

                result = conn.GetListByPaging<Test>(2, 6);
                Assert.AreEqual("b", result.data.ToList()[0].Name);
                Assert.AreEqual("c", result.data.ToList()[5].Name);

                conn.Execute("DROP TABLE [dbo].[Test]");
                Assert.Pass();
            }
        }

        [Test]
        public void IsValidItemsPerPage_CorrectItemContent_Async()
        {
            InitialData("Test");

            using (var conn = new SqlConnection(connectionString))
            {
                Task.Run(async () =>
                {
                    var result = conn.GetListByPaging<Test>(1, 10);
                    Assert.AreEqual("a", result.data.ToList()[0].Name);
                    Assert.AreEqual("a", result.data.ToList()[1].Name);

                    result = conn.GetListByPaging<Test>(2, 6);
                    Assert.AreEqual("b", result.data.ToList()[0].Name);
                    Assert.AreEqual("c", result.data.ToList()[5].Name);

                    conn.Execute("DROP TABLE [dbo].[Test]");
                    Assert.Pass();
                }).GetAwaiter().GetResult();
            }
        }

        [Test]
        public void IsValidOrderByAttribute_OrderbyName()
        {
            InitialData("TestOrderBy");

            using (var conn = new SqlConnection(connectionString))
            {
                var result = conn.GetListByPaging<TestOrderBy>(1, 10);
                Assert.AreEqual("n", result.data.ToList()[0].Name);
                Assert.AreEqual("o", result.data.ToList()[1].Name);

                result = conn.GetListByPaging<TestOrderBy>(2, 6);
                Assert.AreEqual("a", result.data.ToList()[0].Name);
                Assert.AreEqual("a", result.data.ToList()[5].Name);

                conn.Execute("DROP TABLE [dbo].[TestOrderBy]");
                Assert.Pass();
            }
        }


        [Test]
        public void IsValidOrderByAttribute_OrderbyName_Async()
        {
            InitialData("TestOrderBy");

            using (var conn = new SqlConnection(connectionString))
            {
                Task.Run(async () =>
                {
                    var result = conn.GetListByPaging<TestOrderBy>(1, 10);
                    Assert.AreEqual("n", result.data.ToList()[0].Name);
                    Assert.AreEqual("o", result.data.ToList()[1].Name);

                    result = conn.GetListByPaging<TestOrderBy>(2, 6);
                    Assert.AreEqual("a", result.data.ToList()[0].Name);
                    Assert.AreEqual("a", result.data.ToList()[5].Name);

                    conn.Execute("DROP TABLE [dbo].[TestOrderBy]");
                    Assert.Pass();
                }).GetAwaiter().GetResult();

            }
        }
    }
}