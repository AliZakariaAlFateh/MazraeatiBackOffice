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
using ClosedXML.Excel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace MazraeatiBackOffice.Controllers
{
    public class FarmersController : BaseController
    {
        private readonly IRepository<Farmer> _FarmerRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<FarmerExtraFeatureType> _FarmerExtraFeatureType;
        private readonly IRepository<FarmerPriceList> _FarmerPriceList;
        private readonly IRepository<FarmerUser> _FarmerUserRepository;
        private readonly IRepository<FarmerImage> _FarmerImage;
        private readonly IRepository<FarmerVideo> _FarmerVideo;
        private readonly IRepository<FarmerReservation> _FarmerReservation;
        private readonly IRepository<FarmerFeedback> _FarmerFeedback;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FarmersController(IRepository<City> cityRepository, IRepository<Country> countryRepository, IRepository<Farmer> FarmerRepository, 
            IRepository<LookupValue> LookupValueRepository, IRepository<FarmerExtraFeatureType> FarmerExtraFeatureType,
            IRepository<FarmerPriceList> FarmerPriceList, IRepository<FarmerImage> FarmerImage, IRepository<FarmerVideo> FarmerVideo,
            IRepository<FarmerUser> FarmerUserRepository, IRepository<FarmerReservation> FarmerReservation , IRepository<FarmerFeedback> FarmerFeedback , IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _FarmerRepository = FarmerRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _LookupValueRepository = LookupValueRepository;
            _FarmerExtraFeatureType = FarmerExtraFeatureType;
            _FarmerPriceList = FarmerPriceList;
            _FarmerImage = FarmerImage;
            _FarmerVideo = FarmerVideo;
            _FarmerUserRepository = FarmerUserRepository;
            _FarmerReservation = FarmerReservation;
            _FarmerFeedback = FarmerFeedback;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public FarmerModel NewFillModel(FarmerModel model)
        {
            model.Owner = "من المالك";
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 2).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                {
                    FarmerId = 0 ,
                    TypeId = Value.Id,
                    DescAr = Value.ValueAr,
                    ExtraText = "",
                    IsCheck = false

                });
            }
            model.PriceList = new List<FarmerPriceList>()
            {
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 1 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 2 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 3 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 4 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 5 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 6 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = 0 , Day = 7 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0, MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" }
            };
            return model;
        }

        public FarmerModel EditFillModel(FarmerModel model)
        {
            model.Countries = _countryRepository.Table.Where(a => a.Id == 2 && a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();

            List<FarmerExtraFeatureType> farmerExtraFeatureTypes = _FarmerExtraFeatureType.Table.Where(x => x.FarmerId == model.Id).ToList();
            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 2).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                if (farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0)
                {
                    model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                    {
                        Id = farmerExtraFeatureTypes.FirstOrDefault(f => f.TypeId == Value.Id).Id,
                        FarmerId = model.Id,
                        TypeId = Value.Id,
                        DescAr = Value.ValueAr,
                        ExtraText = "",
                        IsCheck = farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false
                    });
                }
                else
                {
                    model.ExtraFeature.Add(new FarmerExtraFeatureTypeDto()
                    {
                        FarmerId = model.Id,
                        TypeId = Value.Id,
                        DescAr = Value.ValueAr,
                        ExtraText = "",
                        IsCheck = farmerExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false

                    });
                }
                
            }
            model.PriceList = _FarmerPriceList.Table.Where(f => f.FarmerId == model.Id).OrderBy(f => f.Person).ThenBy(f => f.Day).ToList();
            return model;
        }

        public FarmerUserModel NewFillFarmerUserModel(FarmerUserModel model)
        {
            model.Id = 0;
            return model;
        }

        public IActionResult Index()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            ViewBag.activePage = "المزارع";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("Index");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime? _reservationDate = ReservationDate != null ? null : DateTime.Parse(ReservationDate);


            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            if (ReservationDate != DateTime.MinValue)
            {
                Reservation = Reservation.Where(r => r.ReservationDate.Date == ReservationDate.Date).ToList();
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                        a.MobileNumber.Contains(search) ||
                                                                                        a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                        (!Reservation.Select(r => r.FarmerId).Contains(a.Id)) && a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            else
            {
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                       a.MobileNumber.Contains(search) ||
                                                                                                       a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                                       a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            
            ViewBag.activePage = "المزارع";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #region :: Farmer Pending Approve

        public IActionResult FarmerPending()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var model = _FarmerRepository.Table.Where(f => f.CountryId == 6 && f.IsApprove == false).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            ViewBag.activePage = "المزارع الغير موافق عليهم";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult FarmerPending(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("FarmerPending");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime _reservationDate = string.IsNullOrEmpty(ReservationDate) ? null : DateTime.Parse(ReservationDate);

            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            if (ReservationDate != DateTime.MinValue)
            {
                Reservation = Reservation.Where(r => r.ReservationDate.Date == ReservationDate.Date).ToList();
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                        a.MobileNumber.Contains(search) ||
                                                                                        a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy)) &&
                                                                                        (Reservation.Select(r => r.FarmerId).Contains(a.Id)) && a.CountryId == 2 
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            else
            {
                model = _FarmerRepository.Table.OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                       a.MobileNumber.Contains(search) ||
                                                                                                       a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy))
                                                                                                       && a.CountryId == 2
                                                                                   ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            }
            ViewBag.activePage = "المزارع الغير موافق عليهم";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #endregion

        #region :: Farmer Not Completed Information

        public IActionResult FarmerNotCompletedInfo()
        {
            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var Reservation = _FarmerReservation.Table.ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var FarmerImage = _FarmerImage.Table.ToList().Select(x => x.FarmerId).ToArray();
            var FarmerPriceList = _FarmerPriceList.Table.ToList().Select(x => x.FarmerId).ToArray();

            var model = _FarmerRepository.Table.Where(f => f.CountryId == 2 && !FarmerImage.Contains(f.Id) || !FarmerPriceList.Contains(f.Id))
                                    .OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));

            ViewBag.activePage = "المزارع غير مكتملة المعلومات";
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public IActionResult FarmerNotCompletedInfo(string search, int CityBy, DateTime ReservationDate)
        {
            if (string.IsNullOrEmpty(search) && CityBy == 0 && ReservationDate == DateTime.MinValue)
                return RedirectToAction("FarmerNotCompletedInfo");

            search = string.IsNullOrEmpty(search) ? "" : search;
            //DateTime _reservationDate = string.IsNullOrEmpty(ReservationDate) ? null : DateTime.Parse(ReservationDate);

            var Countries = _countryRepository.Table.Where(f => f.Id == 2).ToList();
            var Cities = _cityRepository.Table.Where(f => f.CountryId == 2).ToList();
            var FarmerFeedback = _FarmerFeedback.Table.ToList();
            var FarmerImage = _FarmerImage.Table.ToList().Select(x => x.FarmerId).ToArray();
            var FarmerPriceList = _FarmerPriceList.Table.ToList().Select(x => x.FarmerId).ToArray();

            IQueryable<FarmerModel> model;
            var Reservation = _FarmerReservation.Table.ToList();
            model = _FarmerRepository.Table.Where(f => !FarmerImage.Contains(f.Id) || !FarmerPriceList.Contains(f.Id))
                                                                        .OrderByDescending(a => a.Id).Where(a => (a.Name.Contains(search) ||
                                                                                                   a.MobileNumber.Contains(search) ||
                                                                                                   a.Number.ToString().Contains(search)) && (CityBy == 0 || (CityBy > 0 && a.CityId == CityBy))
                                                                                                   && a.CountryId == 2
                                                                               ).Select(c => c.ToModel(Countries, Cities, Reservation, FarmerFeedback));
            ViewBag.activePage = "المزارع غير مكتملة المعلومات";
            ViewBag.search = search;
            ViewBag.cities = Cities.Where(c => c.CountryId == 2);
            ViewBag.DefaultDate = DateTime.Now;
            return View(model);
        }

        #endregion

        public IActionResult Create()
        {
            ViewBag.activePage = "المزارع";
            return View(NewFillModel(new FarmerModel()));
        }

        [HttpPost]
        public IActionResult Create(FarmerModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    // get max number of farms
                    long nMaxNumber = _FarmerRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();
                    int nId = _FarmerRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new farms
                    model.Number = nMaxNumber + 1;
                    model.Description = "";
                    //model.Owner = "من المالك";
                    model.IssueDate = DateTime.Now;
                    model.ExpiryDate = DateTime.Now.AddMonths(3);

                    _UnitOfWork.FarmerRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();


                    int nFarmsId = _FarmerRepository.Table.FirstOrDefault(f => f.Number == (nMaxNumber + 1)).Id;

                    // add extra feature
                    foreach (FarmerExtraFeatureTypeDto extra in model.ExtraFeature.Where(e=>e.IsCheck == true))
                    {
                        FarmerExtraFeatureType farmerExtraFeatureType = new FarmerExtraFeatureType();
                        farmerExtraFeatureType.FarmerId = nFarmsId;
                        farmerExtraFeatureType.TypeId = extra.TypeId;
                        farmerExtraFeatureType.ExtraText = extra.ExtraText;

                        _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);

                    }

                    // add price list
                    foreach (FarmerPriceList priceList in model.PriceList)
                    {
                        FarmerPriceList farmerPriceList = new FarmerPriceList();
                        farmerPriceList.FarmerId = nFarmsId;
                        farmerPriceList.Day = priceList.Day;
                        farmerPriceList.MorningPrice = priceList.MorningPrice;
                        farmerPriceList.EveningPrice = priceList.EveningPrice;
                        farmerPriceList.FullDayPrice = priceList.FullDayPrice;
                        farmerPriceList.OfferPrice = priceList.OfferPrice;
                        farmerPriceList.OfferEveningPrice = priceList.OfferEveningPrice;
                        farmerPriceList.OfferFullDayPrice = priceList.OfferFullDayPrice;
                        farmerPriceList.MorningPeriodText = priceList.MorningPeriodText;
                        farmerPriceList.EveningPeriodText = priceList.EveningPeriodText;
                        farmerPriceList.FullDayPeriodText = string.IsNullOrEmpty(priceList.FullDayPeriodText) ? "" : priceList.FullDayPeriodText;

                        _UnitOfWork.FarmerPriceListRepository.Insert(farmerPriceList);

                    }


                    // add image
                    int nSortImage = 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            FarmerImage farmerImage = new FarmerImage();

                            if (file != null)
                                farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(file, webHostEnvironment, "farmer");

                            farmerImage.FarmerId = nFarmsId;
                            farmerImage.Sort = nSortImage;
                            farmerImage.Vip = true;
                            farmerImage.Active = true;

                            _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                            nSortImage++;
                        }
                    }
                   

                    // add videos
                    int nSortVideo = 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            FarmerVideo farmerVideo = new FarmerVideo();

                            if (file != null)
                                farmerVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            farmerVideo.FarmerId = nFarmsId;
                            farmerVideo.Sort = nSortVideo;
                            farmerVideo.Active = true;

                            _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                            nSortVideo++;
                        }
                    }


                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving farms , please contact to administrator");
                logFile.LogCustomInfo("Create Farmer - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create Farmer - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create Farmer - Inner Exception Message ", e.InnerException.ToString());
            }
            return View(NewFillModel(new FarmerModel()));
        }

        public IActionResult Edit(int id)
        {
            Farmer farmer = _UnitOfWork.FarmerRepository.GetById(id);
            if (farmer == null)
                return RedirectToAction("Index");


            ViewBag.activePage = "المزارع";
            return View(EditFillModel(farmer.ToModel()));
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public IActionResult Edit(FarmerModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int maxOrderIdImage = 0;
                    int maxOrderIdVideo = 0;

                    if (_UnitOfWork.FarmerImageRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
                        maxOrderIdImage = _UnitOfWork.FarmerImageRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    if (_UnitOfWork.FarmerVideoRepository.Table.ToList().Count(f => f.FarmerId == model.Id) > 0)
                        maxOrderIdVideo = _UnitOfWork.FarmerVideoRepository.Table.ToList().Where(f => f.FarmerId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    if (model.PriceList != null)
                    {
                        // edit price list
                        foreach (FarmerPriceList priceList in model.PriceList)
                        {
                            _UnitOfWork.FarmerPriceListRepository.Update(priceList);
                        }
                    }

                    if (model.ExtraFeature != null)
                    {
                        // edit extra feature
                        foreach (FarmerExtraFeatureTypeDto extra in model.ExtraFeature)
                        {
                            FarmerExtraFeatureType farmerExtraFeatureType = new FarmerExtraFeatureType();
                            farmerExtraFeatureType.Id = extra.Id;
                            farmerExtraFeatureType.FarmerId = model.Id;
                            farmerExtraFeatureType.ExtraText = extra.ExtraText;
                            farmerExtraFeatureType.TypeId = extra.TypeId;

                            if (extra.Id > 0)
                            {
                                if (!extra.IsCheck)
                                {
                                    _UnitOfWork.FarmerExtraFeatureTypeRepository.Delete(farmerExtraFeatureType);
                                }
                                else
                                {
                                    if (_FarmerExtraFeatureType.Table.Count(f => f.FarmerId == model.Id && f.TypeId == extra.TypeId) == 0)
                                        _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
                                }
                            }
                            else
                            {
                                if (extra.IsCheck)
                                    _UnitOfWork.FarmerExtraFeatureTypeRepository.Insert(farmerExtraFeatureType);
                            }
                        }
                    }


                    // add new image
                    int nSortImage = maxOrderIdImage + 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            FarmerImage farmerImage = new FarmerImage();

                            if (file != null)
                                farmerImage.Url = "farmer/" + GenericFunction.UploadedFile(file, webHostEnvironment, "farmer");

                            farmerImage.FarmerId = model.Id;
                            farmerImage.Sort = nSortImage;
                            farmerImage.Vip = true;
                            farmerImage.Active = true;

                            _UnitOfWork.FarmerImageRepository.Insert(farmerImage);
                            nSortImage++;
                        }
                    }
                    

                    // add new video
                    int nSortVideo = maxOrderIdVideo + 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            FarmerVideo farmerVideo = new FarmerVideo();

                            if (file != null)
                                farmerVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            farmerVideo.FarmerId = model.Id;
                            farmerVideo.Sort = nSortVideo;
                            farmerVideo.Active = true;

                            _UnitOfWork.FarmerVideoRepository.Insert(farmerVideo);
                            nSortVideo++;
                        }
                    }

                    _UnitOfWork.FarmerRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving farms , please contact to administrator");
            }
            return View(model);
        }


        public IActionResult Delete(int id)
        {
            string result = "1";
            Farmer farmer = _FarmerRepository.GetById(id);
            if (farmer == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.FarmerRepository.Delete(farmer);
                _UnitOfWork.Save();
                SuccessNotification("Delete Succesfuly");
            }
            catch (Exception ex)
            {
                result = "There is data associated with this record";
            }

            return Json(result);
        }



        #region :: Edit Image 

        public IActionResult FarmerImages(int id)
        {
            List<FarmerImage> farmerImages = _UnitOfWork.FarmerImageRepository.Table.Where(x => x.FarmerId == id).ToList();
            FarmerImagesModel farmerImagesModel = new FarmerImagesModel();
            farmerImagesModel.Images = farmerImages;
            farmerImagesModel.FarmerName = _UnitOfWork.FarmerRepository.GetById(id).Name;
            farmerImagesModel.FarmerId = id;
            ViewBag.activePage = "صور المزارع";
            return View(farmerImagesModel);
        }


        [HttpPost]
        public IActionResult EditFarmsVIPImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    farmsImage.Vip = !farmsImage.Vip;
                    _UnitOfWork.FarmerImageRepository.Update(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult EditActiveOrNotImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    farmsImage.Active = !farmsImage.Active;
                    _UnitOfWork.FarmerImageRepository.Update(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteFarmsImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerImage farmsImage = _FarmerImage.Table.FirstOrDefault(i => i.Id == item);
                    _UnitOfWork.FarmerImageRepository.Delete(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }


        #region :: Download Multiple Files

        [HttpPost]
        public IActionResult SaveFarmerImage(int farmerId)
        {
            List<FarmerImage> _images = _FarmerImage.Table.Where(f => f.FarmerId == farmerId).ToList();
            foreach (FarmerImage img in _images)
            {
                string url = $"{Request.Scheme}://{Request.Host}/Images/{img.Url}";
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save("Image1.png", ImageFormat.Png);
                }

                stream.Flush();
                stream.Close();
                client.Dispose();
            }
            return Json(1);
        }

        [HttpPost]
        public IActionResult DeleteAllImg(int farmerId)
        {
            try
            {
                List<FarmerImage> farmerImges = _FarmerImage.Table.Where(f => f.FarmerId == farmerId).ToList();
                foreach (FarmerImage farmsImage in farmerImges)
                {
                    _UnitOfWork.FarmerImageRepository.Delete(farmsImage);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion


        #endregion


        #region :: Edit Videos

        public IActionResult FarmerVideos(int id)
        {
            List<FarmerVideo> farmerVideos = _UnitOfWork.FarmerVideoRepository.Table.Where(x => x.FarmerId == id).ToList();
            FarmerVideosModel farmerVideosModel = new FarmerVideosModel();
            farmerVideosModel.Videos = farmerVideos;
            farmerVideosModel.FarmerName = _UnitOfWork.FarmerRepository.GetById(id).Name;
            farmerVideosModel.FarmerId = id;
            ViewBag.activePage = "فيديوهات المزارع";
            return View(farmerVideosModel);
        }


        [HttpPost]
        public IActionResult EditActiveOrNotVideo(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الفيدوهات");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerVideo farmsVideos = _FarmerVideo.Table.FirstOrDefault(i => i.Id == item);
                    farmsVideos.Active = !farmsVideos.Active;
                    _UnitOfWork.FarmerVideoRepository.Update(farmsVideos);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public IActionResult DeleteFarmsVideo(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الفيدوهات");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    FarmerVideo farmsVideos = _FarmerVideo.Table.FirstOrDefault(i => i.Id == item);
                    _UnitOfWork.FarmerVideoRepository.Delete(farmsVideos);
                }

                _UnitOfWork.Save();
                return Json(1);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion

        #region :: Farmer User

        public IActionResult FarmerUser(int farmerId)
        {
            Farmer farmer = _UnitOfWork.FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);

            #region :: Check If Have Any User For Farmer 

            FarmerUserModel farmerUserModel = new FarmerUserModel();
            int _count = _UnitOfWork.FarmerUserRepository.Table.Count(f => f.FarmerId == farmerId);
            if (_count > 0)
            {
                farmerUserModel = _FarmerUserRepository.Table.Where(f => f.FarmerId == farmerId).Select(c => c.ToModel()).FirstOrDefault();
            }
            else
            {
                farmerUserModel.Id = 0;
                farmerUserModel.FarmerId = farmerId;
            }


            #endregion
           

            ViewBag.activePage = "معلومات الدخول للتطبيق الادمن";
            ViewBag.farmerName = farmer.Name;
            
            return View(farmerUserModel);
        }

        [HttpPost]
        public IActionResult FarmerUser(FarmerUserModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 


                    model.UserName = model.UserName.Replace(" ", string.Empty);
                    model.Password = model.Password.Replace(" ", string.Empty);

                    int userNameExists = _FarmerUserRepository.Table.Count(f => f.UserName == model.UserName && f.Id != model.Id);
                    if (userNameExists > 0)
                    {
                        ErrorNotification("اسم المستخدم موجود مسبقا يجب عدم تكراره");
                        return View(model);
                    }

                    #endregion

                    if(model.Id == 0)
                    {
                        model.Id = 0;
                        model.CreatedDate = DateTime.Now;
                        model.UpdateDate = DateTime.Now;
                        _UnitOfWork.FarmerUserRepository.Insert(model.ToEntity());
                    }
                    else
                    {
                        model.UpdateDate = DateTime.Now;
                        _UnitOfWork.FarmerUserRepository.Update(model.ToEntity());
                    }

                  
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
               
            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        #endregion

        #region :: Farmer Price List

        public IActionResult FarmerPriceList(int farmerId)
        {
            Farmer farmer = _UnitOfWork.FarmerRepository.Table.FirstOrDefault(f => f.Id == farmerId);

            FarmerPriceModel farmerPriceModel = new FarmerPriceModel();

            farmerPriceModel.FarmerId = farmerId;
            farmerPriceModel.PriceList = new List<FarmerPriceList>()
            {
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 1 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 2 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 3 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 4 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 5 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 6 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0,MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" },
                new FarmerPriceList(){ Id = 0 , FarmerId = farmerId , Day = 7 , MorningPrice = 0 , EveningPrice = 0, FullDayPrice = 0, OfferPrice = 0, OfferEveningPrice = 0, OfferFullDayPrice = 0, MorningPeriodText = "" , EveningPeriodText = "", FullDayPeriodText = "" }
            };

            ViewBag.activePage = "اسعار المزرعة";
            ViewBag.farmerName = farmer.Name;
            return View(farmerPriceModel);
        }

        [HttpPost]
        public IActionResult FarmerPriceList(FarmerPriceModel model, IFormFile formFile)
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

                    int personExists = _FarmerPriceList.Table.Count(f => f.Person == model.Person && f.FarmerId == model.FarmerId);
                    if (personExists > 0)
                    {
                        ErrorNotification("عدد الاشخاص موجود مسبقا");
                        return View(model);
                    }

                    #endregion

                    foreach (var item in model.PriceList)
                    {
                        item.FarmerId = model.FarmerId;
                        item.Person = model.Person;
                        _UnitOfWork.FarmerPriceListRepository.Insert(item);
                    }
                    
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        [HttpPost]
        public IActionResult DeletePriceList(DeleteFarmerPriceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 

                    if (model.FarmerId <= 0)
                    {
                        ErrorNotification("يجب ادخال رقم المزرعة");
                        return View(model);
                    }

                    #endregion


                    List<FarmerPriceList> farmerPriceList = _FarmerPriceList.Table.Where(f => f.FarmerId == model.FarmerId & f.Person == model.Person).Distinct().ToList();

                    foreach (FarmerPriceList item in farmerPriceList)
                    {
                        _UnitOfWork.FarmerPriceListRepository.Delete(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم الحذف بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillFarmerUserModel(new FarmerUserModel()));
        }

        #endregion

        #region

        public IActionResult FarmerExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("مزارع");
                var currentRow = 1;

                List<City> cities = _cityRepository.Table.ToList();
                List<FarmerReservation> farmerReservations = _FarmerReservation.Table.ToList();
                List<Farmer> farms = _FarmerRepository.Table.ToList();

                worksheet.Cell(currentRow, 1).Value = "#";
                worksheet.Cell(currentRow, 2).Value = "اسم المزرعة";
                worksheet.Cell(currentRow, 3).Value = "رقم المزرعة";
                worksheet.Cell(currentRow, 4).Value = "المدينة";
                worksheet.Cell(currentRow, 5).Value = "عدد الحجوزات خلال الشهر";
                worksheet.Cell(currentRow, 6).Value = "رقم التلفون";
               

                foreach (var model in farms)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = model.Id.ToString();
                    worksheet.Cell(currentRow, 2).Value = model.Name;
                    worksheet.Cell(currentRow, 3).Value = model.Number;
                    worksheet.Cell(currentRow, 4).Value = cities.FirstOrDefault(c => c.Id == model.CityId).DescAr;
                    worksheet.Cell(currentRow, 5).Value = farmerReservations.Count(f => f.FarmerId == model.Id);
                    worksheet.Cell(currentRow, 6).Value = model.MobileNumber;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Farmers.xlsx");
                }
            }
        }

        #endregion


       

    }
}
