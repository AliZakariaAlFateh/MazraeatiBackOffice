using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Configuration;

namespace MazraeatiBackOffice.Controllers
{
    public class FarmerFeedBackController : BaseController
    {
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<FarmerFeedback> _FarmerFeedbackRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FarmerFeedBackController(IRepository<Farmer> farmerRepository,IRepository<FarmerFeedback> farmerFeedbackRepository, IRepository<LookupValue> LookupValueRepository,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _FarmerRepository = farmerRepository;
            _FarmerFeedbackRepository = farmerFeedbackRepository;
            _LookupValueRepository = LookupValueRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public FarmerFeedbackModel NewFillModel(FarmerFeedbackModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l=>l.LookupId == 7).ToList();
            return model;
        }

        public FarmerFeedbackModel EditFillModel(FarmerFeedbackModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 7).ToList();
            return model;
        }

        public IActionResult Index(int farmerId)
        {
            Farmer farmer = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);
            List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 7).ToList().Prepend(new LookupValue() { Id = 0, LookupId = 7, Code = "All", ValueAr = "جميع التقييمات", ValueEn = "All" }).ToList();
            var model = _FarmerFeedbackRepository.Table.Where(t=>t.FarmerId == farmerId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            ViewBag.activePage = "ملاحظات المزرعة";
            ViewBag.FarmerName = farmer.Name;
            ViewBag.FarmerId = farmerId;
            ViewBag.FeedbackList = lookupValues;
            ViewBag.FeedbackId = 0;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int farmerId, string search, int FeedbackId)
        {
            search = string.IsNullOrEmpty(search) ? "" : search;
            List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 7).ToList().Prepend(new LookupValue() { Id = 0, LookupId = 7, Code = "All", ValueAr = "جميع التقييمات", ValueEn = "All" }).ToList();
            IQueryable<FarmerFeedbackModel> model;
            if (string.IsNullOrEmpty(search))
            {
                model = _FarmerFeedbackRepository.Table.Where(t => t.FarmerId == farmerId && (FeedbackId == 0 || (FeedbackId > 0 && t.FeedbackId == FeedbackId))).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            }
            else
            {
                model = _FarmerFeedbackRepository.Table.Where(t => t.FarmerId == farmerId && (t.Note.Contains(search)) && (FeedbackId == 0 || (FeedbackId > 0 && t.FeedbackId == FeedbackId))).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            }
           

            ViewBag.activePage = "ملاحظات المزرعة";
            ViewBag.search = search;
            ViewBag.FarmerId = farmerId;
            ViewBag.FeedbackList = lookupValues;
            ViewBag.FeedbackId = FeedbackId;
            return View(model);
        }

        public IActionResult Create(int farmerId)
        {
            ViewBag.activePage = "ملاحظات المزرعة";
            FarmerFeedbackModel farmer = new FarmerFeedbackModel();
            farmer.FarmerId = farmerId;
            return View(NewFillModel(farmer));
        }

        [HttpPost]
        public IActionResult Create(FarmerFeedbackModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;
                    _UnitOfWork.FarmerFeedbackRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index", new { farmerId = model.FarmerId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving  , please contact to administrator");
            }
            return View(NewFillModel(new FarmerFeedbackModel()));
        }

        public IActionResult Edit(int id)
        {
            FarmerFeedback farmerFeedback = _UnitOfWork.FarmerFeedbackRepository.GetById(id);
            if (farmerFeedback == null)
                return RedirectToAction("Index",new { farmerId = 0 });


            ViewBag.activePage = "ملاحظات المزرعة";
            return View(EditFillModel(farmerFeedback.ToModel(null)));
        }

        [HttpPost]
        public IActionResult Edit(FarmerFeedbackModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.FarmerFeedbackRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index", new { farmerId = model.FarmerId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving  , please contact to administrator");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            string result = "1";
            FarmerFeedback farmerFeedback = _FarmerFeedbackRepository.GetById(id);
            if (farmerFeedback == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.FarmerFeedbackRepository.Delete(farmerFeedback);
                _UnitOfWork.Save();
                SuccessNotification("Delete Succesfuly");
            }
            catch (Exception)
            {
                result = "There is data associated with this record";
            }

            return Json(result);
        }

    }
}
