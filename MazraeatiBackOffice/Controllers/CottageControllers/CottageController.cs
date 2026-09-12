using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Configuration.Enums;
using MazraeatiBackOffice.Core.CottageCore;
using MazraeatiBackOffice.Core.FarmCore;
using MazraeatiBackOffice.Core.SportCore;
using MazraeatiBackOffice.Core.UserManagementCore;
using MazraeatiBackOffice.Dto.CottageDtos;
using MazraeatiBackOffice.Dto.SportDtos;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models.CottageModel;
using MazraeatiBackOffice.Models.FarmModel;
using MazraeatiBackOffice.Models.SportModels;
using MazraeatiBackOffice.SportCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.CottageControllers
{
    public class CottageController : BaseController
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IRepository<FarmerPriceList> _FarmerPriceList;
        
        public CottageController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        #region Cottage Actions ....

        // GET: Cottages/Index
        public IActionResult Index(string search, int? cityId)
        {
            var countries = _UnitOfWork.CountryRepository.Table.Where(c => c.Active == true).ToList();
            var cities = _UnitOfWork.CityRepository.Table.Where(c => c.Active == true).ToList();
            var regions = _UnitOfWork.RegionRepository.Table.ToList();
            var users = _UnitOfWork.UserRepository.Table.ToList();

            var query = _UnitOfWork.CottageRepository.Table
                .Include(s => s.Country)
                .Include(s => s.City)
                .Include(s => s.Region)
                .Where(s => s.IsActive == true && s.IsDeleted == false);

            // فلتر البحث
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.NameAr.Contains(search) ||
                    s.NameEn.Contains(search) ||
                    s.MobileNumber.Contains(search) ||
                    s.Number.ToString().Contains(search) ||
                    s.SerialCottageKey.Contains(search));
            }

            // فلتر المدينة
            if (cityId.HasValue && cityId.Value > 0)
            {
                query = query.Where(s => s.CityId == cityId.Value);
            }

            var cottages = query.OrderByDescending(s => s.Id).ToList();
            var model = cottages.Select(s => s.ToModel(countries, cities, regions, users)).ToList();

            // الإحصائيات
            ViewBag.TotalCottages = model.Count();
            ViewBag.ApprovedCottages = model.Count(m => m.IsApprove);
            ViewBag.VIPCottages = model.Count(m => m.IsVIP);
            ViewBag.BlockedCottages = model.Count(m => m.IsBlocked);

            ViewBag.activePage = "الأكواخ";
            ViewBag.search = search;
            ViewBag.CityBy = cityId;
            ViewBag.cities = cities;
            ViewBag.DefaultDate = DateTime.Now;

            return View(model);
        }


        [HttpPost]
        public IActionResult Index(string search, int cityId)
        {
            return RedirectToAction("Index", new
            {
                search = search,
                cityId = cityId == 0 ? (int?)null : cityId
            });
        }


        public IActionResult Create()
        {
            ViewBag.activePage = "الأكواخ";
            ViewBag.Users = _UnitOfWork.UserRepository.Table
                .Where(u => u.IsActive == true)
                .OrderBy(u => u.UserName)
                .ToList();
            var model = NewFillModel(new CottageModel());

            model.GeneralFacilities ??= new List<CottageGeneralFacilityDto>();
            model.PropertyTemplates ??= new List<CottagePropertyTemplateDto>();
            model.PriceList ??= new List<CottagePriceList>();
            model.PropertyValues ??= new List<CottagePropertyValueDto>();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CottageModel model)
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


                //if (model.UserId <= 0)
                //    ModelState.AddModelError("UserId", "برجاء اختيار المالك من القائمة");

                if (ModelState.IsValid)
                {
                    // get max number (مثل Farmer)
                    long nMaxNumber = _UnitOfWork.CottageRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();
                    int nId = _UnitOfWork.CottageRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new sport (مثل Farmer)
                    var cottage = model.ToEntity();
                    cottage.Number = nMaxNumber + 1;
                    cottage.IssueDate = DateTime.Now;
                    cottage.ExpiryDate = DateTime.Now.AddMonths(3);
                    cottage.CreatedDate = DateTime.Now;
                    cottage.IsActive = true;
                    cottage.IsDeleted = false;
                    if (cottage.UserId > 0)
                    {
                        var User = _UnitOfWork.UserRepository.Table.FirstOrDefault(U => U.Id == cottage.UserId);
                        cottage.MobileOwnerAppUser = User.MobilePhone;
                    }
                    _UnitOfWork.CottageRepository.InsertEntity(cottage);
                    _UnitOfWork.Save();

                    int nCottageId = _UnitOfWork.CottageRepository.Table.FirstOrDefault(f => f.Number == nMaxNumber + 1).Id;

                    cottage.SerialCottageKey = $"{nCottageId}";
                    cottage.Id = nCottageId;
                    _UnitOfWork.CottageRepository.Update(cottage);
                    _UnitOfWork.Save();


                    // =====  (General Facilities) =====
                    foreach (CottageGeneralFacilityDto facility in model.GeneralFacilities.Where(e => e.IsCheck == true))
                    {
                        _UnitOfWork.CottageCottageGeneralFacilityRepository.Insert(new CottageCottageGeneralFacility
                        {
                            CottageId = nCottageId,
                            GeneralFacilityId = facility.FacilityId,
                            IsActive = true
                        });
                    }


                    if (model.PriceList != null && model.PriceList.Any())
                    {
                        // add price list
                        foreach (CottagePriceList priceList in model.PriceList)
                        {
                            CottagePriceList cottagePriceList = new CottagePriceList();
                            cottagePriceList.CottageId = nCottageId;
                            cottagePriceList.Day = priceList.Day;
                            cottagePriceList.MorningPrice = priceList.MorningPrice;
                            cottagePriceList.EveningPrice = priceList.EveningPrice;
                            cottagePriceList.FullDayPrice = priceList.FullDayPrice;
                            cottagePriceList.OfferPrice = priceList.OfferPrice;
                            cottagePriceList.OfferEveningPrice = priceList.OfferEveningPrice;
                            cottagePriceList.OfferFullDayPrice = priceList.OfferFullDayPrice;
                            cottagePriceList.MorningPeriodText = priceList.MorningPeriodText;
                            cottagePriceList.EveningPeriodText = priceList.EveningPeriodText;
                            cottagePriceList.FullDayPeriodText = string.IsNullOrEmpty(priceList.FullDayPeriodText) ? "" : priceList.FullDayPeriodText;

                            _UnitOfWork.CottagePriceListRepository.Insert(cottagePriceList);

                        }
                    }


                    // ===== حفظ تفاصيل العقار الديناميكية =====
                    if (model.PropertyValues != null && model.PropertyValues.Any())
                    {
                        foreach (var valueDto in model.PropertyValues.Where(v => v.PropertyTemplateId > 0))
                        {
                            var value = new CottagePropertyValue
                            {
                                CottageId = nCottageId,
                                PropertyTemplateId = valueDto.PropertyTemplateId,
                                ValueText = valueDto.ValueText,
                                ValueBool = valueDto.ValueBool,
                                ValueOptionId = valueDto.ValueOptionId
                            };
                            _UnitOfWork.CottagePropertyValueRepository.Insert(value);
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
                                _UnitOfWork.CottageImageRepository.Insert(new CottageImage
                                {
                                    CottageId = nCottageId,
                                    Url = "cottages/" + GenericFunction.UploadedFile(file, _webHostEnvironment, "cottages"),
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
                                _UnitOfWork.CottageVideoRepository.Insert(new CottageVideo
                                {
                                    CottageId = nCottageId,
                                    Url = "cottages/" + GenericFunction.UploadedVideo(file, _webHostEnvironment, "cottages"),
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
                    SuccessNotification("تم اضافة  السجل بنجاح");
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Saving: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Create Cottage - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create Cottage - Stack Trace Message ", e.StackTrace);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Create Cottage - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Create");
            }


            ViewBag.Users = _UnitOfWork.UserRepository.Table
                .Where(u => u.IsActive == true)
                .OrderBy(u => u.UserName)
                .ToList();



            ViewBag.RegionId = model.RegionId;
            model = NewFillModel(model);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var cottage = _UnitOfWork.CottageRepository
                .Table
                .Include(s => s.Country)
                .Include(s => s.City)
                .Include(s => s.Region)
                .Include(s => s.CottageImages)
                .Include(s => s.CottageVideos)
                .Include(s => s.PriceList)
                .FirstOrDefault(s => s.Id == id);

            if (cottage == null)
                return RedirectToAction("Index");
            ViewBag.Users = _UnitOfWork.UserRepository.Table.ToList();
            ViewBag.RegionId = cottage.RegionId;

            ViewBag.activePage = "الأكواخ";
            // ===== إضافة هذا السطر =====
            //return View(EditFillModel(sport.ToModel()));
            var model = EditFillModel(cottage.ToModel());
            ViewBag.Users = _UnitOfWork.UserRepository.Table
                .Where(u => u.IsActive == true)
                .OrderBy(u => u.UserName)
                .ToList();
            // ===== تعيين ViewBag.Users =====
            return View(model);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public IActionResult Edit(CottageModel model)
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

                    if (_UnitOfWork.CottageImageRepository.Table.Any(f => f.CottageId == model.Id))
                        maxOrderIdImage = _UnitOfWork.CottageImageRepository.Table.Where(f => f.CottageId == model.Id).Max(x => x.Sort);

                    if (_UnitOfWork.CottageVideoRepository.Table.Any(f => f.CottageId == model.Id))
                        maxOrderIdVideo = _UnitOfWork.CottageVideoRepository.Table.Where(f => f.CottageId == model.Id).Max(x => x.Sort);

                    // ============================================================
                    // 1. تحديث جدول الأسعار
                    // ============================================================
                    if (model.PriceList != null)
                    {
                        foreach (CottagePriceList priceList in model.PriceList)
                        {
                            _UnitOfWork.CottagePriceListRepository.Update(priceList);
                        }
                    }



                    // ============================================================
                    // 1. تحديث المرافق العامة (General Facilities)
                    // ============================================================
                    if (model.GeneralFacilities != null)
                    {
                        // جلب المرافق الموجودة
                        var existingFacilities = _UnitOfWork.CottageCottageGeneralFacilityRepository.Table
                            .Where(f => f.CottageId == model.Id)
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
                            _UnitOfWork.CottageCottageGeneralFacilityRepository.Delete(item);
                        }

                        // إضافة المرافق المختارة
                        foreach (var facility in model.GeneralFacilities.Where(f => f.IsCheck == true))
                        {
                            var exists = existingFacilities.Any(f => f.GeneralFacilityId == facility.FacilityId);
                            if (!exists)
                            {
                                _UnitOfWork.CottageCottageGeneralFacilityRepository.Insert(new CottageCottageGeneralFacility
                                {
                                    CottageId = model.Id,
                                    GeneralFacilityId = facility.FacilityId,
                                    IsActive = true
                                });
                            }
                        }
                    }


                    // ===== تحديث تفاصيل العقار الديناميكية =====
                    var oldValues = _UnitOfWork.CottagePropertyValueRepository.Table
                        .Where(v => v.CottageId == model.Id)
                        .ToList();

                    foreach (var old in oldValues)
                    {
                        _UnitOfWork.CottagePropertyValueRepository.Delete(old);
                        _UnitOfWork.Save();
                    }

                    if (model.PropertyValues != null && model.PropertyValues.Any())
                    {
                        foreach (var valueDto in model.PropertyValues.Where(v => v.PropertyTemplateId > 0))
                        {
                            var value = new CottagePropertyValue
                            {
                                CottageId = model.Id,
                                PropertyTemplateId = valueDto.PropertyTemplateId,
                                ValueText = valueDto.ValueText,
                                ValueBool = valueDto.ValueBool,
                                ValueOptionId = valueDto.ValueOptionId
                            };
                            _UnitOfWork.CottagePropertyValueRepository.Insert(value);
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
                                _UnitOfWork.CottageImageRepository.Insert(new CottageImage
                                {
                                    CottageId = model.Id,
                                    Url = "cottages/" + GenericFunction.UploadedFile(file, _webHostEnvironment, "cottages"),
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
                                _UnitOfWork.CottageVideoRepository.Insert(new CottageVideo
                                {
                                    CottageId = model.Id,
                                    Url = "cottages/" + GenericFunction.UploadedVideo(file, _webHostEnvironment, "cottages"),
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
                    var cottage = model.ToEntity();
                    cottage.ModifiedDate = DateTime.Now;
                    var existingCottage = _UnitOfWork.CottageRepository.GetById(model.Id);
                    if (existingCottage != null)
                    {
                        cottage.CreatedDate = existingCottage.CreatedDate;
                    }

                    ViewBag.RegionId = model.RegionId;
                    model.Regions = _UnitOfWork.RegionRepository.Table.Where(r => r.Id == model.CityId).ToList();

                    _UnitOfWork.CottageRepository.Update(cottage);
                    _UnitOfWork.Save();

                    SuccessNotification("تم تحديث  السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"Error while Update: {e.Message}");
                logFile.LogCustomInfo("Edit Cottage - Exception Message ", e.Message);
                logFile.LogCustomInfo("Edit Cottage - Stack Trace Message ", e.StackTrace);
                if (e.InnerException != null)
                    logFile.LogCustomInfo("Edit Cottage - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Edit", new { id = model.Id });
            }

            // ===== عند الرجوع بالخطأ =====

            ViewBag.Users = _UnitOfWork.UserRepository.Table
                .Where(u => u.IsActive == true)
                .OrderBy(u => u.UserName)
                .ToList();

            ViewBag.RegionId = model.RegionId;
            model = EditFillModel(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteCottageImage(int id)
        {
            try
            {
                var image = _UnitOfWork.CottageImageRepository.GetById(id);
                if (image != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", image.Url);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    _UnitOfWork.CottageImageRepository.Delete(image);
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
        public IActionResult DeleteCottageVideo(int id)
        {
            try
            {
                var video = _UnitOfWork.CottageVideoRepository.GetById(id);
                if (video != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Videos", video.Url);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    _UnitOfWork.CottageVideoRepository.Delete(video);
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
                var sport = _UnitOfWork.CottageRepository.GetById(id);
                if (sport != null)
                {

                    var images = _UnitOfWork.CottageImageRepository.Table.Where(i => i.CottageId == id).ToList();
                    foreach (var img in images)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", img.Url);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        _UnitOfWork.CottageImageRepository.Delete(img);
                    }


                    var videos = _UnitOfWork.CottageVideoRepository.Table.Where(v => v.CottageId == id).ToList();
                    foreach (var vid in videos)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Videos", vid.Url);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        _UnitOfWork.CottageVideoRepository.Delete(vid);
                    }


                    var priceList = _UnitOfWork.CottagePriceListRepository.Table.Where(p => p.CottageId == id).ToList();
                    foreach (var price in priceList)
                        _UnitOfWork.CottagePriceListRepository.Delete(price);


                    var facilities = _UnitOfWork.CottageCottageGeneralFacilityRepository.Table.Where(f => f.CottageId == id).ToList();
                    foreach (var facility in facilities)
                        _UnitOfWork.CottageCottageGeneralFacilityRepository.Delete(facility);



                    _UnitOfWork.CottageRepository.Delete(sport);
                    _UnitOfWork.Save();

                    SuccessNotification("تم حذف السجل بنجاح");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ErrorNotification($"Error while deleting: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        //public IActionResult GetUsersBySportType(int sportTypeId)
        //{
        //    if (sportTypeId <= 0)
        //        return Json(new List<object>());
        //    //For User ---&& u.IsActive == true
        //    var searchValue = sportTypeId.ToString();
        //    var users = _UnitOfWork.UserRepository.Table
        //                .Where(u => u.UserType != null && u.UserType.Contains(searchValue) && u.IsActive == true)
        //                .OrderBy(u => u.UserName)
        //                .Select(u => new { u.Id, u.UserName, u.MobileNumber })
        //                .ToList();
        //    return Json(users);
        //}


        public CottageModel NewFillModel(CottageModel model)
        {
            model.Owner = "من المالك";
            model.Countries = _UnitOfWork.CountryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _UnitOfWork.CityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.Users = _UnitOfWork.UserRepository.Table.ToList();
            model.Regions = _UnitOfWork.RegionRepository.Table.ToList();


            //List<CottageGeneralFacility> generalFacilities = _UnitOfWork.CottageGeneralFacilityRepository.Table.Where(f => f.IsActive == true).ToList();
            //foreach (CottageGeneralFacility facility in generalFacilities)
            //{
            //    model.GeneralFacilities.Add(new CottageGeneralFacilityDto()
            //    {
            //        CottageId = 0,
            //        FacilityId = facility.Id,
            //        FacilityText = facility.FacilityTextAr,
            //        FacilityTextEn = facility.FacilityTextEn,
            //        IsCheck = false
            //    });
            //}

            model.GeneralFacilities.Clear();
            var generalFacilities = _UnitOfWork.CottageGeneralFacilityRepository.Table
                                    .Where(f => f.IsActive == true)
                                    .ToList();

            foreach (var facility in generalFacilities)
            {
                model.GeneralFacilities.Add(new CottageGeneralFacilityDto()
                {
                    CottageId = 0,
                    FacilityId = facility.Id,
                    FacilityText = facility.FacilityTextAr,
                    FacilityTextEn = facility.FacilityTextEn,
                    IsCheck = false
                });
            }


            // ===== تفاصيل العقار الديناميكية =====
            var templates = _UnitOfWork.CottagePropertyTemplateRepository.Table
                .Where(t => t.IsActive == true)
                .OrderBy(t => t.SortOrder)
                .ToList();

            foreach (var template in templates)
            {
                var options = new List<CottagePropertyOptionDto>();
                if (template.PropertyType == PropertyTypeEnum.Dropdown || template.PropertyType == PropertyTypeEnum.RadioButton)
                {
                    options = _UnitOfWork.CottagePropertyOptionRepository.Table
                        .Where(o => o.PropertyTemplateId == template.Id && o.IsActive == true)
                        .OrderBy(o => o.SortOrder)
                        .Select(o => new CottagePropertyOptionDto
                        {
                            Id = o.Id,
                            OptionValue = o.OptionValue,
                            OptionTextAr = o.OptionTextAr,
                            OptionTextEn = o.OptionTextEn,
                            SortOrder = o.SortOrder
                        })
                        .ToList();
                }

                model.PropertyTemplates.Add(new CottagePropertyTemplateDto
                {
                    Id = template.Id,
                    PropertyKey = template.PropertyKey,
                    PropertyLabelAr = template.PropertyLabelAr,
                    PropertyLabelEn = template.PropertyLabelEn,
                    PropertyType = (int)template.PropertyType,
                    IsRequired = template.IsRequired,
                    SortOrder = template.SortOrder,
                    Options = options
                });
            }


            if (model.PriceList == null)
            {
                model.PriceList = new List<CottagePriceList>();
            }

            return model;
        }


        public CottageModel EditFillModel(CottageModel model)
        {
            model.Countries = _UnitOfWork.CountryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _UnitOfWork.CityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.Users = _UnitOfWork.UserRepository.Table.ToList();
            //model.Regions = _UnitOfWork.RegionRepository.Table.Where(r => r.Id == model.CityId).ToList();
            //model.Regions = _UnitOfWork.RegionRepository.Table
            //                .Where(r => r.CityId == model.CityId)
            //                .OrderBy(r => r.DescAr)
            //                .ToList();

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
            model.CottageImages = _UnitOfWork.CottageImageRepository.Table.Where(i => i.CottageId == model.Id && i.Active == true).OrderBy(i => i.Sort).ToList();
            model.CottageVideos = _UnitOfWork.CottageVideoRepository.Table.Where(v => v.CottageId == model.Id && v.Active == true).OrderBy(v => v.Sort).ToList();


            model.GeneralFacilities.Clear();
            // ===== المرافق العامة (CottageGeneral Facilities) - زى ما هى =====
            List<CottageCottageGeneralFacility> cottageGeneralFacilities = _UnitOfWork.CottageCottageGeneralFacilityRepository.Table.Where(x => x.CottageId == model.Id).ToList();
            List<CottageGeneralFacility> generalFacilities = _UnitOfWork.CottageGeneralFacilityRepository.Table.Where(f => f.IsActive == true).ToList();

            foreach (CottageGeneralFacility facility in generalFacilities)
            {
                var existing = cottageGeneralFacilities.FirstOrDefault(f => f.GeneralFacilityId == facility.Id);
                model.GeneralFacilities.Add(new CottageGeneralFacilityDto()
                {
                    CottageId = model.Id,
                    FacilityId = facility.Id,
                    FacilityText = facility.FacilityTextAr,
                    FacilityTextEn = facility.FacilityTextEn,
                    IsCheck = existing != null
                });
            }


            // ===== تفاصيل العقار الديناميكية =====

            var templates = _UnitOfWork.CottagePropertyTemplateRepository.Table
                .Where(t => t.IsActive == true)
                .OrderBy(t => t.SortOrder)
                .ToList();

            var savedValues = _UnitOfWork.CottagePropertyValueRepository.Table
                .Where(v => v.CottageId == model.Id)
                .ToDictionary(v => v.PropertyTemplateId, v => v);

            foreach (var template in templates)
            {
                var value = savedValues.ContainsKey(template.Id) ? savedValues[template.Id] : null;

                var options = new List<CottagePropertyOptionDto>();
                if (template.PropertyType == PropertyTypeEnum.Dropdown || template.PropertyType == PropertyTypeEnum.RadioButton)
                {
                    options = _UnitOfWork.CottagePropertyOptionRepository.Table
                        .Where(o => o.PropertyTemplateId == template.Id && o.IsActive == true)
                        .OrderBy(o => o.SortOrder)
                        .Select(o => new CottagePropertyOptionDto
                        {
                            Id = o.Id,
                            OptionValue = o.OptionValue,
                            OptionTextAr = o.OptionTextAr,
                            OptionTextEn = o.OptionTextEn,
                            SortOrder = o.SortOrder
                        })
                        .ToList();
                }

                model.PropertyTemplates.Add(new CottagePropertyTemplateDto
                {
                    Id = template.Id,
                    PropertyKey = template.PropertyKey,
                    PropertyLabelAr = template.PropertyLabelAr,
                    PropertyLabelEn = template.PropertyLabelEn,
                    PropertyType = (int)template.PropertyType,
                    IsRequired = template.IsRequired,
                    SortOrder = template.SortOrder,
                    Options = options
                });

                model.PropertyValues.Add(new CottagePropertyValueDto
                {
                    Id = value?.Id ?? 0,
                    CottageId = model.Id,
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

            // Price List
            model.PriceList = _UnitOfWork.CottagePriceListRepository.Table.Where(f => f.CottageId == model.Id).OrderBy(f => f.Person).ThenBy(f => f.Day).ToList();

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


        #region :: Cottage Price List

        public IActionResult CottagePriceList(int cottageId)
        {
            if (cottageId == 0)
            {
                ViewBag.activePage = "الأكواخ";
                ErrorNotification("الرجاء حفظ الكوخ أولا");
                //return View("Create", NewFillModel(new FarmerModel()));
                return RedirectToAction("Create", "Cottage");
            }
            Cottage cottage = _UnitOfWork.CottageRepository.Table.FirstOrDefault(f => f.Id == cottageId);

            CottagePriceModel cottagePriceModel = new CottagePriceModel();

            cottagePriceModel.CottageId = cottageId;

            cottagePriceModel.PriceList = new List<CottagePriceList>()
            {
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 1 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 2 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 3 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 4 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 5 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 6 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new CottagePriceList(){ Id = 0 , CottageId = cottageId , Day = 7 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0, MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" }
            };

            ViewBag.activePage = "اسعار المزرعة";
            ViewBag.cottageName = cottage.NameAr;
            return View(cottagePriceModel);
        }

        [HttpPost]
        public IActionResult CottagePriceList(CottagePriceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 

                    if (model.Person <= 0)
                    {
                        ErrorNotification("يجب ادخال عدد الاشخاص");
                        return View(model);
                    }

                    int personExists = _UnitOfWork.CottagePriceListRepository.Table.Count(f => f.Person == model.Person && f.CottageId == model.CottageId);
                    if (personExists > 0)
                    {
                        ErrorNotification("عدد الاشخاص موجود مسبقا");
                        return View(model);
                    }

                    #endregion

                    foreach (var item in model.PriceList)
                    {
                        item.CottageId = model.CottageId;
                        item.Person = model.Person;
                        _UnitOfWork.CottagePriceListRepository.Insert(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", new { id = model.CottageId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
            }
            return View(model);
        }


        [HttpPost]
        public IActionResult EditPriceList([FromBody] List<CottagePriceList> priceLists)
        {
            if (priceLists != null && priceLists.Any())
            {
                try
                {
                    foreach (CottagePriceList priceList in priceLists)
                    {
                        if (priceList.Id > 0)
                        {
                            _UnitOfWork.CottagePriceListRepository.Update(priceList);
                        }
                        _UnitOfWork.Save();
                    }


                    return Json(new { success = true, message = "تم التحديث بنجاح" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "لا توجد بيانات للتحديث" });
        }

        //[HttpPost]
        //public IActionResult DeletePriceList(DeleteFarmerPriceModel model, IFormFile formFile)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            #region :: Validation 

        //            if (model.FarmerId <= 0)
        //            {
        //                ErrorNotification("يجب ادخال رقم المزرعة");
        //                return View(model);
        //            }

        //            #endregion


        //            List<FarmerPriceList> farmerPriceList = _FarmerPriceList.Table.Where(f => f.FarmerId == model.FarmerId & f.Person == model.Person).Distinct().ToList();

        //            foreach (FarmerPriceList item in farmerPriceList)
        //            {
        //                _UnitOfWork.FarmerPriceListRepository.Delete(item);
        //            }

        //            _UnitOfWork.Save();
        //            SuccessNotification("تم الحذف بنجاح");
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return View(model);
        //}
        [HttpPost]
        public IActionResult DeleteSportPriceList(int cottageId, int person)
        {
            try
            {
                if (cottageId <= 0)
                {
                    ErrorNotification("يجب إدخال رقم الكوخ");
                    return RedirectToAction("Index");
                }

                if (person <= 0)
                {
                    ErrorNotification("يجب إدخال عدد الأشخاص");
                    return RedirectToAction("Index");
                }

                // جلب الأسعار المراد حذفها
                var prices = _UnitOfWork.CottagePriceListRepository.Table
                    .Where(p => p.CottageId == cottageId && p.Person == person)
                    .ToList();

                if (!prices.Any())
                {
                    ErrorNotification("لا توجد أسعار لهذا العدد من الأشخاص");
                    return RedirectToAction("Edit", new { id = cottageId });
                }

                foreach (var price in prices)
                {
                    _UnitOfWork.CottagePriceListRepository.Delete(price);
                }

                _UnitOfWork.Save();
                SuccessNotification("تم حذف الأسعار بنجاح");
                return RedirectToAction("Edit", new { id = cottageId });
            }
            catch (Exception e)
            {
                ErrorNotification($"خطأ: {e.Message}");
                return RedirectToAction("Edit", new { id = cottageId });
            }
        }
        #endregion




    }
}
