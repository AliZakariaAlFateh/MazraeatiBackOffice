using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class SportFeaturesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<SportFeature> _sportFeatureRepository;
        private readonly IRepository<SportType> _sportTypeRepository;

        public SportFeaturesController(
            IUnitOfWork unitOfWork,
            IRepository<SportFeature> sportFeatureRepository,
            IRepository<SportType> sportTypeRepository)
        {
            _UnitOfWork = unitOfWork;
            _sportFeatureRepository = sportFeatureRepository;
            _sportTypeRepository = sportTypeRepository;
        }

        // ============================================================
        // FillModel
        // ============================================================
        public SportFeatureModel FillModel(SportFeatureModel model)
        {
            return model;
        }

        // ============================================================
        // INDEX - GET
        // ============================================================
        public IActionResult Index(int? sportTypeId, string search)
        {
            ViewBag.activePage = "المرفقات الخاصة";

            ViewBag.SportTypes = _sportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            var query = _sportFeatureRepository.Table
                //.Where(f => f.IsActive == true)
                .AsQueryable();

            if (sportTypeId.HasValue && sportTypeId.Value > 0)
            {
                query = query.Where(f => f.SportTypeId == sportTypeId.Value);
                var sportType = _sportTypeRepository.GetById(sportTypeId.Value);
                ViewBag.SelectedSportTypeName = sportType?.NameAr ?? "غير معروف";
            }
            else
            {
                ViewBag.SelectedSportTypeName = "جميع الأقسام";
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f =>
                    f.FeatureTextAr.Contains(search) ||
                    f.FeatureTextEn.Contains(search));
            }

            var model = query
                .OrderBy(f => f.FeatureTextAr)
                .Select(c => c.ToModel())
                .ToList();

            // ===== الإحصائيات =====
            var allFeatures = _sportFeatureRepository.Table
                //.Where(f => f.IsActive == true)
                .ToList();

            if (sportTypeId.HasValue && sportTypeId.Value > 0)
            {
                allFeatures = allFeatures.Where(f => f.SportTypeId == sportTypeId.Value).ToList();
            }

            ViewBag.TotalFeatures = allFeatures.Count();
            ViewBag.ActiveFeatures = allFeatures.Count(f => f.IsActive);
            ViewBag.InactiveFeatures = allFeatures.Count(f => !f.IsActive);

            ViewBag.SelectedSportTypeId = sportTypeId;
            ViewBag.search = search;

            return View(model);
        }

        // ============================================================
        // INDEX - POST
        // ============================================================
        [HttpPost]
        public IActionResult Index(IFormCollection form)  
        {
            var sportTypeId = string.IsNullOrEmpty(form["sportTypeId"]) ? (int?)null : int.Parse(form["sportTypeId"]);
            var search = form["search"];

            return RedirectToAction("Index", new
            {
                sportTypeId = sportTypeId,
                search = search
            });
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "المرفقات الخاصة";
            ViewBag.SportTypes = _sportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            var model = new SportFeatureModel
            {
                IsActive = true
            };
            return View(FillModel(model));
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create(SportFeatureModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int count = _sportFeatureRepository.Table
                        .Count(f => f.SportTypeId == model.SportTypeId && f.FeatureTextAr == model.FeatureTextAr);

                    if (count > 0)
                    {
                        ErrorNotification("هذا المرفق موجود مسبقاً لهذا القسم");
                        ViewBag.SportTypes = _sportTypeRepository.Table
                            .Where(s => s.IsActive == true)
                            .OrderBy(s => s.NameAr)
                            .ToList();
                        return View(FillModel(model));
                    }

                    var entity = model.ToEntity();
                    //entity.CreatedDate = DateTime.Now;

                    _UnitOfWork.SportFeatureRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة المرفق الخاص بنجاح");
                    return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.SportTypes = _sportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();
            return View(FillModel(model));
        }

        // ============================================================
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            var feature = _sportFeatureRepository.GetById(id);
            if (feature == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المرفقات الخاصة";
            ViewBag.SportTypes = _sportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            return View(FillModel(feature.ToModel()));
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        [HttpPost]
        public IActionResult Edit(SportFeatureModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    //entity.ModifiedDate = DateTime.Now;

                    _UnitOfWork.SportFeatureRepository.Update(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث المرفق الخاص بنجاح");
                    return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.SportTypes = _sportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();
            return View(FillModel(model));
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var feature = _sportFeatureRepository.GetById(id);
                if (feature == null)
                    return Json(new { success = false, message = "المرفق غير موجود" });

                var hasRelations = _UnitOfWork.SportSportFeatureRepository.Table
                    .Any(f => f.SportFeatureId == id);

                if (hasRelations)
                {
                    return Json(new
                    {
                        success = false,
                        message = "لا يمكن حذف هذا المرفق لأنه مرتبط بأنشطة رياضية"
                    });
                }

                int sportTypeId = feature.SportTypeId;
                _UnitOfWork.SportFeatureRepository.Delete(feature);
                _UnitOfWork.Save();

                return Json(new { success = true, message = "تم الحذف بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
