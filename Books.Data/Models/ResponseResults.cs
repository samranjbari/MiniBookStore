using System.Collections.Generic;

namespace Books.Data.Models
{
    public enum ResultStatuses
    {
        Success,
        Warning,
        Error
    }

    public class ResponseResults
    {
        public ResponseResults()
        {
            Messages = new List<string>();
        }

        public ResultStatuses ResultStatus { get; set; }

        public IList<string> Messages { get; set; }
    }
}
