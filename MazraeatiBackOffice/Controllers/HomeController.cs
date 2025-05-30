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

        public HomeController(IRepository<Farmer> FarmerRepository, IRepository<Trip> TripRepository, IRepository<TripProfile> TripProfileRepository,
            IRepository<FarmerReservation> ReservationFarmerRepository , IWebHostEnvironment hostEnvironment)
        {
            _FarmerRepository = FarmerRepository;
            _TripRepository = TripRepository;
            _TripProfileRepository = TripProfileRepository;
            _ReservationFarmerRepository = ReservationFarmerRepository;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.TotalVIPFarmer = _FarmerRepository.Table.Count(s => s.IsVIP == true);
            ViewBag.TotalTrustFarmer = _FarmerRepository.Table.Count(s => s.IsTrust == true);
            ViewBag.TotalFarms = _FarmerRepository.Table.Count();
            ViewBag.TotalTrip = _TripRepository.Table.Count();
            /*ViewBag.TotalOrganizeProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 1);
            ViewBag.TotalInternalExternalProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 2);
            ViewBag.TotalTwoOrganizeProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 5);
            ViewBag.TotalRentCarProfile = _TripProfileRepository.Table.Count(t => t.TypeId == 6);
            ViewBag.TotalEntermentProfile = _TripRepository.Table.Count(t => t.TypeId == 7);
            ViewBag.TotalReservationByCurrentMonth = _ReservationFarmerRepository.Table.Count(f => f.ReservationDate.Month == DateTime.Now.Month);
            ViewBag.TotalReservationByPreviousMonth = _ReservationFarmerRepository.Table.Count(f => f.ReservationDate.Month == (DateTime.Now.Month - 1));*/
            return View();
        }

    }
}
