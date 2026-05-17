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
        public AppUserController(IUnitOfWork unitOfWork,IRepository<AppUser> userRepository, 
            IWebHostEnvironment hostEnvironment, IConfiguration configuration,
            IRepository<Farmer> FarmerRepository, IRepository<Country> countryRepository,
            IRepository<City> cityRepository, IRepository<FarmerReservation> FarmerReservation, 
            IRepository<FarmerFeedback> FarmerFeedback,IRepository<Regions> regionRepository)
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

        public UserModel FillModel(UserModel model)
        {

            var farms = _FarmerRepository.Table.ToList();

            model.Farms = farms.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return model;
        }

        public IActionResult Index()
        {

            var blockedUserIds = _UnitOfWork.AppUserBlackListRepository.Table
                .Where(b => b.UserId != null && b.IsBlocked == true)
                .Select(b => b.UserId)
                .ToList();

            var model = _userRepository.Table
                .Where(u => !blockedUserIds.Contains(u.Id))
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel())
                .ToList();
            ViewBag.activePage = "المستخدمين";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");
            var UsersBlackListIds = _UnitOfWork.AppUserBlackListRepository.Table
                .Where(a => a.UserId != null && a.IsBlocked == true)
                .Select(a => a.UserId)
                .ToList();

            var model = _userRepository.Table
                .Where(a =>
                    (a.UserName.Contains(search) || a.MobilePhone.Contains(search))
                    && !UsersBlackListIds.Contains(a.Id)
                )
                .OrderByDescending(a => a.Id)
                .Select(c => c.ToModel());

            ViewBag.activePage = "المستخدمين";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult UserFarms(int id)
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var Users = _userRepository.Table.ToList();
            var Regions = _regionRepository.Table.ToList();
            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
                                     .Select(a => a.FarmerId).ToList();
            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && f.UserId==id && !farmerBlackListIds.Contains(f.Id)).OrderByDescending(a => a.Id)
                .Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback, Users, Regions));
            ViewBag.activePage = "المزارع";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "المستخدمين";
            return View(FillModel(new UserModel()));
        }

        [HttpPost]
        public IActionResult Create(UserModel model, IFormFile formFile)
        {
            try
            {
                var exist_1 = _userRepository.Table.Any(U => U.MobilePhone == model.MobilePhone);
                var exist_2 = _userRepository.Table.Any(U => U.MobileNumber == model.MobileNumber);
                if (exist_1 || exist_2)
                {
                    ErrorNotification("أرقام التليفونات هذه مسجلة من قبل و لا يمكن تكرارها");
                    return View(FillModel(model));
                }
                if (ModelState.IsValid)
                {
                    var userEntity = model.ToEntity();
                    //_UnitOfWork.UserRepository.Insert(userEntity);
                    //_UnitOfWork.Save(); // مهم جدا عشان الـ Id يتولد

                    _UnitOfWork.UserRepository.Insert(userEntity);
                    SuccessNotification("تم اضافة السجل بنجاح");
                    _UnitOfWork.Save();
                    var userId = userEntity.Id;
                    //var Last_User = _UnitOfWork.UserRepository.Table.last;
                    var farms = _UnitOfWork.FarmerRepository.Table
                        .Where(x => model.FarmIds.Contains(x.Id))
                        .ToList();
                    foreach (var farm in farms)
                    {
                        farm.UserId = userId; // ربط المستخدم بالمزرعة
                        _UnitOfWork.FarmerRepository.Update(farm);
                        _UnitOfWork.Save();
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //ErrorNotification(e.Message);
                ErrorNotification(e.InnerException.ToString());
                return View(FillModel(model));
            }
            return View(FillModel(model));
        }

        public IActionResult Edit(int id)
        {
            AppUser user = _userRepository.GetById(id);
            //if (user == null)
            //    return RedirectToAction("Index");

            //ViewBag.activePage = "المستخدمين";
            //return View(FillModel(user.ToModel()));
            //Users user = _userRepository.GetById(id);

            if (user == null)
                return RedirectToAction("Index");

            var model = FillModel(user.ToModel());

            var selectedFarms = _UnitOfWork.FarmerRepository.Table
                                            .Where(f => f.UserId == id)
                                            .Select(f => f.Id)
                                            .ToList();

            model.FarmIds = selectedFarms;
            ViewBag.activePage = "المستخدمين";
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.UserRepository.Update(model.ToEntity());
                    SuccessNotification("تم تحديث السجل بنجاح");
                    _UnitOfWork.Save();

                    var oldFarms = _UnitOfWork.FarmerRepository.Table
                    .Where(x => x.UserId == model.Id)
                    .ToList();

                    var newFarmIds = model.FarmIds ?? new List<int>();

                    var removedFarms = oldFarms
                    .Where(x => !newFarmIds.Contains(x.Id))
                    .ToList();


                    foreach (var farm in removedFarms)
                    {
                        farm.UserId = null;
                        _UnitOfWork.FarmerRepository.Update(farm);
                        _UnitOfWork.Save();
                    }

                    //Farms Selected ...
                    var farms = _UnitOfWork.FarmerRepository.Table
                                            .Where(x => model.FarmIds.Contains(x.Id))
                                            .ToList();
                    foreach (var farm in farms)
                    {
                        farm.UserId = model.Id; // ربط المستخدم بالمزرعة
                        _UnitOfWork.FarmerRepository.Update(farm);
                        _UnitOfWork.Save();
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillModel(model));
        }

        public IActionResult Delete(int id)
        {
            var farms = _UnitOfWork.FarmerRepository.Table
                                    .Where(f => f.UserId == id)
                                    .ToList();
            //in case you want delete all farms connect with AppUser ..
            //else you can not delete user ...
            //if (farms.Any())
            //{
            //    foreach (var farm in farms)
            //    {
            //        _UnitOfWork.FarmerRepository.Delete(farm);
            //        _UnitOfWork.Save();
            //    }
            //}
            if (farms.Any())
            {
                return Json("يجب حذف المزارع المتعلقة بهذا الستخدم");
            }

            AppUser user = _userRepository.GetById(id);

            if (user == null)
                return Json("السجل غير معرف");

            _UnitOfWork.UserRepository.Delete(user);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
