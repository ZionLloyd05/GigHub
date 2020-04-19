using GigHub.Models;
using GigHub.Respositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{

    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
            _genreRepository = new GenreRepository(_context);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
                       
            var viewModel = new GigsViewModel
            {
                UpcomingGigs = _gigRepository.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var vm = new GigFormViewModel
            {
                Genres = _genreRepository.GetGenres()
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
        
        public ActionResult Detail(int gigId)
        {
            if (gigId < 1)
                return RedirectToAction("index", "Home");

            var viewModel = new GigsDetailViewModel();
            Gig gig = _gigRepository.GetUserGigithArtistAndGenre(gigId);

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