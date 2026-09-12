using DocumentFormat.OpenXml.Wordprocessing;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Configuration.Enums;
using MazraeatiBackOffice.Core.CottageCore;
using MazraeatiBackOffice.Core.FarmCore;
using MazraeatiBackOffice.Core.SportCore;
using MazraeatiBackOffice.Core.SystemCore;
using MazraeatiBackOffice.Core.UserManagementCore;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.CottageModel;
using MazraeatiBackOffice.Models.FarmModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers.CottageControllers
{
    public class CottageReservationsController : BaseController
    {
        private readonly IRepository<Cottage> _CottageRepository;
        private readonly IRepository<CottageReservation> _CottageReservationRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<DeviceToken> _deviceToken;
        private readonly IRepository<AppUser> _appUser;
        private readonly FirebaseNotificationService _notificationService;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public object D { get; private set; }

        public CottageReservationsController(IRepository<Cottage> cottageRepository,
            IRepository<CottageReservation> CottageReservationRepository,
            IRepository<LookupValue> LookupValueRepository, IRepository<DeviceToken> DeviceToken,
            FirebaseNotificationService notificationService, IRepository<AppUser> AppUser,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            _CottageRepository = cottageRepository;
            _CottageReservationRepository = CottageReservationRepository;
            _LookupValueRepository = LookupValueRepository;
            _deviceToken = DeviceToken;
            _appUser = AppUser;
            _notificationService = notificationService;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public CottageReservationModel NewFillModel(CottageReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Cottages = _UnitOfWork.CottageRepository.Table.ToList();
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            return model;
        }

        public CottageReservationModel EditFillModel(CottageReservationModel model)
        {
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Cottages = _UnitOfWork.CottageRepository.Table.ToList();
            model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
            return model;
        }


        [HttpGet]
        public IActionResult Index(int? cottageId, DateTime? fromDate, DateTime? toDate)
        {
            LogFile logFile = new LogFile();
            try
            {
                var lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .ToList();
                var lookupDict = lookupValues.ToDictionary(l => l.Id, l => l.ValueAr);

                var query = _CottageReservationRepository.Table
                    .AsNoTracking()
                    .AsQueryable();

                if (cottageId.HasValue && cottageId.Value > 0)
                {
                    query = query.Where(t => t.CottageId == cottageId.Value);
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
                    .Select(r => new CottageReservationModel
                    {
                        Id = r.Id,
                        CottageId = (int)r.CottageId,
                        CustomerId = (int)r.CottageId,
                        ReservationTypeId = r.ReservationTypeId,
                        ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
                        ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
                        CustMobNum = r.CustMobNum ?? string.Empty,
                        CustomerName = r.CustomerName ?? string.Empty,
                        PersonCount = r.PersonCount,
                        CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
                        ReservationAmt = r.ReservationAmt,
                        NetProfit = r.NetProfit,
                        ReservationDepositAmt = r.ReservationDepositAmt,
                        ReservationRemainAmt = r.ReservationRemainAmt,
                        Note = r.Note ?? string.Empty,
                        MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
                        IsMahjouzReservation = r.IsMahjouzReservation,
                        IsReceiveCommession = r.IsReceiveCommession,
                        //AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
                        CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
                        Reason = r.Reason ?? string.Empty,
                        ReservStatus = r.ReservStatus
                    })
                    .ToList();

                var totalCount = model.Count;
                var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
                var acceptedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Accepted);
                var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
                var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

                string cottageName = "جميع الأكواخ";
                if (cottageId.HasValue && cottageId.Value > 0)
                {
                    var cottage = _CottageRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == cottageId.Value);
                    cottageName = cottage?.NameAr ?? "كوخ غير محددة";
                }

                var reservationTypes = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .OrderBy(l => l.ValueAr)
                    .ToList();

                var cottages = _CottageRepository.Table
                    .OrderBy(f => f.NameAr)
                    .ToList();

                ViewBag.activePage = "حجوزات الأكواخ";
                ViewBag.CottageName = cottageName;
                ViewBag.CottageId = cottageId;
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
                ViewBag.AcceptedCount = acceptedCount;
                ViewBag.ConfirmedCount = confirmedCount;
                ViewBag.CancelledCount = cancelledCount;
                ViewBag.Cottages = cottages;
                ViewBag.ReservationTypes = reservationTypes;

                return View(model);
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while loading reservations: {e.Message}");
                logFile.LogCustomInfo("Index CottageReservations - Exception Message ", e.Message);
                logFile.LogCustomInfo("Index CottageReservations - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Index CottageReservations - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");
                return View(new List<CottageReservationModel>());
            }
        }
        // ============================================================
        // POST: Index (مع فلتر محسن + تاريخ من-إلى)
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int? cottageId, string search, int? MonthId, int? YearId,
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
                var query = _CottageReservationRepository.Table
                    .AsNoTracking()
                    .AsQueryable();

                // ===== فلتر حسب المزرعة =====
                if (cottageId.HasValue && cottageId.Value > 0)
                {
                    query = query.Where(t => t.CottageId == cottageId.Value);
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
                    .Select(r => new CottageReservationModel
                    {
                        Id = r.Id,
                        CottageId = (int)r.CottageId,
                        CustomerId = (int)r.CustomerId,
                        ReservationTypeId = r.ReservationTypeId,
                        ReservationTypeDesc = lookupDict.ContainsKey(r.ReservationTypeId) ? lookupDict[r.ReservationTypeId] : string.Empty,
                        ReservationDate = r.ReservationDate != null ? r.ReservationDate : DateTime.Now,
                        CustMobNum = r.CustMobNum ?? string.Empty,
                        CustomerName = r.CustomerName ?? string.Empty,
                        PersonCount = r.PersonCount,
                        CostReservationAmtOnMahjouz = r.CostReservationAmtOnMahjouz,
                        ReservationAmt = r.ReservationAmt,
                        NetProfit = r.NetProfit,
                        ReservationDepositAmt = r.ReservationDepositAmt,
                        ReservationRemainAmt = r.ReservationRemainAmt,
                        Note = r.Note ?? string.Empty,
                        MobileOwnerAppUser = r.MobileOwnerAppUser ?? string.Empty,
                        IsMahjouzReservation = r.IsMahjouzReservation,
                        IsReceiveCommession = r.IsReceiveCommession,
                        //AutomaticallyNote = r.AutomaticallyNote ?? string.Empty,
                        CreatedDate = r.CreatedDate != null ? r.CreatedDate : DateTime.Now,
                        Reason = r.Reason ?? string.Empty,
                        ReservStatus = r.ReservStatus
                    })
                    .ToList();

                // ===== الإحصائيات =====
                var totalCount = model.Count;
                var pendingCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Pending);
                var acceptedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Accepted);
                var confirmedCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Confirmed);
                var cancelledCount = model.Count(r => r.ReservStatus == ReservStatusEnum.Cancelled);

                // ===== جلب اسم المزرعة =====
                string cottageName = "جميع الأكواخ";
                if (cottageId.HasValue && cottageId.Value > 0)
                {
                    var farm = _CottageRepository.Table.AsNoTracking().FirstOrDefault(f => f.Id == cottageId.Value);
                    cottageName = farm?.NameAr ?? "كوخ غير محددة";
                }

                // ===== جلب قوائم الفلتر =====
                var reservationTypes = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .OrderBy(l => l.ValueAr)
                    .ToList();

                var cottages = _CottageRepository.Table
                    .OrderBy(f => f.NameAr)
                    .ToList();

                // ===== ViewBag =====
                ViewBag.activePage = "حجوزات الأكواخ";
                ViewBag.CottageName = cottageName;
                ViewBag.CottageId = cottageId;
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
                ViewBag.AcceptedCount = acceptedCount;
                ViewBag.ConfirmedCount = confirmedCount;
                ViewBag.CancelledCount = cancelledCount;
                ViewBag.Cottages = cottages;
                ViewBag.ReservationTypes = reservationTypes;

                return View(model);
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while filtering reservations: {e.Message}");
                logFile.LogCustomInfo("Index POST CottageReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Index POST CottageReservation - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Index POST CottageReservation - Inner Exception Message ", e.InnerException?.ToString() ?? "No Inner Exception");

                ViewBag.Farms = _CottageRepository.Table.OrderBy(f => f.NameAr).ToList();
                ViewBag.ReservationTypes = _LookupValueRepository.Table.Where(l => l.LookupId == 6).OrderBy(l => l.ValueAr).ToList();
                return View(new List<CottageReservationModel>());
            }
        }

        public IActionResult Create(int cottageId)
        {
            ViewBag.activePage = "حجوزات الأكواخ";
            CottageReservationModel cottageReservation = new CottageReservationModel();
            cottageReservation.CottageId = cottageId;
            cottageReservation.ReservationDate = DateTime.Now;

            return View(NewFillModel(cottageReservation));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CottageReservationModel model, IFormFile formFile)
        {

            //we have two task we should make it 
            //1- make update for isMahjouzReservation the default is zero that the reservation
            //come from dashboard ...
            //2- make a set the mobileAppUser from Query you make to aviod any wrong after 
            //on apis ....
            LogFile logFile = new LogFile();

            try
            {
                if (model.CottageId <= 0)
                {
                    ModelState.AddModelError("CottageId", "برجاء اختيار الكوخ ");
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
                    if (_UnitOfWork.CottageReservationRepository.Table.Where(f => f.ReservationDate.Date == model.ReservationDate.Date && f.ReservationTypeId == model.ReservationTypeId && f.CottageId == model.CottageId).Count() > 0)
                    {
                        ErrorNotification(" يوجد حجز في نفس اليوم ونفس الفترة");
                        model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                        //return View(model);
                        return RedirectToAction("Create", model);
                    }
                    //we can make check on those two line next ...
                    var Cottage = _UnitOfWork.CottageRepository.Table.FirstOrDefault(f => f.Id == model.CottageId);
                    var User = _appUser.Table.FirstOrDefault(U => U.Id == Cottage.UserId);
                    model.MobileOwnerAppUser = User.MobilePhone;
                    var entity = model.ToEntity();
                    entity.CreatedDate = DateTime.Now;
                    entity.ResponseDate = DateTime.Now;

                    //we must update api for this property okay ....
                    //entity.AutomaticallyNote = "تم الحجز من قبل محجوز بلوحة التحكم";
                    entity.IsMahjouzReservation = false;
                    //_UnitOfWork.FarmerReservationRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.CottageReservationRepository.Insert(entity);
                    _UnitOfWork.Save();
                    //send to customer ...
                    if (Cottage.UserId == null)
                    {
                        Cottage.UserId = 0;
                    }
                    await SendNotification((int)Cottage.UserId, model.CustomerId,Cottage.NameAr,model.CustomerName, 0);
                    

                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index", new { cottageId = model.CottageId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Saving CottageReservation: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Create CottageReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create CottageReservation - Stack Trace Message ", e.StackTrace);
                //logFile.LogCustomInfo("Create CottageReservation - Inner Exception Message ", e.InnerException.ToString());
                if (e.InnerException != null)
                {
                    logFile.LogCustomInfo("Create Cottage Reservation - Inner Exception ", e.InnerException.ToString());
                }
                model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                model.Customers = _UnitOfWork.CustomerRepository.Table.ToList();
                model.Cottages = _UnitOfWork.CottageRepository.Table.ToList();
                return RedirectToAction("Create", model);
            }
            ViewBag.CottageId = model.CottageId;
            return View(NewFillModel(new CottageReservationModel()));
        }

        public IActionResult Edit(int id)
        {
            CottageReservation reservation = _UnitOfWork.CottageReservationRepository.GetById(id);
            if (reservation == null)
                return RedirectToAction("Index", new { farmerId = 0 });


            ViewBag.activePage = "حجوزات الأكواخ";
            return View(EditFillModel(reservation.ToModel()));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CottageReservationModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();
            model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
            model.Cottages = _CottageRepository.Table.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    if (_UnitOfWork.CottageReservationRepository.Table.Where(f => f.ReservationDate.Date == model.ReservationDate.Date && f.CottageId == model.CottageId && f.ReservationTypeId == model.ReservationTypeId && f.Id != model.Id).Count() > 0)
                    {
                        ErrorNotification(" يوجد حجز في نفس اليوم ونفس الفترة");
                        model.LookupValues = _LookupValueRepository.Table.Where(l => l.LookupId == 6).ToList();
                        return View(model);
                    }
                    var Cottage = _UnitOfWork.CottageRepository.Table.FirstOrDefault(f => f.Id == model.CottageId);
                    var User = _appUser.Table.FirstOrDefault(U => U.Id == Cottage.UserId);
                    model.MobileOwnerAppUser = User.MobilePhone;
                    model.ModifiedDate = DateTime.Now;
                    //model.AutomaticallyNote = "تم الحجز من قبل محجوز";
                    _UnitOfWork.CottageReservationRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    if (Cottage.UserId == null)
                    {
                        Cottage.UserId = 0;
                    }
                    await SendNotification((int)Cottage.UserId, model.CustomerId, Cottage.NameAr, model.CustomerName, 1);

                    SuccessNotification("تم تحديث السجل بنجاح");

                    return RedirectToAction("Index", new { cottageId = model.CottageId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Update CottageReservation: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Edit CottageReservation - Exception Message ", e.Message);
                logFile.LogCustomInfo("Edit CottageReservation - Stack Trace Message ", e.StackTrace);
                //logFile.LogCustomInfo("Edit CottgaeReservation - Inner Exception Message ", e.InnerException.ToString());
                if (e.InnerException != null)
                {
                    logFile.LogCustomInfo("Edit Cottage Reservation - Inner Exception ", e.InnerException.ToString());
                }
                return RedirectToAction("Edit", model);
            }
            ViewBag.CottageId = model.CottageId;
            model.Cottages = _UnitOfWork.CottageRepository.Table.ToList();

            return View(model);
        }



        // ============================================================
        // GET: GetConfirmCottageReservationData (للـ Popup)
        // ============================================================
        [HttpGet]
        public IActionResult GetConfirmCottageReservationData(int id)
        {
            try
            {
                var lookupValues = _LookupValueRepository.Table
                    .Where(l => l.LookupId == 6)
                    .ToList();

                var reservation = _UnitOfWork.CottageReservationRepository.Table
                    .Include(r => r.Customer)
                    .Include(r => r.Cottage)
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
                    customerId: (int)reservation.CustomerId,
                    bookingType: "Cottage",
                    referenceId: (int)reservation.CottageId
                );

                var viewModel = new ConfirmReservationCottageViewModel
                {
                    ReservationId = reservation.Id,
                    CustomerId = reservation.Customer.Id,
                    CustomerName = reservation.Customer?.FullName ?? "",
                    CustomerPhone = reservation.Customer?.MobileNumber ?? "",
                    CottageName = reservation.Cottage?.NameAr ?? "",
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

                return PartialView("Partials/_ConfirmReservationCottagePopup", viewModel);
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
        public async Task<IActionResult> ConfirmCottageReservation(int reservationId, int customerId,
                                                    int pointsUsed, decimal discountAmount,
                                                    decimal newTotal, decimal netProfit,
                                                    bool isReceiveCommission)
        {
            using (var transaction = _UnitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                    var reservation = _UnitOfWork.CottageReservationRepository.Table
                        .FirstOrDefault(r => r.Id == reservationId);

                    if (reservation == null)
                    {
                        return Json(new { success = false, message = "الحجز غير موجود" });
                    }

                    // ===== تحديث الحجز =====
                    reservation.ReservStatus = ReservStatusEnum.Confirmed;
                    reservation.ReservationAmt = newTotal;
                    reservation.NetProfit = netProfit;
                    reservation.IsReceiveCommession = isReceiveCommission;
                    //reservation.ModifiedDate = DateTime.Now;

                    _UnitOfWork.CottageReservationRepository.Update(reservation);

                    var objCottageReserve = _UnitOfWork.CottageReservationRepository
                                            .Table.Where(CR => CR.Id == reservationId).FirstOrDefault();

                    string reservedName = _UnitOfWork.CottageRepository
                     .Table.Where(S => S.Id == objCottageReserve.CottageId).FirstOrDefault().NameAr;

                    // ===== خصم النقاط (لو موجودة) =====
                    if (pointsUsed > 0 && customerId > 0)
                    {
                        //var loyaltyService = new LoyaltyService(_UnitOfWork, HttpContext);
                        var redeemSuccess = await loyaltyService.RedeemPointsAsync(
                            customerId: customerId,
                            points: pointsUsed,
                            reservationId: reservationId,
                            reservationType: "CottageReservation",
                            reservedName
                        );

                        if (!redeemSuccess)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "فشل خصم النقاط" });
                        }
                    }

                    // جلب ActivityTypeId من جدول LoyaltyActivityType
                    var activityType = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                        .FirstOrDefault(a => a.Code == "COTTAGE" && a.IsActive == true);

                    if (activityType != null)
                    {
                        // حساب النقاط
                        var points = loyaltyService.CalculatePoints(
                            activityTypeId: activityType.Id,
                            referenceType: activityType.ReferenceTable,
                            referenceId: reservation.CottageId
                        );

                        if (points > 0)
                        {
                            // إضافة النقاط
                            loyaltyService.AddPointsAsync(
                               customerId: (int)reservation.CustomerId,
                               activityTypeId: activityType.Id,
                               referenceType: activityType.ReferenceTable,
                               referenceId: reservation.CottageId,
                               reservationId: reservation.Id,
                               reservationType: "CottageReservation"
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
                    //Send Notifications .....
                    var Customer = _UnitOfWork.CustomerRepository.Table.Where(C => C.Id == reservation.CustomerId).FirstOrDefault();
                    List<string> tokens_customer = await _UnitOfWork.CustomerRepository.Table
                                .Where(c => c.Id == reservation.CustomerId && !string.IsNullOrEmpty(c.DeviceToken))
                                .Select(c => c.DeviceToken)
                                .ToListAsync(); // أو .ToList() إذا لم تكن تستخدم Async

                    var data = new Dictionary<string, string>
                    {
                        { "type", "cottage_notification" }
                    };
                    var cottage = await _UnitOfWork.CottageRepository.Table
                                .FirstOrDefaultAsync(F => F.Id == reservation.CottageId);

                    string cottageName = cottage?.NameAr ?? "";
                    string title = " تأكيد حجز الكوخ";
                    string source = "لوحة التحكم";

                    // {0} تأخذ farmerName، و {1} تأخذ source
                    string body = string.Format("\u200Fتم تأكيد حجزك على الكوخ {0}   (عبر {1} )",
                        cottageName,
                        source
                    );

                    if (tokens_customer.Any())
                    {
                        await _notificationService.SendNotificationAsync(tokens_customer, title, body, data);
                    }
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
                var reservation = _UnitOfWork.CottageReservationRepository.Table
                    .Include(r => r.Cottage)
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

                _UnitOfWork.CottageReservationRepository.Update(reservation);
                _UnitOfWork.Save();
                //Notification For Cancel
                var Customer = _UnitOfWork.CustomerRepository.Table.Where(C => C.Id == reservation.CustomerId).FirstOrDefault();
                List<string> tokens = await _UnitOfWork.CustomerRepository.Table
                            .Where(c => c.Id == reservation.CustomerId && !string.IsNullOrEmpty(c.DeviceToken))
                            .Select(c => c.DeviceToken)
                            .ToListAsync(); // أو .ToList() إذا لم تكن تستخدم Async
                var data = new Dictionary<string, string>
                    {
                        { "type", "general_notification" }
                    };
                var cottage = await _UnitOfWork.CottageRepository.Table
                            .FirstOrDefaultAsync(F => F.Id == reservation.CottageId);

                string cottageName = cottage?.NameAr ?? "";
                string title = " تأكيد حجز الكوخ";
                string source = "لوحة التحكم";

                // {0} تأخذ farmerName، و {1} تأخذ source
                string body = string.Format("\u200Fتم إلغاء حجزك على كوخ {0}، وذلك بسبب: {2} (عبر {1})",
                    cottageName,         // {0}
                    source,             // {1}
                    reservation.Reason  // {2}
                );

                if (tokens.Any())
                {
                    await _notificationService.SendNotificationAsync(tokens, title, body, data);
                }
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
                        //لا تنس عمل هذا COTTAGE
                        //أو ادخالة من الشاشة 
                        var activityType = _UnitOfWork.LoyaltyActivityTypeRepository.Table
                            .FirstOrDefault(a => a.Code == "COTTAGE" && a.IsActive == true);

                        if (activityType != null)
                        {
                            // حساب النقاط
                            var points = loyaltyService.CalculatePoints(
                                activityTypeId: activityType.Id,
                                referenceType: activityType.ReferenceTable,
                                referenceId: reservation.CottageId
                            );

                            if (points > 0)
                            {
                                // إضافة النقاط
                                await loyaltyService.AddPointsAsync(
                                    customerId: (int)reservation.CustomerId,
                                    activityTypeId: activityType.Id,
                                    referenceType: activityType.ReferenceTable,
                                    referenceId: reservation.CottageId,
                                    reservationId: reservation.Id,
                                    reservationType: "CottageReservation"
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

        private int GetCurrentAdminId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }


        private async Task SendNotification(int userId,int customerId,string realtyName,string customerName,int mode)
        {

            var Customer = _UnitOfWork.CustomerRepository.Table.Where(C => C.Id == customerId).FirstOrDefault();
            List<string> tokens_customer = await _UnitOfWork.CustomerRepository.Table
                        .Where(c => c.Id == customerId && !string.IsNullOrEmpty(c.DeviceToken))
                        .Select(c => c.DeviceToken)
                        .ToListAsync(); // أو .ToList() إذا لم تكن تستخدم Async

            //send to owner ...
            List<string> tokens_owner = await _UnitOfWork.DeviceTokenRepository
                          .Table.AsNoTracking()
                          .Where(D => D.UserId == userId)
                          .Select(D => D.Token)
                          .ToListAsync();
            string title = "";
            string source = "لوحة التحكم";
            Dictionary<string, string> data=new Dictionary<string, string>();
            string body="";
            if (mode == 0)
            {
                title = "حجز الأكواخ";
                 data = new Dictionary<string, string>
                    {
                        { "type", "CottageReservation_notification" }
                    };
                 body = string.Format("\u200Fتم حجز الكوخ {0} بواسطة {1} (عبر {2})",
                        realtyName,
                        customerName,
                        source);

            }
            if(mode==1)
            {
                title = "تحديث حجز الأكواخ";
                    data = new Dictionary<string, string>
                    {
                        { "type", "CottageReservation_notification" }
                    };
                    body = string.Format("\u200Fتم  تحديث حجز الكوخ {0} بواسطة {1} (عبر {2})",
                        realtyName,
                        customerName,
                        source);
            }

            if (tokens_owner.Any())
            {
                await _notificationService.SendNotificationAsync(tokens_owner, title, body, data);
            }
            if (tokens_customer.Any())
            {
                await _notificationService.SendNotificationAsync(tokens_customer, title, body, data);
            }



        }

    }
}
