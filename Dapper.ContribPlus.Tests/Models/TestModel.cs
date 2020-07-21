using Dapper.ContribPlus.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Tests.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
