using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core.CottageCore;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.CottageModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.CottageControllers
{
    public class CottageGeneralFacilitiesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<CottageGeneralFacility> _generalFacilityRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public CottageGeneralFacilitiesController(
            IUnitOfWork unitOfWork,
            IRepository<CottageGeneralFacility> generalFacilityRepository,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _generalFacilityRepository = generalFacilityRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public CottageGeneralFacilityModel FillModel(CottageGeneralFacilityModel model)
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
            return View(FillModel(new CottageGeneralFacilityModel()));
        }


        [HttpPost]
        public IActionResult Create(CottageGeneralFacilityModel model, IFormFile formFile)
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

                    _UnitOfWork.CottageGeneralFacilityRepository.Insert(model.ToEntity());
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
            CottageGeneralFacility facility = _generalFacilityRepository.GetById(id);
            if (facility == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المرافق العامة";
            return View(FillModel(facility.ToModel()));
        }


        [HttpPost]
        public IActionResult Edit(CottageGeneralFacilityModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.CottageGeneralFacilityRepository.Update(model.ToEntity());
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
            CottageGeneralFacility facility = _generalFacilityRepository.GetById(id);
            if (facility == null)
                return Json("السجل غير معرف");

            // التحقق من وجود ارتباطات
            var hasRelations = _UnitOfWork.CottageGeneralFacilityRepository.Table
                .Any(f => f.Id == id);

            if (hasRelations)
            {
                return Json("لا يمكن حذف هذا المرفق لأنه مرتبط بأنشطة رياضية");
            }

            _UnitOfWork.CottageGeneralFacilityRepository.Delete(facility);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
