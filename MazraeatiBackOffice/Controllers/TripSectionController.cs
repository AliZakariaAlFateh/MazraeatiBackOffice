using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Configuration;

namespace MazraeatiBackOffice.Controllers
{
    public class TripSectionController : BaseController
    {
        private readonly IRepository<Country> _CountryRepository;
        private readonly IRepository<TripSection> _TripSectionRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public TripSectionController(IRepository<Country> CountryRepository , IRepository<TripSection> TripSectionRepository ,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _CountryRepository = CountryRepository;
            _TripSectionRepository = TripSectionRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public TripSectionModel NewFillModel(TripSectionModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            return model;
        }

        public TripSectionModel EditFillModel(TripSectionModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            return model;
        }

        public IActionResult Index()
        {
            var Countries = _CountryRepository.Table.ToList();
            var model = _TripSectionRepository.Table.OrderByDescending(a => a.OrderId).Select(c => c.ToModel(Countries));
            ViewBag.activePage = "اقسام الخدمات الاخرى";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var Countries = _CountryRepository.Table.ToList();
            var model = _TripSectionRepository.Table.OrderByDescending(a => a.OrderId).Where(a => a.Title.Contains(search)).Select(c => c.ToModel(Countries));
            ViewBag.activePage = "اقسام الخدمات الاخرى";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "اقسام الخدمات الاخرى";
            return View(NewFillModel(new TripSectionModel()));
        }

        [HttpPost]
        public IActionResult Create(TripSectionModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.FileMainImage != null)
                        model.MainImage = "tripSection/" + GenericFunction.UploadedFile(model.FileMainImage, webHostEnvironment, "tripSection");

                    _UnitOfWork.TripSectionRepository.Insert(model.ToEntity());
                    SuccessNotification("تم اضافة السجل بنجاح");
                    _UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving Trip Section , please contact to administrator");
            }
            return View(NewFillModel(new TripSectionModel()));
        }

        public IActionResult Edit(int id)
        {
            TripSection trip = _UnitOfWork.TripSectionRepository.GetById(id);
            if (trip == null)
                return RedirectToAction("Index");


            ViewBag.activePage = "اقسام الخدمات الاخرى";
            return View(EditFillModel(trip.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(TripSectionModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.FileMainImage != null)
                        model.MainImage = "tripSection/" + GenericFunction.UploadedFile(model.FileMainImage, webHostEnvironment, "tripSection");

                    _UnitOfWork.TripSectionRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving trip section , please contact to administrator");
            }
            return View(model);
        }
    }
}
