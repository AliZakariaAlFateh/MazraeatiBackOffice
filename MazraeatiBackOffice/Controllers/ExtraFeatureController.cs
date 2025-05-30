using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class ExtraFeatureController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Lookup> _LookupRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        public ExtraFeatureController(IRepository<Lookup> lookupRepository, IRepository<LookupValue> lookupValueRepository, IUnitOfWork unitOfWork)
        {
            _LookupRepository = lookupRepository;
            _LookupValueRepository = lookupValueRepository;
            _UnitOfWork = unitOfWork;
        }

        public LookupValueModel NewFillModel(LookupValueModel model, int lookupId)
        {
            string LookupCode = _LookupRepository.Table.FirstOrDefault(l => l.Id == lookupId).LookupCode;

            model.LookupId = lookupId;
            model.LookupDesc = (LookupCode == "FarmerExtraFeatureType" ? "مزايا المزرعة" : "مزايا الاقسام الاخرى");
            return model;
        }

        public LookupValueModel EditFillModel(LookupValueModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _LookupRepository.Table.Where(a => a.LookupCode == "FarmerExtraFeatureType" || a.LookupCode == "TripExtraFeatureType").Select(a => a.ToModel());
            ViewBag.activePage = "خدمات اخرى";
            return View(model);
        }

        #region : Extra Feature Details 

        public IActionResult ExtraFeatureDetailsIndex(int lookupId)
        {
            string LookupCode = _LookupRepository.Table.FirstOrDefault(l => l.Id == lookupId).LookupCode;
            var model = _LookupValueRepository.Table.Where(t => t.LookupId == lookupId).OrderByDescending(a => a.Id).Select(c => c.ToModel(LookupCode));
            ViewBag.activePage = " خدمات اخرى - تفاصيل ";
            ViewBag.lookupId = lookupId;
            return View(model);
        }

        [HttpPost]
        public IActionResult ExtraFeatureDetailsIndex(int lookupId, string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = lookupId });

            string LookupCode = _LookupRepository.Table.FirstOrDefault(l => l.Id == lookupId).LookupCode;

            var model = _LookupValueRepository.Table.Where(t => t.LookupId == lookupId).OrderByDescending(a => a.Id)
                .Where(a => a.ValueAr.Contains(search) || a.ValueEn.Contains(search)).Select(c => c.ToModel(LookupCode));

            ViewBag.activePage = " خدمات اخرى - تفاصيل ";
            ViewBag.search = search;
            ViewBag.lookupId = lookupId;
            return View(model);
        }

        public IActionResult Create(int lookupId)
        {
            ViewBag.activePage = " خدمات اخرى - تفاصيل ";
            return View(NewFillModel(new LookupValueModel(), lookupId));
        }

        [HttpPost]
        public IActionResult Create(LookupValueModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {

                    if (_LookupValueRepository.Table.Count(t => t.Code == model.Code) > 0)
                    {
                        ErrorNotification("الرمز موجود مسبقا");
                        return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = model.LookupId });
                    }
                        

                    _UnitOfWork.LookupValueRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();

                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = model.LookupId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving Trip , please contact to administrator");
                logFile.LogCustomInfo("Create LookupValue - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create LookupValue - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create LookupValue - Inner Exception Message ", e.InnerException.ToString());
            }
            return View(NewFillModel(new LookupValueModel(), model.LookupId));
        }

        public IActionResult Edit(int id)
        {
            
            LookupValue LookupValue = _UnitOfWork.LookupValueRepository.GetById(id);
            if (LookupValue == null)
                return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = LookupValue.LookupId });


            ViewBag.activePage = " خدمات اخرى - تفاصيل ";

            string LookupCode = _LookupRepository.Table.FirstOrDefault(l => l.Id == LookupValue.LookupId).LookupCode;
            return View(EditFillModel(LookupValue.ToModel(LookupCode)));
        }

        [HttpPost]
        public IActionResult Edit(LookupValueModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (_LookupValueRepository.Table.Count(t => t.Code == model.Code & t.Id != model.Id) > 0)
                    {
                        ErrorNotification("الرمز موجود مسبقا");
                        return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = model.LookupId });
                    }

                    _UnitOfWork.LookupValueRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("ExtraFeatureDetailsIndex", new { lookupId = model.LookupId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving trip , please contact to administrator");
            }
            return View(model);
        }


        #endregion


    }
}
