using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class LoyaltyRedeemRulesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public LoyaltyRedeemRulesController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index()
        {
            ViewBag.activePage = "قواعد الصرف";

            var model = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                .OrderBy(r => r.Points)
                .Select(c => c.ToModel())
                .ToList();

            return View(model);
        }

        // ============================================================
        // CREATE
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "قواعد الصرف";
            return View(new LoyaltyRedeemRuleModel());
        }

        [HttpPost]
        public IActionResult Create(LoyaltyRedeemRuleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exist = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                        .Any(r => r.Points == model.Points);

                    if (exist)
                    {
                        ErrorNotification("توجد قاعدة بنفس عدد النقاط");
                        return View(model);
                    }

                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;

                    _UnitOfWork.LoyaltyRedeemRuleRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة قاعدة الصرف بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            return View(model);
        }

        // ============================================================
        // EDIT
        // ============================================================
        public IActionResult Edit(int id)
        {
            var entity = _UnitOfWork.LoyaltyRedeemRuleRepository.GetById(id);
            if (entity == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "قواعد الصرف";
            return View(entity.ToModel());
        }

        [HttpPost]
        public IActionResult Edit(LoyaltyRedeemRuleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = model.ToEntity();

                    _UnitOfWork.LoyaltyRedeemRuleRepository.Update(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث قاعدة الصرف بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

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
                var entity = _UnitOfWork.LoyaltyRedeemRuleRepository.GetById(id);
                if (entity == null)
                    return Json(new { success = false, message = "القاعدة غير موجودة" });

                _UnitOfWork.LoyaltyRedeemRuleRepository.Delete(entity);
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
