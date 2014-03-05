using System.Collections.Generic;

namespace Books.Data.Models
{
    public class EmailServiceModels
    {
        public EmailServiceModels()
        {
            To = new List<string>();
        }

        public string Body { get; set; }
        
        public bool IsBodyHtml { get; set; }

        public string Subject { get; set; }

        public IList<string> To { get; set; }

        public IList<string> Attachments { get; set; }
    }
}
