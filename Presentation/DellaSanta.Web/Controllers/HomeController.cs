using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DellaSanta.Core;
using DellaSanta.Models;
using DellaSanta.Services;

namespace DellaSanta.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(
           IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Student, Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Student")]
        public async Task<ActionResult> CourseEnrollment()
        {
            ViewBag.Message = "Course Enrollment.";

            var identity = (ClaimsIdentity)User.Identity;
            var path = identity.Claims.Where(x => x.Type == "SelectedPath").Select(x => x.Value).First();
            var sid = identity.Claims.Where(x => x.Type == ClaimTypes.Sid).Select(x => x.Value).First();
            var courses = await _userService.GetCoursesAsync(path);
            var listcourses = courses.Select(x => new SelectListItem { Value = x.CourseId.ToString(), Text = x.CourseName }).ToList();

            return View(new CourseEnrollmentViewModel { UserId = sid,  Courses = listcourses });

        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CourseEnrollment(CourseEnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = (ClaimsIdentity)User.Identity;
                int sid = int.Parse(identity.Claims.Where(x => x.Type == ClaimTypes.Sid).Select(x => x.Value).First());

                EnrolledClass myClass = new EnrolledClass { CourseId = int.Parse(model.CourseId), StudentId = sid };
                var res = await _userService.AddClassAsync(myClass);

                if (res > 0)
                {
                    return Json(true);
                    //return View(model);
                    //return RedirectToAction("Index", "Home");
                }

                //NOTA questo funziona solo se NON è ajax!!!
                //else
                //{
                //    TempData["MessageToClient"] = "Course not enrolled. Please retry in 5 minutes or contact the administrator.";
                //    ModelState.AddModelError("", "Course not enrolled. Please retry in 5 minutes or contact the administrator.");
                //}
                    

            }

            // If we got this far, something failed, redisplay form
            //return View(model);
            return Json(false);
        }

    }
}