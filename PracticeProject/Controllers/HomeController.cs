using PracticeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeProject.Controllers
{
    public class HomeController : Controller
    {
        Employee obj = new Employee();
        public ActionResult Index()
        {          
            TempData["Hobbies"] = obj.hobbylist;
            TempData["countries"]=obj.countrylist;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Save(Employee emp, FormCollection col)
        {            
            string hoby = string.Empty;
            foreach(var item in obj.hobbylist)
            {
                if (col[item].ToString().Contains("true"))
                {
                    hoby += item + ", ";
                }
            }
            obj.Hobbies = hoby;
            obj.country = emp.country;
            TempData["Hobbies"] = obj.hobbylist;
            TempData["countries"] = obj.countrylist;
            return View("Index",obj);
        }
    }
}