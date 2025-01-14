﻿using Bll;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class LogController : BaseController
    {
        LogBll bll = new LogBll();
        // GET: Admin/Log
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogList() 
        {
            List<LogDTO> dto = new List<LogDTO>();
            dto = bll.GetAllLog();
            return View(dto);
        }
    }
}