using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers
{
    public class TripProfileController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<TripSection> _TripSectionRepository;
        private readonly IRepository<TripProfile> _TripProfileRepository;
        private readonly IRepository<Trip> _TripRepository;
        private readonly IRepository<Country> _CountryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<TripImage> _TripImage;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public TripProfileController(IRepository<TripSection> tripSectionRepository,IRepository<TripProfile> tripProfileRepository, IRepository<Trip> tripRepository, IRepository<Country> countryRepository,
            IRepository<TripImage> tripImage, IRepository<City> cityRepository,
            IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _TripSectionRepository = tripSectionRepository;
            _TripProfileRepository = tripProfileRepository;
            _TripRepository = tripRepository;
            _CountryRepository = countryRepository;
            _cityRepository = cityRepository;
            _TripImage = tripImage;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public TripProfileModel FillModel(TripProfileModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            //model.TripSections = _TripSectionRepository.Table.Where(t => t.IsTripProfile == true && t.Active == true).ToList();
            return model;
        }

        public TripModel TripFillModel(TripModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            //model.TripSections = _TripSectionRepository.Table.Where(t => t.IsTripProfile == true && t.Active == true).ToList();
            return model;
        }

        public IActionResult Index()
        {
            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            List<TripSection> tripSections = new List<TripSection>();// _TripSectionRepository.Table.Where(s => s.IsTripProfile == true).ToList();
            var model = _TripProfileRepository.Table.Where(t => tripSections.Select(s => s.Id).Contains(t.TypeId)).OrderByDescending(a => a.Id).Select(c => c.ToModel(countries, Cities, tripSections));
            ViewBag.activePage = "انواع الخدمة / شركات";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            List<TripSection> tripSections = new List<TripSection>();//_TripSectionRepository.Table.Where(s => s.IsTripProfile == true).ToList();
            var model = _TripProfileRepository.Table.Where(t => tripSections.Select(s => s.Id).Contains(t.TypeId)).OrderByDescending(a => a.Id).Where(a => a.Name.Contains(search) || a.Description.Contains(search)).Select(c => c.ToModel(countries, Cities, tripSections));
            ViewBag.activePage = "انواع الخدمة / شركات";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "انواع الخدمة / شركات";
            return View(FillModel(new TripProfileModel()));
        }

        [HttpPost]
        public IActionResult Create(TripProfileModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "trip/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "trip");

                    _UnitOfWork.TripProfileRepository.Insert(model.ToEntity());
                    SuccessNotification("تم اضافة السجل بنجاح");
                    _UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillModel(model));
        }

        public IActionResult Edit(int id)
        {
            TripProfile tripProfile = _TripProfileRepository.GetById(id);
            if (tripProfile == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "انواع الخدمة / شركات";
            return View(FillModel(tripProfile.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(TripProfileModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "trip/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "trip");

                    _UnitOfWork.TripProfileRepository.Update(model.ToEntity());
                    SuccessNotification("تم تحديث السجل بنجاح");
                    _UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillModel(model));
        }

        public IActionResult Delete(int id)
        {

            TripProfile tripProfile = _TripProfileRepository.GetById(id);
            if (tripProfile == null)
                return Json("السجل غير معرف");

            _UnitOfWork.TripProfileRepository.Delete(tripProfile);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
