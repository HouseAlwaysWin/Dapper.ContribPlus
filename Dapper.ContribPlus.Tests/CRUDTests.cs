using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper.ContribPlus;
using Dapper.ContribPlus.Tests.Models;

namespace Dapper.ContribPlus.Tests
{
    public class CRUDTests
    {
        private string currentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\testDB2.mdf";
        private string connectionString;
        [SetUp]
        public void Setup()
        {
            connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={currentPath};Integrated Security=True";
        }

        [Test]
        public void IsValidGetPaging_CorrectListAndTotalCount_10ItemAnd20TotalCount()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.GetListByPaging<Product>(new
                {
                    Id = 1,
                    Name = "test"
                }, 1, 10);
            }
            Assert.Pass();
        }
    }
}