using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class LoyaltyBookingActivitiesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public LoyaltyBookingActivitiesController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index()
        {
            ViewBag.activePage = "ربط الحجوزات";

            var model = _UnitOfWork.LoyaltyBookingActivityRepository.Table
                .Include(b => b.ActivityType)
                .OrderBy(b => b.BookingType)
                .Select(c => c.ToModel())
                .ToList();

            return View(model);
        }

        // ============================================================
        // CREATE
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "ربط الحجوزات";
            ViewBag.ActivityTypes = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                .Where(a => a.IsActive == true)
                .OrderBy(a => a.NameAr)
                .ToList();

            return View(new LoyaltyBookingActivityModel());
        }

        [HttpPost]
        public IActionResult Create(LoyaltyBookingActivityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var exist = _UnitOfWork.LoyaltyBookingActivityRepository.Table
                        .Any(b => b.BookingType == model.BookingType && b.ActivityTypeId == model.ActivityTypeId);

                    if (exist)
                    {
                        ErrorNotification("هذا الربط موجود بالفعل");
                        ViewBag.ActivityTypes = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                            .Where(a => a.IsActive == true)
                            .OrderBy(a => a.NameAr)
                            .ToList();
                        return View(model);
                    }

                    var entity = model.ToEntity();

                    _UnitOfWork.LoyaltyBookingActivityRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة الربط بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.ActivityTypes = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                .Where(a => a.IsActive == true)
                .OrderBy(a => a.NameAr)
                .ToList();
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
                var entity = _UnitOfWork.LoyaltyBookingActivityRepository.GetById(id);
                if (entity == null)
                    return Json(new { success = false, message = "الربط غير موجود" });

                _UnitOfWork.LoyaltyBookingActivityRepository.Delete(entity);
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
