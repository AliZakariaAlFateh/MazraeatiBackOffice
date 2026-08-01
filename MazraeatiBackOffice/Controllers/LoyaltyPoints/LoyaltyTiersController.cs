using DocumentFormat.OpenXml.Office2010.Excel;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class LoyaltyTiersController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public LoyaltyTiersController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index()
        {
            ViewBag.activePage = "مستويات العملاء";

            var model = _UnitOfWork.LoyaltyTierRepository.Table
                .OrderBy(a => a.MinPoints)
                .Select(c => c.ToModel())
                .ToList();

            return View(model);
        }

        // ============================================================
        // CREATE
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "مستويات العملاء";
            return View(new LoyaltyTierModel());
        }

        [HttpPost]
        public IActionResult Create(LoyaltyTierModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exist = _UnitOfWork.LoyaltyTierRepository.Table
                        .Any(t => t.MinPoints == model.MinPoints);

                    if (exist)
                    {
                        ErrorNotification("يوجد مستوى بنفس عدد النقاط");
                        return View(model);
                    }

                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;

                    _UnitOfWork.LoyaltyTierRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة المستوى بنجاح");
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
            var entity = _UnitOfWork.LoyaltyTierRepository.GetById(id);
            if (entity == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "مستويات العملاء";
            return View(entity.ToModel());
        }

        [HttpPost]
        public IActionResult Edit(LoyaltyTierModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CreateDate = _UnitOfWork.LoyaltyTierRepository.GetById(model.Id).CreatedDate;
                    var entity = model.ToEntity();
                    entity.CreatedDate = CreateDate;
                    _UnitOfWork.LoyaltyTierRepository.Update(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث المستوى بنجاح");
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
                var entity = _UnitOfWork.LoyaltyTierRepository.GetById(id);
                if (entity == null)
                    return Json(new { success = false, message = "المستوى غير موجود" });

                var hasAccounts = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                    .Any(a => a.CurrentTierId == id);

                if (hasAccounts)
                {
                    return Json(new { success = false, message = "لا يمكن الحذف لوجود عملاء مرتبطين بهذا المستوى" });
                }

                _UnitOfWork.LoyaltyTierRepository.Delete(entity);
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
