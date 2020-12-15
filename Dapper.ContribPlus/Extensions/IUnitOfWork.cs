using System;
using System.Data;

namespace Dapper.ContribPlus.Extensions
{
   public interface IUnitOfWork : IDisposable {

        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void BeginTrans ();
        void Commit ();
        void Rollback ();
    }
}