using DPTS.Domain.Common;
using DPTS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    public class PictureController : Controller
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            this._pictureService = pictureService;
        }

        [HttpPost]
        //do not validate request token (XSRF)
        public ActionResult AsyncUpload()
        {
            try
            {
                Stream stream = null;
                var fileName = "";
                var contentType = "";
                if (String.IsNullOrEmpty(Request["qqfile"]))
                {
                    // IE
                    HttpPostedFileBase httpPostedFile = Request.Files[0];
                    if (httpPostedFile == null)
                        throw new ArgumentException("No file uploaded");
                    stream = httpPostedFile.InputStream;
                    fileName = Path.GetFileName(httpPostedFile.FileName);
                    contentType = httpPostedFile.ContentType;
                }
                else
                {
                    //Webkit, Mozilla
                    stream = Request.InputStream;
                    fileName = Request["qqfile"];
                }

                var fileBinary = new byte[stream.Length];
                stream.Read(fileBinary, 0, fileBinary.Length);

                var fileExtension = Path.GetExtension(fileName);
                if (!String.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();
                //contentType is not always available 
                //that's why we manually update it here
                //http://www.sfsu.edu/training/mimetype.htm
                if (String.IsNullOrEmpty(contentType))
                {
                    switch (fileExtension)
                    {
                        case ".bmp":
                            contentType = MimeTypes.ImageBmp;
                            break;
                        case ".gif":
                            contentType = MimeTypes.ImageGif;
                            break;
                        case ".jpeg":
                        case ".jpg":
                        case ".jpe":
                        case ".jfif":
                        case ".pjpeg":
                        case ".pjp":
                            contentType = MimeTypes.ImageJpeg;
                            break;
                        case ".png":
                            contentType = MimeTypes.ImagePng;
                            break;
                        case ".tiff":
                        case ".tif":
                            contentType = MimeTypes.ImageTiff;
                            break;
                        default:
                            break;
                    }
                }

                var picture = _pictureService.InsertPicture(fileBinary, contentType, null);
                //when returning JSON the mime-type must be set to text/plain
                //otherwise some browsers will pop-up a "Save As" dialog.
                return Json(new
                {
                    success = true,
                    pictureId = picture.Id,
                    imageUrl = _pictureService.GetPictureUrl(picture, 100)
                },
                    MimeTypes.TextPlain);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                return Content(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        public ActionResult GetUserPictureByUser(string userId)
        {
            string pictureUrl = string.Empty;
            if(!string.IsNullOrWhiteSpace(userId))
            {
                var pictures = _pictureService.GetPicturesByUserId(userId);
                var defaultPicture = pictures.FirstOrDefault();
                pictureUrl = _pictureService.GetPictureUrl(defaultPicture, 365, false);
            }

            return Json(new
            {
                imageUrl = pictureUrl
            },
                MimeTypes.TextPlain);
        }
    }
}