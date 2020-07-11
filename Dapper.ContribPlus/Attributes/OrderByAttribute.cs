using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderByAttribute : Attribute
    {
        public OrderByAttribute(bool isDesc = false)
        {
            IsDesc = isDesc;
        }
        /// <summary>
        /// Whether a field is writable in the database.
        /// </summary>
        public bool IsDesc { get; }
    }
}
