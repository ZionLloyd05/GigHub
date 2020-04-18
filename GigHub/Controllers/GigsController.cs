using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{

    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var userGigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = userGigs,
                ShowActions = false,
                Heading = "Gigs I'm Attending!",
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("index", "Home");
        }

        public ActionResult Detail(int? gigId)
        {
            if (gigId == null)
                return RedirectToAction("index", "Home");

            var viewModel = new GigsDetailViewModel();

            var gig = _context.Gigs
              .Where(g => g.Id == gigId)
              .Include(g => g.Artist)
              .Include(g => g.Genre)
              .FirstOrDefault();
            
            viewModel.Gig = gig;

            if (User.Identity.IsAuthenticated)
            {
              
                var userId = User.Identity.GetUserId();

                var isUserFollowingGigArtist = _context.Followings
                    .Any(
                        f => f.FollowerId == userId
                        && f.FolloweeId == gig.ArtistId
                    );

                var isUserAttending = _context.Attendances
                    .Any(
                        a => a.GigId == gig.Id
                        && a.AttendeeId == userId
                    );
                
                viewModel.IsAttendingGig = isUserAttending;
                viewModel.IsFollowingGigArtist = isUserFollowingGigArtist;
                
            }
            
            return View(viewModel);
        }
    }
}