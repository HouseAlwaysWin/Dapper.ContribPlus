using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.ContribPlus.Helpers
{
    /// <summary>
    /// Defines a table name mapper for getting table names from types.
    /// </summary>
    public interface ITableNameMapper
    {
        /// <summary>
        /// Gets a table name from a given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get a name from.</param>
        /// <returns>The table name for the given <paramref name="type"/>.</returns>
        string GetTableName(Type type);
    }
}
