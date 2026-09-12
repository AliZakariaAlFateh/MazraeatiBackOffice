using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.SportCore;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.SportModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.SportControllers
{
    public class GeneralFacilitiesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<GeneralFacility> _generalFacilityRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public GeneralFacilitiesController(
            IUnitOfWork unitOfWork,
            IRepository<GeneralFacility> generalFacilityRepository,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _generalFacilityRepository = generalFacilityRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public GeneralFacilityModel FillModel(GeneralFacilityModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _generalFacilityRepository.Table
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel());
            ViewBag.activePage = "المرافق العامة";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _generalFacilityRepository.Table
                .OrderByDescending(a => a.Id)
                .Where(a =>
                    a.FacilityTextAr.Contains(search) ||
                    a.FacilityTextEn.Contains(search))
                .Select(c => c.ToModel());
            ViewBag.activePage = "المرافق العامة";
            ViewBag.search = search;
            return View(model);
        }


        public IActionResult Create()
        {
            ViewBag.activePage = "المرافق العامة";
            return View(FillModel(new GeneralFacilityModel()));
        }


        [HttpPost]
        public IActionResult Create(GeneralFacilityModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // التحقق من عدم وجود مرفق بنفس الاسم
                    int facilityCount = _generalFacilityRepository.Table
                        .Where(a => a.FacilityTextAr == model.FacilityTextAr)
                        .Count();

                    if (facilityCount > 0)
                    {
                        ErrorNotification("هذا المرفق موجود مسبقاً");
                        return View(FillModel(model));
                    }

                    _UnitOfWork.GeneralFacilityRepository.Insert(model.ToEntity());
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
            GeneralFacility facility = _generalFacilityRepository.GetById(id);
            if (facility == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المرافق العامة";
            return View(FillModel(facility.ToModel()));
        }


        [HttpPost]
        public IActionResult Edit(GeneralFacilityModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.GeneralFacilityRepository.Update(model.ToEntity());
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
            GeneralFacility facility = _generalFacilityRepository.GetById(id);
            if (facility == null)
                return Json("السجل غير معرف");

            // التحقق من وجود ارتباطات
            var hasRelations = _UnitOfWork.SportGeneralFacilityRepository.Table
                .Any(f => f.GeneralFacilityId == id);

            if (hasRelations)
            {
                return Json("لا يمكن حذف هذا المرفق لأنه مرتبط بأنشطة رياضية");
            }

            _UnitOfWork.GeneralFacilityRepository.Delete(facility);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
