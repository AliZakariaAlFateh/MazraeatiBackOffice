using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class LoyaltyPointRulesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public LoyaltyPointRulesController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX - عرض جميع الأنشطة (كـ Cards)
        // ============================================================
        public IActionResult Index()
        {
            ViewBag.activePage = "قواعد النقاط";

            var model = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                .Where(a => a.IsActive == true)
                .OrderBy(a => a.NameAr)
                .ToList();

            return View(model);
        }

        // ============================================================
        // SPORT RULES - عرض قواعد النقاط لنشاط معين
        // ============================================================
        public IActionResult SportRules(int activityTypeId)
        {
            if (activityTypeId <= 0)
            {
                ErrorNotification("يرجى اختيار نوع النشاط أولاً");
                return RedirectToAction("Index");
            }

            var activity = _UnitOfWork.LoyaltyActivityTypeRepository.GetById(activityTypeId);
            if (activity == null)
            {
                ErrorNotification("نوع النشاط غير موجود");
                return RedirectToAction("Index");
            }

            ViewBag.activePage = "قواعد النقاط";
            ViewBag.ActivityTypeName = activity.NameAr;
            ViewBag.ActivityTypeId = activityTypeId;

            // تحديد نوع المرجع
            //string referenceType = activityTypeId == 1 ? "Farm" : "Sport";
            //ViewBag.ReferenceType = referenceType;

            // ===== تحديد نوع المرجع =====
            string referenceType = activity.ReferenceTable ?? "Sports";  //  مرن جداً
            ViewBag.ReferenceType = referenceType;
            // ===== جلب العقارات حسب الـ ReferenceTable =====
            if (referenceType == "Farmer")
            {
                ViewBag.Properties = _UnitOfWork.FarmerRepository.Table
                    .OrderBy(f => f.Name)
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Name
                    })
                    .ToList();
            }
            else if (referenceType == "Sports")
            {
                var sportTypeId = activity.SportTypeId;
                ViewBag.Properties = _UnitOfWork.SportRepository.Table
                    .Where(s => s.SportTypeId == sportTypeId && s.IsActive == true)
                    .OrderBy(s => s.NameAr)
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.NameAr
                    })
                    .ToList();
            }
            else if (referenceType == "Restaurants")
            {
                // ✅ مستقبلي: جلب المطاعم من جدول Restaurants
                // ViewBag.Properties = _UnitOfWork.RestaurantRepository.Table ...
            }
            else if (referenceType == "Hotels")
            {
                // ✅ مستقبلي: جلب الفنادق من جدول Hotels
                // ViewBag.Properties = _UnitOfWork.HotelRepository.Table ...
            }

            // جلب القواعد
            var rules = _UnitOfWork.LoyaltyPointRuleRepository.Table
                .Where(r => r.ActivityTypeId == activityTypeId && r.ReferenceType == referenceType)
                .OrderByDescending(r => r.Id)
                .ToList();

            var model = rules.Select(r => r.ToModel()).ToList();

            // ربط الأسماء
            foreach (var rule in model)
            {
                if (rule.ReferenceId.HasValue)
                {
                    if (referenceType == "Farmer")
                    {
                        var farm = _UnitOfWork.FarmerRepository.GetById(rule.ReferenceId.Value);
                        rule.ReferenceName = farm?.Name ?? "";
                    }
                    else
                    {
                        var sport = _UnitOfWork.SportRepository.GetById(rule.ReferenceId.Value);
                        rule.ReferenceName = sport?.NameAr ?? "";
                    }
                }
            }

            return View(model);
        }

        // ============================================================
        // SPORT RULES - POST (إضافة قاعدة جديدة)
        // ============================================================
        //[HttpPost]
        //public IActionResult SportRules(LoyaltyPointRuleModel model)
        //{
        //    try
        //    {
        //        if (model.ActivityTypeId <= 0)
        //        {
        //            ErrorNotification("نوع النشاط مطلوب");
        //            return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            var exist = _UnitOfWork.LoyaltyPointRuleRepository.Table
        //                .Any(r => r.ActivityTypeId == model.ActivityTypeId &&
        //                          r.ReferenceType == model.ReferenceType &&
        //                          r.ReferenceId == model.ReferenceId &&
        //                          r.IsActive == true);

        //            if (exist)
        //            {
        //                ErrorNotification("توجد قاعدة بالفعل لهذا العقار");
        //                return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
        //            }

        //            var entity = model.ToEntity();
        //            entity.CreatedDate = DateTime.Now;

        //            _UnitOfWork.LoyaltyPointRuleRepository.Insert(entity);
        //            _UnitOfWork.Save();

        //            SuccessNotification("تم إضافة القاعدة بنجاح");
        //            return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification(e.Message);
        //    }

        //    return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
        //}


        [HttpPost]
        public IActionResult SportRules(LoyaltyPointRuleModel model)
        {
            try
            {
                if (model.ActivityTypeId <= 0)
                {
                    ErrorNotification("نوع النشاط مطلوب");
                    return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
                }

                if (ModelState.IsValid)
                {
                    var exist = _UnitOfWork.LoyaltyPointRuleRepository.Table
                        .Any(r => r.ActivityTypeId == model.ActivityTypeId &&
                                  r.ReferenceType == model.ReferenceType &&
                                  r.ReferenceId == model.ReferenceId &&
                                  r.IsActive == true);

                    if (exist)
                    {
                        ErrorNotification("توجد قاعدة بالفعل لهذا العقار");
                        return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
                    }

                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;

                    // ===== توليد الكود تلقائياً =====
                    if (string.IsNullOrEmpty(entity.Code))
                    {
                        var activity = _UnitOfWork.LoyaltyActivityTypeRepository.GetById(model.ActivityTypeId);
                        entity.Code = activity?.Code ?? entity.ReferenceType + "_" + entity.ActivityTypeId;
                    }

                    _UnitOfWork.LoyaltyPointRuleRepository.Insert(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم إضافة القاعدة بنجاح");
                    return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            return RedirectToAction("SportRules", new { activityTypeId = model.ActivityTypeId });
        }


        // ============================================================
        // DELETE RULE
        // ============================================================
        [HttpPost]
        public IActionResult DeleteRule(int id)
        {
            try
            {
                var rule = _UnitOfWork.LoyaltyPointRuleRepository.GetById(id);
                if (rule == null)
                    return Json(new { success = false, message = "القاعدة غير موجودة" });

                _UnitOfWork.LoyaltyPointRuleRepository.Delete(rule);
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