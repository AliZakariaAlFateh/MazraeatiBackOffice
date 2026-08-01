using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class SafetyFeaturesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<SafetyFeature> _safetyFeatureRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public SafetyFeaturesController(
            IUnitOfWork unitOfWork,
            IRepository<SafetyFeature> safetyFeatureRepository,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _safetyFeatureRepository = safetyFeatureRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        // ============================================================
        // FillModel (مثل GeneralFacilities بالضبط)
        // ============================================================
        public SafetyFeatureModel FillModel(SafetyFeatureModel model)
        {
            return model;
        }

        // ============================================================
        // INDEX - GET
        // ============================================================
        public IActionResult Index()
        {
            var model = _safetyFeatureRepository.Table
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel());
            ViewBag.activePage = "ميزات الأمان";
            return View(model);
        }

        // ============================================================
        // INDEX - POST
        // ============================================================
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _safetyFeatureRepository.Table
                .OrderByDescending(a => a.Id)
                .Where(a =>
                    a.FeatureTextAr.Contains(search) ||
                    a.FeatureTextEn.Contains(search))
                .Select(c => c.ToModel());
            ViewBag.activePage = "ميزات الأمان";
            ViewBag.search = search;
            return View(model);
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "ميزات الأمان";
            return View(FillModel(new SafetyFeatureModel()));
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create(SafetyFeatureModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // التحقق من عدم وجود ميزة بنفس الاسم
                    int featureCount = _safetyFeatureRepository.Table
                        .Where(a => a.FeatureTextAr == model.FeatureTextAr)
                        .Count();

                    if (featureCount > 0)
                    {
                        ErrorNotification("هذه الميزة موجودة مسبقاً");
                        return View(FillModel(model));
                    }

                    _UnitOfWork.SafetyFeatureRepository.Insert(model.ToEntity());
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

        // ============================================================
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            SafetyFeature feature = _safetyFeatureRepository.GetById(id);
            if (feature == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "ميزات الأمان";
            return View(FillModel(feature.ToModel()));
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        [HttpPost]
        public IActionResult Edit(SafetyFeatureModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.SafetyFeatureRepository.Update(model.ToEntity());
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

        // ============================================================
        // DELETE
        // ============================================================
        public IActionResult Delete(int id)
        {
            SafetyFeature feature = _safetyFeatureRepository.GetById(id);
            if (feature == null)
                return Json("السجل غير معرف");

            // التحقق من وجود ارتباطات
            var hasRelations = _UnitOfWork.SportSafetyFeatureRepository.Table
                .Any(f => f.SafetyFeatureId == id);

            if (hasRelations)
            {
                return Json("لا يمكن حذف هذه الميزة لأنها مرتبطة بأنشطة رياضية");
            }

            _UnitOfWork.SafetyFeatureRepository.Delete(feature);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
