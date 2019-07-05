using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Legacy;
using ServiceStack.OrmLite.Sqlite;

namespace TestBase
{
    public class InMemoryDatabaseFixture : IDisposable
    {
        private readonly OrmLiteConnectionFactory _dbFactory =
            new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);

        private IDbConnection _db;

        public IDbConnection OpenConnection() => _dbFactory.OpenDbConnection();

        public void Insert<T>(IEnumerable<T> items, string tableName)
        {
            _db = _db ?? OpenConnection();
            _db.DropAndCreateTable<T>(tableName);
            foreach (var item in items)
            {
                _db.Insert(tableName, item);
            }
        }

        public void Dispose()
        {
            _db?.Dispose();
            _db = null;
        }
    }

    public static class GenericTableExtensions
    {
        static object ExecWithAlias<T>(string table, Func<object> fn)
        {
            var modelDef = typeof(T).GetModelMetadata();
            lock (modelDef)
            {
                var hold = modelDef.Alias;
                try
                {
                    modelDef.Alias = table;
                    return fn();
                }
                finally
                {
                    modelDef.Alias = hold;
                }
            }
        }

        public static void DropAndCreateTable<T>(this IDbConnection db, string table)
        {
            ExecWithAlias<T>(table, () =>
            {
                db.DropAndCreateTable<T>();
                return null;
            });
        }

        public static long Insert<T>(this IDbConnection db, string table, T obj, bool selectIdentity = false)
        {
            return (long) ExecWithAlias<T>(table, () => db.Insert(obj, selectIdentity));
        }

        public static List<T> Select<T>(this IDbConnection db, string table,
            Func<SqlExpression<T>, SqlExpression<T>> expression)
        {
            return (List<T>) ExecWithAlias<T>(table, () => db.Select<T>(expression));
        }

        public static int Update<T>(this IDbConnection db, string table, T item, Expression<Func<T, bool>> where)
        {
            return (int) ExecWithAlias<T>(table, () => db.Update(item, where));
        }
    }
}