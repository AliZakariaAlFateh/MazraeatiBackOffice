using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers
{
    public class SportReservationsController : BaseController
    {
        //private readonly IUnitOfWork _UnitOfWork;

        //public SportReservationsController(IUnitOfWork unitOfWork)
        //{
        //    _UnitOfWork = unitOfWork;
        //}

        //// ============================================================
        //// INDEX - عرض جميع الحجوزات
        //// ============================================================
        //public IActionResult Index(string search, int? sportTypeId, DateTime? fromDate, DateTime? toDate, ReservStatusEnum? status)
        //{
        //    ViewBag.activePage = "الحجوزات الرياضية";

        //    var query = _UnitOfWork.SportReservationRepository.Table
        //        .Include(r => r.Sport)
        //        .Include(r => r.SportType)
        //        .OrderByDescending(r => r.Id);

        //    // فلتر حسب نوع الرياضة
        //    if (sportTypeId.HasValue && sportTypeId.Value > 0)
        //    {
        //        query = (IOrderedQueryable<SportReservation>)query.Where(r => r.SportTypeId == sportTypeId.Value);
        //    }

        //    // فلتر حسب التاريخ
        //    if (fromDate.HasValue)
        //    {
        //        query = (IOrderedQueryable<SportReservation>)query.Where(r => r.ReservationDate >= fromDate.Value);
        //    }
        //    if (toDate.HasValue)
        //    {
        //        query = (IOrderedQueryable<SportReservation>)query.Where(r => r.ReservationDate <= toDate.Value);
        //    }

        //    // فلتر حسب الحالة
        //    if (status.HasValue)
        //    {
        //        query = (IOrderedQueryable<SportReservation>)query.Where(r => r.ReservStatus == status.Value);
        //    }

        //    // فلتر البحث
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = (IOrderedQueryable<SportReservation>)query.Where(r =>
        //            r.CustomerName.Contains(search) ||
        //            r.CustMobNum.Contains(search));
        //    }

        //    var model = query.ToList();

        //    // إحصائيات
        //    ViewBag.TotalReservations = model.Count();
        //    ViewBag.PendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //    ViewBag.ConfirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //    ViewBag.CancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
        //    ViewBag.SelectedSportTypeId = sportTypeId;
        //    ViewBag.Status = status;

        //    return View(model);
        //}

        //// ============================================================
        //// CREATE - GET
        //// ============================================================
        //public IActionResult Create()
        //{
        //    ViewBag.activePage = "الحجوزات الرياضية";
        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();

        //    var model = new SportReservationModel
        //    {
        //        ReservationDate = DateTime.Now.Date,
        //        StartTime = new TimeSpan(9, 0, 0),
        //        EndTime = new TimeSpan(10, 0, 0),
        //        IsMahjouzReservation = true,
        //        PersonCount = 1
        //    };
        //    //// ===== تعبئة ViewBag.Sports بكل الأقسام الرياضية =====
        //    //ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //    //    .Where(s => s.IsActive == true)
        //    //    .OrderBy(s => s.NameAr)
        //    //    .ToList();

        //    //// ===== تعبئة ViewBag.SportTypes بأنواع الرياضات =====
        //    //ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //    //    .Where(s => s.IsActive == true)
        //    //    .OrderBy(s => s.NameAr)
        //    //    .ToList();
        //    return View(model);
        //}

        //// ============================================================
        //// CREATE - POST
        //// ============================================================
        //[HttpPost]
        //public IActionResult Create(SportReservationModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // جلب بيانات المنشأة الرياضية
        //            var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
        //            if (sport == null)
        //            {
        //                ErrorNotification("القسم الرياضي غير موجود");
        //                return View(model);
        //            }

        //            // حساب عدد الساعات
        //            var totalHours = (model.EndTime - model.StartTime).TotalHours;
        //            if (totalHours <= 0)
        //            {
        //                ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
        //                return View(model);
        //            }

        //            model.TotalHours = (int)totalHours;

        //            // حساب الربح والمبلغ المتبقي
        //            model.NetProfit = model.ReservationAmt - model.CostReservationAmtOnMahjouz;
        //            model.ReservationRemainAmt = model.ReservationAmt - model.ReservationDepositAmt;

        //            // تعيين رقم جوال المالك
        //            model.MobileOwnerAppUser = sport.MobileNumber;

        //            var entity = model.ToEntity();
        //            entity.CreatedDate = DateTime.Now;
        //            entity.ReservStatus = ReservStatusEnum.Pending;

        //            _UnitOfWork.SportReservationRepository.Insert(entity);
        //            _UnitOfWork.Save();

        //            SuccessNotification("تم إضافة الحجز بنجاح");
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"خطأ: {e.Message}");
        //    }

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
        //    if (model.SportTypeId > 0)
        //    {
        //        ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //            .ToList();
        //    }

        //    return View(model);
        //}

        //// ============================================================
        //// EDIT - GET
        //// ============================================================
        //public IActionResult Edit(int id)
        //{
        //    var reservation = _UnitOfWork.SportReservationRepository
        //        .Table
        //        .Include(r => r.Sport)
        //        .Include(r => r.SportType)
        //        .FirstOrDefault(r => r.Id == id);

        //    if (reservation == null)
        //        return RedirectToAction("Index");

        //    ViewBag.activePage = "الحجوزات الرياضية";
        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
        //    ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //        .Where(s => s.SportTypeId == reservation.SportTypeId && s.IsActive == true)
        //        .ToList();


        //    // ===== تعبئة ViewBag.Sports بكل الأقسام الرياضية =====
        //    //ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //    //    .Where(s => s.IsActive == true)
        //    //    .OrderBy(s => s.NameAr)
        //    //    .ToList();

        //    // ===== تعبئة ViewBag.SportTypes بأنواع الرياضات =====
        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();



        //    var model = reservation.ToModel();
        //    return View(model);
        //}

        //// ============================================================
        //// EDIT - POST
        //// ============================================================
        //[HttpPost]
        //public IActionResult Edit(SportReservationModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // جلب بيانات المنشأة الرياضية
        //            var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
        //            if (sport == null)
        //            {
        //                ErrorNotification("القسم الرياضي غير موجود");
        //                return View(model);
        //            }

        //            // حساب عدد الساعات
        //            var totalHours = (model.EndTime - model.StartTime).TotalHours;
        //            if (totalHours <= 0)
        //            {
        //                ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
        //                return View(model);
        //            }

        //            model.TotalHours = (int)totalHours;
        //            model.NetProfit = model.ReservationAmt - model.CostReservationAmtOnMahjouz;
        //            model.ReservationRemainAmt = model.ReservationAmt - model.ReservationDepositAmt;
        //            model.MobileOwnerAppUser = sport.MobileNumber;

        //            var entity = model.ToEntity();
        //            entity.ModifiedDate = DateTime.Now;

        //            _UnitOfWork.SportReservationRepository.Update(entity);
        //            _UnitOfWork.Save();

        //            SuccessNotification("تم تحديث الحجز بنجاح");
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"خطأ: {e.Message}");
        //    }

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
        //    if (model.SportTypeId > 0)
        //    {
        //        ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //            .ToList();
        //    }

        //    return View(model);
        //}

        //// ============================================================
        //// DELETE
        //// ============================================================
        //public IActionResult Delete(int id)
        //{
        //    var reservation = _UnitOfWork.SportReservationRepository.GetById(id);
        //    if (reservation == null)
        //        return Json("السجل غير معرف");

        //    _UnitOfWork.SportReservationRepository.Delete(reservation);
        //    _UnitOfWork.Save();

        //    return Json(1);
        //}

        //// ============================================================
        //// تغيير حالة الحجز
        //// ============================================================
        //[HttpPost]
        //public IActionResult ChangeStatus(int id, ReservStatusEnum status, string reason = null)
        //{
        //    try
        //    {
        //        var reservation = _UnitOfWork.SportReservationRepository.GetById(id);
        //        if (reservation == null)
        //            return Json(new { success = false, message = "الحجز غير موجود" });

        //        reservation.ReservStatus = status;
        //        reservation.ModifiedDate = DateTime.Now;

        //        if (status == ReservStatusEnum.Cancelled && !string.IsNullOrEmpty(reason))
        //        {
        //            reservation.Reason = reason;
        //        }

        //        _UnitOfWork.SportReservationRepository.Update(reservation);
        //        _UnitOfWork.Save();

        //        return Json(new { success = true });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { success = false, message = e.Message });
        //    }
        //}

        //// ============================================================
        //// AJAX: جلب الأقسام حسب نوع الرياضة
        // ============================================================
        public IActionResult GetSportsByType(int sportTypeId)
        {
            if (sportTypeId <= 0)
                return Json(new List<object>());

            var sports = _UnitOfWork.SportRepository.Table
                .Where(s => s.SportTypeId == sportTypeId && s.IsActive == true)
                //.OrderBy(s => s.NameAr)
                .Select(s => new { s.Id, s.NameAr, s.MobileNumber })
                .ToList();

            return Json(sports);
        }
        [HttpGet]
        public IActionResult GetAllSports()
        {
            var sports = _UnitOfWork.SportRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .Select(s => new { id = s.Id, nameAr = s.NameAr })
                .ToList();

            return Json(sports);
        }
        private readonly IUnitOfWork _UnitOfWork;

        public SportReservationsController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        //Last One Work ...
        // ============================================================
        // INDEX - GET
        // ============================================================
        //[HttpGet]
        //public IActionResult Index(string search, int? sportTypeId, int? sportId,
        //    DateTime? fromDate, DateTime? toDate, TimeSpan? fromTime, TimeSpan? toTime,
        //    ReservStatusEnum? status)
        //{
        //    ViewBag.activePage = "الحجوزات الرياضية";

        //    // ===== جلب اسم الرياضة لو sportId موجود =====
        //    string sportName = "";
        //    if (sportId.HasValue && sportId.Value > 0)
        //    {
        //        var sport = _UnitOfWork.SportRepository.GetById(sportId.Value);
        //        sportName = sport?.NameAr ?? "";
        //    }
        //    ViewBag.SportName = sportName;
        //    ViewBag.SportId = sportId;

        //    var query = _UnitOfWork.SportReservationRepository.Table
        //        .Include(r => r.Sport)
        //        .Include(r => r.SportType)
        //        .AsQueryable();

        //    // فلتر حسب نوع الرياضة
        //    if (sportTypeId.HasValue && sportTypeId.Value > 0)
        //    {
        //        query = query.Where(r => r.SportTypeId == sportTypeId.Value);
        //    }

        //    // فلتر حسب القسم الرياضي
        //    if (sportId.HasValue && sportId.Value > 0)
        //    {
        //        query = query.Where(r => r.SportId == sportId.Value);
        //    }

        //    // فلتر حسب التاريخ
        //    if (fromDate.HasValue)
        //    {
        //        query = query.Where(r => r.ReservationDate >= fromDate.Value);
        //    }
        //    if (toDate.HasValue)
        //    {
        //        query = query.Where(r => r.ReservationDate <= toDate.Value);
        //    }

        //    // ===== فلتر حسب الوقت =====
        //    if (fromTime.HasValue)
        //    {
        //        query = query.Where(r => r.StartTime >= fromTime.Value);
        //    }
        //    if (toTime.HasValue)
        //    {
        //        query = query.Where(r => r.EndTime <= toTime.Value);
        //    }

        //    // فلتر حسب الحالة
        //    if (status.HasValue)
        //    {
        //        query = query.Where(r => r.ReservStatus == status.Value);
        //    }

        //    // فلتر البحث
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(r =>
        //            r.CustomerName.Contains(search) ||
        //            r.CustMobNum.Contains(search) ||
        //            (r.Sport != null && r.Sport.NameAr.Contains(search)) ||
        //            (r.SportType != null && r.SportType.NameAr.Contains(search)));
        //    }

        //    var model = query.OrderByDescending(r => r.Id).ToList();

        //    // إحصائيات
        //    ViewBag.TotalReservations = model.Count();
        //    ViewBag.PendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
        //    ViewBag.ConfirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
        //    ViewBag.CancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

        //    // للفلاتر
        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();

        //    ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();

        //    ViewBag.SelectedSportTypeId = sportTypeId;
        //    ViewBag.SelectedSportId = sportId;
        //    ViewBag.Status = status;
        //    ViewBag.FromDate = fromDate;
        //    ViewBag.ToDate = toDate;
        //    ViewBag.FromTime = fromTime;
        //    ViewBag.ToTime = toTime;

        //    return View(model);
        //}

        //// ============================================================
        //// INDEX - POST (بحث)
        //// ============================================================
        //[HttpPost]
        //public IActionResult Index(string search, int? sportTypeId, int? sportId,
        //    DateTime? fromDate, DateTime? toDate, string fromTime, string toTime, int? status)
        //{
        //    TimeSpan? fromTimeSpan = null;
        //    TimeSpan? toTimeSpan = null;

        //    if (!string.IsNullOrEmpty(fromTime))
        //    {
        //        fromTimeSpan = TimeSpan.Parse(fromTime);
        //    }
        //    if (!string.IsNullOrEmpty(toTime))
        //    {
        //        toTimeSpan = TimeSpan.Parse(toTime);
        //    }

        //    ReservStatusEnum? statusEnum = status.HasValue ? (ReservStatusEnum)status.Value : (ReservStatusEnum?)null;

        //    return RedirectToAction("Index", new
        //    {
        //        search = search,
        //        sportTypeId = sportTypeId,
        //        sportId = sportId,
        //        fromDate = fromDate,
        //        toDate = toDate,
        //        fromTime = fromTimeSpan,
        //        toTime = toTimeSpan,
        //        status = statusEnum
        //    });
        //}

        //// ============================================================
        //// GET SPORTS BY TYPE (AJAX)
        //// ============================================================
        //[HttpGet]
        //public IActionResult GetSportsByType(int sportTypeId)
        //{
        //    if (sportTypeId <= 0)
        //        return Json(new List<object>());

        //    var sports = _UnitOfWork.SportRepository.Table
        //        .Where(s => s.SportTypeId == sportTypeId && s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .Select(s => new { id = s.Id, nameAr = s.NameAr })
        //        .ToList();

        //    return Json(sports);
        //}
        ////Last One Work ...



        // ============================================================
        // INDEX - GET
        // ============================================================
        [HttpGet]
        public IActionResult Index(string search, int? sportTypeId, int? sportId,
            DateTime? fromDate, DateTime? toDate, TimeSpan? fromTime, TimeSpan? toTime,
            ReservStatusEnum? status)
        {
            ViewBag.activePage = "الحجوزات الرياضية";

            // ===== جلب اسم الرياضة لو sportId موجود =====
            string sportName = "";
            if (sportId.HasValue && sportId.Value > 0)
            {
                var sport = _UnitOfWork.SportRepository.GetById(sportId.Value);
                sportName = sport?.NameAr ?? "";
            }
            ViewBag.SportName = sportName;
            ViewBag.SportId = sportId;

            var query = _UnitOfWork.SportReservationRepository.Table
                .Include(r => r.Sport)
                .Include(r => r.SportType)
                .AsQueryable();

            // فلتر حسب نوع الرياضة
            if (sportTypeId.HasValue && sportTypeId.Value > 0)
            {
                query = query.Where(r => r.SportTypeId == sportTypeId.Value);
            }

            // فلتر حسب القسم الرياضي
            if (sportId.HasValue && sportId.Value > 0)
            {
                query = query.Where(r => r.SportId == sportId.Value);
            }

            // فلتر حسب التاريخ
            if (fromDate.HasValue)
            {
                query = query.Where(r => r.ReservationDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                var endDate = toDate.Value.AddDays(1);
                query = query.Where(r => r.ReservationDate < endDate);
            }

            // ===== فلتر حسب الوقت (بمنطق التقاطع) =====
            if (fromTime.HasValue && toTime.HasValue)
            {
                // الحجز يتقاطع مع الفترة المحددة
                // StartTime <= toTime AND EndTime >= fromTime
                query = query.Where(r => r.StartTime <= toTime.Value && r.EndTime >= fromTime.Value);
            }
            else if (fromTime.HasValue)
            {
                // فقط من وقت معين (بداية الحجز بعد أو تساوي fromTime)
                query = query.Where(r => r.StartTime >= fromTime.Value);
            }
            else if (toTime.HasValue)
            {
                // فقط إلى وقت معين (نهاية الحجز قبل أو تساوي toTime)
                query = query.Where(r => r.EndTime <= toTime.Value);
            }

            // فلتر حسب الحالة
            if (status.HasValue)
            {
                query = query.Where(r => r.ReservStatus == status.Value);
            }

            // فلتر البحث
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                query = query.Where(r =>
                    r.CustomerName.Contains(search) ||
                    r.CustMobNum.Contains(search) ||
                    (r.Sport != null && r.Sport.NameAr.Contains(search)) ||
                    (r.SportType != null && r.SportType.NameAr.Contains(search)));
            }

            var model = query.OrderByDescending(r => r.ReservationDate)
                             .ThenBy(r => r.StartTime)
                             .ToList();

            // إحصائيات
            ViewBag.TotalReservations = model.Count();
            ViewBag.PendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
            ViewBag.ConfirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
            ViewBag.CancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

            // ===== للفلاتر =====
            // أنواع الرياضات
            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            // الرياضات (كلها)
            ViewBag.Sports = _UnitOfWork.SportRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            // ===== حفظ قيم الفلاتر المختارة =====
            ViewBag.search = search;
            ViewBag.SelectedSportTypeId = sportTypeId;
            ViewBag.SelectedSportId = sportId;
            ViewBag.Status = status;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.FromTime = fromTime;
            ViewBag.ToTime = toTime;

            return View(model);
        }

        // ============================================================
        // INDEX - POST (بحث)
        // ============================================================
        [HttpPost]
        public IActionResult Index(string search, int? sportTypeId, int? sportFilterId,
            DateTime? fromDate, DateTime? toDate, string fromTime, string toTime, int? status)
        {
            // تحويل الـ Time strings إلى TimeSpan
            TimeSpan? fromTimeSpan = null;
            TimeSpan? toTimeSpan = null;

            if (!string.IsNullOrEmpty(fromTime))
            {
                if (TimeSpan.TryParse(fromTime, out var parsedFrom))
                    fromTimeSpan = parsedFrom;
            }

            if (!string.IsNullOrEmpty(toTime))
            {
                if (TimeSpan.TryParse(toTime, out var parsedTo))
                    toTimeSpan = parsedTo;
            }

            // تحويل الـ status int إلى Enum
            ReservStatusEnum? statusEnum = null;
            if (status.HasValue)
            {
                statusEnum = (ReservStatusEnum)status.Value;
            }

            // إعادة التوجيه إلى GET مع نفس الفلاتر
            return RedirectToAction("Index", new
            {
                search = search,
                sportTypeId = sportTypeId,
                sportId = sportFilterId,
                fromDate = fromDate,
                toDate = toDate,
                fromTime = fromTimeSpan,
                toTime = toTimeSpan,
                status = statusEnum
            });
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        //public IActionResult Create(int? sportId)
        //{
        //    ViewBag.activePage = "الحجوزات الرياضية";

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();

        //    var model = new SportReservationModel
        //    {
        //        ReservationDate = DateTime.Now.Date,
        //        StartTime = new TimeSpan(9, 0, 0),
        //        EndTime = new TimeSpan(10, 0, 0),
        //        IsMahjouzReservation = true,
        //        PersonCount = 1,
        //    };
        //    model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
        //    // لو فيه SportId محدد، اجيب بياناته
        //    if (sportId.HasValue && sportId.Value > 0)
        //    {
        //        var sport = _UnitOfWork.SportRepository.GetById(sportId.Value);
        //        if (sport != null)
        //        {
        //            model.SportId = sport.Id;
        //            model.SportTypeId = sport.SportTypeId;
        //            ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //                .Where(s => s.SportTypeId == sport.SportTypeId && s.IsActive == true)
        //                .OrderBy(s => s.NameAr)
        //                .ToList();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Sports = new List<Sport>();
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult Create(SportReservationModel model)
        //{
        //    model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
        //            if (sport == null)
        //            {
        //                ErrorNotification("القسم الرياضي غير موجود");
        //                ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //                                .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //                                .OrderBy(s => s.NameAr)
        //                                .ToList();
        //                ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //                                    .Where(s => s.IsActive == true)
        //                                    .OrderBy(s => s.NameAr)
        //                                    .ToList();
        //                return View(model);
        //            }

        //            var totalHours = (model.EndTime - model.StartTime).TotalHours;
        //            if (totalHours <= 0)
        //            {
        //                ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
        //                ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //                                .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //                                .OrderBy(s => s.NameAr)
        //                                .ToList();
        //                ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //                                    .Where(s => s.IsActive == true)
        //                                    .OrderBy(s => s.NameAr)
        //                                    .ToList();
        //                return View(model);
        //            }

        //            // ============================================================
        //            // ✅ التحقق من عدم تعارض المواعيد
        //            // ============================================================
        //            var isTimeSlotAvailable = !_UnitOfWork.SportReservationRepository.Table
        //                .Any(r => r.SportId == model.SportId &&
        //                          r.ReservationDate == model.ReservationDate &&
        //                          r.ReservStatus != ReservStatusEnum.Cancelled && // استثناء الملغى
        //                          (
        //                              (model.StartTime >= r.StartTime && model.StartTime < r.EndTime) ||
        //                              (model.EndTime > r.StartTime && model.EndTime <= r.EndTime) ||
        //                              (model.StartTime <= r.StartTime && model.EndTime >= r.EndTime)
        //                          ));

        //            if (!isTimeSlotAvailable)
        //            {
        //                ErrorNotification("هذا الموعد محجوز مسبقاً، يرجى اختيار وقت آخر");
        //                ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //                                .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //                                .OrderBy(s => s.NameAr)
        //                                .ToList();
        //                ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //                                    .Where(s => s.IsActive == true)
        //                                    .OrderBy(s => s.NameAr)
        //                                    .ToList();
        //                return View(model);
        //            }

        //            // ============================================================
        //            // ✅ التحقق من أن وقت البداية أكبر من الوقت الحالي (اختياري)
        //            // ============================================================
        //            if (model.ReservationDate == DateTime.Now.Date && model.StartTime <= DateTime.Now.TimeOfDay)
        //            {
        //                ErrorNotification("لا يمكن الحجز في وقت مضى، يرجى اختيار وقت مستقبلي");
        //                ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //                                .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //                                .OrderBy(s => s.NameAr)
        //                                .ToList();
        //                ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //                                    .Where(s => s.IsActive == true)
        //                                    .OrderBy(s => s.NameAr)
        //                                    .ToList();
        //                return View(model);
        //            }

        //            model.TotalHours = (int)totalHours;
        //            model.MobileOwnerAppUser = sport.MobileNumber;

        //            var entity = model.ToEntity();
        //            entity.CreatedDate = DateTime.Now;
        //            entity.ReservStatus = ReservStatusEnum.Pending;

        //            _UnitOfWork.SportReservationRepository.Insert(entity);
        //            _UnitOfWork.Save();

        //            SuccessNotification("تم إضافة الحجز بنجاح");
        //            return RedirectToAction("Index", new { sportId = model.SportId });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"خطأ: {e.Message}");
        //    }

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();

        //    if (model.SportTypeId > 0)
        //    {
        //        ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //            .OrderBy(s => s.NameAr)
        //            .ToList();
        //    }

        //    return View(model);
        //}


        public IActionResult Create(int? sportId)
        {
            ViewBag.activePage = "الحجوزات الرياضية";

            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            var model = new SportReservationModel
            {
                ReservationDate = DateTime.Now.Date,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                IsMahjouzReservation = true,
                PersonCount = 1,
                ReservationAmt = 0
            };
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();

            // ===== 🆕 جلب نقاط العميل (لو موجود) =====
            if (model.CustomerId > 0)
            {
                var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                    .FirstOrDefault(a => a.CustomerId == model.CustomerId);

                model.CustomerAvailablePoints = account?.AvailablePoints ?? 0;
            }

            // لو فيه SportId محدد، اجيب بياناته
            if (sportId.HasValue && sportId.Value > 0)
            {
                var sport = _UnitOfWork.SportRepository.GetById(sportId.Value);
                if (sport != null)
                {
                    model.SportId = sport.Id;
                    model.SportTypeId = sport.SportTypeId;
                    ViewBag.Sports = _UnitOfWork.SportRepository.Table
                        .Where(s => s.SportTypeId == sport.SportTypeId && s.IsActive == true)
                        .OrderBy(s => s.NameAr)
                        .ToList();
                }
            }
            else
            {
                ViewBag.Sports = new List<Sport>();
            }

            // ===== 🆕 قواعد صرف النقاط للـ Dropdown =====
            ViewBag.RedeemRules = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                .Where(r => r.IsActive == true)
                .OrderBy(r => r.Points)
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SportReservationModel model)
        {
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
                    if (sport == null)
                    {
                        ErrorNotification("القسم الرياضي غير موجود");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    var totalHours = (model.EndTime - model.StartTime).TotalHours;
                    if (totalHours <= 0)
                    {
                        ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    // ============================================================
                    // ✅ التحقق من عدم تعارض المواعيد
                    // ============================================================
                    var isTimeSlotAvailable = !_UnitOfWork.SportReservationRepository.Table
                        .Any(r => r.SportId == model.SportId &&
                                  r.ReservationDate == model.ReservationDate &&
                                  r.ReservStatus != ReservStatusEnum.Cancelled &&
                                  (
                                      (model.StartTime >= r.StartTime && model.StartTime < r.EndTime) ||
                                      (model.EndTime > r.StartTime && model.EndTime <= r.EndTime) ||
                                      (model.StartTime <= r.StartTime && model.EndTime >= r.EndTime)
                                  ));

                    if (!isTimeSlotAvailable)
                    {
                        ErrorNotification("هذا الموعد محجوز مسبقاً، يرجى اختيار وقت آخر");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    // ============================================================
                    // ✅ التحقق من أن وقت البداية أكبر من الوقت الحالي (اختياري)
                    // ============================================================
                    if (model.ReservationDate == DateTime.Now.Date && model.StartTime <= DateTime.Now.TimeOfDay)
                    {
                        ErrorNotification("لا يمكن الحجز في وقت مضى، يرجى اختيار وقت مستقبلي");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    // ============================================================
                    // 🆕 تطبيق خصم النقاط
                    // ============================================================
                    if (model.UseLoyaltyPoints && model.RedeemPoints > 0)
                    {
                        // 1. التحقق من رصيد العميل
                        var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                            .FirstOrDefault(a => a.CustomerId == model.CustomerId);

                        if (account == null || account.AvailablePoints < model.RedeemPoints)
                        {
                            ErrorNotification("رصيد النقاط غير كافٍ");
                            ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                                .Where(s => s.IsActive == true)
                                                .OrderBy(s => s.NameAr)
                                                .ToList();
                            return View(model);
                        }

                        // 2. حساب قيمة الخصم حسب القاعدة
                        var redeemRule = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                            .Where(r => r.IsActive == true && r.Points <= model.RedeemPoints)
                            .OrderByDescending(r => r.Points)
                            .FirstOrDefault();

                        if (redeemRule != null)
                        {
                            var discountAmount = (model.RedeemPoints / redeemRule.Points) * redeemRule.DiscountAmount;
                            model.DiscountAmount = discountAmount;
                            //model.ReservationAmt -= discountAmount; // خصم من المبلغ الكلي
                        }
                        else
                        {
                            ErrorNotification("لا توجد قاعدة صرف تناسب عدد النقاط المطلوبة");
                            ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                                .Where(s => s.IsActive == true)
                                                .OrderBy(s => s.NameAr)
                                                .ToList();
                            return View(model);
                        }
                    }

                    model.TotalHours = (int)totalHours;
                    model.MobileOwnerAppUser = sport.MobileNumber;

                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;
                    entity.ReservStatus = ReservStatusEnum.Pending;

                    _UnitOfWork.SportReservationRepository.Insert(entity);
                    _UnitOfWork.Save();

                    int reservationId = entity.Id;

                    // ============================================================
                    // 🆕 خصم النقاط فعلياً (تسجيل في الجداول)
                    // ============================================================
                    if (model.UseLoyaltyPoints && model.RedeemPoints > 0)
                    {
                        var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);

                        // خصم النقاط
                        var redeemSuccess = await loyaltyService.RedeemPointsAsync(
                            customerId: model.CustomerId,
                            points: model.RedeemPoints,
                            reservationId: reservationId,
                            reservationType: "SportReservation"
                        );

                        if (!redeemSuccess)
                        {
                            // لو فشل الخصم، نحذف الحجز ونرجع خطأ
                            _UnitOfWork.SportReservationRepository.Delete(entity);
                            _UnitOfWork.Save();
                            ErrorNotification("حدث خطأ أثناء خصم النقاط");
                            ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                                .Where(s => s.IsActive == true)
                                                .OrderBy(s => s.NameAr)
                                                .ToList();
                            return View(model);
                        }
                    }

                    SuccessNotification("تم إضافة الحجز بنجاح");
                    return RedirectToAction("Index", new { sportId = model.SportId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
            }

            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            if (model.SportTypeId > 0)
            {
                ViewBag.Sports = _UnitOfWork.SportRepository.Table
                    .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                    .OrderBy(s => s.NameAr)
                    .ToList();
            }

            // ===== 🆕 قواعد الصرف للـ View عند الرجوع =====
            ViewBag.RedeemRules = _UnitOfWork.LoyaltyRedeemRuleRepository.Table
                .Where(r => r.IsActive == true)
                .OrderBy(r => r.Points)
                .ToList();

            return View(model);
        }

        // ============================================================
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            var reservation = _UnitOfWork.SportReservationRepository
                .Table
                .Include(r => r.Sport)
                .Include(r => r.SportType)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "الحجوزات الرياضية";
            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            ViewBag.Sports = _UnitOfWork.SportRepository.Table
                .Where(s => s.SportTypeId == reservation.SportTypeId && s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();
            var model = reservation.ToModel();
            model.CustomerId = (int)reservation.CustomerId;
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            //model.CustomerId=_UnitOfWork.SportReservationRepository
            //    .Table
            //    .Select(r => r.CustomerId)
            //    .FirstOrDefault(r => r.Id== id);
            return View(model);
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        //[HttpPost]
        //public IActionResult Edit(SportReservationModel model)
        //{
        //    model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
        //    try
        //    {
        //        //ModelState.Remove("SportDesc");

        //        if (ModelState.IsValid)
        //        {
        //            var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
        //            if (sport == null)
        //            {
        //                ErrorNotification("القسم الرياضي غير موجود");
        //                return View(model);
        //            }

        //            var totalHours = (model.EndTime - model.StartTime).TotalHours;
        //            if (totalHours <= 0)
        //            {
        //                ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
        //                return View(model);
        //            }
        //            var Reservation = _UnitOfWork.SportReservationRepository.Table.Where(R => R.Id == model.Id).ToList().FirstOrDefault();
        //            model.CreatedDate = Reservation.CreatedDate;
        //            model.TotalHours = (int)totalHours;
        //            //model.NetProfit = model.ReservationAmt - model.CostReservationAmtOnMahjouz;
        //            //model.ReservationRemainAmt = model.ReservationAmt - model.ReservationDepositAmt;
        //            model.MobileOwnerAppUser = sport.MobileNumber;

        //            var entity = model.ToEntity();
        //            entity.ModifiedDate = DateTime.Now;

        //            _UnitOfWork.SportReservationRepository.Update(entity);
        //            _UnitOfWork.Save();

        //            SuccessNotification("تم تحديث الحجز بنجاح");
        //            return RedirectToAction("Index", new { sportId = model.SportId });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"خطأ: {e.Message}");
        //    }

        //    ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
        //        .Where(s => s.IsActive == true)
        //        .OrderBy(s => s.NameAr)
        //        .ToList();

        //    if (model.SportTypeId > 0)
        //    {
        //        ViewBag.Sports = _UnitOfWork.SportRepository.Table
        //            .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
        //            .OrderBy(s => s.NameAr)
        //            .ToList();
        //    }

        //    return View(model);
        //}
        [HttpPost]
        public IActionResult Edit(SportReservationModel model)
        {
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
                    if (sport == null)
                    {
                        ErrorNotification("القسم الرياضي غير موجود");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    var totalHours = (model.EndTime - model.StartTime).TotalHours;
                    if (totalHours <= 0)
                    {
                        ErrorNotification("وقت النهاية يجب أن يكون بعد وقت البداية");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    // ============================================================
                    // ✅ التحقق من عدم تعارض المواعيد (مع استثناء الحجز الحالي)
                    // ============================================================
                    var isTimeSlotAvailable = !_UnitOfWork.SportReservationRepository.Table
                        .Any(r => r.SportId == model.SportId &&
                                  r.ReservationDate == model.ReservationDate &&
                                  r.Id != model.Id && // استثناء الحجز الحالي
                                  r.ReservStatus != ReservStatusEnum.Cancelled && // استثناء الملغى
                                  (
                                      (model.StartTime >= r.StartTime && model.StartTime < r.EndTime) ||
                                      (model.EndTime > r.StartTime && model.EndTime <= r.EndTime) ||
                                      (model.StartTime <= r.StartTime && model.EndTime >= r.EndTime)
                                  ));

                    if (!isTimeSlotAvailable)
                    {
                        ErrorNotification("هذا الموعد محجوز مسبقاً، يرجى اختيار وقت آخر");
                        ViewBag.Sports = _UnitOfWork.SportRepository.Table
                                        .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                                        .OrderBy(s => s.NameAr)
                                        .ToList();
                        ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                                            .Where(s => s.IsActive == true)
                                            .OrderBy(s => s.NameAr)
                                            .ToList();
                        return View(model);
                    }

                    var Reservation = _UnitOfWork.SportReservationRepository.Table.Where(R => R.Id == model.Id).ToList().FirstOrDefault();
                    model.CreatedDate = Reservation.CreatedDate;
                    model.TotalHours = (int)totalHours;
                    model.MobileOwnerAppUser = sport.MobileNumber;

                    var entity = model.ToEntity();
                    entity.ModifiedDate = DateTime.Now;

                    _UnitOfWork.SportReservationRepository.Update(entity);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث الحجز بنجاح");
                    return RedirectToAction("Index", new { sportId = model.SportId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
            }

            ViewBag.SportTypes = _UnitOfWork.SportTypeRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.NameAr)
                .ToList();

            if (model.SportTypeId > 0)
            {
                ViewBag.Sports = _UnitOfWork.SportRepository.Table
                    .Where(s => s.SportTypeId == model.SportTypeId && s.IsActive == true)
                    .OrderBy(s => s.NameAr)
                    .ToList();
            }

            return View(model);
        }
        // ============================================================
        // CHANGE STATUS (AJAX)
        // ============================================================
        //[HttpPost]
        //public IActionResult ChangeStatus(int id, int status, string reason)
        //{
        //    try
        //    {
        //        var reservation = _UnitOfWork.SportReservationRepository.GetById(id);
        //        if (reservation == null)
        //            return Json(new { success = false, message = "الحجز غير موجود" });

        //        var newStatus = (ReservStatusEnum)status;
        //        reservation.ReservStatus = newStatus;
        //        reservation.ModifiedDate = DateTime.Now;

        //        if (newStatus == ReservStatusEnum.Cancelled && !string.IsNullOrEmpty(reason))
        //        {
        //            reservation.Reason = reason;
        //        }

        //        _UnitOfWork.SportReservationRepository.Update(reservation);
        //        _UnitOfWork.Save();

        //        return Json(new { success = true, message = "تم تغيير الحالة بنجاح" });
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { success = false, message = e.Message });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status, string reason)
        {
            LogFile logFile = new LogFile();
            try
            {
                var reservation = _UnitOfWork.SportReservationRepository.Table
                    .Include(r => r.Sport)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                    return Json(new { success = false, message = "الحجز غير موجود" });

                var oldStatus = reservation.ReservStatus;
                var newStatus = (ReservStatusEnum)status;

                // ============================================================
                // 1. تحديث حالة الحجز
                // ============================================================
                reservation.ReservStatus = newStatus;
                reservation.ModifiedDate = DateTime.Now;

                if (newStatus == ReservStatusEnum.Cancelled && !string.IsNullOrEmpty(reason))
                {
                    reservation.Reason = reason;
                }

                _UnitOfWork.SportReservationRepository.Update(reservation);
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
                            .FirstOrDefault(a => a.SportTypeId == reservation.SportTypeId && a.IsActive == true);

                        if (activityType != null)
                        {
                            // حساب النقاط
                            var points = loyaltyService.CalculatePoints(
                                activityTypeId: activityType.Id,
                                referenceType: activityType.ReferenceTable,
                                referenceId: reservation.SportId
                            );

                            if (points > 0)
                            {
                                // إضافة النقاط
                                await loyaltyService.AddPointsAsync(
                                    customerId: (int)reservation.CustomerId,
                                    activityTypeId: activityType.Id,
                                    referenceType: activityType.ReferenceTable,
                                    referenceId: reservation.SportId,
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
                            System.Diagnostics.Debug.WriteLine($"⚠️ لم يتم العثور على ActivityType للـ SportTypeId: {reservation.SportTypeId}");
                        }
                    }

                    // ===== حالة الإلغاء =====
                    else if (newStatus == ReservStatusEnum.Cancelled && oldStatus == ReservStatusEnum.Confirmed)
                    {
                        // استرجاع النقاط (إلغاء الحجز)
                        await loyaltyService.ReversePointsOnCancellationAsync(reservation.Id, "SportReservation");
                        System.Diagnostics.Debug.WriteLine($"🔄 تم استرجاع نقاط الحجز الملغى {reservation.Id}");
                    }
                }

                return Json(new { success = true, message = "تم تغيير الحالة بنجاح" });
            }
            catch (Exception e)
            {
                logFile.LogCustomInfo("Confirm SportReservation - Inner Exception Message ", e.InnerException.ToString());
                return Json(new { success = false, message = e.Message });
            }
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var reservation = _UnitOfWork.SportReservationRepository.GetById(id);
            if (reservation == null)
                return Json("السجل غير معرف");

            _UnitOfWork.SportReservationRepository.Delete(reservation);
            _UnitOfWork.Save();
            return Json(1);
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

        [HttpPost]
        public IActionResult ConfirmReservation(int reservationId, int customerId,
                                                int pointsUsed, decimal discountAmount,
                                                decimal newTotal, decimal netProfit,
                                                bool isReceiveCommission)
        {
            // ✅ استخدام BeginTransaction العادي مع IsolationLevel
            using (var transaction = _UnitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);

                    // ============================================================
                    // الخطوة 1: تحديث الحجز
                    // ============================================================
                    var reservation = _UnitOfWork.SportReservationRepository
                        .Table
                        .Include(r => r.Customer)
                        .Include(r => r.Sport)
                        .FirstOrDefault(r => r.Id == reservationId);

                    if (reservation == null)
                    {
                        return Json(new { success = false, message = "الحجز غير موجود" });
                    }

                    reservation.ReservStatus = ReservStatusEnum.Confirmed;
                    reservation.ReservationAmt = newTotal;
                    reservation.NetProfit = netProfit;
                    reservation.IsReceiveCommession = isReceiveCommission;
                    //reservation.ConfirmedDate = DateTime.Now;

                    _UnitOfWork.SportReservationRepository.Update(reservation);
                    //saveChanges
                    // ============================================================
                    // الخطوة 2: خصم النقاط (Redeem) - Synchronous
                    // ============================================================
                    if (pointsUsed > 0)
                    {
                        // ✅ استخدام GetAwaiter().GetResult() عشان نحول async لـ sync
                        var redeemSuccess = loyaltyService.RedeemPointsAsync(
                            customerId: customerId,
                            points: pointsUsed,
                            reservationId: reservationId,
                            reservationType: "SportReservation"
                        ).GetAwaiter().GetResult();

                        if (!redeemSuccess)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "فشل خصم النقاط" });
                        }
                    }

                    // ============================================================
                    // الخطوة 3: إضافة نقاط على الحجز (Earn)
                    // ============================================================
                    

                    // جلب ActivityTypeId من جدول LoyaltyActivityType
                    var activityType = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                        .FirstOrDefault(a => a.SportTypeId == reservation.SportTypeId && a.IsActive == true);

                    if (activityType != null)
                    {
                        // حساب النقاط
                        var points = loyaltyService.CalculatePoints(
                            activityTypeId: activityType.Id,
                            referenceType: activityType.ReferenceTable,
                            referenceId: reservation.SportId
                        );

                        if (points > 0)
                        {
                            // إضافة النقاط
                             loyaltyService.AddPointsAsync(
                                customerId: (int)reservation.CustomerId,
                                activityTypeId: activityType.Id,
                                referenceType: activityType.ReferenceTable,
                                referenceId: reservation.SportId,
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
                        System.Diagnostics.Debug.WriteLine($"⚠️ لم يتم العثور على ActivityType للـ SportTypeId: {reservation.SportTypeId}");
                    }


                    _UnitOfWork.Save();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        message = "تم تأكيد الحجز بنجاح",
                        newTotal = newTotal,
                        pointsUsed = pointsUsed,
                        discountAmount = discountAmount//,
                        //earnedPoints = earnedPoints
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }

        // ============================================================
        // دالة مساعدة لجلب معرف المستخدم الحالي
        // ============================================================
        private int GetCurrentAdminId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        [HttpGet]
        public IActionResult GetConfirmReservationData(int id)
        {
            try
            {
                var reservation = _UnitOfWork.SportReservationRepository
                    .Table
                    .Include(r => r.Customer)
                    .Include(r => r.Sport)
                    .Include(r => r.SportType)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                {
                    return Json(new { success = false, message = "الحجز غير موجود" });
                }

                // ===== جلب نقاط العميل =====
                var account = _UnitOfWork.CustomerLoyaltyAccountRepository
                    .Table
                    .FirstOrDefault(a => a.CustomerId == reservation.CustomerId);

                int availablePoints = account?.AvailablePoints ?? 0;

                // ===== جلب المستوى الحالي =====
                string tierName = "لا يوجد مستوى";
                string tierIcon = "";
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

                // ===== حساب نقاط الحجز المستحقة =====
                var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                var earnedPoints = loyaltyService.CalculateReservationEarnedPoints(
                    customerId: (int)reservation.CustomerId,
                    bookingType: "Sport",
                    referenceId: (int)reservation.SportId
                );

                var viewModel = new ConfirmReservationSportViewModel
                {
                    ReservationId = reservation.Id,
                    CustomerId = reservation.Customer.Id,
                    CustomerName = reservation.Customer?.FullName ?? "",
                    CustomerPhone = reservation.Customer?.MobileNumber ?? "",
                    SportTypeName = reservation.SportType?.NameAr ?? "",
                    SportName = reservation.Sport?.NameAr ?? "",
                    ReservationDate = reservation.ReservationDate,
                    StartTime = reservation.StartTime,
                    EndTime = reservation.EndTime,
                    TotalHours = reservation.TotalHours,
                    OriginalAmount = reservation.ReservationAmt,
                    NetProfit = reservation.NetProfit,
                    CustomerAvailablePoints = availablePoints,
                    CurrentTierName = tierName,
                    TierIcon = tierIcon,
                    EarnedPoints = earnedPoints  // ✅ إضافة النقاط المستحقة
                };

                // ===== إرسال قواعد الخصم للـ View =====
                var redeemRules = _UnitOfWork.LoyaltyRedeemRuleRepository
                    .Table
                    .Where(r => r.IsActive)
                    .Select(r => new { r.Points, r.DiscountAmount })
                    .ToList();

                ViewBag.RedeemRules = redeemRules;

                return PartialView("_ConfirmReservationPopup", viewModel);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }





    }
}
