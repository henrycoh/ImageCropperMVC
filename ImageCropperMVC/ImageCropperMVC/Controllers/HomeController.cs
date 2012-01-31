using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

using ImageCropperMVC.Infrastructure.CustomResult;

namespace ImageCropperMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "ImageCropperMVC";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public PartialViewResult _ImageCrop(string fileName)
        {
            ViewBag.FileName = fileName;
            return PartialView();
        }

        public ImageResult RenderOriginalImage(int width, int height, string fileName)
        {
            return new ImageResult(width, height, fileName);
        }

        public ActionResult RenderFromSession()
        {
            if (Session["Cropped"] != null)
                return new FileContentResult((byte[])Session["Cropped"], "image/jpeg");
            else
                return Content("");
        }

        [HttpPost]
        public JsonResult ProcessImageCrop(
            double x1,
            double y1,
            double x2,
            double y2,
            double w1,
            double h1,
            string file_name,
            double img_width,
            double img_height)
        {
            Bitmap orgBMP = null;
            Bitmap bmpOut = null;
            Graphics g = null;

            int width = Convert.ToInt32(img_width * 2);
            int height = Convert.ToInt32(img_height * 2);

            orgBMP = new Bitmap(Path.Combine(HttpContext.Server.MapPath("~/Temp"), file_name));

            decimal ratio;
            int newWidth = 0;
            int newHeight = 0;
            decimal orgImageRatio = (decimal)orgBMP.Width / orgBMP.Height;
            decimal newImageRatio = (decimal)width / height;

            if (newImageRatio >= orgImageRatio)
            {
                newHeight = (int)height;
                ratio = (decimal)height / orgBMP.Height;
                decimal temp = orgBMP.Width * ratio;
                newWidth = (int)temp;
            }
            else
            {
                newWidth = (int)width;
                ratio = (decimal)width / orgBMP.Width;
                decimal temp = orgBMP.Height * ratio;
                newHeight = (int)temp;
            }

            bmpOut = new Bitmap(width, height);
            g = Graphics.FromImage(bmpOut);

            int x = (width - newWidth) / 2;
            int y = (height - newHeight) / 2;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, width, height);
            g.DrawImage(orgBMP, x, y, newWidth, newHeight);

            int top = (int)y1 * 2;
            int left = (int)x1 * 2;
            int bottom = (int)y2 * 2;
            int right = (int)x2 * 2;

            Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
            Bitmap cropped = bmpOut.Clone(rect, bmpOut.PixelFormat);

            MemoryStream toStream = new MemoryStream();
            cropped.Save(toStream, ImageFormat.Jpeg);
            byte[] tempStorage = toStream.GetBuffer();

            toStream.Close();

            // Save the cropped image to session
            Session["Cropped"] = tempStorage;

            return Json(new
            {
                Successful = true
            });
        }
    }
}
