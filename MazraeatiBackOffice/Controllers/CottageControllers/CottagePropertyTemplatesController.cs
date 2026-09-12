using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Configuration.Enums;
using MazraeatiBackOffice.Core.CottageCore;
using MazraeatiBackOffice.Core.SportCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.CottageControllers
{
    //تفاصيل العقار
    public class CottagePropertyTemplatesController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<CottagePropertyTemplate> _templateRepository;
        private readonly IRepository<CottagePropertyOption> _optionRepository;

        public CottagePropertyTemplatesController(
            IUnitOfWork unitOfWork,
            IRepository<CottagePropertyTemplate> templateRepository,
            IRepository<CottagePropertyOption> optionRepository)
        {
            _UnitOfWork = unitOfWork;
            _templateRepository = templateRepository;
            _optionRepository = optionRepository;
        }

        // ============================================================
        // INDEX
        // ============================================================
        public IActionResult Index()
        {
            ViewBag.activePage = "قوالب تفاصيل العقار";

            var query = _templateRepository.Table.AsQueryable();

            var model = query.OrderBy(t => t.SortOrder).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            return RedirectToAction("Index");
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create()
        {
            ViewBag.activePage = "قوالب تفاصيل العقار للأكواخ";

            var model = new  CottagePropertyTemplate
            {
                IsActive = true
            };
            return View(model);
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create(CottagePropertyTemplate model, string[] optionTextAr, string[] optionTextEn, string[] optionValue)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;

                    _UnitOfWork.CottagePropertyTemplateRepository.Insert(model);
                    _UnitOfWork.Save();

                    // ===== إضافة الخيارات (لو كان Dropdown أو RadioButton) =====
                    if ((model.PropertyType == PropertyTypeEnum.Dropdown || model.PropertyType == PropertyTypeEnum.RadioButton) && optionTextAr != null)
                    {
                        for (int i = 0; i < optionTextAr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(optionTextAr[i]))
                            {
                                _UnitOfWork.CottagePropertyOptionRepository.Insert(new CottagePropertyOption
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
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            var template = _templateRepository.GetById(id);
            if (template == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "قوالب تفاصيل العقار للأكواخ";

            // جلب الخيارات
            ViewBag.Options = _optionRepository.Table
                .Where(o => o.PropertyTemplateId == id && o.IsActive == true)
                .OrderBy(o => o.SortOrder)
                .ToList();

            return View(template);
        }


        [HttpPost]
        public IActionResult Edit(CottagePropertyTemplate model, string[] optionTextAr, string[] optionTextEn, string[] optionValue, int[] optionId)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    model.ModifiedDate = DateTime.Now;
                    _UnitOfWork.CottagePropertyTemplateRepository.Update(model);
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
                            var hasRelations = _UnitOfWork.CottagePropertyValueRepository.Table
                                .Any(v => v.ValueOptionId == option.Id);

                            if (hasRelations)
                            {
                                // 🔴 لا يمكن الحذف - مربوط برياضة
                                ErrorNotification($"لا يمكن حذف الخيار '{option.OptionTextAr}' لأنه مربوط بأنشطة رياضية");
                                ViewBag.Options = existingOptions;
                                return View(model);
                            }
                            else
                            {
                                // ✅ يمكن الحذف - مش مربوط
                                _UnitOfWork.CottagePropertyOptionRepository.Delete(option);
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
                                    _UnitOfWork.CottagePropertyOptionRepository.Update(option);
                                }
                                else
                                {
                                    // إضافة خيار جديد
                                    _UnitOfWork.CottagePropertyOptionRepository.Insert(new CottagePropertyOption
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
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Edit SportPropertyTemplates  - Inner Exception Message ", e.InnerException.ToString());
            }

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
                var hasValues = _UnitOfWork.CottagePropertyValueRepository.Table
                    .Any(v => v.PropertyTemplateId == id);

                if (hasValues)
                {
                    return Json(new { success = false, message = "لا يمكن حذف هذا القالب لأنه مستخدم في بيانات" });
                }

                _UnitOfWork.CottagePropertyTemplateRepository.Delete(template);
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
