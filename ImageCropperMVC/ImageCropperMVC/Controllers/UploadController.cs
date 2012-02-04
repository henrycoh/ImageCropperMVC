using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ImageCropperMVC.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "attachments" 
            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var physicalPath = Path.Combine(Server.MapPath("~/Temp"), fileName);

                file.SaveAs(physicalPath);
            }
            // Return an empty string to signify success
            return Content("");
        }
        public ActionResult Remove(string[] files)
        {
            if (files != null)
            {
                // The parameter of the Remove action must be called "fileNames"
                foreach (var fullName in files)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Temp"), fileName);

                    // TODO: Verify user permissions
                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            // Return an empty string to signify success
            return Content("");
        }
    }
}
