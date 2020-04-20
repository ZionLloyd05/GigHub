using GigHub.Models;
using GigHub.Persistence;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{

    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
                       
            var viewModel = new GigsViewModel
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenres()
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

            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();

            return RedirectToAction("index", "Home");
        }
        
        public ActionResult Detail(int gigId)
        {
            if (gigId < 1)
                return RedirectToAction("index", "Home");

            var viewModel = new GigsDetailViewModel();
            Gig gig = _unitOfWork.Gigs.GetUserGigithArtistAndGenre(gigId);

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