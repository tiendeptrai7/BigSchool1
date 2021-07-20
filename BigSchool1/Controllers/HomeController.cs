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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext context = new BigSchoolContext();
            var upcommingCourse = context.Courses.Where(x => x.DateTime > DateTime.Now).OrderBy(x => x.DateTime).ToList();
            //lấy user login hiện tại
            var userID = User.Identity.GetUserId();
            foreach (Course item in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(item.LecturerId);

                item.Name = user.Name;

                //lấy ds tham gia khóa học
                if (userID != null)

                {
                    item.isLogin = true;
                    //ktra user đó chưa tham gia khóa học

                    Attendance find = context.Attendances.FirstOrDefault(p =>

                    p.CourseId == item.Id && p.Attendee == userID);
                    if (find == null)
                        item.isShowGoing = true;
                    //ktra user đã theo dõi giảng viên của khóa học ?

                    Following findFollow = context.Followings.FirstOrDefault(p =>

                    p.FollowerId == userID && p.FolloweeId == item.LecturerId);

                    if (findFollow == null)
                        item.isShowFollow = true;
                }

            }
            return View(upcommingCourse);
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
    }
}