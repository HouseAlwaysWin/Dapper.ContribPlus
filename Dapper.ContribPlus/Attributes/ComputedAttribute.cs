using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Attributes
{
    /// <summary>
    /// Specifies that this is a computed column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
    }
}
