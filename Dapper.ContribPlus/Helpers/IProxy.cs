using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Helpers
{
    /// <summary>
    /// Defined a proxy object with a possibly dirty state.
    /// </summary>
    public interface IProxy //must be kept public
    {
        /// <summary>
        /// Whether the object has been changed.
        /// </summary>
        bool IsDirty { get; set; }
    }
}
