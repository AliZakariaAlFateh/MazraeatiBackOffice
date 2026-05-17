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
    public class AppUserBlackListController : BaseController
    {
        private readonly IRepository<AppUserBlackList> _AppUserBlackListRepository;
        private readonly IRepository<AppUser> _userRepository;
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<FarmerReservation> _FarmerReservation;
        private readonly IRepository<FarmerFeedback> _FarmerFeedback;
        private readonly IRepository<Regions> _regionRepository;
        private readonly IRepository<FarmerBlackList> _FarmerBlackListRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public AppUserBlackListController(IRepository<AppUserBlackList> appUserBlackListRepository,
            IRepository<AppUser> userRepository, IRepository<Farmer> FarmerRepository,
            IRepository<Country> countryRepository, IRepository<City> cityRepository,
            IRepository<FarmerReservation> FarmerReservation, IRepository<FarmerFeedback> FarmerFeedback,
            IRepository<Regions> regionRepository, IRepository<FarmerBlackList> farmerBlackListRepository,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _AppUserBlackListRepository = appUserBlackListRepository;
            _FarmerRepository = FarmerRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _FarmerReservation = FarmerReservation;
            _FarmerFeedback = FarmerFeedback;
            _regionRepository = regionRepository;
            _FarmerBlackListRepository = farmerBlackListRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
            _userRepository = userRepository;
        }


        public AppUserBlackListModel NewFillModel(AppUserBlackListModel model)
        {
            model.Users = _userRepository.Table.ToList();
            return model;
        }

        public AppUserBlackListModel EditFillModel(AppUserBlackListModel model)
        {
            model.Users = _userRepository.Table.ToList();
            return model;
        }

        public IActionResult Index()
        {
            var model = _AppUserBlackListRepository.Table.Select(c => c.ToModel()).ToList();
            ViewBag.activePage = "قائمة العملاء المحظوريين (أصحاب المزارع)";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            search = string.IsNullOrEmpty(search) ? "" : search;
            var model = _AppUserBlackListRepository.Table.Where(t => (t.Reason.Contains(search) || t.CustMobileNum.Contains(search) || t.CustName.Contains(search))).Select(c => c.ToModel());

            ViewBag.activePage = "قائمة العملاء المحظوريين (أصحاب المزارع)";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "قائمة العملاء المحظوريين (أصحاب المزارع)";
            return View(NewFillModel(new AppUserBlackListModel()));
        }

        [HttpPost]
        public IActionResult Create(AppUserBlackListModel model, IFormFile formFile)
        {

            LogFile logFile = new LogFile();

            try
            {
                var B_user = _AppUserBlackListRepository.Table.FirstOrDefault(Bu => Bu.CustMobileNum == model.CustMobileNum);
                if (B_user != null)
                {
                    ErrorNotification($" هذا الرقم مسجل من قبل مع المستخدم  {B_user.CustName}  و لا يمكن تكراره");
                    return View(NewFillModel(model));
                }

                if (ModelState.IsValid)
                {
                    #region Select farms and farms farmsblacklist to add farms in blacklist
                    var farms = _FarmerRepository.Table
                                                 .Where(f => f.UserId == model.UserId)
                                                 .OrderByDescending(a => a.Id).ToList();

                    var blackListFarmIds = _FarmerBlackListRepository.Table
                                                    .Select(b => b.FarmerId)
                                                    .ToList();
                    #endregion
                    if (formFile != null)
                    {
                        //model.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");
                        var fileName = GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");
                        model.ImageUrl = $"blacklist/{fileName}";
                    }

                    _UnitOfWork.AppUserBlackListRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    #region here we will make block for all farms that its owner blocked also ..

                    if (model.IsBlocked == true)
                    {
                        foreach (var farm in farms)
                        {
                            if (!blackListFarmIds.Contains(farm.Id))
                            {
                                var blackListEntity = new FarmerBlackList
                                {
                                    FarmerId = farm.Id,
                                    FarmerName = farm.Name,
                                    FarmerNameEn = farm.Name,
                                    FarmerMobNum = farm.MobileNumber,
                                    Reason = "حظر هذه المزرعه",
                                    ReasonEn = "Block this farm",
                                    IsBlocked = true
                                };

                                _UnitOfWork.FarmerBlackListRepository.Insert(blackListEntity);
                                _UnitOfWork.Save();
                            }

                        }


                    }

                    #endregion
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving , please contact to administrator");
            }
            return View(NewFillModel(new AppUserBlackListModel()));
        }



        //[HttpPost]
        //public IActionResult Create(AppUserBlackListModel model, IFormFile formFile)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return View(NewFillModel(model));

        //        // ==============================
        //        // 1. رفع الصورة
        //        // ==============================
        //        if (formFile != null)
        //        {
        //            model.ImageUrl = "blacklist/" +
        //                GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");
        //        }

        //        // ==============================
        //        // 2. حفظ AppUserBlackList
        //        // ==============================
        //        var entity = model.ToEntity();
        //        _UnitOfWork.AppUserBlackListRepository.InsertEntity(entity);

        //        // مهم نحفظ علشان نضمن البيانات
        //        _UnitOfWork.Save();

        //        // ==============================
        //        // 3. جلب مزارع المستخدم
        //        // ==============================
        //        var farms = _UnitOfWork.FarmerRepository.Table
        //            .Where(f => f.UserId == model.UserId)
        //            .ToList();

        //        // ==============================
        //        // 4. جلب FarmerBlackList
        //        // ==============================
        //        var farmerBlackList = _UnitOfWork.FarmerBlackListRepository.Table
        //            .ToList();

        //        // نحولها Dictionary للأداء
        //        var dict = farmerBlackList.ToDictionary(x => x.FarmerId);

        //        // ==============================
        //        // 5. لو المستخدم Blocked
        //        // ==============================
        //        if (model.IsBlocked)
        //        {
        //            foreach (var farm in farms)
        //            {
        //                if (dict.ContainsKey(farm.Id))
        //                {
        //                    // 🔄 Update
        //                    var existing = dict[farm.Id];

        //                    existing.IsBlocked = true;
        //                    existing.FarmerName = farm.Name;
        //                    existing.FarmerNameEn = farm.Name;
        //                    existing.FarmerMobNum = farm.MobileNumber;
        //                    existing.Reason = "حظر هذه المزرعه";
        //                    existing.ReasonEn = "Block this farm";
        //                    _UnitOfWork.FarmerBlackListRepository.Update(existing);
        //                }
        //                else
        //                {
        //                    // ➕ Insert
        //                    _UnitOfWork.FarmerBlackListRepository.Insert(new FarmerBlackList
        //                    {
        //                        FarmerId = farm.Id,
        //                        FarmerName = farm.Name,
        //                        FarmerNameEn = farm.Name,
        //                        FarmerMobNum = farm.MobileNumber,
        //                        Reason = "حظر هذه المزرعه",
        //                        ReasonEn = "Block this farm",
        //                        IsBlocked = true,
        //                    });
        //                }
        //            }
        //        }

        //        // ==============================
        //        // 6. Save مرة واحدة
        //        // ==============================
        //        _UnitOfWork.Save();

        //        SuccessNotification("تم اضافة السجل بنجاح");
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification("error while saving , please contact administrator");
        //    }

        //    return View(NewFillModel(model));
        //}



        public IActionResult Edit(int id)
        {
            AppUserBlackList AppUserBlackList = _UnitOfWork.AppUserBlackListRepository.GetById(id);
            if (AppUserBlackList == null)
                return RedirectToAction("Index", new { farmerId = 0 });


            ViewBag.activePage = "قائمة العملاء المحظوريين (أصحاب المزارع)";
            return View(EditFillModel(AppUserBlackList.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(AppUserBlackListModel model, IFormFile formFile)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        if (formFile != null)
            //            model.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");

            //        _UnitOfWork.AppUserBlackListRepository.Update(model.ToEntity());
            //        _UnitOfWork.Save();
            //        SuccessNotification("تم تحديث السجل بنجاح");
            //        return RedirectToAction("Index");
            //    }
            //}
            //catch (Exception e)
            //{
            //    ErrorNotification("error while saving  , please contact to administrator");
            //}
            //return View(model);
            try
            {
                if (ModelState.IsValid)
                {
                    // 1. رفع الصورة لو موجودة
                    if (formFile != null)
                    {
                        //model.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");
                        var fileName = GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");
                        model.ImageUrl = $"blacklist/{fileName}";
                    }

                    // 2. Update AppUserBlackList
                    _UnitOfWork.AppUserBlackListRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    // 3. هات كل المزارع الخاصة بالمستخدم
                    var farmsIds = _UnitOfWork.FarmerRepository.Table
                        .Where(f => f.UserId == model.UserId)
                        .Select(f => f.Id)
                        .ToList();

                    // 4. هات المزارع الموجودة بالفعل في FarmerBlackList
                    var farmerBlackList = _UnitOfWork.FarmerBlackListRepository.Table
                        .Where(b => farmsIds.Contains(b.Id))
                        .ToList();

                    // ==============================
                    // ✅ الحالة 1: Block = true
                    // ==============================
                    if (model.IsBlocked)
                    {
                        foreach (var farmId in farmsIds)
                        {
                            var existing = farmerBlackList
                                .FirstOrDefault(b => b.FarmerId == farmId);

                            if (existing != null)
                            {
                                // موجود → تأكد إنه blocked
                                existing.IsBlocked = true;
                                _UnitOfWork.FarmerBlackListRepository.Update(existing);
                            }
                            else
                            {
                                // مش موجود → أضفه
                                _UnitOfWork.FarmerBlackListRepository.Insert(new FarmerBlackList
                                {
                                    FarmerId = farmId,
                                    IsBlocked = true
                                });
                            }
                        }
                    }
                    // ==============================
                    // ❌ الحالة 2: Block = false
                    // ==============================
                    else
                    {
                        foreach (var item in farmerBlackList)
                        {
                            item.IsBlocked = false;
                            //or make delete
                            _UnitOfWork.FarmerBlackListRepository.Update(item);
                            _UnitOfWork.Save();
                        }
                    }

                    // 5. حفظ مرة واحدة


                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving , please contact administrator");
            }

            return View(model);
        }

        //public IActionResult Delete(int id)
        //{
        //    var farmsIds = _UnitOfWork.FarmerRepository.Table
        //                            .Where(f => f.UserId == id)
        //                            .Select(f => f.Id)
        //                            .ToList();
        //    var farmerBlackList = _UnitOfWork.FarmerBlackListRepository.Table
        //                                    .Where(b => farmsIds.Contains(b.Id))
        //                                    .ToList();
        //    foreach (var farmId in farmsIds)
        //    {
        //        var existing = farmerBlackList
        //            .FirstOrDefault(b => b.FarmerId == farmId);

        //        if (existing != null)
        //        {
        //            _UnitOfWork.FarmerBlackListRepository.Delete(existing);
        //        }
        //    }
        //        string result = "1";
        //    AppUserBlackList AppUserBlackList = _AppUserBlackListRepository.GetById(id);
        //    if (AppUserBlackList == null)
        //        return Json("Record Not Exists");

        //    try
        //    {
        //        _UnitOfWork.AppUserBlackListRepository.Delete(AppUserBlackList);
        //        _UnitOfWork.Save();
        //        SuccessNotification("Delete Succesfuly");
        //    }
        //    catch (Exception)
        //    {
        //        result = "There is data associated with this record";
        //    }

        //    return Json(result);
        //}
        public IActionResult Delete(int id)
        {
            string result = "1";

            try
            {
                var User = _UnitOfWork.AppUserBlackListRepository.Table
                                        .Where(u => u.Id == id)
                                        .Select(f => f.UserId)
                                        .ToList();

                //var farmsIds = _UnitOfWork.FarmerRepository.Table
                //    .Where(U=> User.Contains(U.UserId))
                //    .Select(f => f.Id)
                //    .ToList();

                //var farmerBlackList = _UnitOfWork.FarmerBlackListRepository.Table
                //    .Where(b => farmsIds.Contains(b.Id))
                //    .ToList();


                //var farms = _FarmerRepository.Table
                //             .Where(U => User.Contains(U.UserId))
                //             .OrderByDescending(a => a.Id).ToList();


                var farms = _FarmerRepository.Table
                            .Where(u => User.Contains(u.UserId ?? 0))
                            .OrderByDescending(a => a.Id)
                            .ToList();
                var blackListFarmIds = _FarmerBlackListRepository.Table
                                                .Select(b => b.FarmerId)
                                                .ToList();
                var farmerBlackList = _FarmerBlackListRepository.Table
                                                .ToList();
                //foreach (var item in farmerBlackList)
                //{
                //    //var FarmerBlackList = _FarmerBlackListRepository.GetById(item.id);
                //    _UnitOfWork.FarmerBlackListRepository.Delete(item);
                //    _UnitOfWork.Save();
                //}
                foreach (var farm in farms)
                {
                    if (blackListFarmIds.Contains(farm.Id))
                    {
                        var existing = farmerBlackList
                                .FirstOrDefault(b => b.FarmerId == farm.Id);
                        var FarmerBlackList_Obj = _FarmerBlackListRepository.GetById(existing.Id);
                        _UnitOfWork.FarmerBlackListRepository.Delete(FarmerBlackList_Obj);
                        _UnitOfWork.Save();
                    }
                }


                var AppUserBlackList = _AppUserBlackListRepository.GetById(id);

                if (AppUserBlackList == null)
                    return Json("Record Not Exists");

                _UnitOfWork.AppUserBlackListRepository.Delete(AppUserBlackList);
                _UnitOfWork.Save();
                ///---------///
                SuccessNotification("Delete Successfully");
            }
            catch (Exception)
            {
                result = "There is data associated with this record";
            }

            return Json(result);
        }
    }
}
