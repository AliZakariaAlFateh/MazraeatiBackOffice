using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MazraeatiBackOffice.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class HomeController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<Trip> _TripRepository;
        private readonly IRepository<TripProfile> _TripProfileRepository;
        private readonly IRepository<FarmerReservation> _ReservationFarmerRepository;
        private readonly IRepository<NotificationsFarm> _NotificationsFarm;
        private readonly TimeCacheService _timeCache;
        public HomeController(IRepository<Farmer> FarmerRepository, IRepository<Trip> TripRepository
            , IRepository<TripProfile> TripProfileRepository,IRepository<FarmerReservation> ReservationFarmerRepository
            , IWebHostEnvironment hostEnvironment, TimeCacheService timeCache, IRepository<NotificationsFarm> notificationsFarm)
        {
            _FarmerRepository = FarmerRepository;
            _TripRepository = TripRepository;
            _TripProfileRepository = TripProfileRepository;
            _ReservationFarmerRepository = ReservationFarmerRepository;
            webHostEnvironment = hostEnvironment;
            _timeCache = timeCache;
            _NotificationsFarm = notificationsFarm;
        }

        public IActionResult Index()
        {
            // Get previous and next from cache
            var result = _timeCache.GetNextAndPrevious();

            // Define beginning of today (1:00 AM) and tomorrow (1:00 AM next day)
            var todayStart = DateTime.Today.AddHours(1);     // today at 1:00 AM
            var tomorrowStart = todayStart.AddDays(1);       // tomorrow at 1:00 AM

            // If previous is null, set it to the beginning of today (1:00 AM)
            var previous = result.previous ?? todayStart;

            // If next is invalid, fallback to now
            var next = result.next == DateTime.MinValue ? DateTime.Now : result.next;

            // Query notifications between 'previous' and 'next', but only for today (1 AM → next 1 AM)
            var notifications = _NotificationsFarm.Table
                .AsNoTracking()
                .AsEnumerable() // move to memory-safe filtering
                .Where(n =>
                    n.TimeDate >= previous &&
                    n.TimeDate <= next &&
                    n.TimeDate >= todayStart &&
                    n.TimeDate < tomorrowStart)
                .Select(n => new NotificationsFarm
                {
                    FarmName = n.FarmName,
                    LocationDescription = n.LocationDescription,
                    TimeDate = n.TimeDate
                })
                .ToList();
            var allNotifications = _NotificationsFarm.Table.Select(N => N).ToList();
            var today = DateTime.Today;
            var firstThis = new DateTime(today.Year, today.Month, 1);
            var firstNext = firstThis.AddMonths(1);
            var firstPrev = firstThis.AddMonths(-1);

            ViewBag.TotalVIPFarmer = _FarmerRepository.Table.Count(s => s.IsVIP == true);
            ViewBag.TotalTrustFarmer = _FarmerRepository.Table.Count(s => s.IsTrust == true);
            ViewBag.TotalFarms = _FarmerRepository.Table.Count();
            ViewBag.TotalTrip = _TripRepository.Table.Count();
            // Current month
            var currentCount = _ReservationFarmerRepository.Table
                .Count(r => r.ReservationDate >= firstThis && r.ReservationDate < firstNext);

            // Immediate previous month
            var prevCount = _ReservationFarmerRepository.Table
                .Count(r => r.ReservationDate >= firstPrev && r.ReservationDate < firstThis);
            /*ViewBag.TotalOrganizeProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 1);
            ViewBag.TotalInternalExternalProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 2);
            ViewBag.TotalTwoOrganizeProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 5);
            ViewBag.TotalRentCarProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 6);
            ViewBag.TotalEntermentProfile = _TripRepository.Table.Count(t => t.TypeId == 7);*/
            ViewBag.TotalReservation = _ReservationFarmerRepository.Table.Count();
            ViewBag.TotalReservationByCurrentMonth = currentCount; //_ReservationFarmerRepository.Table.Count(f => f.ReservationDate.Month == DateTime.Now.Month);
            ViewBag.TotalReservationByPreviousMonth = prevCount;//_ReservationFarmerRepository.Table.Count(f => f.ReservationDate.Month == (DateTime.Now.Month - 1));
            return View();
        }

        public IActionResult ReturnNotifications()
        {
            var result = _timeCache.GetNextAndPrevious();
            var allNotifications = _NotificationsFarm.Table.Select(N=>N);
            return Ok(new
            {
                Previous = result.previous?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null",
                Next = result.next.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

    }
}
