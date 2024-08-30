using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToDoApi.Controllers
{
    public class TodoRepository : Controller
    {
        // GET: TodoRepository
        public ActionResult Index()
        {
            return View();
        }
    }
}