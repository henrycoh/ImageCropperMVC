using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ImageCropperMVC.Infrastructure.CustomResult
{
    public class ImageResult : ActionResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string FileName { get; set; }

        public ImageResult(int width, int height, string fileName)
        {
            Width = width;
            Height = height;
            FileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Bitmap orgBMP = null;
            Bitmap bmpOut = null;
            Graphics g = null;

            orgBMP = new Bitmap(Path.Combine(context.HttpContext.Server.MapPath("~/Temp"), FileName));

            decimal ratio;
            int newWidth = 0;
            int newHeight = 0;
            decimal orgImageRatio = (decimal)orgBMP.Width / orgBMP.Height;
            decimal newImageRatio = (decimal)Width / Height;

            if (newImageRatio >= orgImageRatio)
            {
                newHeight = (int)Height;
                ratio = (decimal)Height / orgBMP.Height;
                decimal temp = orgBMP.Width * ratio;
                newWidth = (int)temp;
            }
            else
            {
                newWidth = (int)Width;
                ratio = (decimal)Width / orgBMP.Width;
                decimal temp = orgBMP.Height * ratio;
                newHeight = (int)temp;
            }

            bmpOut = new Bitmap(Width, Height);
            g = Graphics.FromImage(bmpOut);

            int x = (Width - newWidth) / 2;
            int y = (Height - newHeight) / 2;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, Width, Height);
            g.DrawImage(orgBMP, x, y, newWidth, newHeight);

            context.HttpContext.Response.ContentType = "image/jpeg";
            bmpOut.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
        }
    }
}