using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Attributes
{
    /// <summary>
    /// Specifies that this field is a primary key in the database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
    }
}
