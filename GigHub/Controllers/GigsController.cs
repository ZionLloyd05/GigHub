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
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
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
                var isUserFollowingGigArtist = false;
                var isUserAttending = false;

                var userId = User.Identity.GetUserId();

                var followingInDb = _unitOfWork.Follows.GetUserFollow(gig.ArtistId, userId);

                var attendanceInDb = _unitOfWork.Attendances.GetAttendancesForGig(gig.Id, userId);

                if (followingInDb != null)
                    isUserFollowingGigArtist = true;

                if (attendanceInDb != null)
                    isUserAttending = true;
                                
                viewModel.IsAttendingGig = isUserAttending;
                viewModel.IsFollowingGigArtist = isUserFollowingGigArtist;

            }

            return View(viewModel);
        }
              
    }
}