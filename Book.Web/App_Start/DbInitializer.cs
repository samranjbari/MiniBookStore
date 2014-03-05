using System.Linq;
using Books.Data;
using DapperExtensions.Mapper;

namespace Books.App_Start
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(SafePluralizedAutoClassMapper<>);
        }
    }

    public class SafePluralizedAutoClassMapper<T> : PluralizedAutoClassMapper<T> where T : class
    {
        private bool _allowAutoMapping;

        public SafePluralizedAutoClassMapper()
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(pi => pi.GetCustomAttributes(typeof(DapperIgnoreAttribute), false).Any());
            foreach (var propertyInfo in properties)
            {
                Map(propertyInfo).Ignore();
            }

            foreach (var propertyInfo in type.GetProperties().Where(pi => !pi.GetCustomAttributes(typeof(DapperIgnoreAttribute), false).Any()))
            {
                Map(propertyInfo).Key(propertyInfo.Name == "Id" ? KeyType.Identity : KeyType.NotAKey);
            }

            _allowAutoMapping = true;
            AutoMap();
        }

        protected override sealed void AutoMap()
        {
            if (!_allowAutoMapping) return;
            base.AutoMap();
        }

        public override void Table(string tableName)
        {
            var type = typeof(T);
            object[] attributes = type.GetCustomAttributes(typeof(ServiceStack.DataAnnotations.AliasAttribute), true);

            if (attributes != null)
            {
                ServiceStack.DataAnnotations.AliasAttribute alias = attributes[0] as ServiceStack.DataAnnotations.AliasAttribute;
                TableName = alias.Name;

                return;
            }

            base.Table(tableName);
        }
    }
}