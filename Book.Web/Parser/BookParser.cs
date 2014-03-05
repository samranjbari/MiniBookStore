using System;
using System.Linq;
using Books.Data.Models;
using HtmlAgilityPack;

namespace Books.Parser
{
    public class BookParser
    {
        public static BookModels GetBookDetails(string url)
        {
            var book = new BookModels();
            book.AmazonUrl = url;

            var webGet = new HtmlWeb();
            var htmlDoc = webGet.Load(url);

            htmlDoc.OptionFixNestedTags = true;

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
            }

            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    var image = bodyNode.SelectSingleNode("//img[@id='main-image']");

                    if (image == null)
                    {
                        image = bodyNode.SelectSingleNode("//img[@id='imgBlkFront']");
                    }

                    if (image != null)
                    {
                        book.CoverUrl = image.Attributes["src"].Value;
                        book.CoverUrl = book.CoverUrl.Substring(0, book.CoverUrl.IndexOf("._")) + ".jpg";
                    }

                    var title = bodyNode.Descendants("span")
                                        .Where(x=>x.Id == "btAsinTitle")
                                        .Select(s => s.InnerText);
                    book.Title = title.FirstOrDefault();

                    if (string.IsNullOrEmpty(book.Title))
                    {
                        title = bodyNode.Descendants("h1")
                                        .Where(x => x.Id == "title")
                                        .Select(s => s.InnerText);

                        book.Title = title.FirstOrDefault();
                    }

                    var price = bodyNode.SelectSingleNode("//b[@class='priceLarge']");
                                        
                    if (price != null)
                    {
                        book.Price = Convert.ToDecimal(price.InnerText.Trim().Replace("$", string.Empty).Replace("\n", string.Empty));
                    }

                    var description = bodyNode.SelectSingleNode("//div[@id='postBodyPS']")
                                              .InnerText;

                    book.Description = description;
                }
            }

            return book;
        }
    }
}