using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using MazraeatiBackOffice;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Models.FarmModel;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            return model;
        }

        public FarmerReservationModel EditFillModel(FarmerReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Farms = _FarmerRepository.Table.ToList();
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
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

        //public IActionResult Index(int farmerId)
        //{
        //    Farmer farmer = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);
        //    List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
        //    var model = _FarmerReservationRepository.Table.Where(t=>t.FarmerId == farmerId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.FarmerName = farmer.Name;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.YearId = DateTime.Now.Year;
        //    ViewBag.MonthId = DateTime.Now.Month;
        //    return View(model);
        //}

        ////public IActionResult Index(int farmerId, string search, int MonthId, int YearId)
        ////{
        ////    List<LookupValue> lookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
        ////    IQueryable<FarmerReservationModel> model;
        ////    if (string.IsNullOrEmpty(search))
        ////    {
        ////        model = _FarmerReservationRepository.Table.Where(t => t.FarmerId == farmerId && t.ReservationDate.Month == MonthId &&
        ////                                                              t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
        ////    }
        ////    else
        ////    {
        ////        model = _FarmerReservationRepository.Table.Where(t => (t.CustMobNum.Contains(search) ||
        ////                                                                                t.CustomerName.Contains(search) ||
        ////                                                                                t.Note.Contains(search)) && t.FarmerId == farmerId || t.ReservationDate.Month == MonthId ||
        ////                                                              t.ReservationDate.Year == YearId).OrderByDescending(a => a.CreatedDate).Select(c => c.ToModel(lookupValues));
        ////    }


        ////    ViewBag.activePage = "حجوزات المزارع";
        ////    ViewBag.search = search;
        ////    ViewBag.FarmerId = farmerId;
        ////    ViewBag.YearId = YearId;
        ////    ViewBag.MonthId = MonthId;
        ////    return View(model);
        ////}
        //[HttpPost]
        //public IActionResult Index(int farmerId, string search, int? MonthId, int? YearId, int? source)
        //{
        //    var lookupValues = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6).ToList();

        //    var query = _FarmerReservationRepository.Table
        //        .Where(t => t.FarmerId == farmerId);

        //    if (MonthId.HasValue && MonthId > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Month == MonthId);
        //    }

        //    if (YearId.HasValue && YearId > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Year == YearId);
        //    }

        //    if (source.HasValue && source > 0)
        //    {
        //        bool isMahjouz = (source == 1);
        //        query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
        //    }

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(t =>
        //            t.CustMobNum.Contains(search) ||
        //            t.CustomerName.Contains(search) ||
        //            (t.Note != null && t.Note.Contains(search))
        //        );
        //    }


        //    var model = query
        //        .OrderByDescending(a => a.CreatedDate)
        //        .Select(c => c.ToModel(lookupValues))
        //        .ToList();

        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.search = search;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.YearId = YearId;
        //    ViewBag.MonthId = MonthId;
        //    ViewBag.source = source;

        //    return View(model);
        //}




        // ============================================================
        // GET: Index (بدون farmerId → كل الحجوزات)
        // ============================================================
        //public IActionResult Index()
        //{
        //    return Index(null, null, null, null, null, null, null);
        //}

        //// ============================================================
        //// GET: Index (مع farmerId)
        //// ============================================================
        //public IActionResult Index(int farmerId)
        //{
        //    return Index(farmerId, null, null, null, null, null, null);
        //}

        //// ============================================================
        //// POST: Index (مع فلتر)
        //// ============================================================


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Index(int? farmerId, string search, int? MonthId, int? YearId, int? source, int? status, int? reservationTypeId)
        //{
        //    var lookupValues = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .ToList();

        //    var query = _FarmerReservationRepository.Table
        //        .Include(r => r.Customer)
        //        .Include(r => r.Farm)
        //        .AsQueryable();

        //    // ===== فلتر حسب المزرعة =====
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        query = query.Where(t => t.FarmerId == farmerId.Value);
        //    }

        //    // ===== فلتر حسب نوع الحجز =====
        //    if (reservationTypeId.HasValue && reservationTypeId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationTypeId == reservationTypeId.Value);
        //    }

        //    // ===== فلتر حسب الشهر =====
        //    if (MonthId.HasValue && MonthId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Month == MonthId.Value);
        //    }

        //    // ===== فلتر حسب السنة =====
        //    if (YearId.HasValue && YearId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Year == YearId.Value);
        //    }

        //    // ===== فلتر حسب مصدر الحجز =====
        //    if (source.HasValue && source.Value >= 0)
        //    {
        //        bool isMahjouz = (source.Value == 1);
        //        query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
        //    }

        //    // ===== فلتر حسب الحالة =====
        //    if (status.HasValue && status.Value >= 0)
        //    {
        //        var reservStatus = (ReservStatusEnum)status.Value;
        //        query = query.Where(t => t.ReservStatus == reservStatus);
        //    }

        //    // ===== فلتر بالبحث =====
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(t =>
        //            (t.CustMobNum != null && t.CustMobNum.Contains(search)) ||
        //            (t.CustomerName != null && t.CustomerName.Contains(search)) ||
        //            (t.Note != null && t.Note.Contains(search)) ||
        //            (t.Customer != null && t.Customer.FullName.Contains(search)) ||
        //            (t.Customer != null && t.Customer.MobileNumber.Contains(search))
        //        );
        //    }

        //    // ===== تنفيذ الاستعلام =====
        //    var reservations = query
        //        .OrderByDescending(t => t.CreatedDate)
        //        .ToList();

        //    var model = reservations
        //        .Select(r => r.ToModel(lookupValues))
        //        .Where(r => r != null)
        //        .ToList();

        //    // ===== الإحصائيات =====
        //    var totalCount = model.Count();
        //    var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //    var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //    var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //    // ===== جلب اسم المزرعة =====
        //    string farmerName = "جميع المزارع";
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        var farm = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId.Value);
        //        farmerName = farm?.Name ?? "مزرعة غير محددة";
        //    }

        //    // ===== جلب أنواع الحجوزات للفلتر =====
        //    var reservationTypes = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .OrderBy(l => l.ValueAr)
        //        .ToList();

        //    // ===== جلب قائمة المزارع للفلتر =====
        //    var farms = _FarmerRepository.Table
        //        .OrderBy(f => f.Name)
        //        .ToList();

        //    // ===== ViewBag =====
        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.FarmerName = farmerName;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.search = search;
        //    ViewBag.YearId = YearId;
        //    ViewBag.MonthId = MonthId;
        //    ViewBag.source = source;
        //    ViewBag.Status = status;
        //    ViewBag.ReservationTypeId = reservationTypeId;

        //    // ===== الإحصائيات =====
        //    ViewBag.TotalReservations = totalCount;
        //    ViewBag.PendingCount = pendingCount;
        //    ViewBag.ConfirmedCount = confirmedCount;
        //    ViewBag.CancelledCount = cancelledCount;

        //    // ===== قوائم للفلتر =====
        //    ViewBag.Farms = farms;
        //    ViewBag.ReservationTypes = reservationTypes;

        //    return View(model);
        //}


        // ============================================================
        // GET: Index (مع أو بدون farmerId)
        // ============================================================
        //public IActionResult Index(int? farmerId)
        //{
        //    var lookupValues = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .ToList();

        //    var query = _FarmerReservationRepository.Table
        //        .Include(r => r.Customer)
        //        .Include(r => r.Farm)
        //        .AsQueryable();

        //    // ===== فلتر حسب المزرعة =====
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        query = query.Where(t => t.FarmerId == farmerId.Value);
        //    }

        //    // ===== تنفيذ الاستعلام =====
        //    var reservations = query
        //        .OrderByDescending(t => t.CreatedDate)
        //        .ToList();

        //    var model = reservations
        //        .Select(r => r.ToModel(lookupValues))
        //        .Where(r => r != null)
        //        .ToList();

        //    // ===== الإحصائيات =====
        //    var totalCount = model.Count();
        //    var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //    var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //    var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //    // ===== جلب اسم المزرعة =====
        //    string farmerName = "جميع المزارع";
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        var farm = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId.Value);
        //        farmerName = farm?.Name ?? "مزرعة غير محددة";
        //    }

        //    // ===== جلب أنواع الحجوزات للفلتر =====
        //    var reservationTypes = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .OrderBy(l => l.ValueAr)
        //        .ToList();

        //    // ===== جلب قائمة المزارع للفلتر =====
        //    var farms = _FarmerRepository.Table
        //        .OrderBy(f => f.Name)
        //        .ToList();

        //    // ===== ViewBag =====
        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.FarmerName = farmerName;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.search = null;
        //    ViewBag.YearId = null;
        //    ViewBag.MonthId = null;
        //    ViewBag.source = null;
        //    ViewBag.Status = null;
        //    ViewBag.ReservationTypeId = null;

        //    // ===== الإحصائيات =====
        //    ViewBag.TotalReservations = totalCount;
        //    ViewBag.PendingCount = pendingCount;
        //    ViewBag.ConfirmedCount = confirmedCount;
        //    ViewBag.CancelledCount = cancelledCount;

        //    // ===== قوائم للفلتر =====
        //    ViewBag.Farms = farms;
        //    ViewBag.ReservationTypes = reservationTypes;

        //    return View(model);
        //}

        //// ============================================================
        //// POST: Index (مع فلتر)
        //// ============================================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Index(int? farmerId, string search, int? MonthId, int? YearId, int? source, int? status, int? reservationTypeId)
        //{
        //    var lookupValues = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .ToList();

        //    var query = _FarmerReservationRepository.Table
        //        .Include(r => r.Customer)
        //        .Include(r => r.Farm)
        //        .AsQueryable();

        //    // ===== فلتر حسب المزرعة =====
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        query = query.Where(t => t.FarmerId == farmerId.Value);
        //    }

        //    // ===== فلتر حسب نوع الحجز =====
        //    if (reservationTypeId.HasValue && reservationTypeId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationTypeId == reservationTypeId.Value);
        //    }

        //    // ===== فلتر حسب الشهر =====
        //    if (MonthId.HasValue && MonthId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Month == MonthId.Value);
        //    }

        //    // ===== فلتر حسب السنة =====
        //    if (YearId.HasValue && YearId.Value > 0)
        //    {
        //        query = query.Where(t => t.ReservationDate.Year == YearId.Value);
        //    }

        //    // ===== فلتر حسب مصدر الحجز =====
        //    if (source.HasValue && source.Value >= 0)
        //    {
        //        bool isMahjouz = (source.Value == 1);
        //        query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
        //    }

        //    // ===== فلتر حسب الحالة =====
        //    if (status.HasValue && status.Value >= 0)
        //    {
        //        var reservStatus = (ReservStatusEnum)status.Value;
        //        query = query.Where(t => t.ReservStatus == reservStatus);
        //    }

        //    // ===== فلتر بالبحث =====
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(t =>
        //            (t.CustMobNum != null && t.CustMobNum.Contains(search)) ||
        //            (t.CustomerName != null && t.CustomerName.Contains(search)) ||
        //            (t.Note != null && t.Note.Contains(search)) ||
        //            (t.Customer != null && t.Customer.FullName.Contains(search)) ||
        //            (t.Customer != null && t.Customer.MobileNumber.Contains(search))
        //        );
        //    }

        //    // ===== تنفيذ الاستعلام =====
        //    var reservations = query
        //        .OrderByDescending(t => t.CreatedDate)
        //        .ToList();

        //    var model = reservations
        //        .Select(r => r.ToModel(lookupValues))
        //        .Where(r => r != null)
        //        .ToList();

        //    // ===== الإحصائيات =====
        //    var totalCount = model.Count();
        //    var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //    var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //    var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //    // ===== جلب اسم المزرعة =====
        //    string farmerName = "جميع المزارع";
        //    if (farmerId.HasValue && farmerId.Value > 0)
        //    {
        //        var farm = _FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId.Value);
        //        farmerName = farm?.Name ?? "مزرعة غير محددة";
        //    }

        //    // ===== جلب أنواع الحجوزات للفلتر =====
        //    var reservationTypes = _LookupValueRepository.Table
        //        .Where(l => l.LookupId == 6)
        //        .OrderBy(l => l.ValueAr)
        //        .ToList();

        //    // ===== جلب قائمة المزارع للفلتر =====
        //    var farms = _FarmerRepository.Table
        //        .OrderBy(f => f.Name)
        //        .ToList();

        //    // ===== ViewBag =====
        //    ViewBag.activePage = "حجوزات المزارع";
        //    ViewBag.FarmerName = farmerName;
        //    ViewBag.FarmerId = farmerId;
        //    ViewBag.search = search;
        //    ViewBag.YearId = YearId;
        //    ViewBag.MonthId = MonthId;
        //    ViewBag.source = source;
        //    ViewBag.Status = status;
        //    ViewBag.ReservationTypeId = reservationTypeId;

        //    // ===== الإحصائيات =====
        //    ViewBag.TotalReservations = totalCount;
        //    ViewBag.PendingCount = pendingCount;
        //    ViewBag.ConfirmedCount = confirmedCount;
        //    ViewBag.CancelledCount = cancelledCount;

        //    // ===== قوائم للفلتر =====
        //    ViewBag.Farms = farms;
        //    ViewBag.ReservationTypes = reservationTypes;

        //    return View(model);
        //}



        // ============================================================
        // GET: Index (مع أو بدون farmerId)
        // ============================================================
        //[HttpGet]
        //public IActionResult Index(int? farmerId)
        //{
        //    LogFile logFile = new LogFile();
        //    try
        //    {
        //        // ===== جلب الـ lookup values =====
        //        var lookupValues = _LookupValueRepository.Table
        //            .Where(l => l.LookupId == 6)
        //            .ToList();

        //        // ===== ✅ تحويل lookupValues لـ Dictionary للسرعة =====
        //        var lookupDict = lookupValues.ToDictionary(l => l.Id, l => l.ValueAr);

        //        // ===== ✅ بناء الاستعلام مع AsNoTracking للقراءة فقط =====
        //        var query = _FarmerReservationRepository.Table
        //            .AsNoTracking()
        //            .AsQueryable();

        //        // ===== فلتر حسب المزرعة =====
        //        if (farmerId.HasValue && farmerId.Value > 0)
        //        {
        //            query = query.Where(t => t.FarmerId == farmerId.Value);
        //        }

        //        // ===== ✅ تنفيذ الاستعلام مع تحويل مباشر =====
        //        var model = query
        //            .OrderByDescending(t => t.CreatedDate)
        //            .Select(r => new FarmerReservationModel
        //            {
        //                Id = r.Id,
        //                FarmerId = r.FarmerId,
        //                CustomerId = r.CustomerId,
        //                ReservationTypeId = r.ReservationTypeId,
        //                ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
        //                ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
        //                CustMobNum = r.CustMobNum ?? string.Empty,
        //                CustomerName = r.CustomerName ?? string.Empty,
        //                NumberOfPerson = r.NumberOfPerson,
        //                CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
        //                ReservationAmt = r.ReservationAmt,
        //                NetProfit = r.NetProfit,
        //                ReservationDepositAmt = r.ReservationDepositAmt,
        //                ReservationRemainAmt = r.ReservationRemainAmt,
        //                Note = r.Note ?? string.Empty,
        //                MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
        //                IsMahjouzReservation = r.IsMahjouzReservation,
        //                IsReciveCommission = r.IsReciveCommission,
        //                AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
        //                CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
        //                Reason = r.Reason ?? string.Empty,
        //                ReservStatus = r.ReservStatus
        //            })
        //            .ToList();

        //        // ===== الإحصائيات =====
        //        var totalCount = model.Count;
        //        var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //        var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //        var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //        // ===== جلب اسم المزرعة =====
        //        string farmerName = "جميع المزارع";
        //        if (farmerId.HasValue && farmerId.Value > 0)
        //        {
        //            var farm = _FarmerRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == farmerId.Value);
        //            farmerName = farm?.Name ?? "مزرعة غير محددة";
        //        }

        //        // ===== جلب أنواع الحجوزات للفلتر =====
        //        var reservationTypes = _LookupValueRepository.Table
        //            .Where(l => l.LookupId == 6)
        //            .OrderBy(l => l.ValueAr)
        //            .ToList();

        //        // ===== جلب قائمة المزارع للفلتر =====
        //        var farms = _FarmerRepository.Table
        //            .OrderBy(f => f.Name)
        //            .ToList();

        //        // ===== ViewBag =====
        //        ViewBag.activePage = "حجوزات المزارع";
        //        ViewBag.FarmerName = farmerName;
        //        ViewBag.FarmerId = farmerId;
        //        ViewBag.search = null;
        //        ViewBag.YearId = null;
        //        ViewBag.MonthId = null;
        //        ViewBag.source = null;
        //        ViewBag.Status = null;
        //        ViewBag.ReservationTypeId = null;
        //        ViewBag.TotalReservations = totalCount;
        //        ViewBag.PendingCount = pendingCount;
        //        ViewBag.ConfirmedCount = confirmedCount;
        //        ViewBag.CancelledCount = cancelledCount;
        //        ViewBag.Farms = farms;
        //        ViewBag.ReservationTypes = reservationTypes;

        //        return View(model);
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"Error while loading reservations: {e.Message}");
        //        logFile.LogCustomInfo("Index FarmerReservation - Exception Message ", e.Message);
        //        logFile.LogCustomInfo("Index FarmerReservation - Stack Trace Message ", e.StackTrace);
        //        logFile.LogCustomInfo("Index FarmerReservation - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");
        //        return View(new List<FarmerReservationModel>());
        //    }
        //}

        //// ============================================================
        //// POST: Index (مع فلتر محسن جداً)
        //// ============================================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Index(int? farmerId, string search, int? MonthId, int? YearId, int? source, int? status, int? reservationTypeId)
        //{
        //    LogFile logFile = new LogFile();
        //    try
        //    {
        //        // ===== جلب الـ lookup values =====
        //        var lookupValues = _LookupValueRepository.Table
        //            .Where(l => l.LookupId == 6)
        //            .ToList();
        //        var lookupDict = lookupValues.ToDictionary(l => l.Id, l => l.ValueAr);

        //        // ===== ✅ بناء الاستعلام مع AsNoTracking =====
        //        var query = _FarmerReservationRepository.Table
        //            .AsNoTracking()
        //            .AsQueryable();

        //        // ===== فلتر حسب المزرعة =====
        //        if (farmerId.HasValue && farmerId.Value > 0)
        //        {
        //            query = query.Where(t => t.FarmerId == farmerId.Value);
        //        }

        //        // ===== فلتر حسب نوع الحجز =====
        //        if (reservationTypeId.HasValue && reservationTypeId.Value > 0)
        //        {
        //            query = query.Where(t => t.ReservationTypeId == reservationTypeId.Value);
        //        }

        //        // ===== فلتر حسب الشهر والسنة =====
        //        if (MonthId.HasValue && MonthId.Value > 0 && YearId.HasValue && YearId.Value > 0)
        //        {
        //            query = query.Where(t => t.ReservationDate.Month == MonthId.Value && t.ReservationDate.Year == YearId.Value);
        //        }
        //        else
        //        {
        //            if (MonthId.HasValue && MonthId.Value > 0)
        //            {
        //                query = query.Where(t => t.ReservationDate.Month == MonthId.Value);
        //            }
        //            if (YearId.HasValue && YearId.Value > 0)
        //            {
        //                query = query.Where(t => t.ReservationDate.Year == YearId.Value);
        //            }
        //        }

        //        // ===== فلتر حسب مصدر الحجز =====
        //        if (source.HasValue && source.Value >= 0)
        //        {
        //            bool isMahjouz = (source.Value == 1);
        //            query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
        //        }

        //        // ===== فلتر حسب الحالة =====
        //        if (status.HasValue && status.Value >= 0)
        //        {
        //            var reservStatus = (ReservStatusEnum)status.Value;
        //            query = query.Where(t => t.ReservStatus == reservStatus);
        //        }

        //        // ===== ✅ فلتر بالبحث (محسن جداً - فلتر في SQL) =====
        //        if (!string.IsNullOrEmpty(search))
        //        {
        //            var searchLower = search.Trim();
        //            query = query.Where(t =>
        //                (t.CustMobNum != null && t.CustMobNum.Contains(searchLower)) ||
        //                (t.CustomerName != null && t.CustomerName.Contains(searchLower)) ||
        //                (t.Note != null && t.Note.Contains(searchLower))
        //            );
        //        }

        //        // ===== ✅ تنفيذ الاستعلام =====
        //        var model = query
        //            .OrderByDescending(t => t.CreatedDate)
        //            .Select(r => new FarmerReservationModel
        //            {
        //                Id = r.Id,
        //                FarmerId = r.FarmerId,
        //                CustomerId = r.CustomerId,
        //                ReservationTypeId = r.ReservationTypeId,
        //                ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
        //                ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
        //                CustMobNum = r.CustMobNum ?? string.Empty,
        //                CustomerName = r.CustomerName ?? string.Empty,
        //                NumberOfPerson = r.NumberOfPerson,
        //                CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
        //                ReservationAmt = r.ReservationAmt,
        //                NetProfit = r.NetProfit,
        //                ReservationDepositAmt = r.ReservationDepositAmt,
        //                ReservationRemainAmt = r.ReservationRemainAmt,
        //                Note = r.Note ?? string.Empty,
        //                MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
        //                IsMahjouzReservation = r.IsMahjouzReservation,
        //                IsReciveCommission = r.IsReciveCommission,
        //                AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
        //                CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
        //                Reason = r.Reason ?? string.Empty,
        //                ReservStatus = r.ReservStatus
        //            })
        //            .ToList();

        //        // ===== الإحصائيات =====
        //        var totalCount = model.Count;
        //        var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //        var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //        var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //        // ===== جلب اسم المزرعة =====
        //        string farmerName = "جميع المزارع";
        //        if (farmerId.HasValue && farmerId.Value > 0)
        //        {
        //            var farm = _FarmerRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == farmerId.Value);
        //            farmerName = farm?.Name ?? "مزرعة غير محددة";
        //        }

        //        // ===== جلب قوائم الفلتر =====
        //        var reservationTypes = _LookupValueRepository.Table
        //            .Where(l => l.LookupId == 6)
        //            .OrderBy(l => l.ValueAr)
        //            .ToList();

        //        var farms = _FarmerRepository.Table
        //            .OrderBy(f => f.Name)
        //            .ToList();

        //        // ===== ViewBag =====
        //        ViewBag.activePage = "حجوزات المزارع";
        //        ViewBag.FarmerName = farmerName;
        //        ViewBag.FarmerId = farmerId;
        //        ViewBag.search = search;
        //        ViewBag.YearId = YearId;
        //        ViewBag.MonthId = MonthId;
        //        ViewBag.source = source;
        //        ViewBag.Status = status;
        //        ViewBag.ReservationTypeId = reservationTypeId;
        //        ViewBag.TotalReservations = totalCount;
        //        ViewBag.PendingCount = pendingCount;
        //        ViewBag.ConfirmedCount = confirmedCount;
        //        ViewBag.CancelledCount = cancelledCount;
        //        ViewBag.Farms = farms;
        //        ViewBag.ReservationTypes = reservationTypes;

        //        return View(model);
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"Error while filtering reservations: {e.Message}");
        //        logFile.LogCustomInfo("Index POST FarmerReservation - Exception Message ", e.Message);
        //        logFile.LogCustomInfo("Index POST FarmerReservation - Stack Trace Message ", e.StackTrace);
        //        logFile.LogCustomInfo("Index POST FarmerReservation - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");

        //        // ===== في حالة الخطأ، أرجع قائمة فارغة =====
        //        ViewBag.Farms = _FarmerRepository.Table.OrderBy(f => f.Name).ToList();
        //        ViewBag.ReservationTypes = _LookupValueRepository.Table.Where(l => l.LookupId == 6).OrderBy(l => l.ValueAr).ToList();
        //        return View(new List<FarmerReservationModel>());
        //    }
        //}

        [HttpGet]
        public IActionResult Index(int? farmerId, DateTime? fromDate, DateTime? toDate)
        {
            LogFile logFile = new LogFile();
            try
            {
                var lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .ToList();
                var lookupDict = lookupValues.ToDictionary(l => l.Id, l => l.ValueAr);

                var query = _FarmerReservationRepository.Table
                    .AsNoTracking()
                    .AsQueryable();

                if (farmerId.HasValue && farmerId.Value > 0)
                {
                    query = query.Where(t => t.FarmerId == farmerId.Value);
                }

                // ===== ✅ فلتر حسب التاريخ (من - إلى) =====
                if (fromDate.HasValue)
                {
                    query = query.Where(t => t.ReservationDate >= fromDate.Value);
                }
                if (toDate.HasValue)
                {
                    var endDate = toDate.Value.AddDays(1);
                    query = query.Where(t => t.ReservationDate < endDate);
                }

                var model = query
                    .OrderByDescending(t => t.CreatedDate)
                    .Select(r => new FarmerReservationModel
                    {
                        Id = r.Id,
                        FarmerId = r.FarmerId,
                        CustomerId = r.CustomerId,
                        ReservationTypeId = r.ReservationTypeId,
                        ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
                        ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
                        CustMobNum = r.CustMobNum ?? string.Empty,
                        CustomerName = r.CustomerName ?? string.Empty,
                        NumberOfPerson = r.NumberOfPerson,
                        CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
                        ReservationAmt = r.ReservationAmt,
                        NetProfit = r.NetProfit,
                        ReservationDepositAmt = r.ReservationDepositAmt,
                        ReservationRemainAmt = r.ReservationRemainAmt,
                        Note = r.Note ?? string.Empty,
                        MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
                        IsMahjouzReservation = r.IsMahjouzReservation,
                        IsReciveCommission = r.IsReciveCommission,
                        AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
                        CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
                        Reason = r.Reason ?? string.Empty,
                        ReservStatus = r.ReservStatus
                    })
                    .ToList();

                var totalCount = model.Count;
                var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
                var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
                var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

                string farmerName = "جميع المزارع";
                if (farmerId.HasValue && farmerId.Value > 0)
                {
                    var farm = _FarmerRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == farmerId.Value);
                    farmerName = farm?.Name ?? "مزرعة غير محددة";
                }

                var reservationTypes = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .OrderBy(l => l.ValueAr)
                    .ToList();

                var farms = _FarmerRepository.Table
                    .OrderBy(f => f.Name)
                    .ToList();

                ViewBag.activePage = "حجوزات المزارع";
                ViewBag.FarmerName = farmerName;
                ViewBag.FarmerId = farmerId;
                ViewBag.search = null;
                ViewBag.YearId = null;
                ViewBag.MonthId = null;
                ViewBag.source = null;
                ViewBag.Status = null;
                ViewBag.ReservationTypeId = null;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.TotalReservations = totalCount;
                ViewBag.PendingCount = pendingCount;
                ViewBag.ConfirmedCount = confirmedCount;
                ViewBag.CancelledCount = cancelledCount;
                ViewBag.Farms = farms;
                ViewBag.ReservationTypes = reservationTypes;

                return View(model);
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while loading reservations: {e.Message}");
                logFile.LogCustomInfo("Index FarmerReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Index FarmerReservation - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Index FarmerReservation - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");
                return View(new List<FarmerReservationModel>());
            }
        }


        // ============================================================
        // POST: Index (مع فلتر محسن + تاريخ من-إلى)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int? farmerId, string search, int? MonthId, int? YearId,
                                  int? source, int? status, int? reservationTypeId,
                                  DateTime? fromDate, DateTime? toDate)
        {
            LogFile logFile = new LogFile();
            try
            {
                // ===== جلب الـ lookup values =====
                var lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .ToList();
                var lookupDict = lookupValues.ToDictionary(l => l.Id, l => l.ValueAr);

                // ===== بناء الاستعلام مع AsNoTracking =====
                var query = _FarmerReservationRepository.Table
                    .AsNoTracking()
                    .AsQueryable();

                // ===== فلتر حسب المزرعة =====
                if (farmerId.HasValue && farmerId.Value > 0)
                {
                    query = query.Where(t => t.FarmerId == farmerId.Value);
                }

                // =====  فلتر حسب التاريخ (من - إلى) =====
                if (fromDate.HasValue)
                {
                    query = query.Where(t => t.ReservationDate >= fromDate.Value);
                }
                if (toDate.HasValue)
                {
                    var endDate = toDate.Value.AddDays(1); // ✅ عشان يشمل اليوم بالكامل
                    query = query.Where(t => t.ReservationDate < endDate);
                }

                // ===== فلتر حسب نوع الحجز =====
                if (reservationTypeId.HasValue && reservationTypeId.Value > 0)
                {
                    query = query.Where(t => t.ReservationTypeId == reservationTypeId.Value);
                }

                // ===== فلتر حسب الشهر والسنة =====
                if (MonthId.HasValue && MonthId.Value > 0 && YearId.HasValue && YearId.Value > 0)
                {
                    query = query.Where(t => t.ReservationDate.Month == MonthId.Value && t.ReservationDate.Year == YearId.Value);
                }
                else
                {
                    if (MonthId.HasValue && MonthId.Value > 0)
                    {
                        query = query.Where(t => t.ReservationDate.Month == MonthId.Value);
                    }
                    if (YearId.HasValue && YearId.Value > 0)
                    {
                        query = query.Where(t => t.ReservationDate.Year == YearId.Value);
                    }
                }

                // ===== فلتر حسب مصدر الحجز =====
                if (source.HasValue && source.Value >= 0)
                {
                    bool isMahjouz = (source.Value == 1);
                    query = query.Where(t => t.IsMahjouzReservation == isMahjouz);
                }

                // ===== فلتر حسب الحالة =====
                if (status.HasValue && status.Value >= 0)
                {
                    var reservStatus = (ReservStatusEnum)status.Value;
                    query = query.Where(t => t.ReservStatus == reservStatus);
                }

                // ===== فلتر بالبحث =====
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.Trim();
                    query = query.Where(t =>
                        (t.CustMobNum != null && t.CustMobNum.Contains(searchLower)) ||
                        (t.CustomerName != null && t.CustomerName.Contains(searchLower)) ||
                        (t.Note != null && t.Note.Contains(searchLower))
                    );
                }

                // ===== تنفيذ الاستعلام =====
                var model = query
                    .OrderByDescending(t => t.CreatedDate)
                    .Select(r => new FarmerReservationModel
                    {
                        Id = r.Id,
                        FarmerId = r.FarmerId,
                        CustomerId = r.CustomerId,
                        ReservationTypeId = r.ReservationTypeId,
                        ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
                        ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
                        CustMobNum = r.CustMobNum ?? string.Empty,
                        CustomerName = r.CustomerName ?? string.Empty,
                        NumberOfPerson = r.NumberOfPerson,
                        CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
                        ReservationAmt = r.ReservationAmt,
                        NetProfit = r.NetProfit,
                        ReservationDepositAmt = r.ReservationDepositAmt,
                        ReservationRemainAmt = r.ReservationRemainAmt,
                        Note = r.Note ?? string.Empty,
                        MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
                        IsMahjouzReservation = r.IsMahjouzReservation,
                        IsReciveCommission = r.IsReciveCommission,
                        AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
                        CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
                        Reason = r.Reason ?? string.Empty,
                        ReservStatus = r.ReservStatus
                    })
                    .ToList();

                // ===== الإحصائيات =====
                var totalCount = model.Count;
                var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
                var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
                var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

                // ===== جلب اسم المزرعة =====
                string farmerName = "جميع المزارع";
                if (farmerId.HasValue && farmerId.Value > 0)
                {
                    var farm = _FarmerRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == farmerId.Value);
                    farmerName = farm?.Name ?? "مزرعة غير محددة";
                }

                // ===== جلب قوائم الفلتر =====
                var reservationTypes = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .OrderBy(l => l.ValueAr)
                    .ToList();

                var farms = _FarmerRepository.Table
                    .OrderBy(f => f.Name)
                    .ToList();

                // ===== ViewBag =====
                ViewBag.activePage = "حجوزات المزارع";
                ViewBag.FarmerName = farmerName;
                ViewBag.FarmerId = farmerId;
                ViewBag.search = search;
                ViewBag.YearId = YearId;
                ViewBag.MonthId = MonthId;
                ViewBag.source = source;
                ViewBag.Status = status;
                ViewBag.ReservationTypeId = reservationTypeId;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.TotalReservations = totalCount;
                ViewBag.PendingCount = pendingCount;
                ViewBag.ConfirmedCount = confirmedCount;
                ViewBag.CancelledCount = cancelledCount;
                ViewBag.Farms = farms;
                ViewBag.ReservationTypes = reservationTypes;

                return View(model);
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while filtering reservations: {e.Message}");
                logFile.LogCustomInfo("Index POST FarmerReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Index POST FarmerReservation - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Index POST FarmerReservation - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");

                ViewBag.Farms = _FarmerRepository.Table.OrderBy(f => f.Name).ToList();
                ViewBag.ReservationTypes = _LookupValueRepository.Table.Where(l => l.LookupId == 6).OrderBy(l => l.ValueAr).ToList();
                return View(new List<FarmerReservationModel>());
            }
        }

        public IActionResult Create(int farmerId)
        {
            ViewBag.activePage = "حجوزات المزارع";
            FarmerReservationModel farmerReservation = new FarmerReservationModel();
            farmerReservation.FarmerId = farmerId;
            farmerReservation.ReservationDate = DateTime.Now;
            
            return View(NewFillModel(farmerReservation));
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
                    //model.CustomerId=model.CustomerId;
                }
                if (model.ReservationTypeId <= 0)
                {
                    ModelState.AddModelError("ReservationTypeId", "برجاء اختيار نوع الحجز  ");
                    //model.CustomerId = model.CustomerId;
                }
                if (model.CustomerId <= 0)
                {
                    ModelState.AddModelError("CustomerId", "برجاء اختيار العميل  ");
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
                model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
                model.Farms = _FarmerRepository.Table.ToList();
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

        private int GetCurrentAdminId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet]
        public IActionResult ValidateRedeemPoints(int customerId, int points)
        {
            try
            {
                // 1. التحقق من رصيد العميل
                var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                    .FirstOrDefault(a => a.CustomerId == customerId);

                if (account == null || account.AvailablePoints < points)
                {
                    return Json(new
                    {
                        success = false,
                        message = "رصيد النقاط غير كافٍ",
                        canRedeem = false
                    });
                }

                // 2. التحقق من وجود قاعدة صرف
                var redeemRule = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                    .Where(r => r.IsActive == true && r.Points <= points)
                    .OrderByDescending(r => r.Points)
                    .FirstOrDefault();

                if (redeemRule == null)
                {
                    var nearestRule = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                        .Where(r => r.IsActive == true)
                        .OrderBy(r => r.Points)
                        .FirstOrDefault();

                    return Json(new
                    {
                        success = false,
                        message = "لا توجد قاعدة صرف لهذا العدد من النقاط",
                        canRedeem = false,
                        nearestPoints = nearestRule?.Points ?? 0
                    });
                }

                // 3. حساب قيمة الخصم
                decimal ratio = (decimal)points / redeemRule.Points;
                decimal discountAmount = ratio * redeemRule.DiscountAmount;

                return Json(new
                {
                    success = true,
                    canRedeem = true,
                    discountAmount = discountAmount,
                    rule = new
                    {
                        points = redeemRule.Points,
                        discountAmount = redeemRule.DiscountAmount
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message,
                    canRedeem = false
                });
            }
        }

        // ============================================================
        // GET: GetConfirmFarmReservationData (للـ Popup)
        // ============================================================
        [HttpGet]
        public IActionResult GetConfirmFarmReservationData(int id)
        {
            try
            {
                var lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .ToList();

                var reservation = _FarmerReservationRepository.Table
                    .Include(r => r.Customer)
                    .Include(r => r.Farm)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                {
                    return Json(new { success = false, message = "الحجز غير موجود" });
                }

                // ===== جلب نقاط العميل =====
                int availablePoints = 0;
                string tierName = "لا يوجد مستوى";
                string tierIcon = "";

                if (reservation.CustomerId > 0)
                {
                    var account = _UnitOfWork.CustomerLoyaltyAccountRepository
                        .Table
                        .FirstOrDefault(a => a.CustomerId == reservation.CustomerId);

                    availablePoints = account?.AvailablePoints ?? 0;

                    if (account?.CurrentTierId != null)
                    {
                        var tier = _UnitOfWork.LoyaltyTierRepository
                            .Table
                            .FirstOrDefault(t => t.Id == account.CurrentTierId);
                        if (tier != null)
                        {
                            tierName = tier.NameAr;
                            tierIcon = tier.IconClass;
                        }
                    }
                }

                // ===== حساب نقاط الحجز المستحقة =====
                var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                var earnedPoints = loyaltyService.CalculateReservationEarnedPoints(
                    customerId: reservation.CustomerId ,
                    bookingType: "Farm",
                    referenceId: reservation.FarmerId
                );

                var viewModel = new ConfirmReservationFarmViewModel
                {
                    ReservationId = reservation.Id,
                    CustomerId = reservation.Customer.Id,
                    CustomerName = reservation.Customer?.FullName ?? "",
                    CustomerPhone = reservation.Customer?.MobileNumber ?? "",
                    FarmName = reservation.Farm?.Name ?? "",
                    ReservationTypeName = lookupValues
                        .FirstOrDefault(l => l.Id == reservation.ReservationTypeId)?.ValueAr ?? "",
                    ReservationDate = reservation.ReservationDate,
                    OriginalAmount = reservation.ReservationAmt,
                    NetProfit = reservation.NetProfit,
                    CustomerAvailablePoints = availablePoints,
                    CurrentTierName = tierName,
                    TierIcon = tierIcon,
                    EarnedPoints = earnedPoints
                };

                // ===== إرسال قواعد الخصم للـ View =====
                var redeemRules = _UnitOfWork.LoyaltyRedeemRuleRepository
                    .Table
                    .Where(r => r.IsActive)
                    .Select(r => new { r.Points, r.DiscountAmount })
                    .ToList();

                ViewBag.RedeemRules = redeemRules;

                return PartialView("_ConfirmReservationFarmPopup", viewModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ============================================================
        // POST: ConfirmFarmReservation
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> ConfirmFarmReservation(int reservationId, int customerId,
                                                    int pointsUsed, decimal discountAmount,
                                                    decimal newTotal, decimal netProfit,
                                                    bool isReceiveCommission)
        {
            using (var transaction = _UnitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                    var reservation = _FarmerReservationRepository.Table
                        .FirstOrDefault(r => r.Id == reservationId);

                    if (reservation == null)
                    {
                        return Json(new { success = false, message = "الحجز غير موجود" });
                    }

                    // ===== تحديث الحجز =====
                    reservation.ReservStatus = ReservStatusEnum.Confirmed;
                    reservation.ReservationAmt = newTotal;
                    reservation.NetProfit = netProfit;
                    reservation.IsReciveCommission = isReceiveCommission;
                    //reservation.ModifiedDate = DateTime.Now;

                    _UnitOfWork.FarmerReservationRepository.Update(reservation);

                    // ===== خصم النقاط (لو موجودة) =====
                    if (pointsUsed > 0 && customerId > 0)
                    {
                        //var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                        var redeemSuccess = await loyaltyService.RedeemPointsAsync(
                            customerId: customerId,
                            points: pointsUsed,
                            reservationId: reservationId,
                            reservationType: "FarmReservation"
                        );

                        if (!redeemSuccess)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "فشل خصم النقاط" });
                        }
                    }

                    // جلب ActivityTypeId من جدول LoyaltyActivityType
                    var activityType = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                        .FirstOrDefault(a => a.Code == "FARM" && a.IsActive == true);

                    if (activityType != null)
                    {
                        // حساب النقاط
                        var points = loyaltyService.CalculatePoints(
                            activityTypeId: activityType.Id,
                            referenceType: activityType.ReferenceTable,
                            referenceId: reservation.FarmerId
                        );

                        if (points > 0)
                        {
                            // إضافة النقاط
                            loyaltyService.AddPointsAsync(
                               customerId: reservation.CustomerId,
                               activityTypeId: activityType.Id,
                               referenceType: activityType.ReferenceTable,
                               referenceId: reservation.FarmerId,
                               reservationId: reservation.Id,
                               reservationType: "SportReservation"
                           );

                            // تسجيل في سجل الحركات
                            System.Diagnostics.Debug.WriteLine($"✅ تم إضافة {points} نقطة للعميل {reservation.CustomerId} من حجز {reservation.Id}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"⚠️ لا توجد نقاط محسوبة للحجز {reservation.Id}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ لم يتم العثور على ActivityType للـ SportTypeId: {"FarmReservation"}");//reservation.SportTypeId
                    }

                    _UnitOfWork.Save();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        message = "تم تأكيد الحجز بنجاح",
                        newTotal = newTotal,
                        pointsUsed = pointsUsed,
                        discountAmount = discountAmount
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status, string reason)
        {
            LogFile logFile = new LogFile();
            try
            {
                var reservation = _UnitOfWork.FarmerReservationRepository.Table
                    .Include(r => r.Farm)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                    return Json(new { success = false, message = "الحجز غير موجود" });

                var oldStatus = reservation.ReservStatus;
                var newStatus = (ReservStatusEnum)status;

                // ============================================================
                // 1. تحديث حالة الحجز
                // ============================================================
                reservation.ReservStatus = newStatus;
                //reservation.ModifiedDate = DateTime.Now;

                if (newStatus == ReservStatusEnum.Cancelled && !string.IsNullOrEmpty(reason))
                {
                    reservation.Reason = reason;
                }

                _UnitOfWork.FarmerReservationRepository.Update(reservation);
                _UnitOfWork.Save();

                // ============================================================
                // 2. التعامل مع النقاط
                // ============================================================
                //HttpContext
                var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);

                // التأكد من وجود CustomerId
                if (reservation.CustomerId > 0)
                {
                    // ===== حالة التأكيد =====
                    if (newStatus == ReservStatusEnum.Confirmed && oldStatus != ReservStatusEnum.Confirmed)
                    {
                        // جلب ActivityTypeId من جدول LoyaltyActivityType

                        var activityType = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                            .FirstOrDefault(a => a.Code == "FARM" && a.IsActive == true);

                        if (activityType != null)
                        {
                            // حساب النقاط
                            var points = loyaltyService.CalculatePoints(
                                activityTypeId: activityType.Id,
                                referenceType: activityType.ReferenceTable,
                                referenceId: reservation.FarmerId
                            );

                            if (points > 0)
                            {
                                // إضافة النقاط
                                await loyaltyService.AddPointsAsync(
                                    customerId: reservation.CustomerId,
                                    activityTypeId: activityType.Id,
                                    referenceType: activityType.ReferenceTable,
                                    referenceId: reservation.FarmerId,
                                    reservationId: reservation.Id,
                                    reservationType: "FarmerReservation"
                                );

                                // تسجيل في سجل الحركات
                                System.Diagnostics.Debug.WriteLine($"✅ تم إضافة {points} نقطة للعميل {reservation.CustomerId} من حجز {reservation.Id}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"⚠️ لا توجد نقاط محسوبة للحجز {reservation.Id}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"⚠️ لم يتم العثور على ActivityType للـ SportTypeId: {activityType.Code}");
                        }
                    }

                    // ===== حالة الإلغاء =====
                    else if (newStatus == ReservStatusEnum.Cancelled && oldStatus == ReservStatusEnum.Confirmed)
                    {
                        // استرجاع النقاط (إلغاء الحجز)
                        await loyaltyService.ReversePointsOnCancellationAsync(reservation.Id, "FarmReservation");
                        System.Diagnostics.Debug.WriteLine($"🔄 تم استرجاع نقاط الحجز الملغى {reservation.Id}");
                    }
                }

                return Json(new { success = true, message = "تم تغيير الحالة بنجاح" });
            }
            catch (Exception e)
            {
                logFile.LogCustomInfo("Confirm FarmReservation - Inner Exception Message ", e.InnerException.ToString());
                return Json(new { success = false, message = e.Message });
            }
        }
    }
}
