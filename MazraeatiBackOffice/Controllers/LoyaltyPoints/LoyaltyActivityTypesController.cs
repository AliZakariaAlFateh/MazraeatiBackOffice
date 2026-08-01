using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class LoyaltyActivityTypesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public LoyaltyActivityTypesController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index(string search)
        {
            ViewBag.activePage = "أنواع الأنشطة";

            var query = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                .OrderByDescending(a => a.Id).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.NameAr.Contains(search) || a.NameEn.Contains(search) || a.Code.Contains(search));
                ViewBag.search = search;
            }

            var model = query.Select(c => c.ToModel()).ToList();
            return View(model);
        }

        // ============================================================
        // CREATE
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "أنواع الأنشطة";
            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            return View(new LoyaltyActivityTypeModel());
        }

        [HttpPost]
        public IActionResult Create(LoyaltyActivityTypeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exist = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                        .Any(a => a.Code == model.Code);

                    if (exist)
                    {
                        ErrorNotification("هذا الكود مستخدم مسبقاً");
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
                        return View(model);
                    }

                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;

                    _UnitOfWork.LoyaltyActivityTypeRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة النشاط بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            return View(model);
        }

        // ============================================================
        // EDIT
        // ============================================================
        public IActionResult Edit(int id)
        {
            var entity = _UnitOfWork.LoyaltyActivityTypeRepository.GetById(id);
            if (entity == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "أنواع الأنشطة";
            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            return View(entity.ToModel());
        }

        [HttpPost]
        public IActionResult Edit(LoyaltyActivityTypeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();
                    entity.ModifiedDate = DateTime.Now;
                    entity.CreatedDate = _UnitOfWork.LoyaltyActivityTypeRepository.GetById(model.Id).CreatedDate;
                    _UnitOfWork.LoyaltyActivityTypeRepository.Update(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث النشاط بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            return View(model);
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var entity = _UnitOfWork.LoyaltyActivityTypeRepository.GetById(id);
                if (entity == null)
                    return Json(new { success = false, message = "النشاط غير موجود" });

                var hasRules = _UnitOfWork.LoyaltyPointRuleRepository.Table
                    .Any(r => r.ActivityTypeId == id);

                if (hasRules)
                {
                    return Json(new { success = false, message = "لا يمكن الحذف لوجود قواعد نقاط مرتبطة" });
                }

                _UnitOfWork.LoyaltyActivityTypeRepository.Delete(entity);
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
