using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;

namespace MazraeatiBackOffice.Controllers
{
    public class CityController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CityController(IUnitOfWork unitOfWork,IRepository<City> cityRepository, IRepository<Country> countryRepository, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            webHostEnvironment = hostEnvironment;
        }

        public CityModel FillCityModel(CityModel model)
        {
            model.Countries = _countryRepository.Table.Where(a => a.Active == true).ToList();
            return model;
        }

        public IActionResult Index()
        {
            var Countries = _countryRepository.Table.ToList();
            var model = _cityRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries));
            ViewBag.activePage = "المحافظات";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var Countries = _countryRepository.Table.ToList();
            var model = _cityRepository.Table.OrderByDescending(a => a.Id).Where(a => a.DescAr.Contains(search) || a.DescEn.Contains(search)).Select(c => c.ToModel(Countries));
            ViewBag.search = search;
            ViewBag.activePage = "المحافظات";
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "المحافظات";
            return View(FillCityModel(new CityModel()));
        }

        [HttpPost]
        public IActionResult Create(CityModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int cityCount = _cityRepository.Table.Where(a => a.DescAr == model.DescAr || a.DescEn == model.DescEn).Count();
                    if (cityCount > 0)
                    {
                        ErrorNotification("المحافظه مستخدمه مسبقا");
                        return View(FillCityModel(model));
                    }

                    _UnitOfWork.CityRepository.Insert(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillCityModel(model));
        }

        public IActionResult Edit(int id)
        {
            City city = _cityRepository.GetById(id);
            if (city == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المحافظات";
            return View(FillCityModel(city.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CityModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.CityRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillCityModel(model));
        }

        public IActionResult Delete(int id)
        {
            City city = _cityRepository.GetById(id);
            if (city == null)
                return Json("السجل غير معرف");

            _UnitOfWork.CityRepository.Delete(city);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
