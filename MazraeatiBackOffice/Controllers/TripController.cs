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
    public class TripController : BaseController
    {
        private readonly IRepository<TripSection> _TripSectionRepository;
        private readonly IRepository<Trip> _TripRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<TripExtraFeatureType> _TripExtraFeatureType;
        private readonly IRepository<TripPriceList> _TripPriceList;
        private readonly IRepository<TripImage> _TripImage;
        private readonly IRepository<TripVideo> _TripVideo;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public TripController(IRepository<TripSection> TripSectionRepository,IRepository<Trip> TripRepository, IRepository<City> cityRepository, IRepository<LookupValue> LookupValueRepository,
            IRepository<TripExtraFeatureType> TripExtraFeatureType, IRepository<TripImage> TripImage, IRepository<TripVideo> TripVideo, IRepository<TripPriceList> TripPriceList,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _TripSectionRepository = TripSectionRepository;
            _TripRepository = TripRepository;
            _cityRepository = cityRepository;
            _LookupValueRepository = LookupValueRepository;
            _TripExtraFeatureType = TripExtraFeatureType;
            _TripPriceList = TripPriceList;
            _TripImage = TripImage;
            _TripVideo = TripVideo;
            _UnitOfWork = UnitOfWork;
            _configuration = configuration;
            webHostEnvironment = hostEnvironment;
        }

        
        public TripModel NewFillModel(TripModel model, int tripSectionId)
        {
            model.Owner = "من المالك";
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            model.TripSectionId = tripSectionId;

            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 5).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                model.ExtraFeature.Add(new TripExtraFeatureTypeDto()
                {
                    TripId = 0 ,
                    TypeId = Value.Id,
                    DescAr = Value.ValueAr,
                    ExtraText = "",
                    IsCheck = false

                });
            }

            model.PriceList = new List<TripPriceList>();
            return model;
        }

        public TripModel EditFillModel(TripModel model)
        {
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2 && a.Id != 20).ToList();
            List<TripExtraFeatureType> tripExtraFeatureTypes = _TripExtraFeatureType.Table.Where(x => x.TripId == model.Id).ToList();
            List<LookupValue> lookupValue = _LookupValueRepository.Table.Where(l => l.LookupId == 5).ToList();
            foreach (LookupValue Value in lookupValue)
            {
                if (tripExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0)
                {
                    model.ExtraFeature.Add(new TripExtraFeatureTypeDto()
                    {
                        Id = tripExtraFeatureTypes.FirstOrDefault(f => f.TypeId == Value.Id).Id,
                        TripId = model.Id,
                        TypeId = Value.Id,
                        DescAr = Value.ValueAr,
                        ExtraText = "",
                        IsCheck = tripExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false
                    });
                }
                else
                {
                    model.ExtraFeature.Add(new TripExtraFeatureTypeDto()
                    {
                        TripId = model.Id,
                        TypeId = Value.Id,
                        DescAr = Value.ValueAr,
                        ExtraText = "",
                        IsCheck = tripExtraFeatureTypes.Count(f => f.TypeId == Value.Id) > 0 ? true : false

                    });
                }

            }
            model.PriceList = _TripPriceList.Table.Where(f => f.TripId == model.Id).OrderBy(f => f.Person).ToList();
            return model;
        }

        public TripPriceModel NewFillTripPriceModel(TripPriceModel model)
        {
            model.Id = 0;
            return model;
        }

        public IActionResult Index(int tripSectionId)
        {
            List<TripSection> TripSections = _TripSectionRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();

            var model = _TripRepository.Table.Where(t=>t.TripSectionId == tripSectionId).OrderByDescending(a => a.Id).Select(c => c.ToModel(Cities, TripSections));
            ViewBag.activePage = "الاقسام اخرى";
            ViewBag.tripSectionId = tripSectionId;
            ViewBag.tripSectionDesc = TripSections.FirstOrDefault(t=>t.Id == tripSectionId).Title;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int tripSectionId, string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            List<TripSection> TripSections = _TripSectionRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();

            var model = _TripRepository.Table.Where(t => t.TripSectionId == tripSectionId).OrderByDescending(a => a.Id)
                .Where(a => a.Name.Contains(search) || a.MobileNumber.Contains(search)).Select(c => c.ToModel(Cities, TripSections));

            ViewBag.activePage = "الاقسام اخرى";
            ViewBag.search = search;
            ViewBag.tripSectionId = tripSectionId;
            ViewBag.tripSectionDesc = TripSections.FirstOrDefault(t => t.Id == tripSectionId).Title;
            return View(model);
        }

        public IActionResult Create(int tripSectionId)
        {
            ViewBag.activePage = "الاقسام اخرى";
            return View(NewFillModel(new TripModel(), tripSectionId));
        }

        [HttpPost]
        public IActionResult Create(TripModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    // get max number of farms
                    long nMaxNumber = _TripRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();

                    // create new farms
                    model.Number = nMaxNumber + 1;
                    model.IssueDate = DateTime.Now;
                    model.ExpiryDate = DateTime.Now.AddYears(1);

                    _UnitOfWork.TripRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();


                    int nTripId = _TripRepository.Table.FirstOrDefault(f => f.Number == (nMaxNumber + 1)).Id;

                    // add extra feature
                    foreach (TripExtraFeatureTypeDto extra in model.ExtraFeature.Where(e => e.IsCheck == true))
                    {
                        TripExtraFeatureType tripExtraFeatureType = new TripExtraFeatureType();
                        tripExtraFeatureType.TripId = nTripId;
                        tripExtraFeatureType.TypeId = extra.TypeId;
                        tripExtraFeatureType.ExtraText = extra.ExtraText;

                        _UnitOfWork.TripExtraFeatureTypeRepository.Insert(tripExtraFeatureType);
                    }

                    // add price list
                    if (model.PriceList != null)
                    {
                        foreach (TripPriceList priceList in model.PriceList)
                        {
                            TripPriceList tripPriceList = new TripPriceList();
                            tripPriceList.TripId = nTripId;
                            tripPriceList.Person = priceList.Person;
                            tripPriceList.Price = priceList.Price;
                            tripPriceList.OfferPrice = priceList.OfferPrice;
                            _UnitOfWork.TripPriceListRepository.Insert(tripPriceList);
                        }
                    }
                   

                    // add image
                    int nSortImage = 1;

                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            TripImage tripImage = new TripImage();

                            if (file != null)
                                tripImage.Url = "trip/" + GenericFunction.UploadedFile(file, webHostEnvironment, "trip");

                            tripImage.TripId = nTripId;
                            tripImage.Sort = nSortImage;
                            tripImage.Active = true;

                            _UnitOfWork.TripImageRepository.Insert(tripImage);
                            nSortImage++;
                        }
                    }
                        

                    // add videos
                    int nSortVideo = 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            TripVideo tripVideo = new TripVideo();

                            if (file != null)
                                tripVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            tripVideo.TripId = nTripId;
                            tripVideo.Sort = nSortVideo;
                            tripVideo.Active = true;

                            _UnitOfWork.TripVideoRepository.Insert(tripVideo);
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
                ErrorNotification("error while saving Trip , please contact to administrator");
                logFile.LogCustomInfo("Create Trip - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create Trip - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create Trip - Inner Exception Message ", e.InnerException.ToString());
            }
            return View(NewFillModel(new TripModel(), model.TripSectionId));
        }

        public IActionResult Edit(int id)
        {
            Trip trip = _UnitOfWork.TripRepository.GetById(id);
            if (trip == null)
                return RedirectToAction("Index", new { tripSectionId = trip.TripSectionId });


            ViewBag.activePage = "الاقسام اخرى";
            return View(EditFillModel(trip.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(TripModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int maxOrderIdImage = 0;
                    int maxOrderIdVideo = 0;

                    if (_UnitOfWork.TripImageRepository.Table.ToList().Count(f => f.TripId == model.Id) > 0)
                         maxOrderIdImage = _UnitOfWork.TripImageRepository.Table.ToList().Where(f => f.TripId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    if (_UnitOfWork.TripVideoRepository.Table.ToList().Count(f => f.TripId == model.Id) > 0)
                        maxOrderIdVideo = _UnitOfWork.TripVideoRepository.Table.ToList().Where(f => f.TripId == model.Id).Select(x => x.Sort).DefaultIfEmpty(0).Max();

                    // edit extra feature
                    foreach (TripExtraFeatureTypeDto extra in model.ExtraFeature)
                    {
                        TripExtraFeatureType tripExtraFeatureType = new TripExtraFeatureType();
                        tripExtraFeatureType.Id = extra.Id;
                        tripExtraFeatureType.TripId = model.Id;
                        tripExtraFeatureType.ExtraText = extra.ExtraText;
                        tripExtraFeatureType.TypeId = extra.TypeId;

                        if (extra.Id > 0)
                        {
                            if (!extra.IsCheck)
                            {
                                _UnitOfWork.TripExtraFeatureTypeRepository.Delete(tripExtraFeatureType);
                            }
                            else
                            {
                                if (_TripExtraFeatureType.Table.Count(f => f.TripId == model.Id && f.TypeId == extra.TypeId) == 0)
                                    _UnitOfWork.TripExtraFeatureTypeRepository.Insert(tripExtraFeatureType);
                            }
                        }
                        else
                        {
                            if (extra.IsCheck)
                                _UnitOfWork.TripExtraFeatureTypeRepository.Insert(tripExtraFeatureType);
                        }
                    }

                    if (model.PriceList != null)
                    {
                        // edit price list
                        foreach (TripPriceList priceList in model.PriceList)
                        {
                            _UnitOfWork.TripPriceListRepository.Update(priceList);
                        }
                    }

                    // add new image
                    int nSortImage = maxOrderIdImage + 1;
                    if (model.Images != null)
                    {
                        foreach (IFormFile file in model.Images)
                        {
                            TripImage tripImage = new TripImage();

                            if (file != null)
                                tripImage.Url = "trip/" + GenericFunction.UploadedFile(file, webHostEnvironment, "trip");

                            tripImage.TripId = model.Id;
                            tripImage.Sort = nSortImage;
                            tripImage.Active = true;

                            _UnitOfWork.TripImageRepository.Insert(tripImage);
                            nSortImage++;
                        }
                    }
                    

                    // add new video
                    int nSortVideo = maxOrderIdVideo + 1;
                    if (model.Videos != null)
                    {
                        foreach (IFormFile file in model.Videos)
                        {
                            TripVideo tripVideo = new TripVideo();

                            if (file != null)
                                tripVideo.Url = GenericFunction.UploadedVideo(file, webHostEnvironment);

                            tripVideo.TripId = model.Id;
                            tripVideo.Sort = nSortVideo;
                            tripVideo.Active = true;

                            _UnitOfWork.TripVideoRepository.Insert(tripVideo);
                            nSortVideo++;
                        }
                    }

                    _UnitOfWork.TripRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving trip , please contact to administrator");
            }
            return View(model);
        }


        #region :: Trip Price List

        public IActionResult TripPriceList(int tripId)
        {
            Trip trip = _UnitOfWork.TripRepository.Table.FirstOrDefault(f => f.Id == tripId);

            TripPriceModel tripPriceModel = new TripPriceModel();
            tripPriceModel.TripId = tripId;
            tripPriceModel.PriceList = new List<TripPriceList>()
            {
                new TripPriceList(){ Id = 0 , Person = 0 , TripId = tripId , Price = 0 , OfferPrice = 0},
            };

            ViewBag.activePage = "اسعار الخدمات الاخرى";
            ViewBag.tripName = trip.Name;
            return View(tripPriceModel);
        }

        [HttpPost]
        public IActionResult TripPriceList(TripPriceModel model, IFormFile formFile)
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

                    int personExists = _TripPriceList.Table.Count(f => f.Person == model.Person && f.TripId == model.TripId);
                    if (personExists > 0)
                    {
                        ErrorNotification("عدد الاشخاص موجود مسبقا");
                        return View(model);
                    }

                    #endregion

                    foreach (var item in model.PriceList)
                    {
                        item.TripId = model.TripId;
                        item.Person = model.Person;
                        _UnitOfWork.TripPriceListRepository.Insert(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة / تعديل  السجل بنجاح");
                    return RedirectToAction("Edit", new { id = model.TripId });
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillTripPriceModel(new TripPriceModel()));
        }

        [HttpPost]
        public IActionResult DeletePriceList(DeleteTripPriceModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: Validation 

                    if (model.TripId <= 0)
                    {
                        ErrorNotification("يجب ادخال رقم المزرعة");
                        return View(model);
                    }

                    #endregion


                    List<TripPriceList> tripPriceList = _TripPriceList.Table.Where(f => f.TripId == model.TripId & f.Person == model.Person).Distinct().ToList();

                    foreach (TripPriceList item in tripPriceList)
                    {
                        _UnitOfWork.TripPriceListRepository.Delete(item);
                    }

                    _UnitOfWork.Save();
                    SuccessNotification("تم الحذف بنجاح");
                    return RedirectToAction("Edit", new { id = model.TripId });
                }
            }
            catch (Exception e)
            {

            }
            return View(NewFillTripPriceModel(new TripPriceModel()));
        }

        #endregion


        #region :: Edit Image 

        public IActionResult Images(int id)
        {
            List<TripImage> tripImage = _UnitOfWork.TripImageRepository.Table.Where(x => x.TripId == id).ToList();
            TripImagesModel tripImagesModel = new TripImagesModel();
            tripImagesModel.Images = tripImage;
            tripImagesModel.TripName = _UnitOfWork.TripRepository.GetById(id).Name;
            ViewBag.activePage = "صور الاقسام الاخرى";
            return View(tripImagesModel);
        }
       
        [HttpPost]
        public IActionResult DeleteImage(string Ids)
        {
            try
            {
                if (string.IsNullOrEmpty(Ids))
                    return Json("الرجاء اختيار الصور");

                List<int> vs = ConvertStringListToIntList(Ids);

                foreach (var item in vs)
                {
                    TripImage tripImages = _TripImage.Table.FirstOrDefault(i => i.Id == item);
                    _UnitOfWork.TripImageRepository.Delete(tripImages);
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

    }
}
