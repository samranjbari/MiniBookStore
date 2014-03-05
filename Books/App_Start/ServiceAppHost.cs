using System.Web.Mvc;
using Books.Services;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Configuration;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.Redis;

namespace Books.App_Start
{
    public class ServiceAppHost : AppHostBase
    {
        public ServiceAppHost()
            : base("BookService", typeof(HomeService).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            SetConfig(new EndpointHostConfig
            {
                ServiceStackHandlerFactoryPath = "api"
            });

            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Plugins.Add(new RegistrationFeature());

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                    new IAuthProvider[] 
                    { 
                        new CredentialsAuthProvider(), 
                        new TwitterAuthProvider(new AppSettings()) 
                    }));
            
            //container.Register<ICacheClient>(new MemoryCacheClient());
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(20, 60, "72.167.34.38:6379"));
            container.Register<ICacheClient>(c => (ICacheClient)c.Resolve<IRedisClientsManager>().GetCacheClient());

            var dbConnectionFactory = new OrmLiteConnectionFactory(ServiceStack.Configuration.ConfigUtils.GetConnectionString("booksql"), SqlServerDialect.Provider)
            {
                ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
            };
            container.Register<IDbConnectionFactory>(dbConnectionFactory);

            //var userRepository = new RedisAuthRepository(Resolve<PooledRedisClientManager>());
            //container.Register<IUserAuthRepository>(userRepository);
            
            //Register all your dependencies
            ContainerDependencyInjector.Register(container);

            //Set MVC to use the same Funq IOC as ServiceStack
            ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));

            // create tables
            //new DatabaseConfiguration(dbConnectionFactory).CreateMissingTables(container);
        }
    }
}