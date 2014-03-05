using Books.Data.API;
using Books.Data.Models;
using Books.Data.OrmLite;
using Books.Data.Service;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface.Auth;

namespace Books.App_Start
{
    public static class ContainerDependencyInjector
    {
        public static void Register(Funq.Container container)
        {
            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
            
            container.Register<IBookRepository>(new BookRepository());
            container.Register<ILookupRepository<CategoryModels>>(new LookupRepository<CategoryModels>());

            container.Register<IEmailService>(new EmailService());
        }
    }
}