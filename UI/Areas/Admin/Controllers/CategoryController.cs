﻿using Bll;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        CategoryBll bll = new CategoryBll();
        // GET: Admin/Category
        public ActionResult AddCategory()
        {
            CategoryDTO dto = new CategoryDTO();
            return View(dto);
        }
        [HttpPost]
        public ActionResult AddCategory(CategoryDTO model) 
        {
            if (ModelState.IsValid)
            {
                if (bll.AddCategory(model))
                {
                    ViewBag.ProcessState =General.Messages.Addsuccess;
                    ModelState.Clear();
                    model = new CategoryDTO();
                }
                else
                {
                    ViewBag.ProcessState = General.Messages.GeneralError;
                }
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public ActionResult CategoryList() 
        {
            List<CategoryDTO> ls = new List<CategoryDTO>();
            ls = bll.GetCategory();
            return View(ls);
        }
        public ActionResult UpdateCategory(int ID) 
        {
            CategoryDTO dto = new CategoryDTO();
            dto = bll.GetCategoryWithID(ID);

            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateCategory(CategoryDTO model) 
        {
            if (ModelState.IsValid)
            {
                if (bll.UpdateCategory(model))
                {
                    ViewBag.ProcessState = General.Messages.UpdateSuccess;
                }
                else
                    ViewBag.ProcessState = General.Messages.GeneralError;
            }
            else
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            return View(model);
        }
        public JsonResult DeleteCategory(int ID) 
        {
            List<PostImageDTO> imgList=bll.DeleteCategory(ID);
            foreach (var item in imgList)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Areas/Admin/Content/PostImage/" + item.ImagePath)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Content/PostImage/" + item.ImagePath));
                }
            }
            return Json("");
        }
    }
}