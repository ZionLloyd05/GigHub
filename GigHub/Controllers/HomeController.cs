﻿using GigHub.Core.ViewModels;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }


        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var upcomingGigs = _unitOfWork.Gigs.GetUpcomingGigs();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
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