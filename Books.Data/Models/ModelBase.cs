
namespace Books.Data.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            ResponseResult = new ResponseResults();
        }

        public long Id { get; set; }

        [DapperIgnore]
        public ResponseResults ResponseResult  { get; set; }
    }
}
