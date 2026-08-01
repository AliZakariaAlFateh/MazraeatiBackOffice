using ClosedXML;
using DocumentFormat.OpenXml.Spreadsheet;
using MazraeatiBackOffice;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MazraeatiBackOffice.Controllers
{
    //public class AppUserController : BaseController
    //{
    //    private readonly IUnitOfWork _UnitOfWork;
    //    private readonly IRepository<AppUser> _userRepository;
    //    private readonly IWebHostEnvironment webHostEnvironment;
    //    private readonly IRepository<Farmer> _FarmerRepository;
    //    private readonly IRepository<Country> _countryRepository;
    //    private readonly IRepository<City> _cityRepository;
    //    private readonly IRepository<FarmerReservation> _FarmerReservation;
    //    private readonly IRepository<FarmerFeedback> _FarmerFeedback;
    //    private readonly IRepository<Regions> _regionRepository;
    //    private IConfiguration _configuration;
    //    public AppUserController(IUnitOfWork unitOfWork, IRepository<AppUser> userRepository,
    //        IWebHostEnvironment hostEnvironment, IConfiguration configuration,
    //        IRepository<Farmer> FarmerRepository, IRepository<Country> countryRepository,
    //        IRepository<City> cityRepository, IRepository<FarmerReservation> FarmerReservation,
    //        IRepository<FarmerFeedback> FarmerFeedback, IRepository<Regions> regionRepository)
    //    {
    //        _UnitOfWork = unitOfWork;
    //        _userRepository = userRepository;
    //        webHostEnvironment = hostEnvironment;
    //        _configuration = configuration;
    //        _FarmerRepository = FarmerRepository;
    //        _countryRepository = countryRepository;
    //        _cityRepository = cityRepository;
    //        _FarmerReservation = FarmerReservation;
    //        _FarmerFeedback = FarmerFeedback;
    //        _regionRepository = regionRepository;

    //    }

    //    public UserModel FillModel(UserModel model)
    //    {

    //        // ===== جلب المزارع =====
    //        var farms = _FarmerRepository.Table.ToList();
    //        model.Farms = farms.Select(x => new SelectListItem
    //        {
    //            Value = x.Id.ToString(),
    //            Text = x.Name
    //        }).ToList();

    //        // ===== جلب العقارات الرياضية =====
    //        var sports = _UnitOfWork.SportRepository.Table.ToList();
    //        model.Sports = sports.Select(x => new SelectListItem
    //        {
    //            Value = x.Id.ToString(),
    //            Text = x.NameAr
    //        }).ToList();

    //        // ===== إذا كان Edit, حدد المختارات =====
    //        if (model.Id > 0)
    //        {
    //            var selectedFarms = _FarmerRepository.Table
    //                .Where(f => f.UserId == model.Id)
    //                .Select(f => f.Id)
    //                .ToList();
    //            model.FarmIds = selectedFarms;

    //            var selectedSports = _UnitOfWork.SportRepository.Table
    //                .Where(f => f.UserId == model.Id)
    //                .Select(f => f.Id)
    //                .ToList();
    //            model.SportIds = selectedSports;
    //        }

    //        // ===== قائمة أنواع العقارات للـ CheckboxList =====
    //        model.UserTypeList = GetUserTypeList(model.UserType);

    //        return model;
    //    }

    //    [HttpPost]
    //    public IActionResult Index(IFormCollection form)
    //    {
    //        var search = form.TryGetValue("search", out var searchValue) ? searchValue.ToString() : null;
    //        var userType = form.TryGetValue("userType", out var userTypeValue) && !string.IsNullOrEmpty(userTypeValue) ? (int?)int.Parse(userTypeValue) : null;
    //        var isActive = form.TryGetValue("isActive", out var isActiveValue) && !string.IsNullOrEmpty(isActiveValue) ? (bool?)bool.Parse(isActiveValue) : null;

    //        return RedirectToAction("Index", new
    //        {
    //            search = search,
    //            userType = userType,
    //            isActive = isActive
    //        });
    //    }

    //    public IActionResult Index(string search, int? userType, bool? isActive)
    //    {
    //        ViewBag.activePage = "المستخدمين";

    //        // ===== جلب المستخدمين المحظورين =====
    //        var blockedUserIds = _UnitOfWork.AppUserBlackListRepository.Table
    //            .Where(b => b.UserId != null && b.IsBlocked == true)
    //            .Select(b => b.UserId)
    //            .ToList();

    //        // ===== بناء الـ Query مع استبعاد المحظورين =====
    //        var query = _UnitOfWork.UserRepository.Table
    //            .Where(u => u.Id != 1 && !blockedUserIds.Contains(u.Id))
    //            .AsQueryable();

    //        // ===== فلتر حسب نوع المستخدم =====
    //        if (userType.HasValue && userType.Value >= 0)
    //        {
    //            query = query.Where(u => (int)u.UserType == userType.Value);
    //        }

    //        // ===== فلتر حسب الحالة (نشط/غير نشط) =====
    //        if (isActive.HasValue)
    //        {
    //            query = query.Where(u => u.IsActive == isActive.Value);
    //        }

    //        // ===== فلتر البحث =====
    //        if (!string.IsNullOrEmpty(search))
    //        {
    //            query = query.Where(u =>
    //                u.UserName.Contains(search) ||
    //                u.MobileNumber.Contains(search) ||
    //                u.MobilePhone.Contains(search));
    //        }

    //        var users = query.OrderByDescending(u => u.Id).ToList();
    //        var model = users.Select(u => u.ToModel()).ToList();

    //        // ===== الإحصائيات (بناءً على الفلتر) =====
    //        // إجمالي المستخدمين بعد الفلتر
    //        ViewBag.TotalUsers = model.Count();

    //        // نشطون بعد الفلتر
    //        ViewBag.ActiveUsers = model.Count(u => u.IsActive);

    //        // غير نشطون بعد الفلتر
    //        ViewBag.InactiveUsers = model.Count(u => !u.IsActive);

    //        // محظورون (ثابت - لا يتغير مع الفلتر)
    //        var allUsers = _UnitOfWork.UserRepository.Table.Where(u => u.Id != 1).ToList();
    //        var blockedUsersCount = allUsers.Count(u => blockedUserIds.Contains(u.Id));
    //        ViewBag.BlockedUsers = blockedUsersCount;

    //        // ===== أنواع المستخدمين للفلتر =====
    //        var userTypesList = Enum.GetValues(typeof(UserTypeEnum))
    //            .Cast<UserTypeEnum>()
    //            .Select(e => new SelectListItem
    //            {
    //                Value = ((int)e).ToString(),
    //                Text = e.GetDisplayName()
    //            })
    //            .ToList();

    //        ViewBag.UserTypes = new SelectList(userTypesList, "Value", "Text", userType);

    //        ViewBag.SelectedUserType = userType;
    //        ViewBag.SelectedIsActive = isActive;
    //        ViewBag.search = search;

    //        return View(model);
    //    }


    //    public IActionResult UserFarms(int id)
    //    {
    //        var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
    //        var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
    //        var Reservation = _FarmerReservation.Table.ToList();
    //        var FarmerFeedback = _FarmerFeedback.Table.ToList();
    //        var Users = _userRepository.Table.ToList();
    //        var Regions = _regionRepository.Table.ToList();
    //        var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
    //                                 .Select(a => a.FarmerId).ToList();
    //        var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && f.UserId == id && !farmerBlackListIds.Contains(f.Id)).OrderByDescending(a => a.Id)
    //            .Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback, Users, Regions));
    //        ViewBag.activePage = "المزارع";
    //        ViewBag.cities = Cities.Where(c => c.CountryId == 2);
    //        ViewBag.DefaultDate = DateTime.Now;
    //        return View(model);
    //    }


    //    public IActionResult Create()
    //    {
    //        // ===== إعداد قائمة أنواع المستخدمين =====
    //        ViewBag.UserTypes = Enum.GetValues(typeof(UserTypeEnum))
    //            .Cast<UserTypeEnum>()
    //            .Select(e => new SelectListItem
    //            {
    //                Value = ((int)e).ToString(),
    //                Text = e.GetDisplayName()
    //            })
    //            .ToList();

    //        // ===== إنشاء Model جديد =====
    //        var model = new UserModel
    //        {
    //            IsActive = false,  // افتراضي غير نشط
    //            UserType = UserTypeEnum.Farmer  // افتراضي عام
    //        };

    //        // ===== تعبئة قائمة المزارع =====
    //        model = FillModel(model);

    //        ViewBag.activePage = "المستخدمين";
    //        return View(model);
    //    }

    //    [HttpPost]
    //    public IActionResult Create([FromForm] UserModel model, IFormFile formFile)
    //    {
    //        try
    //        {
    //            // ===== التحقق من وجود الأرقام =====
    //            var exist_1 = _userRepository.Table.Any(U => U.MobilePhone == model.MobilePhone);
    //            var exist_2 = _userRepository.Table.Any(U => U.MobileNumber == model.MobileNumber);
    //            if (exist_1 || exist_2)
    //            {
    //                ErrorNotification("أرقام التليفونات هذه مسجلة من قبل و لا يمكن تكرارها");

    //                // ===== إعادة تحميل الـ DropDowns =====
    //                ViewBag.UserTypes = Enum.GetValues(typeof(UserTypeEnum))
    //                    .Cast<UserTypeEnum>()
    //                    .Select(e => new SelectListItem
    //                    {
    //                        Value = ((int)e).ToString(),
    //                        Text = e.GetDisplayName()
    //                    })
    //                    .ToList();

    //                model.Farms = _FarmerRepository.Table
    //                    .Select(x => new SelectListItem
    //                    {
    //                        Value = x.Id.ToString(),
    //                        Text = x.Name
    //                    }).ToList();

    //                model.Sports = _UnitOfWork.SportRepository.Table
    //                            .Select(x => new SelectListItem
    //                            {
    //                                Value = x.Id.ToString(),
    //                                Text = x.NameAr
    //                            }).ToList();

    //                return View(model);
    //            }

    //            if (ModelState.IsValid)
    //            {
    //                // ===== تحويل الـ Model إلى Entity =====
    //                var userEntity = model.ToEntity();

    //                // ===== حفظ المستخدم =====
    //                _UnitOfWork.UserRepository.Insert(userEntity);
    //                _UnitOfWork.Save();

    //                var userId = userEntity.Id;
    //                var MobileOwner = userEntity.MobilePhone;
    //                // ===== ربط المزارع بالمستخدم =====
    //                if (model.FarmIds != null && model.FarmIds.Any())
    //                {
    //                    var farms = _UnitOfWork.FarmerRepository.Table
    //                        .Where(x => model.FarmIds.Contains(x.Id))
    //                        .ToList();

    //                    foreach (var farm in farms)
    //                    {
    //                        farm.UserId = userId;
    //                        farm.MobileOwnerAppUser = MobileOwner;
    //                        _UnitOfWork.FarmerRepository.Update(farm);
    //                    }
    //                    _UnitOfWork.Save();
    //                }
    //                // ===== ربط العقار الرياضي بالمستخدم =====
    //                if (model.SportIds != null && model.SportIds.Any())
    //                {
    //                    var sports = _UnitOfWork.SportRepository.Table
    //                        .Where(x => model.SportIds.Contains(x.Id))
    //                        .ToList();
    //                    foreach (var sport in sports)
    //                    {
    //                        sport.UserId = userId;
    //                        sport.MobileOwnerAppUser = MobileOwner;
    //                        _UnitOfWork.SportRepository.Update(sport);
    //                    }
    //                    _UnitOfWork.Save();
    //                }

    //                SuccessNotification("تم اضافة السجل بنجاح");
    //                return RedirectToAction("Index");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            ErrorNotification(e.InnerException?.Message ?? e.Message);
    //        }

    //        // ===== عند وجود خطأ، إعادة تحميل البيانات =====
    //        ViewBag.UserTypes = Enum.GetValues(typeof(UserTypeEnum))
    //            .Cast<UserTypeEnum>()
    //            .Select(e => new SelectListItem
    //            {
    //                Value = ((int)e).ToString(),
    //                Text = e.GetDisplayName()
    //            })
    //            .ToList();

    //        model.Farms = _FarmerRepository.Table
    //            .Select(x => new SelectListItem
    //            {
    //                Value = x.Id.ToString(),
    //                Text = x.Name
    //            }).ToList();

    //        return View(model);
    //    }

    //    public IActionResult Edit(int id)
    //    {
    //        AppUser user = _userRepository.GetById(id);
    //        if (user == null)
    //            return RedirectToAction("Index");

    //        var model = user.ToModel();

    //        // ===== جلب المزارع المختارة =====
    //        var selectedFarms = _UnitOfWork.FarmerRepository.Table
    //            .Where(f => f.UserId == id)
    //            .Select(f => f.Id)
    //            .ToList();
    //        model.FarmIds = selectedFarms;

    //        // ===== تعبئة الـ Farms للـ MultiSelect =====
    //        model.Farms = _FarmerRepository.Table
    //            .Select(x => new SelectListItem
    //            {
    //                Value = x.Id.ToString(),
    //                Text = x.Name
    //            }).ToList();

    //        var selectedSports = _UnitOfWork.SportRepository.Table
    //            .Where(f => f.UserId == id)
    //            .Select(f => f.Id)
    //            .ToList();
    //        model.SportIds = selectedSports;

    //        // ===== تعبئة الـ Sports للـ MultiSelect =====
    //        model.Sports = _UnitOfWork.SportRepository.Table
    //            .Select(x => new SelectListItem
    //            {
    //                Value = x.Id.ToString(),
    //                Text = x.NameAr
    //            }).ToList();


    //        // ===== أنواع المستخدمين للـ DropDown =====
    //        ViewBag.UserTypes = Enum.GetValues(typeof(UserTypeEnum))
    //            .Cast<UserTypeEnum>()
    //            .Select(e => new SelectListItem
    //            {
    //                Value = ((int)e).ToString(),
    //                Text = e.GetDisplayName(),
    //                Selected = ((int)e == (int)model.UserType)  // ✅ مهم جداً
    //            })
    //            .ToList();

    //        ViewBag.activePage = "المستخدمين";
    //        return View(model);
    //    }


    //    [HttpPost]
    //    public IActionResult Edit(UserModel model, IFormFile formFile)
    //    {
    //        try
    //        {
    //            if (ModelState.IsValid)
    //            {
    //                // ===== جلب المستخدم القديم =====
    //                var existingUser = _userRepository.GetById(model.Id);
    //                if (existingUser == null)
    //                    return RedirectToAction("Index");

    //                // ===== تحديث الخصائص =====
    //                existingUser.UserName = model.UserName;
    //                existingUser.MobilePhone = model.MobilePhone;
    //                existingUser.MobileNumber = model.MobileNumber;
    //                existingUser.UserType = model.UserType; // مهم جدًا
    //                existingUser.IsActive = model.IsActive; // مهم جدًا
    //                existingUser.IsDeleted = model.IsDeleted;

    //                if (!string.IsNullOrEmpty(model.PasswordHash))
    //                {
    //                    existingUser.PasswordHash = model.PasswordHash;
    //                }

    //                _UnitOfWork.UserRepository.Update(existingUser);
    //                _UnitOfWork.Save();

    //                if (model.UserType == UserTypeEnum.Farmer)
    //                {
    //                    // ===== تحديث المزارع =====
    //                    var oldFarms = _UnitOfWork.FarmerRepository.Table
    //                        .Where(x => x.UserId == model.Id)
    //                        .ToList();
    //                    var newFarmIds = model.FarmIds ?? new List<int>();
    //                    // إزالة المزارع التي لم تعد محددة
    //                    var removedFarms = oldFarms
    //                        .Where(x => !newFarmIds.Contains(x.Id))
    //                        .ToList();
    //                    foreach (var farm in removedFarms)
    //                    {
    //                        farm.UserId = null;
    //                        _UnitOfWork.FarmerRepository.Update(farm);
    //                    }

    //                    // إضافة المزارع الجديدة
    //                    var farmsToAdd = _UnitOfWork.FarmerRepository.Table
    //                        .Where(x => newFarmIds.Contains(x.Id) && x.UserId != model.Id)
    //                        .ToList();
    //                    foreach (var farm in farmsToAdd)
    //                    {
    //                        farm.UserId = model.Id;
    //                        _UnitOfWork.FarmerRepository.Update(farm);
    //                    }
    //                }
    //                if (model.UserType != UserTypeEnum.Farmer)
    //                {
    //                    // ===== تحديث المزارع =====
    //                    var oldSports = _UnitOfWork.SportRepository.Table
    //                        .Where(x => x.UserId == model.Id)
    //                        .ToList();
    //                    var newSportIds = model.SportIds ?? new List<int>();
    //                    // إزالة المزارع التي لم تعد محددة
    //                    var removedSports = oldSports
    //                        .Where(x => !newSportIds.Contains(x.Id))
    //                        .ToList();
    //                    foreach (var sport in removedSports)
    //                    {
    //                        sport.UserId = null;
    //                        _UnitOfWork.SportRepository.Update(sport);
    //                    }

    //                    // إضافة المزارع الجديدة
    //                    var sportsToAdd = _UnitOfWork.SportRepository.Table
    //                        .Where(x => newSportIds.Contains(x.Id) && x.UserId != model.Id)
    //                        .ToList();
    //                    foreach (var sport in sportsToAdd)
    //                    {
    //                        sport.UserId = model.Id;
    //                        _UnitOfWork.SportRepository.Update(sport);
    //                    }
    //                }





    //                _UnitOfWork.Save();
    //                SuccessNotification("تم تحديث السجل بنجاح");
    //                return RedirectToAction("Index");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            ErrorNotification(e.Message);
    //        }

    //        // ===== عند الرجوع =====
    //        ViewBag.UserTypes = Enum.GetValues(typeof(UserTypeEnum))
    //            .Cast<UserTypeEnum>()
    //            .Select(e => new SelectListItem
    //            {
    //                Value = ((int)e).ToString(),
    //                Text = e.GetDisplayName()
    //            })
    //            .ToList();

    //        model.Farms = _FarmerRepository.Table
    //            .Select(x => new SelectListItem
    //            {
    //                Value = x.Id.ToString(),
    //                Text = x.Name
    //            }).ToList();
    //        model.Sports = _UnitOfWork.SportRepository.Table
    //            .Select(x => new SelectListItem
    //            {
    //                Value = x.Id.ToString(),
    //                Text = x.NameAr
    //            }).ToList();

    //        return View(model);
    //    }

    //    public IActionResult Delete(int id)
    //    {
    //        var farms = _UnitOfWork.FarmerRepository.Table
    //                                .Where(f => f.UserId == id)
    //                                .ToList();
    //        //in case you want delete all farms connect with AppUser ..
    //        //else you can not delete user ...
    //        //if (farms.Any())
    //        //{
    //        //    foreach (var farm in farms)
    //        //    {
    //        //        _UnitOfWork.FarmerRepository.Delete(farm);
    //        //        _UnitOfWork.Save();
    //        //    }
    //        //}
    //        if (farms.Any())
    //        {
    //            return Json("يجب حذف المزارع المتعلقة بهذا الستخدم");
    //        }

    //        AppUser user = _userRepository.GetById(id);

    //        if (user == null)
    //            return Json("السجل غير معرف");

    //        _UnitOfWork.UserRepository.Delete(user);
    //        _UnitOfWork.Save();
    //        return Json(1);
    //    }


    //    // ============================================================
    //    // تبديل حالة المستخدم (نشط/غير نشط)
    //    // ============================================================
    //    [HttpPost]
    //    public IActionResult ToggleActive(int id)
    //    {
    //        try
    //        {
    //            var user = _UnitOfWork.UserRepository.GetById(id);
    //            if (user == null)
    //                return Json(new { success = false, message = "المستخدم غير موجود" });

    //            // تبديل الحالة
    //            user.IsActive = !user.IsActive;
    //            //user.ModifiedDate = DateTime.Now;

    //            _UnitOfWork.UserRepository.Update(user);
    //            _UnitOfWork.Save();

    //            var statusText = user.IsActive ? "نشط" : "غير نشط";
    //            return Json(new
    //            {
    //                success = true,
    //                message = $"تم تغيير حالة المستخدم إلى {statusText}",
    //                isActive = user.IsActive
    //            });
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(new { success = false, message = e.Message });
    //        }
    //    }

    //    public IActionResult GetSportsByType(int sportTypeId)
    //    {
    //        //sportTypeId -= 1;
    //        if (sportTypeId <= 0)
    //            return Json(new List<object>());

    //        var sports = _UnitOfWork.SportRepository.Table
    //            .Where(s => s.SportTypeId == sportTypeId && s.IsActive == true)
    //            //.OrderBy(s => s.NameAr)
    //            .Select(s => new { s.Id, s.NameAr, s.MobileNumber })
    //            .ToList();

    //        return Json(sports);
    //    }


    //    private List<SelectListItem> GetUserTypeList(string selectedValues)
    //    {
    //        var selectedList = (selectedValues ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

    //        var types = new List<SelectListItem>
    //{
    //    new SelectListItem { Value = "0", Text = "مالك مزرعة" },
    //    new SelectListItem { Value = "1", Text = "مالك ملعب كرة قدم" },
    //    new SelectListItem { Value = "2", Text = "مالك ملعب بادل" },
    //    new SelectListItem { Value = "3", Text = "مالك ملعب تنس" },
    //    new SelectListItem { Value = "4", Text = "مالك ملعب كرة سلة" },
    //    new SelectListItem { Value = "5", Text = "مالك ملعب كرة طائرة" },
    //    new SelectListItem { Value = "6", Text = "مالك مسبح" },
    //    new SelectListItem { Value = "7", Text = "مالك مركز فروسية" },
    //    new SelectListItem { Value = "8", Text = "مالك ميدان رماية" },
    //    new SelectListItem { Value = "9", Text = "مالك Pickleball" },
    //    new SelectListItem { Value = "10", Text = "مالك تنس طاولة" },
    //    new SelectListItem { Value = "11", Text = "مالك اسكواش" },
    //    new SelectListItem { Value = "12", Text = "مالك ريشة طائرة" }
    //};

    //        foreach (var item in types)
    //        {
    //            item.Selected = selectedList.Contains(item.Value);
    //        }

    //        return types;
    //    }

    //}


    public class AppUserController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<AppUser> _userRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<FarmerReservation> _FarmerReservation;
        private readonly IRepository<FarmerFeedback> _FarmerFeedback;
        private readonly IRepository<Regions> _regionRepository;
        private IConfiguration _configuration;

        public AppUserController(IUnitOfWork unitOfWork, IRepository<AppUser> userRepository,
            IWebHostEnvironment hostEnvironment, IConfiguration configuration,
            IRepository<Farmer> FarmerRepository, IRepository<Country> countryRepository,
            IRepository<City> cityRepository, IRepository<FarmerReservation> FarmerReservation,
            IRepository<FarmerFeedback> FarmerFeedback, IRepository<Regions> regionRepository)
        {
            _UnitOfWork = unitOfWork;
            _userRepository = userRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
            _FarmerRepository = FarmerRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _FarmerReservation = FarmerReservation;
            _FarmerFeedback = FarmerFeedback;
            _regionRepository = regionRepository;
        }

        // ============================================================
        // FillModel
        // ============================================================
        public UserModel FillModel(UserModel model)
        {
            // ===== جلب المزارع =====
            var farms = _FarmerRepository.Table.ToList();
            model.Farms = farms.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            // ===== جلب العقارات الرياضية =====
            var sports = _UnitOfWork.SportRepository.Table.ToList();
            model.Sports = sports.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.NameAr
            }).ToList();

            // ===== إذا كان Edit, حدد المختارات =====
            if (model.Id > 0)
            {
                var selectedFarms = _FarmerRepository.Table
                    .Where(f => f.UserId == model.Id)
                    .Select(f => f.Id)
                    .ToList();
                model.FarmIds = selectedFarms;

                var selectedSports = _UnitOfWork.SportRepository.Table
                    .Where(f => f.UserId == model.Id)
                    .Select(f => f.Id)
                    .ToList();
                model.SportIds = selectedSports;
            }

            // ===== قائمة أنواع العقارات للـ MultiSelect =====
            model.UserTypeList = GetUserTypeList();

            // ===== تحديد القيم المختارة =====
            if (!string.IsNullOrEmpty(model.UserType))
            {
                model.UserTypeListSelected = model.UserType.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return model;
        }

        // ============================================================
        // INDEX - POST
        // ============================================================
        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var search = form.TryGetValue("search", out var searchValue) ? searchValue.ToString() : null;
            var userType = form.TryGetValue("userType", out var userTypeValue) && !string.IsNullOrEmpty(userTypeValue) ? (int?)int.Parse(userTypeValue) : null;
            var isActive = form.TryGetValue("isActive", out var isActiveValue) && !string.IsNullOrEmpty(isActiveValue) ? (bool?)bool.Parse(isActiveValue) : null;

            return RedirectToAction("Index", new
            {
                search = search,
                userType = userType,
                isActive = isActive
            });
        }

        // ============================================================
        // INDEX - GET
        // ============================================================
        public IActionResult Index(string search, int? userType, bool? isActive)
        {
            ViewBag.activePage = "المستخدمين";

            var blockedUserIds = _UnitOfWork.AppUserBlackListRepository.Table
                .Where(b => b.UserId != null && b.IsBlocked == true)
                .Select(b => b.UserId)
                .ToList();

            var query = _UnitOfWork.UserRepository.Table
                .Where(u => u.Id != 1 && !blockedUserIds.Contains(u.Id))
                .AsQueryable();

            // ===== فلتر حسب نوع المستخدم (يحتوي على القيمة) =====
            if (userType.HasValue && userType.Value >= 0)
            {
                var searchValue = userType.Value.ToString();
                query = query.Where(u => u.UserType != null && u.UserType.Contains(searchValue));
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(search) ||
                    u.MobileNumber.Contains(search) ||
                    u.MobilePhone.Contains(search));
            }

            var users = query.OrderByDescending(u => u.Id).ToList();
            var model = users.Select(u => u.ToModel()).ToList();

            ViewBag.TotalUsers = model.Count();
            ViewBag.ActiveUsers = model.Count(u => u.IsActive);
            ViewBag.InactiveUsers = model.Count(u => !u.IsActive);

            var allUsers = _UnitOfWork.UserRepository.Table.Where(u => u.Id != 1).ToList();
            ViewBag.BlockedUsers = allUsers.Count(u => blockedUserIds.Contains(u.Id));

            // ===== أنواع المستخدمين للفلتر =====
            var userTypesList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "مالك مزرعة" },
                new SelectListItem { Value = "1", Text = "مالك ملعب كرة قدم" },
                new SelectListItem { Value = "2", Text = "مالك ملعب بادل" },
                new SelectListItem { Value = "3", Text = "مالك ملعب تنس" },
                new SelectListItem { Value = "4", Text = "مالك ملعب كرة سلة" },
                new SelectListItem { Value = "5", Text = "مالك ملعب كرة طائرة" },
                new SelectListItem { Value = "6", Text = "مالك مسبح" },
                new SelectListItem { Value = "7", Text = "مالك مركز فروسية" },
                new SelectListItem { Value = "8", Text = "مالك ميدان رماية" },
                new SelectListItem { Value = "9", Text = "مالك Pickleball" },
                new SelectListItem { Value = "10", Text = "مالك تنس طاولة" },
                new SelectListItem { Value = "11", Text = "مالك اسكواش" },
                new SelectListItem { Value = "12", Text = "مالك ريشة طائرة" }
            };

            ViewBag.UserTypes = new SelectList(userTypesList, "Value", "Text", userType);
            ViewBag.SelectedUserType = userType;
            ViewBag.SelectedIsActive = isActive;
            ViewBag.search = search;

            return View(model);
        }

        // ============================================================
        // UserFarms
        // ============================================================
        public IActionResult UserFarms(int id)
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var Users = _userRepository.Table.ToList();
            var Regions = _regionRepository.Table.ToList();
            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table
                .Where(a => a.FarmerId != null && a.IsBlocked == true)
                .Select(a => a.FarmerId).ToList();

            var model = _FarmerRepository.Table
                .Where(f => f.CountryId == 2 && f.UserId == id && !farmerBlackListIds.Contains(f.Id))
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback, Users, Regions));

            ViewBag.activePage = "المزارع";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        // ============================================================
        // CREATE - GET
        // ============================================================
        public IActionResult Create()
        {
            var model = new UserModel
            {
                IsActive = false,
                UserType = "0"
            };

            model = FillModel(model);
            ViewBag.activePage = "المستخدمين";
            return View(model);
        }

        // ============================================================
        // CREATE - POST
        // ============================================================
        [HttpPost]
        public IActionResult Create([FromForm] UserModel model, IFormFile formFile)
        {
            try
            {
                var exist_1 = _userRepository.Table.Any(U => U.MobilePhone == model.MobilePhone);
                var exist_2 = _userRepository.Table.Any(U => U.MobileNumber == model.MobileNumber);

                if (exist_1 || exist_2)
                {
                    ErrorNotification("أرقام التليفونات هذه مسجلة من قبل و لا يمكن تكرارها");
                    model = FillModel(model);
                    return View(model);
                }

                if (ModelState.IsValid)
                {
                    if (model.UserTypeListSelected != null && model.UserTypeListSelected.Any())
                    {
                        model.UserType = string.Join(",", model.UserTypeListSelected);
                    }
                    else
                    {
                        model.UserType = "0";
                    }

                    var userEntity = model.ToEntity();
                    userEntity.UserType = CleanUserType(model.UserType);

                    _UnitOfWork.UserRepository.Insert(userEntity);
                    _UnitOfWork.Save();

                    var userId = userEntity.Id;
                    var mobileOwner = userEntity.MobilePhone;

                    // ===== ربط المزارع =====
                    if (model.FarmIds != null && model.FarmIds.Any())
                    {
                        var farms = _UnitOfWork.FarmerRepository.Table
                            .Where(x => model.FarmIds.Contains(x.Id))
                            .ToList();
                        foreach (var farm in farms)
                        {
                            farm.UserId = userId;
                            farm.MobileOwnerAppUser = mobileOwner;
                            _UnitOfWork.FarmerRepository.Update(farm);
                        }
                        _UnitOfWork.Save();
                    }

                    // ===== ربط العقارات الرياضية =====
                    if (model.SportIds != null && model.SportIds.Any())
                    {
                        var sports = _UnitOfWork.SportRepository.Table
                            .Where(x => model.SportIds.Contains(x.Id))
                            .ToList();
                        foreach (var sport in sports)
                        {
                            sport.UserId = userId;
                            sport.MobileOwnerAppUser = mobileOwner;
                            _UnitOfWork.SportRepository.Update(sport);
                        }
                        _UnitOfWork.Save();
                    }

                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.InnerException?.Message ?? e.Message);
            }

            model = FillModel(model);
            return View(model);
        }

        // ============================================================
        // EDIT - GET
        // ============================================================
        public IActionResult Edit(int id)
        {
            AppUser user = _userRepository.GetById(id);
            if (user == null)
                return RedirectToAction("Index");

            var model = user.ToModel();

            var selectedFarms = _UnitOfWork.FarmerRepository.Table
                .Where(f => f.UserId == id)
                .Select(f => f.Id)
                .ToList();
            model.FarmIds = selectedFarms;

            model.Farms = _FarmerRepository.Table
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            var selectedSports = _UnitOfWork.SportRepository.Table
                .Where(f => f.UserId == id)
                .Select(f => f.Id)
                .ToList();
            model.SportIds = selectedSports;

            model.Sports = _UnitOfWork.SportRepository.Table
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.NameAr
                }).ToList();

            model = FillModel(model);

            ViewBag.activePage = "المستخدمين";
            return View(model);
        }

        // ============================================================
        // EDIT - POST
        // ============================================================
        [HttpPost]
        public IActionResult Edit(UserModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // ===== دمج القيم المختارة في string =====
                    if (model.UserTypeListSelected != null && model.UserTypeListSelected.Any())
                    {
                        model.UserType = string.Join(",", model.UserTypeListSelected);
                    }
                    else
                    {
                        model.UserType = "0";
                    }
                    var existingUser = _userRepository.GetById(model.Id);
                    if (existingUser == null)
                        return RedirectToAction("Index");

                    existingUser.UserName = model.UserName;
                    existingUser.MobilePhone = model.MobilePhone;
                    existingUser.MobileNumber = model.MobileNumber;
                    existingUser.UserType = CleanUserType(model.UserType);
                    existingUser.IsActive = model.IsActive;
                    existingUser.IsDeleted = model.IsDeleted;

                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        existingUser.PasswordHash = model.PasswordHash;
                    }

                    _UnitOfWork.UserRepository.Update(existingUser);
                    _UnitOfWork.Save();
                    var User = _UnitOfWork.UserRepository.Table.Where(U => U.Id == model.Id).FirstOrDefault();
                    var MobileOwner = User.MobilePhone;
                    // ===== تحديث المزارع =====
                    var oldFarms = _UnitOfWork.FarmerRepository.Table
                        .Where(x => x.UserId == model.Id)
                        .ToList();
                    var newFarmIds = model.FarmIds ?? new List<int>();

                    var removedFarms = oldFarms.Where(x => !newFarmIds.Contains(x.Id)).ToList();
                    foreach (var farm in removedFarms)
                    {
                        farm.UserId = null;

                        _UnitOfWork.FarmerRepository.Update(farm);
                    }

                    var farmsToAdd = _UnitOfWork.FarmerRepository.Table
                        .Where(x => newFarmIds.Contains(x.Id) && x.UserId != model.Id)
                        .ToList();
                    foreach (var farm in farmsToAdd)
                    {
                        farm.UserId = model.Id;
                        farm.MobileOwnerAppUser = MobileOwner;
                        _UnitOfWork.FarmerRepository.Update(farm);
                    }

                    // ===== تحديث العقارات الرياضية =====
                    var oldSports = _UnitOfWork.SportRepository.Table
                        .Where(x => x.UserId == model.Id)
                        .ToList();
                    var newSportIds = model.SportIds ?? new List<int>();

                    var removedSports = oldSports.Where(x => !newSportIds.Contains(x.Id)).ToList();
                    foreach (var sport in removedSports)
                    {
                        sport.UserId = null;
                        _UnitOfWork.SportRepository.Update(sport);
                    }

                    var sportsToAdd = _UnitOfWork.SportRepository.Table
                        .Where(x => newSportIds.Contains(x.Id) && x.UserId != model.Id)
                        .ToList();
                    foreach (var sport in sportsToAdd)
                    {
                        sport.UserId = model.Id;
                        sport.MobileOwnerAppUser = MobileOwner;
                        _UnitOfWork.SportRepository.Update(sport);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }

            model = FillModel(model);
            return View(model);
        }

        // ============================================================
        // DELETE
        // ============================================================
        public IActionResult Delete(int id)
        {
            var farms = _UnitOfWork.FarmerRepository.Table
                .Where(f => f.UserId == id)
                .ToList();

            if (farms.Any())
            {
                return Json("يجب حذف المزارع المتعلقة بهذا المستخدم");
            }

            AppUser user = _userRepository.GetById(id);
            if (user == null)
                return Json("السجل غير معرف");

            _UnitOfWork.UserRepository.Delete(user);
            _UnitOfWork.Save();
            return Json(1);
        }

        // ============================================================
        // ToggleActive
        // ============================================================
        [HttpPost]
        public IActionResult ToggleActive(int id)
        {
            try
            {
                var user = _UnitOfWork.UserRepository.GetById(id);
                if (user == null)
                    return Json(new { success = false, message = "المستخدم غير موجود" });

                user.IsActive = !user.IsActive;
                _UnitOfWork.UserRepository.Update(user);
                _UnitOfWork.Save();

                return Json(new
                {
                    success = true,
                    message = $"تم تغيير حالة المستخدم إلى {(user.IsActive ? "نشط" : "غير نشط")}",
                    isActive = user.IsActive
                });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        // ============================================================
        // GetSportsByType
        // ============================================================
        public IActionResult GetSportsByType(int sportTypeId)
        {
            if (sportTypeId <= 0)
                return Json(new List<object>());

            var searchValue = sportTypeId.ToString();
            var users = _UnitOfWork.UserRepository.Table
                .Where(u => u.IsActive == true && u.UserType != null && u.UserType.Contains(searchValue))
                .OrderBy(u => u.UserName)
                .Select(u => new { u.Id, u.UserName, u.MobileNumber })
                .ToList();

            return Json(users);
        }

        // ============================================================
        // Helper Methods
        // ============================================================
        private List<SelectListItem> GetUserTypeList(string selectedValues)
        {
            var selectedList = (selectedValues ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var types = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "مالك مزرعة" },
                new SelectListItem { Value = "1", Text = "مالك ملعب كرة قدم" },
                new SelectListItem { Value = "2", Text = "مالك ملعب بادل" },
                new SelectListItem { Value = "3", Text = "مالك ملعب تنس" },
                new SelectListItem { Value = "4", Text = "مالك ملعب كرة سلة" },
                new SelectListItem { Value = "5", Text = "مالك ملعب كرة طائرة" },
                new SelectListItem { Value = "6", Text = "مالك مسبح" },
                new SelectListItem { Value = "7", Text = "مالك مركز فروسية" },
                new SelectListItem { Value = "8", Text = "مالك ميدان رماية" },
                new SelectListItem { Value = "9", Text = "مالك Pickleball" },
                new SelectListItem { Value = "10", Text = "مالك تنس طاولة" },
                new SelectListItem { Value = "11", Text = "مالك اسكواش" },
                new SelectListItem { Value = "12", Text = "مالك ريشة طائرة" }
            };

            foreach (var item in types)
            {
                item.Selected = selectedList.Contains(item.Value);
            }

            return types;
        }

        private string CleanUserType(string userType)
        {
            if (string.IsNullOrEmpty(userType))
                return "0";

            var values = userType.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Distinct()
                .Where(v => int.TryParse(v, out _))
                .ToList();

            return values.Any() ? string.Join(",", values) : "0";
        }

        private List<SelectListItem> GetUserTypeList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "0", Text = "مالك مزرعة" },
        new SelectListItem { Value = "1", Text = "مالك ملعب كرة قدم" },
        new SelectListItem { Value = "2", Text = "مالك ملعب بادل" },
        new SelectListItem { Value = "3", Text = "مالك ملعب تنس" },
        new SelectListItem { Value = "4", Text = "مالك ملعب كرة سلة" },
        new SelectListItem { Value = "5", Text = "مالك ملعب كرة طائرة" },
        new SelectListItem { Value = "6", Text = "مالك مسبح" },
        new SelectListItem { Value = "7", Text = "مالك مركز فروسية" },
        new SelectListItem { Value = "8", Text = "مالك ميدان رماية" },
        new SelectListItem { Value = "9", Text = "مالك Pickleball" },
        new SelectListItem { Value = "10", Text = "مالك تنس طاولة" },
        new SelectListItem { Value = "11", Text = "مالك اسكواش" },
        new SelectListItem { Value = "12", Text = "مالك ريشة طائرة" }
    };
        }


        // ============================================================
        // جلب المزارع حسب النوع (AJAX)
        // ============================================================
        public IActionResult GetFarmsByUserType(string userType)
        {
            if (string.IsNullOrEmpty(userType) || userType != "0")
                return Json(new List<object>());

            var farms = _FarmerRepository.Table
                .Select(f => new { f.Id, f.Name })
                .ToList();

            return Json(farms);
        }

        // ============================================================
        // جلب العقارات الرياضية حسب النوع (AJAX)
        // ============================================================
        public IActionResult GetSportsByUserType(string userType)
        {
            if (string.IsNullOrEmpty(userType))
                return Json(new List<object>());

            if (!int.TryParse(userType, out int sportTypeId))
                return Json(new List<object>());

            var sports = _UnitOfWork.SportRepository.Table
                .Where(s => s.SportTypeId == sportTypeId && s.IsActive == true)
                .Select(s => new { s.Id, s.NameAr })
                .ToList();

            return Json(sports);
        }


        // ============================================================
        // جلب كل العقارات الرياضية حسب أنواع متعددة (AJAX)
        // ============================================================
        public IActionResult GetAllSportsByTypes(string sportTypeIds)
        {
            if (string.IsNullOrEmpty(sportTypeIds))
                return Json(new List<object>());

            var ids = sportTypeIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            var sports = _UnitOfWork.SportRepository.Table
                .Where(s => ids.Contains(s.SportTypeId) && s.IsActive == true)
                .Select(s => new { s.Id, s.NameAr })
                .ToList();

            return Json(sports);
        }


    }



}
