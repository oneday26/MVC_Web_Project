﻿using Bll;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        UserBll bll = new UserBll();
        // GET: Admin/User
        public ActionResult UserList() 
        {
            List<UserDTO> user = new List<UserDTO>();
            user = bll.GetUser();
            return View(user);
        }
        public ActionResult Adduser()
        {
            UserDTO model = new UserDTO();
            return View(model);
        }
        [HttpPost]
        public ActionResult Adduser(UserDTO model) 
        {
            if (model.UserImage==null)
            {
                ViewBag.ProcessState = General.Messages.ImageMissing;
            }
            else if (ModelState.IsValid)
            {
                string filename = "";
                HttpPostedFileBase postedFile = model.UserImage;
                Bitmap UserImage = new Bitmap(postedFile.InputStream);
                Bitmap Imageresize = new Bitmap(UserImage,128,128);
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext==".jpg" || ext==".jpeg" || ext==".png" || ext==".gif")
                {
                    string uniquenumber = Guid.NewGuid().ToString();
                    filename = uniquenumber + postedFile.FileName;

                    Imageresize.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + filename));
                    model.ImagePath = filename;
                    if (bll.AddUser(model))
                    {
                        ViewBag.ProcessState = General.Messages.Addsuccess;
                        model = new UserDTO();
                        ModelState.Clear();
                    }
                    else
                        ViewBag.ProcessState = General.Messages.GeneralError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public ActionResult UpdateUser(int ID) 
        {
            UserDTO user = new UserDTO();
            user = bll.GetUserWithID(ID);
            return View(user);
        }
        [HttpPost]
        public ActionResult UpdateUser(UserDTO model) 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else if (model.UserImage!=null)
            {
                string filename = "";
                HttpPostedFileBase postedFile = model.UserImage;
                Bitmap UserImage = new Bitmap(postedFile.InputStream);
                Bitmap Imageresize = new Bitmap(UserImage, 128, 128);
                string ext = Path.GetExtension(postedFile.FileName);
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                {
                    string uniquenumber = Guid.NewGuid().ToString();
                    filename = uniquenumber + postedFile.FileName;

                    Imageresize.Save(Server.MapPath("~/Areas/Admin/Content/UserImage/" + filename));
                    model.ImagePath = filename;
                }
                string oldimage = bll.UpdateUser(model);
                if (model.ImagePath!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Areas/Admin/Content/UserImage/" + oldimage)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Content/UserImage/" + oldimage));
                    }
                }
                ViewBag.ProcessState = General.Messages.UpdateSuccess;
            }
            return View(model);
        }
        public JsonResult DeleteUser(int ID) 
        {
            string imgpath = bll.DeleteUser(ID);
            if (System.IO.File.Exists(Server.MapPath("~/Areas/Admin/Content/UserImage/" + imgpath)))
            {
                System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Content/UserImage/" + imgpath));
            }
            return Json("");
        }
    }
}