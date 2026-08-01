using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class SportTypesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<SportType> _sportTypeRepository;

        public SportTypesController(
            IUnitOfWork unitOfWork,
            IRepository<SportType> sportTypeRepository)
        {
            _UnitOfWork = unitOfWork;
            _sportTypeRepository = sportTypeRepository;
        }

        // ============================================================
        // FillModel
        // ============================================================
        public SportTypeModel FillModel(SportTypeModel model)
        {
            return model;
        }

        // ============================================================
        // INDEX - GET
        // ============================================================
        public IActionResult Index()
        {
            var model = _sportTypeRepository.Table
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel());
            ViewBag.activePage = "أنواع الرياضات";
            return View(model);
        }

        // ============================================================
        // INDEX - POST (بحث)
        // ============================================================
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _sportTypeRepository.Table
                .OrderByDescending(a => a.Id)
                .Where(a =>
                    a.NameAr.Contains(search) ||
                    a.NameEn.Contains(search))
                .Select(c => c.ToModel());
            ViewBag.activePage = "أنواع الرياضات";
            ViewBag.search = search;
            return View(model);
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "أنواع الرياضات";
            return View(FillModel(new SportTypeModel()));
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create(SportTypeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int count = _sportTypeRepository.Table
                        .Where(a => a.NameAr == model.NameAr)
                        .Count();

                    if (count > 0)
                    {
                        ErrorNotification("هذا النوع موجود مسبقاً");
                        return View(FillModel(model));
                    }

                    _UnitOfWork.SportTypeRepository.Insert(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم إضافة نوع الرياضة بنجاح");
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
            var sportType = _sportTypeRepository.GetById(id);
            if (sportType == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "أنواع الرياضات";
            return View(FillModel(sportType.ToModel()));
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        [HttpPost]
        public IActionResult Edit(SportTypeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.SportTypeRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث نوع الرياضة بنجاح");
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
            var sportType = _sportTypeRepository.GetById(id);
            if (sportType == null)
                return Json("السجل غير معرف");

            // التحقق من وجود أنشطة رياضية تابعة لهذا النوع
            var hasSports = _UnitOfWork.SportRepository.Table
                .Any(s => s.SportTypeId == id);

            if (hasSports)
            {
                return Json("لا يمكن حذف هذا النوع لأنه مرتبط بأنشطة رياضية موجودة");
            }

            // التحقق من وجود مرفقات خاصة تابعة لهذا النوع
            var hasFeatures = _UnitOfWork.SportFeatureRepository.Table
                .Any(f => f.SportTypeId == id);

            if (hasFeatures)
            {
                return Json("لا يمكن حذف هذا النوع لأنه يحتوي على مرفقات خاصة");
            }

            _UnitOfWork.SportTypeRepository.Delete(sportType);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
