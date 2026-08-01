using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Dto;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class SportsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SportsController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        #region Sport Actions


        [HttpPost]
        public IActionResult Index(string search, int cityId, DateTime reservationDate, int sportTypeId)
        {
            return RedirectToAction("Index", new
            {
                search = search,
                cityId = cityId == 0 ? (int?)null : cityId,
                reservationDate = reservationDate == DateTime.MinValue ? (DateTime?)null : reservationDate,
                sportTypeId = sportTypeId == 0 ? (int?)null : sportTypeId
            });
        }

        public IActionResult Index(string search, int? cityId, DateTime? reservationDate, int? sportTypeId)
        {

            var countries = _UnitOfWork.CountryRepository.Table.Where(c => c.Active == true).ToList();
            var cities = _UnitOfWork.CityRepository.Table.Where(c => c.Active == true).ToList();
            var regions = _UnitOfWork.RegionRepository.Table.ToList();
            var users = _UnitOfWork.UserRepository.Table.ToList();
            var sportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();


            var query = _UnitOfWork.SportRepository.Table
                .Include(s => s.Country)
                .Include(s => s.City)
                .Include(s => s.Region)
                .Include(s => s.SportType)
                .Where(s => s.IsActive == true && s.IsDeleted == false);


            if (sportTypeId.HasValue && sportTypeId.Value > 0)
            {
                query = query.Where(s => s.SportTypeId == sportTypeId.Value);
            }


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.NameAr.Contains(search) ||
                    s.NameEn.Contains(search) ||
                    s.MobileNumber.Contains(search) ||
                    s.Number.ToString().Contains(search) ||
                    s.SerialSportKey.Contains(search));
            }


            if (cityId.HasValue && cityId.Value > 0)
            {
                query = query.Where(s => s.CityId == cityId.Value);
            }


            var sports = query.OrderByDescending(s => s.Id).ToList();


            var model = sports.Select(s => s.ToModel(countries, cities, regions, users, sportTypes)).ToList();


            ViewBag.TotalSports = model.Count();
            ViewBag.ApprovedSports = model.Count(m => m.IsApprove);
            ViewBag.VIPSports = model.Count(m => m.IsVIP);
            ViewBag.BlockedSports = model.Count(m => m.IsBlocked);


            ViewBag.activePage = "الأنشطة الرياضية";
            ViewBag.search = search;
            ViewBag.CityBy = cityId;
            ViewBag.ReservationDate = reservationDate;
            ViewBag.cities = cities;
            ViewBag.DefaultDate = DateTime.Now;
            ViewBag.SportTypeId = sportTypeId;
            ViewBag.SportTypes = sportTypes;
            ViewBag.SelectedSportTypeName = sportTypeId.HasValue && sportTypeId.Value > 0
                ? sportTypes.FirstOrDefault(s => s.Id == sportTypeId.Value)?.NameAr
                : "";
            return View(model);
        }

        public IActionResult Create(int? SportTypeId)
        {
            ViewBag.activePage = "الأنشطة الرياضية";
            var sportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            ViewBag.SportTypeTitle = EnumExtensions.GetDisplayName((UserTypeEnumTitle)SportTypeId.Value);
            ViewBag.SelectedSportTypeId = SportTypeId ?? 0;
            ViewData["SelectedSportTypeId"] = SportTypeId ?? 0;
            //ViewBag.Users = _UnitOfWork.UserRepository.Table.ToList();

            //ViewData["SelectedSportTypeId"] = SportTypeId ?? 0;
            //ViewBag.SelectedSportTypeId = SportTypeId ?? 0;
            //return View(NewFillModel(new SportModel(), SportTypeId));
            var model = NewFillModel(new SportModel(), SportTypeId ?? 0);
            //ViewBag.Users = model.Users;
            // ===== جلب المستخدمين حسب نوع الرياضة (Contains) =====
            if (SportTypeId.HasValue && SportTypeId.Value > 0)
            {
                var searchValue = SportTypeId.Value.ToString();
                ViewBag.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                ViewBag.Users = new List<AppUser>();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(SportModel model)
        {
            //, IFormFile formFile
            LogFile logFile = new LogFile();

            try
            {
                // التحقق من صحة البيانات (مثل Farmer)
                if (model.CountryId <= 0)
                    ModelState.AddModelError("CountryId", "برجاء اختيار البلد (الدولة) من القائمة");

                if (model.CityId <= 0)
                    ModelState.AddModelError("CityId", "برجاء اختيار المدينة (المحافظة) من القائمة");

                if (model.RegionId <= 0)
                    ModelState.AddModelError("RegionId", "برجاء اختيار المنطقة من القائمة");

                if (model.SportTypeId <= 0)
                    ModelState.AddModelError("SportTypeId", "برجاء اختيار نوع الرياضة من القائمة");

                //if (model.UserId <= 0)
                //    ModelState.AddModelError("UserId", "برجاء اختيار المالك من القائمة");

                if (ModelState.IsValid)
                {
                    // get max number (مثل Farmer)
                    long nMaxNumber = _UnitOfWork.SportRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();
                    int nId = _UnitOfWork.SportRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new sport (مثل Farmer)
                    var sport = model.ToEntity();
                    sport.Number = nMaxNumber + 1;
                    sport.IssueDate = DateTime.Now;
                    sport.ExpiryDate = DateTime.Now.AddMonths(3);
                    sport.CreatedDate = DateTime.Now;
                    sport.IsActive = true;
                    sport.IsDeleted = false;
                    if (sport.UserId > 0)
                    {
                        var User = _UnitOfWork.UserRepository.Table.FirstOrDefault(U => U.Id == sport.UserId);
                        sport.MobileOwnerAppUser = User.MobilePhone;
                    }
                    _UnitOfWork.SportRepository.InsertEntity(sport);
                    _UnitOfWork.Save();

                    int nSportId = _UnitOfWork.SportRepository.Table.FirstOrDefault(f => f.Number == (nMaxNumber + 1)).Id;

                    sport.SerialSportKey = $"{nSportId}";
                    sport.Id = nSportId;
                    _UnitOfWork.SportRepository.Update(sport);
                    _UnitOfWork.Save();

                    // =====  (Sport Features) =====
                    foreach (SportFeatureDto feature in model.SportFeatures.Where(e => e.IsCheck == true))
                    {
                        _UnitOfWork.SportSportFeatureRepository.Insert(new SportSportFeature
                        {
                            SportId = nSportId,
                            SportFeatureId = feature.TypeId,
                            IsChecked = true,
                            DescriptionAr = feature.DescriptionAr,
                            DescriptionEn = feature.DescriptionEn
                        });
                    }

                    // =====  (General Facilities) =====
                    foreach (GeneralFacilityDto facility in model.GeneralFacilities.Where(e => e.IsCheck == true))
                    {
                        _UnitOfWork.SportGeneralFacilityRepository.Insert(new SportGeneralFacility
                        {
                            SportId = nSportId,
                            GeneralFacilityId = facility.FacilityId,
                            IsActive = true
                        });
                    }

                    // ===== (Additional Services) =====
                    foreach (AdditionalServiceDto service in model.AdditionalServices.Where(e => e.IsCheck == true))
                    {
                        _UnitOfWork.SportAdditionalServiceRepository.Insert(new SportAdditionalService
                        {
                            SportId = nSportId,
                            AdditionalServiceId = service.ServiceId,
                            IsActive = true
                        });
                    }


                    if (model.PriceList != null && model.PriceList.Any())
                    {
                        foreach (var priceDto in model.PriceList)
                        {

                            if (priceDto.HourlyPrice > 0)
                            {
                                var price = new SportPriceList
                                {
                                    SportId = nSportId,
                                    Day = priceDto.Day,
                                    Person = priceDto.Person > 0 ? priceDto.Person : 1,
                                    HourlyPrice = priceDto.HourlyPrice,
                                    PeakHourlyPrice = priceDto.PeakHourlyPrice,
                                    PeakStartTime = priceDto.PeakStartTime,
                                    PeakEndTime = priceDto.PeakEndTime,
                                    OfferHourlyPrice = priceDto.OfferHourlyPrice,
                                    MinBookingHours = 1 // أو قيمة افتراضية
                                };
                                _UnitOfWork.SportPriceListRepository.Insert(price);
                            }
                        }
                    }
                    // ===== (Safety Features) =====
                    foreach (SafetyFeatureDto feature in model.SafetyFeatures.Where(e => e.IsCheck == true))
                    {
                        _UnitOfWork.SportSafetyFeatureRepository.Insert(new SportSafetyFeature
                        {
                            SportId = nSportId,
                            SafetyFeatureId = feature.TypeId,
                            IsChecked = true,
                            DescriptionAr = feature.DescriptionAr,
                            DescriptionEn = feature.DescriptionEn
                        });
                    }

                    // ===== حفظ تفاصيل العقار الديناميكية =====
                    if (model.PropertyValues != null && model.PropertyValues.Any())
                    {
                        foreach (var valueDto in model.PropertyValues.Where(v => v.PropertyTemplateId > 0))
                        {
                            var value = new SportPropertyValue
                            {
                                SportId = nSportId,
                                PropertyTemplateId = valueDto.PropertyTemplateId,
                                ValueText = valueDto.ValueText,
                                ValueBool = valueDto.ValueBool,
                                ValueOptionId = valueDto.ValueOptionId
                            };
                            _UnitOfWork.SportPropertyValueRepository.Insert(value);
                            _UnitOfWork.Save();
                        }
                    }


                    int nSortImage = 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            if (file != null && file.Length > 0)
                            {
                                _UnitOfWork.SportImageRepository.Insert(new SportImage
                                {
                                    SportId = nSportId,
                                    Url = "sports/" + GenericFunction.UploadedFile(file, _webHostEnvironment, "sports"),
                                    Sort = nSortImage,
                                    Vip = true,
                                    Active = true
                                });
                                //_UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                                _UnitOfWork.Save();
                                nSortImage++;
                            }
                        }
                    }


                    int nSortVideo = 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            if (file != null && file.Length > 0)
                            {
                                _UnitOfWork.SportVideoRepository.Insert(new SportVideo
                                {
                                    SportId = nSportId,
                                    Url = "sports/" + GenericFunction.UploadedVideo(file, _webHostEnvironment, "sports"),
                                    Sort = nSortVideo,
                                    Active = true
                                });
                                //_UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                                _UnitOfWork.Save();
                                nSortVideo++;
                            }
                        }
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة النشاط الرياضى بنجاح");
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", new { SportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Saving: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Create Sport - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create Sport - Stack Trace Message ", e.StackTrace);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Create Sport - Inner Exception Message ", e.InnerException.ToString());
                ViewBag.SportTypeTitle = EnumExtensions.GetDisplayName((UserTypeEnumTitle)model.SportTypeId);
                //return RedirectToAction("Create", new { id = model.Id });
                //new { sportTypeId = model.SportTypeId }
                return RedirectToAction("Create", new { SportTypeId = model.SportTypeId });
            }


            // عند الرجوع، نفلتر المستخدمين تاني
            if (model.SportTypeId > 0)
            {
                //+ 1
                //var userType = (UserTypeEnum)(model.SportTypeId);
                //ViewBag.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                //var userType = model.SportTypeId.ToString();  // ✅ تحويل إلى string
                //ViewBag.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                var searchValue = model.SportTypeId.ToString();
                ViewBag.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                ViewBag.Users = new List<AppUser>();
            }

            ViewBag.RegionId = model.RegionId;
            model = NewFillModel(model, model.SportTypeId);
            return View(model);
            //ViewBag.Users = _UnitOfWork.UserRepository.Table.ToList();
            //ViewBag.RegionId = model.RegionId;
            //model = NewFillModel(model);
            //return View(model);
        }

        public IActionResult Edit(int id)
        {
            var sport = _UnitOfWork.SportRepository
                .Table
                .Include(s => s.Country)
                .Include(s => s.City)
                .Include(s => s.Region)
                .Include(s => s.SportType)
                .Include(s => s.SportImages)
                .Include(s => s.SportVideos)
                .Include(s => s.PriceList)
                .FirstOrDefault(s => s.Id == id);

            if (sport == null)
                return RedirectToAction("Index");
            ViewBag.Users = _UnitOfWork.UserRepository.Table.ToList();
            ViewBag.RegionId = sport.RegionId;
            ViewData["SelectedSportTypeId"] = sport.SportTypeId;

            ViewBag.SelectedSportTypeId = sport.SportTypeId;
            ViewBag.activePage = "الأنشطة الرياضية";
            ViewBag.SportTypeTitle = EnumExtensions.GetDisplayName((UserTypeEnumTitle)sport.SportTypeId);
            // ===== إضافة هذا السطر =====
            ViewData["SelectedSportTypeId"] = sport.SportTypeId;
            //return View(EditFillModel(sport.ToModel()));
            var model = EditFillModel(sport.ToModel());
            // ===== تعيين ViewBag.Users =====
            //ViewBag.Users = model.Users;
            // ===== جلب المستخدمين حسب نوع الرياضة (Contains) =====
            if (sport.SportTypeId > 0)
            {
                var searchValue = sport.SportTypeId.ToString();
                ViewBag.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                ViewBag.Users = new List<AppUser>();
            }
            return View(model);
        }

        //[HttpPost]
        //[RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        //public IActionResult Edit(SportModel model)
        //{
        //    //, IFormFile formFile
        //    LogFile logFile = new LogFile();

        //    try
        //    {
        //        // التحقق من صحة البيانات (مثل Farmer)
        //        if (model.CountryId <= 0)
        //            ModelState.AddModelError("CountryId", "برجاء اختيار البلد (الدولة) من القائمة");

        //        if (model.CityId <= 0)
        //            ModelState.AddModelError("CityId", "برجاء اختيار المدينة (المحافظة) من القائمة");

        //        if (model.RegionId <= 0)
        //            ModelState.AddModelError("RegionId", "برجاء اختيار المنطقة من القائمة");

        //        if (model.SportTypeId <= 0)
        //            ModelState.AddModelError("SportTypeId", "برجاء اختيار نوع الرياضة من القائمة");

        //        //if (model.UserId <= 0)
        //        //    ModelState.AddModelError("UserId", "برجاء اختيار المالك من القائمة");

        //        if (ModelState.IsValid)
        //        {


        //            int maxOrderIdImage = 0;
        //            int maxOrderIdVideo = 0;

        //            if (model.UserId > 0)
        //            {
        //                var User = _UnitOfWork.UserRepository.Table.FirstOrDefault(U => U.Id == model.UserId);
        //                model.MobileOwnerAppUser = User.MobilePhone;
        //            }



        //            if (_UnitOfWork.SportImageRepository.Table.Any(f => f.SportId == model.Id))
        //                maxOrderIdImage = _UnitOfWork.SportImageRepository.Table.Where(f => f.SportId == model.Id).Max(x => x.Sort);

        //            if (_UnitOfWork.SportVideoRepository.Table.Any(f => f.SportId == model.Id))
        //                maxOrderIdVideo = _UnitOfWork.SportVideoRepository.Table.Where(f => f.SportId == model.Id).Max(x => x.Sort);

        //            if (model.PriceList != null)
        //            {
        //                foreach (SportPriceList priceList in model.PriceList)
        //                {
        //                    _UnitOfWork.SportPriceListRepository.Update(priceList);
        //                    //_UnitOfWork.Save();
        //                }
        //            }

        //            if (model.SportFeatures != null)
        //            {
        //                foreach (SportFeatureDto feature in model.SportFeatures)
        //                {
        //                    SportSportFeature sportFeature = new SportSportFeature();
        //                    sportFeature.Id = feature.Id;
        //                    sportFeature.SportId = model.Id;
        //                    sportFeature.SportFeatureId = feature.TypeId;
        //                    sportFeature.DescriptionAr = feature.DescriptionAr;
        //                    sportFeature.DescriptionEn = feature.DescriptionEn;

        //                    if (feature.Id > 0)
        //                    {
        //                        if (!feature.IsCheck)
        //                        {
        //                            _UnitOfWork.SportSportFeatureRepository.Delete(sportFeature);
        //                        }
        //                        else
        //                        {
        //                            if (_UnitOfWork.SportSportFeatureRepository.Table.Count(f => f.SportId == model.Id && f.SportFeatureId == feature.TypeId) == 0)
        //                                _UnitOfWork.SportSportFeatureRepository.Insert(sportFeature);
        //                            else if (_UnitOfWork.SportSportFeatureRepository.Table.Count(f => f.SportId == model.Id && f.SportFeatureId == feature.TypeId) == 1)
        //                            {
        //                                _UnitOfWork.SportSportFeatureRepository.Update(sportFeature);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (feature.IsCheck)
        //                            _UnitOfWork.SportSportFeatureRepository.Insert(sportFeature);
        //                    }
        //                }
        //            }

        //            if (model.GeneralFacilities != null)
        //            {
        //                var oldFacilities = _UnitOfWork.SportGeneralFacilityRepository.Table.Where(f => f.SportId == model.Id).ToList();
        //                foreach (var old in oldFacilities)
        //                {
        //                    _UnitOfWork.SportGeneralFacilityRepository.Delete(old);
        //                }

        //                foreach (GeneralFacilityDto facility in model.GeneralFacilities.Where(e => e.IsCheck == true))
        //                {
        //                    _UnitOfWork.SportGeneralFacilityRepository.Insert(new SportGeneralFacility
        //                    {
        //                        SportId = model.Id,
        //                        GeneralFacilityId = facility.FacilityId,
        //                        IsActive = true
        //                    });
        //                }
        //            }

        //            if (model.AdditionalServices != null)
        //            {
        //                var oldServices = _UnitOfWork.SportAdditionalServiceRepository.Table.Where(s => s.SportId == model.Id).ToList();
        //                foreach (var old in oldServices)
        //                {
        //                    _UnitOfWork.SportAdditionalServiceRepository.Delete(old);
        //                }

        //                foreach (AdditionalServiceDto service in model.AdditionalServices.Where(e => e.IsCheck == true))
        //                {
        //                    _UnitOfWork.SportAdditionalServiceRepository.Insert(new SportAdditionalService
        //                    {
        //                        SportId = model.Id,
        //                        AdditionalServiceId = service.ServiceId,
        //                        IsActive = true
        //                    });
        //                }
        //            }
        //            if (model.SafetyFeatures != null)
        //            {
        //                foreach (SafetyFeatureDto feature in model.SafetyFeatures)
        //                {
        //                    SportSafetyFeature safetyFeature = new SportSafetyFeature();
        //                    safetyFeature.Id = feature.Id;
        //                    safetyFeature.SportId = model.Id;
        //                    safetyFeature.SafetyFeatureId = feature.TypeId;
        //                    safetyFeature.DescriptionAr = feature.DescriptionAr;
        //                    safetyFeature.DescriptionEn = feature.DescriptionEn;

        //                    if (feature.Id > 0)
        //                    {
        //                        if (!feature.IsCheck)
        //                        {
        //                            _UnitOfWork.SportSafetyFeatureRepository.Delete(safetyFeature);
        //                        }
        //                        else
        //                        {
        //                            if (_UnitOfWork.SportSafetyFeatureRepository.Table.Count(f => f.SportId == model.Id && f.SafetyFeatureId == feature.TypeId) == 0)
        //                                _UnitOfWork.SportSafetyFeatureRepository.Insert(safetyFeature);
        //                            else if (_UnitOfWork.SportSafetyFeatureRepository.Table.Count(f => f.SportId == model.Id && f.SafetyFeatureId == feature.TypeId) == 1)
        //                            {
        //                                _UnitOfWork.SportSafetyFeatureRepository.Update(safetyFeature);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (feature.IsCheck)
        //                            _UnitOfWork.SportSafetyFeatureRepository.Insert(safetyFeature);
        //                    }
        //                }
        //            }


        //            int nSortImage = maxOrderIdImage + 1;
        //            if (model.Images != null)
        //            {
        //                foreach (IFormFile file in model.Images)
        //                {
        //                    if (file != null && file.Length > 0)
        //                    {
        //                        _UnitOfWork.SportImageRepository.Insert(new SportImage
        //                        {
        //                            SportId = model.Id,
        //                            Url = "sports/" + GenericFunction.UploadedFile(file, _webHostEnvironment, "sports"),
        //                            Sort = nSortImage,
        //                            Vip = true,
        //                            Active = true
        //                        });
        //                        _UnitOfWork.Save();
        //                        nSortImage++;
        //                    }
        //                }
        //            }

        //            int nSortVideo = maxOrderIdVideo + 1;
        //            if (model.Videos != null)
        //            {
        //                foreach (IFormFile file in model.Videos)
        //                {
        //                    if (file != null && file.Length > 0)
        //                    {
        //                        _UnitOfWork.SportVideoRepository.Insert(new SportVideo
        //                        {
        //                            SportId = model.Id,
        //                            Url = "sports/" + GenericFunction.UploadedVideo(file, _webHostEnvironment, "sports"),
        //                            Sort = nSortVideo,
        //                            Active = true
        //                        });
        //                        _UnitOfWork.Save();
        //                        nSortVideo++;
        //                    }
        //                }
        //            }




        //            var sport = model.ToEntity();
        //            sport.ModifiedDate = DateTime.Now;
        //            var existingSport = _UnitOfWork.SportRepository.GetById(model.Id);
        //            if (existingSport != null)
        //            {
        //                sport.CreatedDate = existingSport.CreatedDate;
        //            }
        //            //sport.CreatedDate = model.CreatedDate;
        //            ViewBag.RegionId = model.RegionId;
        //            model.Regions = _UnitOfWork.RegionRepository.Table.Where(r => r.Id == model.CityId).ToList();
        //            _UnitOfWork.SportRepository.Update(sport);
        //            _UnitOfWork.Save();
        //            SuccessNotification("تم تحديث النشاط الرياضى بنجاح");
        //            return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorNotification($"Error while Update: {e.Message}. Please contact the administrator.");
        //        logFile.LogCustomInfo("Edit Sport - Exception Message ", e.Message);
        //        logFile.LogCustomInfo("Edit Sport - Stack Trace Message ", e.StackTrace);
        //        if (e.InnerException != null)
        //            logFile.LogCustomInfo("Edit Sport - Inner Exception Message ", e.InnerException.ToString());
        //        return RedirectToAction("Edit", new { id = model.Id });

        //    }


        //    if (model.SportTypeId > 0)
        //    {
        //        var userType = (UserTypeEnum)(model.SportTypeId + 1);
        //        ViewBag.Users = _UnitOfWork.UserRepository.Table
        //            .Where(u => u.UserType == userType)
        //            .OrderBy(u => u.UserName)
        //            .ToList();
        //    }
        //    else
        //    {
        //        ViewBag.Users = new List<AppUser>();
        //    }

        //    ViewBag.RegionId = model.RegionId;
        //    ViewData["SelectedSportTypeId"] = model.SportTypeId;
        //    model = EditFillModel(model);
        //    return View(model);
        //    //ViewBag.Users = _UnitOfWork.UserRepository.Table.ToList();
        //    //ViewBag.RegionId = model.RegionId;
        //    //ViewData["SelectedSportTypeId"] = model.SportTypeId;
        //    //model = EditFillModel(model);
        //    //return View(model);
        //}

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public IActionResult Edit(SportModel model)
        {
            LogFile logFile = new LogFile();

            try
            {
                // ===== التحقق من صحة البيانات =====
                if (model.CountryId <= 0)
                    ModelState.AddModelError("CountryId", "برجاء اختيار البلد (الدولة) من القائمة");

                if (model.CityId <= 0)
                    ModelState.AddModelError("CityId", "برجاء اختيار المدينة (المحافظة) من القائمة");

                if (model.RegionId <= 0)
                    ModelState.AddModelError("RegionId", "برجاء اختيار المنطقة من القائمة");

                if (model.SportTypeId <= 0)
                    ModelState.AddModelError("SportTypeId", "برجاء اختيار نوع الرياضة من القائمة");

                if (ModelState.IsValid)
                {
                    // ===== جلب بيانات المستخدم =====
                    if (model.UserId > 0)
                    {
                        var User = _UnitOfWork.UserRepository.Table.FirstOrDefault(U => U.Id == model.UserId);
                        model.MobileOwnerAppUser = User?.MobilePhone;
                    }

                    // ===== الصور والفيديوهات =====
                    int maxOrderIdImage = 0;
                    int maxOrderIdVideo = 0;

                    if (_UnitOfWork.SportImageRepository.Table.Any(f => f.SportId == model.Id))
                        maxOrderIdImage = _UnitOfWork.SportImageRepository.Table.Where(f => f.SportId == model.Id).Max(x => x.Sort);

                    if (_UnitOfWork.SportVideoRepository.Table.Any(f => f.SportId == model.Id))
                        maxOrderIdVideo = _UnitOfWork.SportVideoRepository.Table.Where(f => f.SportId == model.Id).Max(x => x.Sort);

                    // ============================================================
                    // 1. تحديث جدول الأسعار
                    // ============================================================
                    if (model.PriceList != null)
                    {
                        foreach (SportPriceList priceList in model.PriceList)
                        {
                            _UnitOfWork.SportPriceListRepository.Update(priceList);
                        }
                    }

                    // ============================================================
                    // 2. تحديث المرفقات الخاصة (Sport Features)
                    // ============================================================
                    if (model.SportFeatures != null)
                    {
                        // جلب المرفقات الموجودة
                        var existingFeatures = _UnitOfWork.SportSportFeatureRepository.Table
                            .Where(f => f.SportId == model.Id)
                            .ToList();

                        // قائمة المرفقات المختارة
                        var checkedFeatureIds = model.SportFeatures
                            .Where(f => f.IsCheck == true)
                            .Select(f => f.TypeId)
                            .ToList();

                        // حذف المرفقات التي تم إلغاء تحديدها
                        var toDelete = existingFeatures
                            .Where(f => !checkedFeatureIds.Contains(f.SportFeatureId))
                            .ToList();

                        foreach (var item in toDelete)
                        {
                            _UnitOfWork.SportSportFeatureRepository.Delete(item);
                        }

                        // إضافة أو تحديث المرفقات المختارة
                        foreach (var feature in model.SportFeatures.Where(f => f.IsCheck == true))
                        {
                            var exists = existingFeatures.Any(f => f.SportFeatureId == feature.TypeId);
                            if (!exists)
                            {
                                _UnitOfWork.SportSportFeatureRepository.Insert(new SportSportFeature
                                {
                                    SportId = model.Id,
                                    SportFeatureId = feature.TypeId,
                                    IsChecked = true,
                                    DescriptionAr = feature.DescriptionAr,
                                    DescriptionEn = feature.DescriptionEn
                                });
                            }
                            else
                            {
                                var existing = existingFeatures.First(f => f.SportFeatureId == feature.TypeId);
                                existing.DescriptionAr = feature.DescriptionAr;
                                existing.DescriptionEn = feature.DescriptionEn;
                                existing.IsChecked = true;
                                _UnitOfWork.SportSportFeatureRepository.Update(existing);
                            }
                        }
                    }

                    // ============================================================
                    // 3. تحديث المرافق العامة (General Facilities)
                    // ============================================================
                    if (model.GeneralFacilities != null)
                    {
                        // جلب المرافق الموجودة
                        var existingFacilities = _UnitOfWork.SportGeneralFacilityRepository.Table
                            .Where(f => f.SportId == model.Id)
                            .ToList();

                        // قائمة المرافق المختارة
                        var checkedFacilityIds = model.GeneralFacilities
                            .Where(f => f.IsCheck == true)
                            .Select(f => f.FacilityId)
                            .ToList();

                        // حذف المرافق التي تم إلغاء تحديدها
                        var toDeleteFacilities = existingFacilities
                            .Where(f => !checkedFacilityIds.Contains(f.GeneralFacilityId))
                            .ToList();

                        foreach (var item in toDeleteFacilities)
                        {
                            _UnitOfWork.SportGeneralFacilityRepository.Delete(item);
                        }

                        // إضافة المرافق المختارة
                        foreach (var facility in model.GeneralFacilities.Where(f => f.IsCheck == true))
                        {
                            var exists = existingFacilities.Any(f => f.GeneralFacilityId == facility.FacilityId);
                            if (!exists)
                            {
                                _UnitOfWork.SportGeneralFacilityRepository.Insert(new SportGeneralFacility
                                {
                                    SportId = model.Id,
                                    GeneralFacilityId = facility.FacilityId,
                                    IsActive = true
                                });
                            }
                        }
                    }

                    // ============================================================
                    // 4. تحديث الخدمات الإضافية (Additional Services)
                    // ============================================================
                    if (model.AdditionalServices != null)
                    {
                        // جلب الخدمات الموجودة
                        var existingServices = _UnitOfWork.SportAdditionalServiceRepository.Table
                            .Where(s => s.SportId == model.Id)
                            .ToList();

                        // قائمة الخدمات المختارة
                        var checkedServiceIds = model.AdditionalServices
                            .Where(s => s.IsCheck == true)
                            .Select(s => s.ServiceId)
                            .ToList();

                        // حذف الخدمات التي تم إلغاء تحديدها
                        var toDeleteServices = existingServices
                            .Where(s => !checkedServiceIds.Contains(s.AdditionalServiceId))
                            .ToList();

                        foreach (var item in toDeleteServices)
                        {
                            _UnitOfWork.SportAdditionalServiceRepository.Delete(item);
                        }

                        // إضافة الخدمات المختارة
                        foreach (var service in model.AdditionalServices.Where(s => s.IsCheck == true))
                        {
                            var exists = existingServices.Any(s => s.AdditionalServiceId == service.ServiceId);
                            if (!exists)
                            {
                                _UnitOfWork.SportAdditionalServiceRepository.Insert(new SportAdditionalService
                                {
                                    SportId = model.Id,
                                    AdditionalServiceId = service.ServiceId,
                                    IsActive = true
                                });
                            }
                        }
                    }

                    // ============================================================
                    // 5. تحديث ميزات الأمان (Safety Features)
                    // ============================================================
                    if (model.SafetyFeatures != null)
                    {
                        // جلب الميزات الموجودة
                        var existingSafety = _UnitOfWork.SportSafetyFeatureRepository.Table
                            .Where(s => s.SportId == model.Id)
                            .ToList();

                        // قائمة الميزات المختارة
                        var checkedSafetyIds = model.SafetyFeatures
                            .Where(s => s.IsCheck == true)
                            .Select(s => s.TypeId)
                            .ToList();

                        // حذف الميزات التي تم إلغاء تحديدها
                        var toDeleteSafety = existingSafety
                            .Where(s => !checkedSafetyIds.Contains(s.SafetyFeatureId))
                            .ToList();

                        foreach (var item in toDeleteSafety)
                        {
                            _UnitOfWork.SportSafetyFeatureRepository.Delete(item);
                        }

                        // إضافة أو تحديث الميزات المختارة
                        foreach (var feature in model.SafetyFeatures.Where(s => s.IsCheck == true))
                        {
                            var exists = existingSafety.Any(s => s.SafetyFeatureId == feature.TypeId);
                            if (!exists)
                            {
                                _UnitOfWork.SportSafetyFeatureRepository.Insert(new SportSafetyFeature
                                {
                                    SportId = model.Id,
                                    SafetyFeatureId = feature.TypeId,
                                    IsChecked = true,
                                    DescriptionAr = feature.DescriptionAr,
                                    DescriptionEn = feature.DescriptionEn
                                });
                            }
                            else
                            {
                                var existing = existingSafety.First(s => s.SafetyFeatureId == feature.TypeId);
                                existing.DescriptionAr = feature.DescriptionAr;
                                existing.DescriptionEn = feature.DescriptionEn;
                                existing.IsChecked = true;
                                _UnitOfWork.SportSafetyFeatureRepository.Update(existing);
                            }
                        }
                    }

                    // ===== تحديث تفاصيل العقار الديناميكية =====
                    var oldValues = _UnitOfWork.SportPropertyValueRepository.Table
                        .Where(v => v.SportId == model.Id)
                        .ToList();

                    foreach (var old in oldValues)
                    {
                        _UnitOfWork.SportPropertyValueRepository.Delete(old);
                        _UnitOfWork.Save();
                    }

                    if (model.PropertyValues != null && model.PropertyValues.Any())
                    {
                        foreach (var valueDto in model.PropertyValues.Where(v => v.PropertyTemplateId > 0))
                        {
                            var value = new SportPropertyValue
                            {
                                SportId = model.Id,
                                PropertyTemplateId = valueDto.PropertyTemplateId,
                                ValueText = valueDto.ValueText,
                                ValueBool = valueDto.ValueBool,
                                ValueOptionId = valueDto.ValueOptionId
                            };
                            _UnitOfWork.SportPropertyValueRepository.Insert(value);
                            _UnitOfWork.Save();
                        }
                    }

                    // ============================================================
                    // 6. إضافة الصور الجديدة
                    // ============================================================
                    int nSortImage = maxOrderIdImage + 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            if (file != null && file.Length > 0)
                            {
                                _UnitOfWork.SportImageRepository.Insert(new SportImage
                                {
                                    SportId = model.Id,
                                    Url = "sports/" + GenericFunction.UploadedFile(file, _webHostEnvironment, "sports"),
                                    Sort = nSortImage,
                                    Vip = true,
                                    Active = true
                                });
                                _UnitOfWork.Save();
                                nSortImage++;
                            }
                        }
                    }

                    // ============================================================
                    // 7. إضافة الفيديوهات الجديدة
                    // ============================================================
                    int nSortVideo = maxOrderIdVideo + 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            if (file != null && file.Length > 0)
                            {
                                _UnitOfWork.SportVideoRepository.Insert(new SportVideo
                                {
                                    SportId = model.Id,
                                    Url = "sports/" + GenericFunction.UploadedVideo(file, _webHostEnvironment, "sports"),
                                    Sort = nSortVideo,
                                    Active = true
                                });
                                _UnitOfWork.Save();
                                nSortVideo++;
                            }
                        }
                    }

                    // ============================================================
                    // 8. تحديث البيانات الرئيسية
                    // ============================================================
                    var sport = model.ToEntity();
                    sport.ModifiedDate = DateTime.Now;
                    var existingSport = _UnitOfWork.SportRepository.GetById(model.Id);
                    if (existingSport != null)
                    {
                        sport.CreatedDate = existingSport.CreatedDate;
                    }

                    ViewBag.RegionId = model.RegionId;
                    model.Regions = _UnitOfWork.RegionRepository.Table.Where(r => r.Id == model.CityId).ToList();

                    _UnitOfWork.SportRepository.Update(sport);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث النشاط الرياضى بنجاح");
                    return RedirectToAction("Index", new { sportTypeId = model.SportTypeId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Update: {e.Message}");
                logFile.LogCustomInfo("Edit Sport - Exception Message ", e.Message);
                logFile.LogCustomInfo("Edit Sport - Stack Trace Message ", e.StackTrace);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Edit Sport - Inner Exception Message ", e.InnerException.ToString());
                ViewBag.SportTypeTitle = EnumExtensions.GetDisplayName((UserTypeEnumTitle)model.SportTypeId);
                return RedirectToAction("Edit", new { id = model.Id });
            }

            // ===== عند الرجوع بالخطأ =====
            if (model.SportTypeId > 0)
            {
                // + 1
                //var userType = (UserTypeEnum)(model.SportTypeId);
                //ViewBag.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                //var userType = model.SportTypeId.ToString();  // ✅ تحويل إلى string
                //ViewBag.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                var searchValue = model.SportTypeId.ToString();
                ViewBag.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                ViewBag.Users = new List<AppUser>();
            }
            ViewBag.SportTypeTitle = EnumExtensions.GetDisplayName((UserTypeEnumTitle)model.SportTypeId);
            ViewBag.RegionId = model.RegionId;
            ViewData["SelectedSportTypeId"] = model.SportTypeId;
            model = EditFillModel(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteSportImage(int id)
        {
            try
            {
                var image = _UnitOfWork.SportImageRepository.GetById(id);
                if (image != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", image.Url);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    _UnitOfWork.SportImageRepository.Delete(image);
                    _UnitOfWork.Save();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "الصورة غير موجودة" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteSportVideo(int id)
        {
            try
            {
                var video = _UnitOfWork.SportVideoRepository.GetById(id);
                if (video != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Videos", video.Url);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    _UnitOfWork.SportVideoRepository.Delete(video);
                    _UnitOfWork.Save();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "الفيديو غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var sport = _UnitOfWork.SportRepository.GetById(id);
                if (sport != null)
                {

                    var images = _UnitOfWork.SportImageRepository.Table.Where(i => i.SportId == id).ToList();
                    foreach (var img in images)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", img.Url);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        _UnitOfWork.SportImageRepository.Delete(img);
                    }


                    var videos = _UnitOfWork.SportVideoRepository.Table.Where(v => v.SportId == id).ToList();
                    foreach (var vid in videos)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Videos", vid.Url);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        _UnitOfWork.SportVideoRepository.Delete(vid);
                    }


                    var priceList = _UnitOfWork.SportPriceListRepository.Table.Where(p => p.SportId == id).ToList();
                    foreach (var price in priceList)
                        _UnitOfWork.SportPriceListRepository.Delete(price);

                    var features = _UnitOfWork.SportSportFeatureRepository.Table.Where(f => f.SportId == id).ToList();
                    foreach (var feature in features)
                        _UnitOfWork.SportSportFeatureRepository.Delete(feature);

                    var facilities = _UnitOfWork.SportGeneralFacilityRepository.Table.Where(f => f.SportId == id).ToList();
                    foreach (var facility in facilities)
                        _UnitOfWork.SportGeneralFacilityRepository.Delete(facility);

                    var services = _UnitOfWork.SportAdditionalServiceRepository.Table.Where(s => s.SportId == id).ToList();
                    foreach (var service in services)
                        _UnitOfWork.SportAdditionalServiceRepository.Delete(service);


                    _UnitOfWork.SportRepository.Delete(sport);
                    _UnitOfWork.Save();

                    SuccessNotification("تم حذف النشاط الرياضى بنجاح");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ErrorNotification($"Error while deleting: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        public IActionResult GetUsersBySportType(int sportTypeId)
        {
            if (sportTypeId <= 0)
                return Json(new List<object>());
            //For User ---&& u.IsActive == true
            var searchValue = sportTypeId.ToString();
            var users = _UnitOfWork.UserRepository.Table
                        .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                        .OrderBy(u => u.UserName)
                        .Select(u => new { u.Id, u.UserName, u.MobileNumber })
                        .ToList();
            return Json(users);
        }

        public IActionResult GetSportPropertyDetails(int sportTypeId, int sportId = 0)
        {
            SportModel model = new SportModel();

            if (sportId > 0)
            {
                var sport = _UnitOfWork.SportRepository.GetById(sportId);
                if (sport != null)
                    model = sport.ToModel();
            }

            model.SportTypeId = sportTypeId;

            switch (sportTypeId)
            {
                case 1: return PartialView("_SportProperty_Football", model);
                case 2: return PartialView("_SportProperty_Padel", model);
                case 3: return PartialView("_SportProperty_Tennis", model);
                case 4: return PartialView("_SportProperty_Basketball", model);
                case 5: return PartialView("_SportProperty_Volleyball", model);
                case 6: return PartialView("_SportProperty_Swimming", model);
                case 7: return PartialView("_SportProperty_Equestrian", model);
                case 8: return PartialView("_SportProperty_Shooting", model);
                case 9: return PartialView("_SportProperty_Pickleball", model);
                case 10: return PartialView("_SportProperty_TableTennis", model);
                case 11: return PartialView("_SportProperty_Squash", model);
                case 12: return PartialView("_SportProperty_Badminton", model);
                default: return PartialView("_SportProperty_Default", model);
            }
        }

        public SportModel NewFillModel(SportModel model, int? SportTypeId = null)
        {
            model.Owner = "من المالك";
            model.Countries = _UnitOfWork.CountryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _UnitOfWork.CityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.Users = _UnitOfWork.UserRepository.Table.ToList();
            model.Regions = _UnitOfWork.RegionRepository.Table.ToList();
            model.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            if (SportTypeId.HasValue && SportTypeId.Value > 0)
            {
                model.SportTypeId = SportTypeId.Value;
            }


            if (SportTypeId.HasValue && SportTypeId.Value > 0)
            {
                //var userType = (UserTypeEnum)(SportTypeId.Value);
                //model.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                var searchValue = SportTypeId.Value.ToString();
                model.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                model.Users = new List<AppUser>();
            }

            List<SportFeature> sportFeatures;
            if (SportTypeId.HasValue && SportTypeId.Value > 0)
            {

                sportFeatures = _UnitOfWork.SportFeatureRepository.Table
                    .Where(f => f.SportTypeId == SportTypeId.Value && f.IsActive == true)
                    .OrderBy(f => f.FeatureTextAr)
                    .ToList();
            }
            else
            {

                sportFeatures = new List<SportFeature>();

            }

            foreach (SportFeature feature in sportFeatures)
            {
                model.SportFeatures.Add(new SportFeatureDto()
                {
                    SportId = 0,
                    TypeId = feature.Id,
                    FeatureText = feature.FeatureTextAr,
                    FeatureTextEn = feature.FeatureTextEn,
                    IsCheck = false,
                    DescriptionAr = "",
                    DescriptionEn = ""
                });
            }


            List<GeneralFacility> generalFacilities = _UnitOfWork.GeneralFacilityRepository.Table.Where(f => f.IsActive == true).ToList();
            foreach (GeneralFacility facility in generalFacilities)
            {
                model.GeneralFacilities.Add(new GeneralFacilityDto()
                {
                    SportId = 0,
                    FacilityId = facility.Id,
                    FacilityText = facility.FacilityTextAr,
                    FacilityTextEn = facility.FacilityTextEn,
                    IsCheck = false
                });
            }


            List<AdditionalService> additionalServices = _UnitOfWork.AdditionalServiceRepository.Table.Where(s => s.IsActive == true).ToList();
            foreach (AdditionalService service in additionalServices)
            {
                model.AdditionalServices.Add(new AdditionalServiceDto()
                {
                    SportId = 0,
                    ServiceId = service.Id,
                    ServiceText = service.ServiceTextAr,
                    ServiceTextEn = service.ServiceTextEn,
                    IsCheck = false
                });
            }

            List<SafetyFeature> safetyFeatures = _UnitOfWork.SafetyFeatureRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.FeatureTextAr)
                .ToList();

            foreach (SafetyFeature feature in safetyFeatures)
            {
                model.SafetyFeatures.Add(new SafetyFeatureDto()
                {
                    SportId = 0,
                    TypeId = feature.Id,
                    FeatureText = feature.FeatureTextAr,
                    FeatureTextEn = feature.FeatureTextEn,
                    IsCheck = false,
                    DescriptionAr = "",
                    DescriptionEn = ""
                });
            }
            // ===== تفاصيل العقار الديناميكية =====
            if (SportTypeId.HasValue && SportTypeId.Value > 0)
            {
                var templates = _UnitOfWork.SportPropertyTemplateRepository.Table
                    .Where(t => t.SportTypeId == SportTypeId.Value && t.IsActive == true)
                    .OrderBy(t => t.SortOrder)
                    .ToList();

                foreach (var template in templates)
                {
                    var options = new List<SportPropertyOptionDto>();
                    if (template.PropertyType == PropertyTypeEnum.Dropdown || template.PropertyType == PropertyTypeEnum.RadioButton)
                    {
                        options = _UnitOfWork.SportPropertyOptionRepository.Table
                            .Where(o => o.PropertyTemplateId == template.Id && o.IsActive == true)
                            .OrderBy(o => o.SortOrder)
                            .Select(o => new SportPropertyOptionDto
                            {
                                Id = o.Id,
                                OptionValue = o.OptionValue,
                                OptionTextAr = o.OptionTextAr,
                                OptionTextEn = o.OptionTextEn,
                                SortOrder = o.SortOrder
                            })
                            .ToList();
                    }

                    model.PropertyTemplates.Add(new SportPropertyTemplateDto
                    {
                        Id = template.Id,
                        SportTypeId = template.SportTypeId,
                        PropertyKey = template.PropertyKey,
                        PropertyLabelAr = template.PropertyLabelAr,
                        PropertyLabelEn = template.PropertyLabelEn,
                        PropertyType = (int)template.PropertyType,
                        IsRequired = template.IsRequired,
                        SortOrder = template.SortOrder,
                        Options = options
                    });
                }
            }
            if (model.PriceList == null)
            {
                model.PriceList = new List<SportPriceList>();
            }

            return model;
        }

        public SportModel EditFillModel(SportModel model)
        {
            model.Countries = _UnitOfWork.CountryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _UnitOfWork.CityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.Users = _UnitOfWork.UserRepository.Table.ToList();
            //model.Regions = _UnitOfWork.RegionRepository.Table.Where(r => r.Id == model.CityId).ToList();
            model.SportTypes = _UnitOfWork.SportTypeRepository.Table.Where(s => s.IsActive == true).ToList();
            //model.Regions = _UnitOfWork.RegionRepository.Table
            //                .Where(r => r.CityId == model.CityId)
            //                .OrderBy(r => r.DescAr)
            //                .ToList();

            if (model.SportTypeId > 0)
            {
                //var userType = (UserTypeEnum)(model.SportTypeId);
                //model.Users = _UnitOfWork.UserRepository.Table
                //    .Where(u => u.UserType == userType)
                //    .OrderBy(u => u.UserName)
                //    .ToList();
                var searchValue = model.SportTypeId.ToString();
                model.Users = _UnitOfWork.UserRepository.Table
                    .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
                    .OrderBy(u => u.UserName)
                    .ToList();
            }
            else
            {
                model.Users = new List<AppUser>();
            }

            var allRegions = _UnitOfWork.RegionRepository.Table
                .Where(r => r.CityId == model.CityId)
                .OrderBy(r => r.DescAr)
                .ToList();

            var currentRegion = _UnitOfWork.RegionRepository.Table
                .FirstOrDefault(r => r.Id == model.RegionId);

            if (currentRegion != null && !allRegions.Any(r => r.Id == currentRegion.Id))
            {
                allRegions.Insert(0, currentRegion);
            }

            model.Regions = allRegions;
            model.SportImages = _UnitOfWork.SportImageRepository.Table.Where(i => i.SportId == model.Id && i.Active == true).OrderBy(i => i.Sort).ToList();
            model.SportVideos = _UnitOfWork.SportVideoRepository.Table.Where(v => v.SportId == model.Id && v.Active == true).OrderBy(v => v.Sort).ToList();


            var sportFeatures = _UnitOfWork.SportFeatureRepository.Table
                .Where(f => f.SportTypeId == model.SportTypeId && f.IsActive == true)
                .OrderBy(f => f.FeatureTextAr)
                .ToList();

            var sportSportFeatures = _UnitOfWork.SportSportFeatureRepository.Table
                .Where(x => x.SportId == model.Id)
                .ToList();

            foreach (SportFeature feature in sportFeatures)
            {
                var existing = sportSportFeatures.FirstOrDefault(f => f.SportFeatureId == feature.Id);
                if (existing != null)
                {
                    model.SportFeatures.Add(new SportFeatureDto()
                    {
                        Id = existing.Id,
                        SportId = model.Id,
                        TypeId = feature.Id,
                        FeatureText = feature.FeatureTextAr,
                        FeatureTextEn = feature.FeatureTextEn,
                        IsCheck = true,
                        DescriptionAr = existing.DescriptionAr ?? "",
                        DescriptionEn = existing.DescriptionEn ?? ""
                    });
                }
                else
                {
                    model.SportFeatures.Add(new SportFeatureDto()
                    {
                        SportId = model.Id,
                        TypeId = feature.Id,
                        FeatureText = feature.FeatureTextAr,
                        FeatureTextEn = feature.FeatureTextEn,
                        IsCheck = false,
                        DescriptionAr = "",
                        DescriptionEn = ""
                    });
                }
            }

            // ===== المرافق العامة (General Facilities) - زى ما هى =====
            List<SportGeneralFacility> sportGeneralFacilities = _UnitOfWork.SportGeneralFacilityRepository.Table.Where(x => x.SportId == model.Id).ToList();
            List<GeneralFacility> generalFacilities = _UnitOfWork.GeneralFacilityRepository.Table.Where(f => f.IsActive == true).ToList();

            foreach (GeneralFacility facility in generalFacilities)
            {
                var existing = sportGeneralFacilities.FirstOrDefault(f => f.GeneralFacilityId == facility.Id);
                model.GeneralFacilities.Add(new GeneralFacilityDto()
                {
                    SportId = model.Id,
                    FacilityId = facility.Id,
                    FacilityText = facility.FacilityTextAr,
                    FacilityTextEn = facility.FacilityTextEn,
                    IsCheck = existing != null
                });
            }


            List<SportAdditionalService> sportAdditionalServices = _UnitOfWork.SportAdditionalServiceRepository.Table.Where(x => x.SportId == model.Id).ToList();
            List<AdditionalService> additionalServices = _UnitOfWork.AdditionalServiceRepository.Table.Where(s => s.IsActive == true).ToList();

            foreach (AdditionalService service in additionalServices)
            {
                var existing = sportAdditionalServices.FirstOrDefault(s => s.AdditionalServiceId == service.Id);
                model.AdditionalServices.Add(new AdditionalServiceDto()
                {
                    SportId = model.Id,
                    ServiceId = service.Id,
                    ServiceText = service.ServiceTextAr,
                    ServiceTextEn = service.ServiceTextEn,
                    IsCheck = existing != null
                });
            }

            List<SportSafetyFeature> sportSafetyFeatures = _UnitOfWork.SportSafetyFeatureRepository.Table
                .Where(x => x.SportId == model.Id)
                .ToList();

            List<SafetyFeature> safetyFeatures = _UnitOfWork.SafetyFeatureRepository.Table
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.FeatureTextAr)
                .ToList();

            foreach (SafetyFeature feature in safetyFeatures)
            {
                var existing = sportSafetyFeatures.FirstOrDefault(f => f.SafetyFeatureId == feature.Id);
                if (existing != null)
                {
                    model.SafetyFeatures.Add(new SafetyFeatureDto()
                    {
                        Id = existing.Id,
                        SportId = model.Id,
                        TypeId = feature.Id,
                        FeatureText = feature.FeatureTextAr,
                        FeatureTextEn = feature.FeatureTextEn,
                        IsCheck = true,
                        DescriptionAr = existing.DescriptionAr ?? "",
                        DescriptionEn = existing.DescriptionEn ?? ""
                    });
                }
                else
                {
                    model.SafetyFeatures.Add(new SafetyFeatureDto()
                    {
                        SportId = model.Id,
                        TypeId = feature.Id,
                        FeatureText = feature.FeatureTextAr,
                        FeatureTextEn = feature.FeatureTextEn,
                        IsCheck = false,
                        DescriptionAr = "",
                        DescriptionEn = ""
                    });
                }
            }
            // ===== تفاصيل العقار الديناميكية =====
            if (model.SportTypeId > 0)
            {
                var templates = _UnitOfWork.SportPropertyTemplateRepository.Table
                    .Where(t => t.SportTypeId == model.SportTypeId && t.IsActive == true)
                    .OrderBy(t => t.SortOrder)
                    .ToList();

                var savedValues = _UnitOfWork.SportPropertyValueRepository.Table
                    .Where(v => v.SportId == model.Id)
                    .ToDictionary(v => v.PropertyTemplateId, v => v);

                foreach (var template in templates)
                {
                    var value = savedValues.ContainsKey(template.Id) ? savedValues[template.Id] : null;

                    var options = new List<SportPropertyOptionDto>();
                    if (template.PropertyType == PropertyTypeEnum.Dropdown || template.PropertyType == PropertyTypeEnum.RadioButton)
                    {
                        options = _UnitOfWork.SportPropertyOptionRepository.Table
                            .Where(o => o.PropertyTemplateId == template.Id && o.IsActive == true)
                            .OrderBy(o => o.SortOrder)
                            .Select(o => new SportPropertyOptionDto
                            {
                                Id = o.Id,
                                OptionValue = o.OptionValue,
                                OptionTextAr = o.OptionTextAr,
                                OptionTextEn = o.OptionTextEn,
                                SortOrder = o.SortOrder
                            })
                            .ToList();
                    }

                    model.PropertyTemplates.Add(new SportPropertyTemplateDto
                    {
                        Id = template.Id,
                        SportTypeId = template.SportTypeId,
                        PropertyKey = template.PropertyKey,
                        PropertyLabelAr = template.PropertyLabelAr,
                        PropertyLabelEn = template.PropertyLabelEn,
                        PropertyType = (int)template.PropertyType,
                        IsRequired = template.IsRequired,
                        SortOrder = template.SortOrder,
                        Options = options
                    });

                    model.PropertyValues.Add(new SportPropertyValueDto
                    {
                        Id = value?.Id ?? 0,
                        SportId = model.Id,
                        PropertyTemplateId = template.Id,
                        PropertyKey = template.PropertyKey,
                        ValueText = value?.ValueText,
                        ValueBool = value?.ValueBool,
                        ValueOptionId = value?.ValueOptionId,
                        PropertyType = (int)template.PropertyType,
                        PropertyLabelAr = template.PropertyLabelAr,
                        IsRequired = template.IsRequired
                    });
                }
            }
            // Price List
            model.PriceList = _UnitOfWork.SportPriceListRepository.Table.Where(f => f.SportId == model.Id).OrderBy(f => f.Person).ThenBy(f => f.Day).ToList();

            return model;
        }

        public IActionResult GetRegionsByCityId(int cityId)
        {
            var regions = _UnitOfWork.RegionRepository.Table
                .Where(r => r.CityId == cityId)
                .OrderBy(r => r.DescAr)
                .Select(r => new { r.Id, r.DescAr, r.DescEn })
                .ToList();

            return Json(regions);
        }


        #endregion



        #region :: Sport Price List


        public IActionResult SportPriceList(int sportId,int? SportTypeId)
        {
            if (sportId == 0)
            {
                //TempData["ErrorMessage"] = "الرجاء حفظ النشاط الرياضي أولاً";
                //return RedirectToAction("Create", "Sports");
                //return RedirectToAction($"Create", new { SportTypeId = 1 }, "Sports");
                return RedirectToAction("Create", "Sports", new { SportTypeId = SportTypeId });
            }

            var sport = _UnitOfWork.SportRepository.Table.FirstOrDefault(s => s.Id == sportId);
            if (sport == null)
                return RedirectToAction("Index", new { sportTypeId = SportTypeId });


            SportPriceModel sportPriceModel = new SportPriceModel();

            sportPriceModel.SportId = sportId;

            sportPriceModel.PriceList = new List<SportPriceList>()
            {
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 1 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 2 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 3 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 4 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 5 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 6 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 },
                new SportPriceList(){ Id = 0 , SportId = sportId , Day = 7 , HourlyPrice = 0 , PeakHourlyPrice = 0, PeakStartTime = null, PeakEndTime = null, OfferHourlyPrice = 0, MinBookingHours = 1 }
            };

            ViewBag.activePage = "أسعار النشاط الرياضي";
            ViewBag.sportName = sport.NameAr;
            return View(sportPriceModel);
        }

        [HttpPost]
        public IActionResult SportPriceList(SportPriceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.Person <= 0)
                    {
                        ErrorNotification("يجب إدخال عدد الأشخاص");
                        return View(model);
                    }


                    var personExists = _UnitOfWork.SportPriceListRepository.Table
                        .Any(p => p.Person == model.Person && p.SportId == model.SportId);

                    if (personExists)
                    {
                        ErrorNotification("عدد الأشخاص موجود مسبقاً");
                        return View(model);
                    }


                    var oldPrices = _UnitOfWork.SportPriceListRepository.Table
                        .Where(p => p.SportId == model.SportId)
                        .ToList();

                    foreach (var old in oldPrices)
                    {
                        _UnitOfWork.SportPriceListRepository.Delete(old);
                    }


                    foreach (var item in model.PriceList)
                    {
                        // نتجاهل الأسعار الفارغة (سعر الساعة = 0)

                        item.SportId = model.SportId;
                        item.Person = model.Person;
                        item.MinBookingHours = 1;
                        _UnitOfWork.SportPriceListRepository.Insert(item);

                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم حفظ الأسعار بنجاح");


                    return RedirectToAction("Edit", new { id = model.SportId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
            }

            ViewBag.activePage = "أسعار النشاط الرياضي";
            var sport = _UnitOfWork.SportRepository.GetById(model.SportId);
            ViewBag.sportName = sport?.NameAr ?? "";
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteSportPriceList(int sportId, int person)
        {
            try
            {
                if (sportId <= 0)
                {
                    ErrorNotification("يجب إدخال رقم النشاط الرياضي");
                    return RedirectToAction("Index");
                }

                if (person <= 0)
                {
                    ErrorNotification("يجب إدخال عدد الأشخاص");
                    return RedirectToAction("Index");
                }

                // جلب الأسعار المراد حذفها
                var prices = _UnitOfWork.SportPriceListRepository.Table
                    .Where(p => p.SportId == sportId && p.Person == person)
                    .ToList();

                if (!prices.Any())
                {
                    ErrorNotification("لا توجد أسعار لهذا العدد من الأشخاص");
                    return RedirectToAction("Edit", new { id = sportId });
                }

                foreach (var price in prices)
                {
                    _UnitOfWork.SportPriceListRepository.Delete(price);
                }

                _UnitOfWork.Save();
                SuccessNotification("تم حذف الأسعار بنجاح");
                return RedirectToAction("Edit", new { id = sportId });
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
                return RedirectToAction("Edit", new { id = sportId });
            }
        }


        #endregion




    }
}
