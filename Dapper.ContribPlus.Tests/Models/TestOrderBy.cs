using Dapper.ContribPlus.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Tests.Models
{
    [Table("TestOrderBy")]
    public class TestOrderBy
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [OrderBy]
        public DateTime CreatedDate { get; set; }
    }
}
