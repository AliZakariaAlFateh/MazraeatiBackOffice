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
    public class OnboardingController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Onboarding> _OnboardingRepository;
        private readonly IRepository<Country> _CountryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public OnboardingController(IRepository<Onboarding> onboardingRepository, IRepository<Country> countryRepository, IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _OnboardingRepository = onboardingRepository;
            _CountryRepository = countryRepository;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public OnboardingModel FillModel(OnboardingModel model)
        {
            model.Countries = _CountryRepository.Table.ToList();
            model.ExpiryDate = DateTime.Now.AddMonths(1);
            return model;
        }

        public IActionResult Index()
        {
            List<Country> countries = _CountryRepository.Table.ToList();
            var model = _OnboardingRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel(countries));
            ViewBag.activePage = "onBoarding";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            List<Country> countries = _CountryRepository.Table.ToList();
            var model = _OnboardingRepository.Table.OrderByDescending(a => a.Id).Where(a => a.ExtraText.Contains(search)).Select(c => c.ToModel(countries));
            ViewBag.activePage = "onBoarding";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "onBoarding";
            return View(FillModel(new OnboardingModel()));
        }

        [HttpPost]
        public IActionResult Create(OnboardingModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.Url = "onboarding/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "onboarding");

                    model.Type = 1;
                    _UnitOfWork.OnboardingRepository.Insert(model.ToEntity());
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
            Onboarding onboarding = _OnboardingRepository.GetById(id);
            if (onboarding == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "onBoarding";
            return View(FillModel(onboarding.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(OnboardingModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.Url = "onboarding/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "onboarding");

                    _UnitOfWork.OnboardingRepository.Update(model.ToEntity());
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
            Onboarding Onboarding = _OnboardingRepository.GetById(id);
            if (Onboarding == null)
                return Json("السجل غير معرف");

            _UnitOfWork.OnboardingRepository.Delete(Onboarding);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
