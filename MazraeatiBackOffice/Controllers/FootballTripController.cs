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
    public class FootballTripController : BaseController
    {
        private readonly IRepository<TripProfile> _TripProfileRepository;
        private readonly IRepository<Trip> _TripRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<LookupValue> _LookupValueRepository;
        private readonly IRepository<TripExtraFeatureType> _TripExtraFeatureType;
        private readonly IRepository<TripImage> _TripImage;
        private readonly IRepository<TripVideo> _TripVideo;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FootballTripController(IRepository<Country> countryRepository, IRepository<City> cityRepository , IRepository<TripProfile> TripProfileRepository, IRepository<Trip> TripRepository, 
            IRepository<LookupValue> LookupValueRepository, IRepository<TripExtraFeatureType> TripExtraFeatureType, IRepository<TripImage> TripImage, IRepository<TripVideo> TripVideo,
            IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _TripProfileRepository = TripProfileRepository;
            _TripRepository = TripRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _LookupValueRepository = LookupValueRepository;
            _TripExtraFeatureType = TripExtraFeatureType;
            _TripImage = TripImage;
            _TripVideo = TripVideo;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        
        public TripModel NewFillModel(TripModel model)
        {
            //model.Countries = _countryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            //model.TypeId = 3;
            return model;
        }

        public TripModel EditFillModel(TripModel model)
        {
            //model.Countries = _countryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            //model.TypeId = 3;
            return model;
        }

        public IActionResult Index()
        {
            var Countries = _countryRepository.Table.ToList();
            var Cities = _cityRepository.Table.ToList();
            var model = _TripRepository.Table.Where(t => t.TypeId == 3).OrderByDescending(a => a.Id).Select(c => c.ToModel(Countries, Cities, null, null));
            ViewBag.activePage = "ملاعب كرة القدم";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var Countries = _countryRepository.Table.ToList();
            var Cities = _cityRepository.Table.ToList();
            var model = _TripRepository.Table.Where(t=>t.TypeId == 3).OrderByDescending(a => a.Id).Where(a => a.Name.Contains(search) || a.MobileNumber.Contains(search)).Select(c => c.ToModel(Countries, Cities, null, null));
            ViewBag.activePage = "ملاعب كرة القدم";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "ملاعب كرة القدم";
            return View(NewFillModel(new TripModel()));
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
                    int nId = _TripRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new farms
                    model.Number = nMaxNumber + 1;
                    //model.TypeId = 3;
                    model.IssueDate = DateTime.Now;

                    _UnitOfWork.TripRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();


                    int nTripId = _TripRepository.Table.FirstOrDefault(f => f.Number == (nMaxNumber + 1)).Id;

                    // add extra feature
                    /*foreach (TripExtraFeatureTypeDto extra in model.ExtraFeature.Where(e=>e.IsCheck == true))
                    {
                        TripExtraFeatureType tripExtraFeatureType = new TripExtraFeatureType();
                        tripExtraFeatureType.TripId = nTripId;
                        tripExtraFeatureType.TypeId = extra.TypeId;
                        tripExtraFeatureType.ExtraText = extra.ExtraText;

                        _UnitOfWork.TripExtraFeatureTypeRepository.Insert(tripExtraFeatureType);

                    }*/

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
            return View(NewFillModel(new TripModel()));
        }

        public IActionResult Edit(int id)
        {
            Trip trip = _UnitOfWork.TripRepository.GetById(id);
            if (trip == null)
                return RedirectToAction("Index");


            ViewBag.activePage = "ملاعب كرة القدم";
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
                    /*foreach (TripExtraFeatureTypeDto extra in model.ExtraFeature)
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
                    }*/


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


        #region :: Edit Image 

        public IActionResult Images(int id)
        {
            List<TripImage> tripImage = _UnitOfWork.TripImageRepository.Table.Where(x => x.TripId == id).ToList();
            TripImagesModel tripImagesModel = new TripImagesModel();
            tripImagesModel.Images = tripImage;
            tripImagesModel.TripName = _UnitOfWork.TripRepository.GetById(id).Name;
            ViewBag.activePage = "صور ملاعب كرة القدم";
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
