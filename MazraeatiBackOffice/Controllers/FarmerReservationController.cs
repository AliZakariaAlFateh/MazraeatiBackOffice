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
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MazraeatiBackOffice.Controllers
{
    public class FarmerReservationController : BaseController
    {
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<FarmerReservation> _FarmerReservationRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<DeviceToken> _deviceToken;
        private readonly IRepository<FarmerViewes> _farmerViewes;
        private readonly IRepository<AppUser> _appUser;
        private readonly FirebaseNotificationService _notificationService;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FarmerReservationController(IRepository<Farmer> farmerRepository,
            IRepository<FarmerReservation> farmerReservationRepository, 
            IRepository<LookupValue> LookupValueRepository, IRepository<DeviceToken> DeviceToken,
            IRepository<FarmerViewes> FarmerViewes, FirebaseNotificationService notificationService,
            IRepository<AppUser> AppUser,IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _FarmerRepository = farmerRepository;
            _FarmerReservationRepository = farmerReservationRepository;
            _LookupValueRepository = LookupValueRepository;
            _deviceToken=DeviceToken;
            _farmerViewes=FarmerViewes;
            _appUser = AppUser;
            _notificationService = notificationService;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public FarmerReservationModel NewFillModel(FarmerReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Farms = _FarmerRepository.Table.ToList();
            return model;
        }

        public FarmerReservationModel EditFillModel(FarmerReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Farms = _FarmerRepository.Table.ToList();
            return model;
        }
        public IActionResult index_Reservation()
        {
            LogFile logFile = new LogFile();

            // 1. تعريف المتغيرات بقيم افتراضية خارج الـ try لضمان وصول الـ View ليها
            List<FarmerReservationModel> model = new List<FarmerReservationModel>();
            int? farmId = null;

            try
            {
                // جلب قيم الـ Lookup
                List<LookupValue> lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6).ToList();

                // 2. جلب البيانات (استخدمنا AsEnumerable عشان ميثود ToModel تشتغل صح)
                model = _FarmerReservationRepository.Table
                        .Where(Fr => Fr.IsMahjouzReservation == false)
                        .OrderByDescending(a => a.CreatedDate)
                        .AsEnumerable()
                        .Select(x => x.ToModel(lookupValues))
                        .ToList();

                // جلب أول مزرعة
                var farm = _FarmerRepository.Table
                    .Select(f => new { f.Id, f.Name })
                    .FirstOrDefault();

                farmId = farm?.Id;
            }
            catch (Exception e)
            {
                // 3. حماية الـ Catch من الانهيار (Null Check على الـ InnerException)
                string innerMsg = e.InnerException != null ? e.InnerException.Message : "No Inner Exception";

                ErrorNotification($"Error: {e.Message}");

                logFile.LogCustomInfo("index_Reservation - Exception", e.Message);
                logFile.LogCustomInfo("index_Reservation - Inner", innerMsg);
                logFile.LogCustomInfo("index_Reservation - StackTrace", e.StackTrace);
            }

            // تعبئة الـ ViewBag من المتغيرات اللي عرفناها فوق
            ViewBag.activePage = "حجوزات المزارع";
            ViewBag.FarmId = farmId;
            ViewBag.YearId = DateTime.Now.Year;
            ViewBag.MonthId = DateTime.Now.Month;

            return View(model);
        }
        //public IActionResult index_Reservation()
        //{
        //    //var model = _FarmerReservationRepository.Table;
        //    List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
        //    var model = _FarmerReservationRepository.Table
        //                .ToList()
        //                .Select(x => x.ToModel(lookupValues))
        //                .ToList();

        //    ViewBag.activePage = "حجوزات المزارع";
        //    var farm = _FarmerRepository.Table
        //        .Select(f => new { f.Id, f.Name })
        //        .FirstOrDefault();
        //    ViewBag.FarmId = farm?.Id;
        //    ViewBag.YearId = DateTime.Now.Year;
        //    ViewBag.MonthId = DateTime.Now.Month;
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult index_Reservation(string search, int? MonthId, int? YearId, int? source)
        {
            var lookupValues = _LookupValueRepository.Table
                .Where(l => l.LookupId == 6).ToList();

            var query = _FarmerReservationRepository.Table.Where(Fr => Fr.IsMahjouzReservation == false).AsQueryable();

            if (MonthId.HasValue)
            {
                query = query.Where(t => t.ReservationDate.Month == MonthId.Value);
            }

            if (YearId.HasValue)
            {
                query = query.Where(t => t.ReservationDate.Year == YearId.Value);
            }

            if (source.HasValue)
            {
                bool isMahjouz = source == 1;
                query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
            }

            // ✅ فلترة بالبحث
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    t.CustMobNum.Contains(search) ||
                    t.CustomerName.Contains(search) ||
                    (t.Note != null && t.Note.Contains(search))
                );
            }

            var model = query
                .OrderByDescending(a => a.CreatedDate)
                .Select(c => c.ToModel(lookupValues))
                .ToList();

            ViewBag.MonthId = MonthId;
            ViewBag.YearId = YearId;
            ViewBag.search = search;
            ViewBag.source = source;

            return View(model);
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


        //public IActionResult Index(int farmerId, string search, int MonthId, int YearId)
        //{
        //    List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
        //    IQueryable<FarmerReservationModel> model;
        //    if (string.IsNullOrEmpty(search))
        //    {
        //        model = _FarmerReservationRepository.Table.Where(t => t.FarmerId == farmerId && t.ReservationDate.Month == MonthId &&
        //                                                              t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
        //    }
        //    else
        //    {
        //        model = _FarmerReservationRepository.Table.Where(t => (t.CustMobNum.Contains(search) ||
        //                                                                                t.CustomerName.Contains(search) ||
        //                                                                                t.Note.Contains(search)) && t.FarmerId == farmerId || t.ReservationDate.Month == MonthId ||
        //                                                              t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
        //    }


        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.search = search;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.YearId = YearId;
        //    ViewBag.MonthId = MonthId;
        //    return View(model);
        //}
        [HttpPost]
        public IActionResult Index(int farmerId, string search, int? MonthId, int? YearId, int? source)
        {
            var lookupValues = _LookupValueRepository.Table
                .Where(l => l.LookupId == 6).ToList();

            var query = _FarmerReservationRepository.Table
                .Where(t => t.FarmerId == farmerId);

            if (MonthId.HasValue && MonthId > 0)
            {
                query = query.Where(t => t.ReservationDate.Month == MonthId);
            }

            if (YearId.HasValue && YearId > 0)
            {
                query = query.Where(t => t.ReservationDate.Year == YearId);
            }

            if (source.HasValue && source > 0)
            {
                bool isMahjouz = (source == 1);
                query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    t.CustMobNum.Contains(search) ||
                    t.CustomerName.Contains(search) ||
                    (t.Note != null && t.Note.Contains(search))
                );
            }

          
            var model = query
                .OrderByDescending(a => a.CreatedDate)
                .Select(c => c.ToModel(lookupValues))
                .ToList();

            ViewBag.activePage = "حجوزات المزارع";
            ViewBag.search = search;
            ViewBag.FarmerId = farmerId;
            ViewBag.YearId = YearId;
            ViewBag.MonthId = MonthId;
            ViewBag.source = source;

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

        //public IActionResult Create()
        //{
        //    ViewBag.activePage = "حجوزات المزارع";
        //    FarmerReservationModel farmerReserv = new FarmerReservationModel();
        //    //farmerReserv.FarmerId = 0;
        //    farmerReserv.ReservationDate = DateTime.Now;
        //    return View(NewFillModel(farmerReserv));
        //}
        [HttpPost]
        public async Task<IActionResult> Create(FarmerReservationModel model, IFormFile formFile)
        {

            //we have two task we should make it 
            //1- make update for isMahjouzReservation the default is zero that the reservation
            //come from dashboard ...
            //2- make a set the mobileAppUser from Query you make to aviod any wrong after 
            //on apis ....
            LogFile logFile = new LogFile();

            try
            {
                if (model.FarmerId <= 0)
                {
                    ModelState.AddModelError("FarmerId", "برجاء اختيار المزرعة ");
                }
                if (model.ReservationTypeId <= 0)
                {
                    ModelState.AddModelError("ReservationTypeId", "برجاء اختيار نوع الحجز  ");
                }
                if (ModelState.IsValid)
                {
                    if (_FarmerReservationRepository.Table.Where(f => f.ReservationDate.Date == model.ReservationDate.Date && f.ReservationTypeId == model.ReservationTypeId && f.FarmerId == model.FarmerId).Count() > 0)
                    {
                        ErrorNotification(" يوجد حجز في نفس اليوم ونفس الفترة");
                        model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                        //return View(model);
                        return RedirectToAction("Create", model);
                    }
                    //we can make check on those two line next ...
                    var Farm = _FarmerRepository.Table.FirstOrDefault(f => f.Id == model.FarmerId);
                    var User = _appUser.Table.FirstOrDefault(U => U.Id == Farm.UserId);
                    model.MobileOwnerAppUser =User.MobilePhone;
                    model.CreatedDate = DateTime.Now;
                    //we must update api for this property okay ....
                    model.AutomaticallyNote = "تم الحجز من قبل محجوز بلوحة التحكم";
                    model.IsMahjouzReservation = false;
                    _UnitOfWork.FarmerReservationRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    var tokens = await _farmerViewes.Table
                        .AsNoTracking()
                        .Where(fv => fv.FarmerId == model.FarmerId)
                        .Join(_deviceToken.Table,
                              fv => fv.DeviceId,
                              dt => dt.DeviceId,
                              (fv, dt) => dt.Token)
                        .Where(token => !string.IsNullOrEmpty(token))
                        .Distinct()
                        .ToListAsync();
                    var data = new Dictionary<string, string>
                    {
                        { "type", "general_notification" }
                    };
                    string title = "حجز مزرعتك";
                    string source = "لوحة التحكم";
                    //string body = $"{model.CustomerName} بواسطة {Farm.Name} تم حجز مزرعتك";
                    // العلامة \u200F هي Right-to-Left Mark (RLM)
                    //string body = $"\u200Fتم حجز مزرعتك: {Farm.Name} بواسطة: {model.CustomerName} (بواسطة لوحة التحكم)";
                    //string body = $"\u200F{model.CustomerName} بواسطة {Farm.Name} تم حجز مزرعتك";
                    string body = string.Format("\u200Fتم حجز مزرعتك {0} بواسطة {1} (عبر {2})",
                            Farm.Name,
                            model.CustomerName,
                            source);
                    if (tokens.Any())
                    {
                        await _notificationService.SendNotificationAsync(tokens, title, body, data);
                    }
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index", new { farmerId = model.FarmerId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Saving FarmerReservation: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Create FarmerReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create FarmerReservation - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create FarmerReservation - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Create", model);
            }
            ViewBag.FarmerId = model.FarmerId;
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
        public async Task<IActionResult> Edit(FarmerReservationModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Farms = _FarmerRepository.Table.ToList();
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
                    var Farm = _FarmerRepository.Table.FirstOrDefault(f => f.Id == model.FarmerId);
                    var User = _appUser.Table.FirstOrDefault(U => U.Id == Farm.UserId);
                    model.MobileOwnerAppUser = User.MobilePhone;
                    model.CreatedDate = DateTime.Now;
                    model.AutomaticallyNote = "تم الحجز من قبل محجوز";
                    _UnitOfWork.FarmerReservationRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();

                    var tokens = await _farmerViewes.Table
                        .AsNoTracking()
                        .Where(fv => fv.FarmerId == model.FarmerId)
                        .Join(_deviceToken.Table,
                              fv => fv.DeviceId,
                              dt => dt.DeviceId,
                              (fv, dt) => dt.Token)
                        .Where(token => !string.IsNullOrEmpty(token))
                        .Distinct()
                        .ToListAsync();
                    var data = new Dictionary<string, string>
                    {
                        { "type", "general_notification" }
                    };
                    string title = "حجز مزرعتك";
                    string source = "لوحة التحكم";
                    string body = string.Format("\u200Fتم  تحديث حجز مزرعتك {0} بواسطة {1} (عبر {2})",
                            Farm.Name,
                            model.CustomerName,
                            source);
                    if (tokens.Any())
                    {
                        await _notificationService.SendNotificationAsync(tokens, title, body, data);
                    }
                    SuccessNotification("تم تحديث السجل بنجاح");

                    return RedirectToAction("Index", new { farmerId = model.FarmerId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Update FarmerReservation: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Edit FarmerReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Edit FarmerReservation - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Edit FarmerReservation - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Edit", model);
            }
            ViewBag.FarmerId = model.FarmerId;
            model.Farms = _FarmerRepository.Table.ToList();
            
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

        public IActionResult CalendarReservation(int farmerid)
        {
            var result = _FarmerReservationRepository.Table.Where(Fr => Fr.FarmerId == farmerid && Fr.IsMahjouzReservation == false);
            //var farm = _FarmerRepository.Table
            //        .Select(f => new { f.Id, f.Name })
            //        .FirstOrDefault();
            var Farm = _FarmerRepository.GetById(farmerid);
            ViewBag.Title = $"   جدول مواعيد الحجوزات بمزرعة -  {Farm.Name}";
            return View(result);
            //return Json(result);
        }

        public IActionResult FillCalendarReservation(int farmerid)
        {
            var result = _FarmerReservationRepository.Table
                .Where(fr => fr.FarmerId == farmerid)
                .Select(fr => new
                {
                    id = fr.Id,
                    title = fr.CustomerName,
                    start = fr.ReservationDate
                        .ToString("yyyy-MM-ddTHH:mm:ss"),
                    phone = fr.CustMobNum,
                    amount = fr.ReservationAmt,
                    persons = fr.NumberOfPerson,
                    note = fr.Note,
                    deposit = fr.ReservationDepositAmt,
                    remain = fr.ReservationRemainAmt,
                    ReservationType=fr.ReservationTypeId,
                    className = "bg-success"

                })
                .ToList();

            return Json(result);
        }


    }
}
