using Dapper.ContribPlus.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Tests.Models
{
    [Table("Test")]
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Where]
        public string Name { get; set; }
    }
}
