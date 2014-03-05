using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Books.Data.Models;
using ServiceStack.ServiceHost;

namespace Books.ViewModels
{
    [Route("/categorybook")]
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<BookModels> CategoryBooks { get; set; }
    }
}