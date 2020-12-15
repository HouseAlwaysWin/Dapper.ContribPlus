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
        private string currentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\sqlServerDB.mdf";
        private string connectionString;
        [SetUp]
        public void Setup()
        {
            //connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={currentPath};Integrated Security=True";
            connectionString = $"Server=AA010064;Integrated Security=True";
        }

        [Test]
        public void Test(){

        }

    }
}