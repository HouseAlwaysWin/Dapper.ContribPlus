﻿// using Dapper.ContribPlus.Tests.Models;
// using Npgsql;
// using NUnit.Framework;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

// namespace Dapper.ContribPlus.Tests
// {
//     public class PostgresSqlTest
//     {
//         private string connectionString;
//         [SetUp]
//         public void Setup()
//         {
//             connectionString = $"Host=localhost;Port=5432;Username=postgres;Password=123456;Database=postgres";
//         }

//         private void InitialData(string tableName)
//         {
//             string sql = @$"
//                 CREATE TABLE {tableName}
//                 (
//                     Id INT NOT NULL PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
//                     Name VARCHAR NULL,
//                     CreatedDate date NOT NULL DEFAULT now()
//                 );

//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-04-30 13:10:02');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-05-31 13:10:02');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-01-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-02-28 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-03-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('a','2007-05-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('b','2007-07-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('b','2007-06-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('b','2013-08-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('b','2015-09-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('b','2007-08-01 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('c','2007-08-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('c','2007-04-01 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('c','2007-02-01 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('c','2007-01-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('d','2007-10-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('d','2007-11-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('d','2007-12-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('d','2008-09-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('e','2018-02-28 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('e','2007-03-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('e','2007-04-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('e','2007-05-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('f','2007-06-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('f','2017-07-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('f','2019-08-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('f','2007-06-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('g','2007-09-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('h','2007-03-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('i','2018-05-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('j','2002-07-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('k','2003-03-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('l','2005-04-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('m','2008-01-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('n','2000-02-28 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('o','2001-06-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('p','2007-09-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('q','2002-10-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('r','2007-11-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('s','2020-05-30 11:11:11');
//                INSERT INTO  {tableName} (name,createddate) VALUES ('t','2010-06-30 11:11:11');
//             ";

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 conn.Execute($"DROP TABLE IF EXISTS {tableName}");
//                 conn.Execute(sql);
//             }
//         }


//         [Test]
//         public void IsValidGetPagingTotalCount_CorrectListAndTotalCount_10ItemAnd20TotalCount()
//         {
//             InitialData("Test");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 var result = conn.GetPaginated<Test>(2, 10);
//                 Assert.AreEqual(10, result.data.Count());
//                 Assert.AreEqual(41, result.totalCount);

//                 result = conn.GetPagination<Test>(2, 3, new { Name = "a" });
//                 Assert.AreEqual(3, result.data.Count());
//                 Assert.AreEqual(6, result.totalCount);

//                 conn.Execute("DROP TABLE Test");
//                 Assert.Pass();
//             }
//         }


//         [Test]
//         public void IsValidGetPagingTotalCount_CorrectListAndTotalCount_10ItemAnd20TotalCount_Acync()
//         {
//             InitialData("Test");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {

//                 Task.Run(async () =>
//                 {
//                     var result = conn.GetPaginationAsync<Test>(2, 10).Result;
//                     Assert.AreEqual(10, result.data.Count());
//                     Assert.AreEqual(41, result.totalCount);

//                     result = conn.GetPaginationAsync<Test>(2, 3, new { Name = "a" }).Result;
//                     Assert.AreEqual(3, result.data.Count());
//                     Assert.AreEqual(6, result.totalCount);

//                     conn.Execute("DROP TABLE Test");
//                     Assert.Pass();
//                 }).GetAwaiter().GetResult();

//             }
//         }

//         [Test]
//         public void IsValidItemsPerPage_CorrectItemContent()
//         {
//             InitialData("Test");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 var result = conn.GetPagination<Test>(1, 10);
//                 Assert.AreEqual("a", result.data.ToList()[0].Name);
//                 Assert.AreEqual("a", result.data.ToList()[1].Name);

//                 result = conn.GetPagination<Test>(2, 6);
//                 Assert.AreEqual("b", result.data.ToList()[0].Name);
//                 Assert.AreEqual("c", result.data.ToList()[5].Name);

//                 conn.Execute("DROP TABLE Test");
//                 Assert.Pass();
//             }
//         }

//         [Test]
//         public void IsValidItemsPerPage_CorrectItemContent_Async()
//         {
//             InitialData("Test");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 Task.Run(async () =>
//                 {
//                     var result = conn.GetPagination<Test>(1, 10);
//                     Assert.AreEqual("a", result.data.ToList()[0].Name);
//                     Assert.AreEqual("a", result.data.ToList()[1].Name);

//                     result = conn.GetPagination<Test>(2, 6);
//                     Assert.AreEqual("b", result.data.ToList()[0].Name);
//                     Assert.AreEqual("c", result.data.ToList()[5].Name);

//                     conn.Execute("DROP TABLE Test");
//                     Assert.Pass();
//                 }).GetAwaiter().GetResult();
//             }
//         }

//         [Test]
//         public void IsValidOrderByAttribute_OrderbyName()
//         {
//             InitialData("TestOrderBy");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 var result = conn.GetPagination<TestOrderBy>(1, 10);
//                 Assert.AreEqual("n", result.data.ToList()[0].Name);
//                 Assert.AreEqual("o", result.data.ToList()[1].Name);

//                 result = conn.GetPagination<TestOrderBy>(2, 6);
//                 Assert.AreEqual("a", result.data.ToList()[0].Name);
//                 Assert.AreEqual("a", result.data.ToList()[5].Name);

//                 conn.Execute("DROP TABLE TestOrderBy");
//                 Assert.Pass();
//             }
//         }


//         [Test]
//         public void IsValidOrderByAttribute_OrderbyName_Async()
//         {
//             InitialData("TestOrderBy");

//             using (var conn = new NpgsqlConnection(connectionString))
//             {
//                 Task.Run(async () =>
//                 {
//                     var result = conn.GetPagination<TestOrderBy>(1, 10);
//                     Assert.AreEqual("n", result.data.ToList()[0].Name);
//                     Assert.AreEqual("o", result.data.ToList()[1].Name);

//                     result = conn.GetPagination<TestOrderBy>(2, 6);
//                     Assert.AreEqual("a", result.data.ToList()[0].Name);
//                     Assert.AreEqual("a", result.data.ToList()[5].Name);

//                     conn.Execute("DROP TABLE TestOrderBy");
//                     Assert.Pass();
//                 }).GetAwaiter().GetResult();

//             }
//         }
//     }
// }
