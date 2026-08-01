using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class SportPropertyTemplatesController:BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<SportPropertyTemplate> _templateRepository;
        private readonly IRepository<SportPropertyOption> _optionRepository;
        private readonly IRepository<SportType> _sportTypeRepository;

        public SportPropertyTemplatesController(
            IUnitOfWork unitOfWork,
            IRepository<SportPropertyTemplate> templateRepository,
            IRepository<SportPropertyOption> optionRepository,
            IRepository<SportType> sportTypeRepository)
        {
            _UnitOfWork = unitOfWork;
            _templateRepository = templateRepository;
            _optionRepository = optionRepository;
            _sportTypeRepository = sportTypeRepository;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index(int? sportTypeId)
        {
            ViewBag.activePage = "قوالب تفاصيل العقار";
            ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();

            var query = _templateRepository.Table.AsQueryable();

            if (sportTypeId.HasValue && sportTypeId.Value > 0)
            {
                query = query.Where(t => t.SportTypeId == sportTypeId.Value);
                var sportType = _sportTypeRepository.GetById(sportTypeId.Value);
                ViewBag.SelectedSportTypeName = sportType?.NameAr;
            }

            var model = query.OrderBy(t => t.SortOrder).ToList();
            ViewBag.SelectedSportTypeId = sportTypeId;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var sportTypeId = string.IsNullOrEmpty(form["sportTypeId"]) ? (int?)null : int.Parse(form["sportTypeId"]);
            return RedirectToAction("Index", new { sportTypeId });
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create(int? sportTypeId)
        {
            ViewBag.activePage = "قوالب تفاصيل العقار";
            ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();

            var model = new SportPropertyTemplate
            {
                IsActive = true,
                SportTypeId = sportTypeId ?? 0
            };
            return View(model);
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create(SportPropertyTemplate model, string[] optionTextAr, string[] optionTextEn, string[] optionValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;

                    _UnitOfWork.SportPropertyTemplateRepository.Insert(model);
                    _UnitOfWork.Save();

                    // ===== إضافة الخيارات (لو كان Dropdown أو RadioButton) =====
                    if ((model.PropertyType == PropertyTypeEnum.Dropdown || model.PropertyType == PropertyTypeEnum.RadioButton) && optionTextAr != null)
                    {
                        for (int i = 0; i < optionTextAr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(optionTextAr[i]))
                            {
                                _UnitOfWork.SportPropertyOptionRepository.Insert(new SportPropertyOption
                                {
                                    PropertyTemplateId = model.Id,
                                    OptionValue = optionValue?[i] ?? optionTextAr[i],
                                    OptionTextAr = optionTextAr[i],
                                    OptionTextEn = optionTextEn?[i],
                                    SortOrder = i + 1,
                                    IsActive = true
                                });
                            }
                        }
                        _UnitOfWork.Save();
                    }

                    SuccessNotification("تم إضافة القالب بنجاح");
                    return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            return View(model);
        }

        // ============================================================
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            var template = _templateRepository.GetById(id);
            if (template == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "قوالب تفاصيل العقار";
            ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();

            // جلب الخيارات
            ViewBag.Options = _optionRepository.Table
                .Where(o => o.PropertyTemplateId == id && o.IsActive == true)
                .OrderBy(o => o.SortOrder)
                .ToList();

            return View(template);
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        //[HttpPost]
        //public IActionResult Edit(SportPropertyTemplate model, string[] optionTextAr, string[] optionTextEn, string[] optionValue, int[] optionId)
        //{
        //    LogFile logFile = new LogFile();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            model.ModifiedDate = DateTime.Now;
        //            _UnitOfWork.SportPropertyTemplateRepository.Update(model);
        //            _UnitOfWork.Save();

        //            // ===== تحديث الخيارات =====
        //            if ((model.PropertyType == PropertyTypeEnum.Dropdown || model.PropertyType == PropertyTypeEnum.RadioButton) && optionTextAr != null)
        //            {
        //                // حذف الخيارات القديمة
        //                var oldOptions = _optionRepository.Table
        //                    .Where(o => o.PropertyTemplateId == model.Id)
        //                    .ToList();
        //                foreach (var old in oldOptions)
        //                {
        //                    _UnitOfWork.SportPropertyOptionRepository.Delete(old);
        //                }

        //                // إضافة الخيارات الجديدة
        //                for (int i = 0; i < optionTextAr.Length; i++)
        //                {
        //                    if (!string.IsNullOrEmpty(optionTextAr[i]))
        //                    {
        //                        _UnitOfWork.SportPropertyOptionRepository.Insert(new SportPropertyOption
        //                        {
        //                            PropertyTemplateId = model.Id,
        //                            OptionValue = optionValue?[i] ?? optionTextAr[i],
        //                            OptionTextAr = optionTextAr[i],
        //                            OptionTextEn = optionTextEn?[i],
        //                            SortOrder = i + 1,
        //                            IsActive = true
        //                        });
        //                    }
        //                }
        //                _UnitOfWork.Save();
        //            }

        //            SuccessNotification("تم تحديث القالب بنجاح");
        //            return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification(e.Message);
        //        if (e.InnerException != null)
        //            logFile.LogCustomInfo("Edit SportPropertyTemplates  - Inner Exception Message ", e.InnerException.ToString());
        //    }

        //    ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
        //    ViewBag.Options = _optionRepository.Table
        //        .Where(o => o.PropertyTemplateId == model.Id && o.IsActive == true)
        //        .OrderBy(o => o.SortOrder)
        //        .ToList();
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult Edit(SportPropertyTemplate model, string[] optionTextAr, string[] optionTextEn, string[] optionValue, int[] optionId)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    model.ModifiedDate = DateTime.Now;
                    _UnitOfWork.SportPropertyTemplateRepository.Update(model);
                    _UnitOfWork.Save();

                    // ===== تحديث الخيارات =====
                    if ((model.PropertyType == PropertyTypeEnum.Dropdown || model.PropertyType == PropertyTypeEnum.RadioButton) && optionTextAr != null)
                    {
                        // 1. جلب الخيارات الموجودة
                        var existingOptions = _optionRepository.Table
                            .Where(o => o.PropertyTemplateId == model.Id)
                            .OrderBy(o => o.SortOrder)
                            .ToList();

                        // 2. قائمة الخيارات الجديدة (اللي جايه من الشاشة)
                        var newOptionTexts = optionTextAr.Where(t => !string.IsNullOrEmpty(t)).ToList();
                        var newOptionValues = optionValue?.Where(v => !string.IsNullOrEmpty(v)).ToList() ?? new List<string>();
                        var newOptionEn = optionTextEn?.Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();

                        // 3. معرفة الخيارات المحذوفة (الموجودة في القديم ومش موجودة في الجديد)
                        var deletedOptions = existingOptions
                            .Where(o => !newOptionTexts.Contains(o.OptionTextAr))
                            .ToList();

                        // 4. التحقق من الخيارات المحذوفة
                        foreach (var option in deletedOptions)
                        {
                            // التحقق من وجود ارتباطات
                            var hasRelations = _UnitOfWork.SportPropertyValueRepository.Table
                                .Any(v => v.ValueOptionId == option.Id);

                            if (hasRelations)
                            {
                                // 🔴 لا يمكن الحذف - مربوط برياضة
                                ErrorNotification($"لا يمكن حذف الخيار '{option.OptionTextAr}' لأنه مربوط بأنشطة رياضية");
                                ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
                                ViewBag.Options = existingOptions;
                                return View(model);
                            }
                            else
                            {
                                // ✅ يمكن الحذف - مش مربوط
                                _UnitOfWork.SportPropertyOptionRepository.Delete(option);
                            }
                        }

                        // 5. تحديث الخيارات الموجودة
                        var remainingOptions = existingOptions
                            .Where(o => !deletedOptions.Contains(o))
                            .ToList();

                        int index = 0;
                        for (int i = 0; i < optionTextAr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(optionTextAr[i]))
                            {
                                if (index < remainingOptions.Count)
                                {
                                    // تحديث الخيار الموجود
                                    var option = remainingOptions[index];
                                    option.OptionValue = optionValue?[i] ?? optionTextAr[i];
                                    option.OptionTextAr = optionTextAr[i];
                                    option.OptionTextEn = optionTextEn?[i];
                                    option.SortOrder = index + 1;
                                    _UnitOfWork.SportPropertyOptionRepository.Update(option);
                                }
                                else
                                {
                                    // إضافة خيار جديد
                                    _UnitOfWork.SportPropertyOptionRepository.Insert(new SportPropertyOption
                                    {
                                        PropertyTemplateId = model.Id,
                                        OptionValue = optionValue?[i] ?? optionTextAr[i],
                                        OptionTextAr = optionTextAr[i],
                                        OptionTextEn = optionTextEn?[i],
                                        SortOrder = index + 1,
                                        IsActive = true
                                    });
                                }
                                index++;
                            }
                        }

                        _UnitOfWork.Save();
                    }

                    SuccessNotification("تم تحديث القالب بنجاح");
                    return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Edit SportPropertyTemplates  - Inner Exception Message ", e.InnerException.ToString());
            }

            ViewBag.SportTypes = _sportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            ViewBag.Options = _optionRepository.Table
                .Where(o => o.PropertyTemplateId == model.Id && o.IsActive == true)
                .OrderBy(o => o.SortOrder)
                .ToList();
            return View(model);
        }



        // ============================================================
        // DELETE
        // ============================================================
        //[HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var template = _templateRepository.GetById(id);
                if (template == null)
                    return Json(new { success = false, message = "القالب غير موجود" });

                // التحقق من وجود قيم مرتبطة
                var hasValues = _UnitOfWork.SportPropertyValueRepository.Table
                    .Any(v => v.PropertyTemplateId == id);

                if (hasValues)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذا القالب لأنه مستخدم في بيانات" });
                }

                _UnitOfWork.SportPropertyTemplateRepository.Delete(template);
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
