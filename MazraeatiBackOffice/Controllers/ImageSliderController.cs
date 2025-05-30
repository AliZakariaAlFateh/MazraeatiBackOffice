using MazraeatiBackOffice;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Controllers;
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
    public class ImageSliderController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<ImageSlider> _ImageSliderRepository;
        private readonly IRepository<Country> _CountryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public ImageSliderController(IRepository<ImageSlider> imageSliderRepository, IRepository<Country> countryRepository , IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _ImageSliderRepository = imageSliderRepository;
            _CountryRepository = countryRepository;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public ImageSliderModel FillModel(ImageSliderModel model)
        {
            model.Countries = _CountryRepository.Table.ToList();
            return model;
        }

        public IActionResult Index()
        {
            List<Country> countries = _CountryRepository.Table.ToList();
            var model = _ImageSliderRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel(countries));
            ViewBag.activePage = "الاعلانات";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            List<Country> countries = _CountryRepository.Table.ToList();
            var model = _ImageSliderRepository.Table.OrderByDescending(a => a.Id).Where(a => a.ExtraText.Contains(search)).Select(c => c.ToModel(countries));
            ViewBag.activePage = "الاعلانات";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "الاعلانات";
            return View(FillModel(new ImageSliderModel()));
        }

        [HttpPost]
        public IActionResult Create(ImageSliderModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.Image = "imageSlider/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "imageSlider");

                    _UnitOfWork.ImageSliderRepository.Insert(model.ToEntity());
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
            ImageSlider imageSlider = _ImageSliderRepository.GetById(id);
            if (imageSlider == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "الاعلانات";
            return View(FillModel(imageSlider.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(ImageSliderModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.Image = "imageSlider/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "imageSlider");

                    _UnitOfWork.ImageSliderRepository.Update(model.ToEntity());
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
            if (id <= 6)
            {
                return Json("لا يمكن حذف السجل لانه يعتبر افترضيا ");
            }

            ImageSlider ImageSlider = _ImageSliderRepository.GetById(id);
            if (ImageSlider == null)
                return Json("السجل غير معرف");

            _UnitOfWork.ImageSliderRepository.Delete(ImageSlider);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
