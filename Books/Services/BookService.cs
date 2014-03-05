using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Books.Data.API;
using Books.Data.Models;
using Books.Parser;
using Books.ViewModels;
using ServiceStack.FluentValidation.Results;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace Books.Services
{
    public class BookService : Service
    {
        public IBookRepository BookRepository { get; set; }
        public ILookupRepository<CategoryModels> LookupRepository { get; set; }
        public IUserAuthRepository UserAuthRepository { get; set; }
        public IEmailService EmailService { get; set; }

        public object Get(BookModels request)
        {
            if (!string.IsNullOrEmpty(request.AmazonUrl))
            {
                try
                {
                    var response = BookParser.GetBookDetails(request.AmazonUrl);
                    return response;
                }
                catch
                {
                }
            }

            if (request.Id > 0)
            {
                var book = BookRepository.Find(request.Id);
                book.Description = HttpContext.Current.Server.HtmlDecode(book.Description);

                return book;
            }

            var books = new List<BookModels>();
            if (request.CategoryId <= 0)
            {
                var userAuth = this.GetSession();
                var userAuthId = userAuth.Id.To<int>();

                books = BookRepository.All().Where(a => a.UserId == userAuthId && (request.CategoryId <= 0 || a.CategoryId == request.CategoryId)).ToList();
            }
            else
            {
                books = BookRepository.All().Where(a => request.CategoryId <= 0 || a.CategoryId == request.CategoryId).ToList();
            }

            foreach (var book in books)
            {
                book.CategoryText = LookupRepository.Find(book.CategoryId).Name;
                book.Description = HttpContext.Current.Server.HtmlDecode(book.Description);
            }

            return books;
        }

        [Authenticate]
        public object Post(BookModels request)
        {
            ValidationResult validations = new BookValidator().Validate(request);

            if (!validations.IsValid)
            {
                request.ResponseResult.ResultStatus = ResultStatuses.Error;
                foreach (var item in validations.Errors)
                {
                    request.ResponseResult.Messages.Add(item.ErrorMessage);
                }

                return request;
            }

            if (request.InsertMode)
            {
                if (request.UserId <= 0)
                {
                    var userAuth = this.GetSession();
                    request.UserId = userAuth.Id.To<int>();
                }
            }

            request.Description = HttpContext.Current.Server.HtmlEncode(request.Description);
            if (request.Description.Length > 1500)
            {
                request.Description = request.Description.Substring(0, 1500);
            }

            if (request.CategoryId <= 0)
            {
                request.CategoryId = LookupRepository.Insert(new CategoryModels
                {
                    Name = request.CategoryText,
                    SortOrder = 0
                });
            }

            if (request.Id > 0)
            {
                BookRepository.Update(request);
            }
            else
            {
                BookRepository.Insert(request);
            }

            var liveMode = ServiceStack.Configuration.ConfigUtils.GetAppSetting("AppMode");
            if (request.InsertMode && liveMode.Equals("Live", StringComparison.InvariantCultureIgnoreCase))
            {
                // send a notification email
                EmailService.SendSmtpEmail(new EmailServiceModels
                {
                    Subject = "A New Book has been submitted and Needs your attention",
                    To = new List<string>() { "samranjbari@gmail.com", "kevinelliott3@gmail.com", "nmcmanmon@gmail.com" },
                    IsBodyHtml = false,
                    Body = string.Format("{0} has uploaded a book named {1}. You will need to upload the book to gumroad.", request.Author, request.Title),
                    Attachments = new List<string>() { string.Format("C:\\websites\\{0}", request.BookUrl) }
                });
            }

            return null;
        }

        [Authenticate]
        public object Put(BookModels request)
        {
            ValidationResult validations = new BookValidator().Validate(request);

            if (!validations.IsValid)
            {
                return validations.Errors;
            }

            request.Description = HttpContext.Current.Server.HtmlEncode(request.Description);

            if (request.CategoryId <= 0)
            {
                request.CategoryId = LookupRepository.Insert(new CategoryModels
                {
                    Name = request.CategoryText,
                    SortOrder = 0
                });
            }

            BookRepository.Update(request);

            return request.Id;
        }

        [Authenticate]
        public void Delete(BookModels request)
        {
            BookRepository.Delete((int)request.Id);
        }

        public object Get(FeaturedBooksViewModel request)
        {
            var books = BookRepository.All().Where(a => a.IsFeature == true);
            return books;
        }

        public object Get(SearchBooksViewModel request)
        {
            return BookRepository.All().Where(a => (string.IsNullOrEmpty(request.Author) || a.Author.ToLowerInvariant().Contains(request.Author.ToLowerInvariant())));
        }

        public object Post(FileUploadViewModel request)
        {
            var files = this.Request.Files;
            if (files == null)
            {
                return null;
            }

            var filename = string.Empty;
            var fileType = new FileInfo(files[0].FileName).Extension;
            var destinationRoot = HttpContext.Current.Server.MapPath("~/img/tmps/");

            if (!request.IsTemporary)
            {
                destinationRoot = string.Format("C:\\Websites\\Storage\\{0}\\", request.FileType);
            }

            filename = Guid.NewGuid() + ".Png";
            var tmpFilename = Guid.NewGuid() + fileType;

            var fileurl = string.Format("{0}{1}", destinationRoot, filename);
            var tmpUrl = string.Format("{0}{1}", destinationRoot, tmpFilename);

            foreach (var file in files)
            {
                using (var fileStream = File.Create(tmpUrl))
                {
                    file.InputStream.CopyTo(fileStream);
                }
            }

            if (request.FileType.Equals("Covers", StringComparison.InvariantCultureIgnoreCase))
            {
                var image = Image.FromFile(tmpUrl);
                var newImage = ScaleImage(image, 170, 250);
                newImage.Save(fileurl, ImageFormat.Png);
                newImage.Dispose();
                File.Delete(tmpUrl);
            }

            if (request.IsTemporary)
            {
                return string.Format("/img/tmps/{0}", filename);
            }
            else
            {
                return string.Format("/Storage/{0}/{1}", request.FileType, filename);
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            image.Dispose();
            return newImage;
        }
    }
}
