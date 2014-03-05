using System.Data;
using Books.Data.Models;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface.Auth;

namespace Books.App_Start
{
    public class DatabaseConfiguration
    {
        private IDbConnectionFactory dbConnectionFactory { get; set; }
        
        public DatabaseConfiguration(OrmLiteConnectionFactory _dbConnectionFactory)
        {
            dbConnectionFactory = _dbConnectionFactory;
        }

        public void CreateMissingTables(Funq.Container container)
        {
            using (IDbConnection db = dbConnectionFactory.OpenDbConnection())
            {
                ((OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>()).CreateMissingTables();

                db.CreateTableIfNotExists<CategoryModels>();
                db.CreateTableIfNotExists<BookModels>();
                db.CreateTableIfNotExists<BookPropertyModels>();
            }
        }
    }
}