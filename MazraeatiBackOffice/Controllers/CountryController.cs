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
    public class CountryController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Country> _countryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public CountryController(IUnitOfWork unitOfWork,IRepository<Country> countryRepository, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _countryRepository = countryRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public CountryModel FillModel(CountryModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _countryRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel());
            ViewBag.activePage = "المدن";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _countryRepository.Table.OrderByDescending(a => a.Id).Where(a => a.DescEn.Contains(search) || a.DescAr.Contains(search) || a.Code.Contains(search)).Select(c => c.ToModel());
            ViewBag.activePage = "المدن";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "المدن";
            return View(FillModel(new CountryModel()));
        }

        [HttpPost]
        public IActionResult Create(CountryModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int countryCount = _countryRepository.Table.Where(a => a.Code == model.Code).Count();
                    if (countryCount > 0)
                    {
                        ErrorNotification("رمز البلد مستخدم مسبقا");
                        return View(FillModel(model));
                    }

                    _UnitOfWork.CountryRepository.Insert(model.ToEntity());
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
            Country country = _countryRepository.GetById(id);
            if (country == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المدن";
            return View(FillModel(country.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CountryModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.CountryRepository.Update(model.ToEntity());
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
            Country country = _countryRepository.GetById(id);
            if (country == null)
                return Json("السجل غير معرف");

            _UnitOfWork.CountryRepository.Delete(country);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
