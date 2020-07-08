using Dapper.ContribPlus.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Tests.Models
{
    public class Product
    {
        [Key]
        [Where]
        public int Id { get; set; }
        [Where]
        public string Name { get; set; }
    }
}
