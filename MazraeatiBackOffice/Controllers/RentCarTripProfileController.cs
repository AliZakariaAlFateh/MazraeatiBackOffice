using MazraeatiBackOffice;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Controllers;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MazraeatiBackOffice.Controllers
{
    public class RentCarTripProfileController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<TripProfile> _TripProfileRepository;
        private readonly IRepository<Trip> _TripRepository;
        private readonly IRepository<Country> _CountryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<TripImage> _TripImage;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public RentCarTripProfileController(IRepository<TripProfile> tripProfileRepository, IRepository<Trip> tripRepository, IRepository<Country> countryRepository ,
            IRepository<TripImage> tripImage, IRepository<City> cityRepository,
            IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _TripProfileRepository = tripProfileRepository;
            _TripRepository = tripRepository;
            _CountryRepository = countryRepository;
            _cityRepository = cityRepository;
            _TripImage = tripImage;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public TripProfileModel FillModel(TripProfileModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.TypeId = 6;
            return model;
        }

        public TripModel TripFillModel(TripModel model)
        {
            model.Countries = _CountryRepository.Table.Where(a => a.Active == true).ToList();
            model.Cities = _cityRepository.Table.Where(a => a.Active == true && a.CountryId == 2).ToList();
            model.TypeId = 6;
            return model;
        }

        public IActionResult Index()
        {
            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            var model = _TripProfileRepository.Table.Where(t => t.TypeId == 6).OrderByDescending(a => a.Id).Select(c => c.ToModel(countries, Cities, null));
            ViewBag.activePage = "منظمي حفلات المزارع";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            var model = _TripProfileRepository.Table.Where(t => t.TypeId == 6).OrderByDescending(a => a.Id).Where(a => a.Name.Contains(search) || a.Description.Contains(search)).Select(c => c.ToModel(countries, Cities, null));
            ViewBag.activePage = "شركات تاجير السيارات";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "شركات تاجير السيارات";
            return View(FillModel(new TripProfileModel()));
        }

        [HttpPost]
        public IActionResult Create(TripProfileModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "trip/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "trip");

                    model.TypeId = 6;
                    _UnitOfWork.TripProfileRepository.Insert(model.ToEntity());
                    SuccessNotification("تم اضافة السجل بنجاح");
                    _UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillModel(model));
        }

        public IActionResult Edit(int id)
        {
            TripProfile tripProfile = _TripProfileRepository.GetById(id);
            if (tripProfile == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "شركات تاجير السيارات";
            return View(FillModel(tripProfile.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(TripProfileModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "trip/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "trip");

                    _UnitOfWork.TripProfileRepository.Update(model.ToEntity());
                    SuccessNotification("تم تحديث السجل بنجاح");
                    _UnitOfWork.Save();
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

            TripProfile tripProfile = _TripProfileRepository.GetById(id);
            if (tripProfile == null)
                return Json("السجل غير معرف");

            _UnitOfWork.TripProfileRepository.Delete(tripProfile);
            _UnitOfWork.Save();
            return Json(1);
        }


        #region :: Trip 

        public IActionResult TripIndex(int id)
        {
            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            var model = _TripRepository.Table.Where(t => t.TypeId == 6 && t.ProfileId == id).OrderByDescending(a => a.Id).Select(c => c.ToModel(countries, Cities, null, null));
            ViewBag.activePage = "انشطة شركات تاجير السيارات";
            ViewBag.profileId = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult TripIndex(int id,string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("TripIndex", new { id = id });

            List<Country> countries = _CountryRepository.Table.ToList();
            List<City> Cities = _cityRepository.Table.ToList();
            var model = _TripRepository.Table.Where(t => t.TypeId == 6 && t.ProfileId == id).OrderByDescending(a => a.Id).Where(a => a.Name.Contains(search) || a.Description.Contains(search)).Select(c => c.ToModel(countries, Cities, null, null));
            ViewBag.activePage = "انشطة شركات تاجير السيارات";
            ViewBag.search = search;
            ViewBag.profileId = id;
            return View(model);
        }

        public IActionResult TripCreate(int id)
        {
            ViewBag.activePage = "انشطة شركات تاجير السيارات";
            TripModel tripModel = new TripModel();
            tripModel.ProfileId = id;
            return View(TripFillModel(tripModel));
        }

        [HttpPost]
        public IActionResult TripCreate(TripModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // get max number of farms
                    long nMaxNumber = _TripRepository.Table.ToList().Select(x => x.Number).DefaultIfEmpty(0).Max();
                    int nId = _TripRepository.Table.ToList().Select(x => x.Id).DefaultIfEmpty(0).Max();

                    // create new farms
                    model.Id = 0;
                    model.TypeId = 6;
                    model.Number = nMaxNumber + 1;
                    model.IssueDate = DateTime.Now;

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
                    return RedirectToAction("TripIndex", new { id = model.ProfileId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving Trip , please contact to administrator");
            }
            return View(TripFillModel(model));
        }

        public IActionResult TripEdit(int id)
        {
            Trip trip = _UnitOfWork.TripRepository.GetById(id);
            if (trip == null)
                return RedirectToAction("TripIndex", new { id = id });


            ViewBag.activePage = "انشطة شركات تاجير السيارات";
            return View(TripFillModel(trip.ToModel()));
        }

        [HttpPost]
        public IActionResult TripEdit(TripModel model, IFormFile formFile)
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
                    /*
                     * foreach (TripExtraFeatureTypeDto extra in model.ExtraFeature)
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
                    */

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
                    return RedirectToAction("TripIndex", new { id = model.ProfileId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving trip , please contact to administrator");
            }
            return View(model);
        }


        #region :: Edit Image 

        public IActionResult TripImages(int id)
        {
            List<TripImage> tripImage = _UnitOfWork.TripImageRepository.Table.Where(x => x.TripId == id).ToList();
            TripImagesModel tripImagesModel = new TripImagesModel();
            tripImagesModel.Images = tripImage;
            tripImagesModel.TripName = _UnitOfWork.TripRepository.GetById(id).Name;
            ViewBag.activePage = "صور انشطة شركات تاجير السيارات";
            ViewBag.profileTripId = _UnitOfWork.TripRepository.GetById(id).ProfileId;
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

        #endregion

    }
}
