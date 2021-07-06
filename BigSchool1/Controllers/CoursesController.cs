using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigSchool1.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        [HttpGet]
        public ActionResult Create()
        {
            // GET LIST CATEGORY
            BigChoolContext context = new BigChoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigChoolContext context = new BigChoolContext();

            //khong xet valid lecttureid vi bang user dang nhap
            ModelState.Remove("LecturerId");
            if(!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }    
            // lay login user id
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            //ad vao csdl
            context.Courses.Add(objCourse);
            context.SaveChanges();
            //tro ve home
            return RedirectToAction("Index", "Home");
        }
    }
}