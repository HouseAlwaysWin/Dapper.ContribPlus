using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper.ContribPlus;
using Dapper.ContribPlus.Tests.Models;
using System.Linq;
using System.Diagnostics;

namespace Dapper.ContribPlus.Tests
{
    public class CRUDTests
    {
        private string currentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\testDB.mdf";
        private string connectionString;
        [SetUp]
        public void Setup()
        {
            connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={currentPath};Integrated Security=True";
        }

        private void InitialData()
        {
            string sql = @"
                CREATE TABLE [dbo].[Test]
                (
                    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
                    [Name] NVARCHAR NULL
                )

               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('a')
               INSERT INTO  [dbo].[Test] VALUES ('b')
               INSERT INTO  [dbo].[Test] VALUES ('b')
               INSERT INTO  [dbo].[Test] VALUES ('b')
               INSERT INTO  [dbo].[Test] VALUES ('b')
               INSERT INTO  [dbo].[Test] VALUES ('b')
               INSERT INTO  [dbo].[Test] VALUES ('c')
               INSERT INTO  [dbo].[Test] VALUES ('c')
               INSERT INTO  [dbo].[Test] VALUES ('c')
               INSERT INTO  [dbo].[Test] VALUES ('c')
               INSERT INTO  [dbo].[Test] VALUES ('d')
               INSERT INTO  [dbo].[Test] VALUES ('d')
               INSERT INTO  [dbo].[Test] VALUES ('d')
               INSERT INTO  [dbo].[Test] VALUES ('d')
               INSERT INTO  [dbo].[Test] VALUES ('e')
               INSERT INTO  [dbo].[Test] VALUES ('e')
               INSERT INTO  [dbo].[Test] VALUES ('e')
               INSERT INTO  [dbo].[Test] VALUES ('e')
               INSERT INTO  [dbo].[Test] VALUES ('f')
               INSERT INTO  [dbo].[Test] VALUES ('f')
               INSERT INTO  [dbo].[Test] VALUES ('f')
               INSERT INTO  [dbo].[Test] VALUES ('f')
               INSERT INTO  [dbo].[Test] VALUES ('g')
               INSERT INTO  [dbo].[Test] VALUES ('h')
               INSERT INTO  [dbo].[Test] VALUES ('i')
               INSERT INTO  [dbo].[Test] VALUES ('j')
               INSERT INTO  [dbo].[Test] VALUES ('k')
               INSERT INTO  [dbo].[Test] VALUES ('l')
               INSERT INTO  [dbo].[Test] VALUES ('m')
               INSERT INTO  [dbo].[Test] VALUES ('n')
               INSERT INTO  [dbo].[Test] VALUES ('o')
               INSERT INTO  [dbo].[Test] VALUES ('p')
               INSERT INTO  [dbo].[Test] VALUES ('q')
               INSERT INTO  [dbo].[Test] VALUES ('r')
               INSERT INTO  [dbo].[Test] VALUES ('s')
               INSERT INTO  [dbo].[Test] VALUES ('t')
            ";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Execute("IF OBJECT_ID('[dbo].[Test]', 'U') IS NOT NULL DROP TABLE[dbo].[Test]");
                conn.Execute(sql);
            }
        }


        [Test]
        public void IsValidGetPagingTotalCount_CorrectListAndTotalCount_10ItemAnd20TotalCount()
        {
            InitialData();

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
        public void IsValidItemsPerPage_CorrectItemContent()
        {
            InitialData();

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
    }
}