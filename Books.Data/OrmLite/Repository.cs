using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Books.Data.API;
using Books.Data.Models;
using DapperExtensions;

namespace Books.Data.OrmLite
{
    public abstract class Repository<T> : RepositoryBase, IRepository<T> where T : ModelBase
    {
        public virtual T Find(long id)
        {
            return UseConnection(conn => conn.Get<T>(id));
        }

        public virtual int Insert(T entity)
        {
            return UseConnection(conn =>
            {
                var id = conn.Insert(entity);
                return id;
            });
        }

        public IEnumerable<T> Insert(IEnumerable<T> entities)
        {
            return UseConnection(conn =>
            {
                var output = new List<T>();
                foreach (var entity in entities)
                {
                    entity.Id = conn.Insert(entity);
                    output.Add(entity);
                }

                return output;
            });
        }

        public virtual bool Update(T entity)
        {
            return UseConnection(conn => conn.Update(entity));
        }

        public virtual void Delete(int id)
        {
            var predicate = Predicates.Field<T>(f => f.Id, Operator.Eq, id);
            UseConnection(conn => conn.Delete<T>(predicate));
        }

        public virtual IEnumerable<T> All()
        {
            return UseConnection(conn => conn.GetList<T>().ToList());
        }

        public IEnumerable<T> Query(IPredicate predicate)
        {
            return UseConnection(conn => conn.GetList<T>(predicate));
        }

        //protected IEnumerable<T> QueryPaged(IPredicate predicate, ISort sort, int page, int resultsPerPage)
        //{
        //    return QueryPaged(predicate, new List<ISort> { sort }, page, resultsPerPage);
        //}

        //protected IEnumerable<T> QueryPaged(IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage)
        //{
        //    return UseConnection(conn => conn.GetPage<T>(predicate, sort, page, resultsPerPage));
        //}
    }

    public class RepositoryBase
    {
        private static IDbConnection GetConnection()
        {
            return new SqlConnection(ServiceStack.Configuration.ConfigUtils.GetConnectionString("booksql"));
        }

        protected static T UseConnection<T>(Func<IDbConnection, T> query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var output = query.Invoke(connection);
                if (connection.State == ConnectionState.Open) connection.Close();
                return output;
            }
        }

        protected static void UseConnection(Action<IDbConnection> query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                query.Invoke(connection);
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }
    }
}