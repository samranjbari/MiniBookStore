using System.Collections.Generic;
using System.Data;
using Books.Data.API;
using ServiceStack.OrmLite;
using Books.Data.Models;

namespace Books.Data.OrmLite
{
    public class LookupRepository<T> : Repository<T>,  ILookupRepository<T> where T : ModelBase
    {
    }
}
