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
    public class FarmerReservationController : BaseController
    {
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<FarmerReservation> _FarmerReservationRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FarmerReservationController(IRepository<Farmer> farmerRepository,IRepository<FarmerReservation> farmerReservationRepository, IRepository<LookupValue> LookupValueRepository,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _FarmerRepository = farmerRepository;
            _FarmerReservationRepository = farmerReservationRepository;
            _LookupValueRepository = LookupValueRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public FarmerReservationModel NewFillModel(FarmerReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l=>l.LookupId == 6).ToList();
            return model;
        }

        public FarmerReservationModel EditFillModel(FarmerReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            return model;
        }

        public IActionResult Index(int farmerId)
        {
            Farmer farmer = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);
            List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            var model = _FarmerReservationRepository.Table.Where(t=>t.FarmerId == farmerId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            ViewBag.activePage = "حجوزات المزارع";
            ViewBag.FarmerName = farmer.Name;
            ViewBag.FarmerId = farmerId;
            ViewBag.YearId = DateTime.Now.Year;
            ViewBag.MonthId = DateTime.Now.Month;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int farmerId, string search, int MonthId, int YearId)
        {
            List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            IQueryable<FarmerReservationModel> model;
            if (string.IsNullOrEmpty(search))
            {
                model = _FarmerReservationRepository.Table.Where(t => t.FarmerId == farmerId && t.ReservationDate.Month == MonthId &&
                                                                      t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            }
            else
            {
                model = _FarmerReservationRepository.Table.Where(t => (t.CustMobNum.Contains(search) ||
                                                                                        t.CustomerName.Contains(search) ||
                                                                                        t.Note.Contains(search)) && t.FarmerId == farmerId && t.ReservationDate.Month == MonthId &&
                                                                      t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
            }
          
            
            ViewBag.activePage = "حجوزات المزارع";
            ViewBag.search = search;
            ViewBag.FarmerId = farmerId;
            ViewBag.YearId = YearId;
            ViewBag.MonthId = MonthId;
            return View(model);
        }

        public IActionResult Create(int farmerId)
        {
            ViewBag.activePage = "حجوزات المزارع";
            FarmerReservationModel farmer = new FarmerReservationModel();
            farmer.FarmerId = farmerId;
            farmer.ReservationDate = DateTime.Now;
            return View(NewFillModel(farmer));
        }

        [HttpPost]
        public IActionResult Create(FarmerReservationModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    if (_FarmerReservationRepository.Table.Where(f => f.ReservationDate.Date == model.ReservationDate.Date && f.ReservationTypeId == model.ReservationTypeId && f.FarmerId == model.FarmerId).Count() > 0)
                    {
                        ErrorNotification(" يوجد حجز في نفس اليوم ونفس الفترة");
                        model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                        return View(model);
                    }

                    model.CreatedDate = DateTime.Now;
                    model.AutomaticallyNote = "تم الحجز من قبل محجوز";
                    _UnitOfWork.FarmerReservationRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index", new { farmerId = model.FarmerId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving  , please contact to administrator");
            }
            return View(NewFillModel(new FarmerReservationModel()));
        }

        public IActionResult Edit(int id)
        {
            FarmerReservation reservation = _UnitOfWork.FarmerReservationRepository.GetById(id);
            if (reservation == null)
                return RedirectToAction("Index",new { farmerId = 0 });


            ViewBag.activePage = "حجوزات المزارع";
            return View(EditFillModel(reservation.ToModel(null)));
        }

        [HttpPost]
        public IActionResult Edit(FarmerReservationModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_FarmerReservationRepository.Table.Where(f => f.ReservationDate.Date == model.ReservationDate.Date && f.FarmerId == model.FarmerId && f.ReservationTypeId == model.ReservationTypeId && f.Id != model.Id).Count() > 0)
                    {
                        ErrorNotification(" يوجد حجز في نفس اليوم ونفس الفترة");
                        model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                        return View(model);
                    }

                    model.AutomaticallyNote = "تم الحجز من قبل محجوز";
                    _UnitOfWork.FarmerReservationRepository.Update(model.ToEntity());
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
            FarmerReservation FarmerReservation = _FarmerReservationRepository.GetById(id);
            if (FarmerReservation == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.FarmerReservationRepository.Delete(FarmerReservation);
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
