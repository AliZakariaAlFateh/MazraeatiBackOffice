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
    public class AdditionalServicesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<AdditionalService> _additionalServiceRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public AdditionalServicesController(
            IUnitOfWork unitOfWork,
            IRepository<AdditionalService> additionalServiceRepository,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _additionalServiceRepository = additionalServiceRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        // ============================================================
        // FillModel (مثل GeneralFacilities بالضبط)
        // ============================================================
        public AdditionalServiceModel FillModel(AdditionalServiceModel model)
        {
            return model;
        }

        // ============================================================
        // INDEX - GET (مثل GeneralFacilities بالضبط)
        // ============================================================
        public IActionResult Index()
        {
            var model = _additionalServiceRepository.Table
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel());
            ViewBag.activePage = "الخدمات الإضافية";
            return View(model);
        }

        // ============================================================
        // INDEX - POST (مثل GeneralFacilities بالضبط)
        // ============================================================
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _additionalServiceRepository.Table
                .OrderByDescending(a => a.Id)
                .Where(a =>
                    a.ServiceTextAr.Contains(search) ||
                    a.ServiceTextEn.Contains(search))
                .Select(c => c.ToModel());
            ViewBag.activePage = "الخدمات الإضافية";
            ViewBag.search = search;
            return View(model);
        }

        // ============================================================
        // CREATE - GET (مثل GeneralFacilities بالضبط)
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "الخدمات الإضافية";
            return View(FillModel(new AdditionalServiceModel()));
        }

        // ============================================================
        // CREATE - POST (مثل GeneralFacilities بالضبط)
        // ============================================================
        [HttpPost]
        public IActionResult Create(AdditionalServiceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // التحقق من عدم وجود خدمة بنفس الاسم
                    int serviceCount = _additionalServiceRepository.Table
                        .Where(a => a.ServiceTextAr == model.ServiceTextAr)
                        .Count();

                    if (serviceCount > 0)
                    {
                        ErrorNotification("هذه الخدمة موجودة مسبقاً");
                        return View(FillModel(model));
                    }

                    _UnitOfWork.AdditionalServiceRepository.Insert(model.ToEntity());
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
        // EDIT - GET (مثل GeneralFacilities بالضبط)
        // ============================================================
        public IActionResult Edit(int id)
        {
            AdditionalService service = _additionalServiceRepository.GetById(id);
            if (service == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "الخدمات الإضافية";
            return View(FillModel(service.ToModel()));
        }

        // ============================================================
        // EDIT - POST (مثل GeneralFacilities بالضبط)
        // ============================================================
        [HttpPost]
        public IActionResult Edit(AdditionalServiceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.AdditionalServiceRepository.Update(model.ToEntity());
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
        // DELETE (مثل GeneralFacilities بالضبط)
        // ============================================================
        public IActionResult Delete(int id)
        {
            AdditionalService service = _additionalServiceRepository.GetById(id);
            if (service == null)
                return Json("السجل غير معرف");

            // التحقق من وجود ارتباطات
            var hasRelations = _UnitOfWork.SportAdditionalServiceRepository.Table
                .Any(f => f.AdditionalServiceId == id);

            if (hasRelations)
            {
                return Json("لا يمكن حذف هذه الخدمة لأنها مرتبطة بأنشطة رياضية");
            }

            _UnitOfWork.AdditionalServiceRepository.Delete(service);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
