using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;

namespace MazraeatiBackOffice.Controllers
{
    public class RegionController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public RegionController(IUnitOfWork unitOfWork,IRepository<Region> regionRepository, IRepository<City> cityRepository, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _regionRepository = regionRepository;
            _cityRepository = cityRepository;
            webHostEnvironment = hostEnvironment;
        }

        public RegionModel FillRegionModel(RegionModel model)
        {
            model.Cities = _cityRepository.Table.ToList();
            return model;
        }

        public IActionResult Index(int? cityid)
        {
            //var Cities = _cityRepository.Table.ToList();
            //var model = _regionRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel(Cities));
            //ViewBag.activePage = "المناطق";
            //return View(model);
            var cities = _cityRepository.Table.ToList();

            var model = _regionRepository.Table
                .Where(r => r.CityId == cityid)  
                .OrderByDescending(r => r.Id)
                .Select(r => r.ToModel(cities))
                .ToList();

            ViewBag.activePage = "المناطق";
            ViewBag.CityId = cityid;
            ViewBag.CreateUrl = Url.Action("Create", "Region", new { cityid = cityid });
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var Cities = _cityRepository.Table.ToList();
            var model = _regionRepository.Table.OrderByDescending(a => a.Id).Where(a => a.DescAr.Contains(search) || a.DescEn.Contains(search)).Select(c => c.ToModel(Cities));
            ViewBag.search = search;
            ViewBag.activePage = "المدن";

            return View(model);
        }

        //public IActionResult Create()
        //{
        //    ViewBag.activePage = "المدن";
        //    return View(FillRegionModel(new RegionModel()));
        //}
        [HttpGet]
        public IActionResult Create(int cityid)
        {
            ViewBag.activePage = "المدن";

            var model = new RegionModel
            {
                CityId = cityid
            };

            return View(FillRegionModel(model));
        }

        [HttpPost]
        public IActionResult Create(RegionModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int regionCount = _regionRepository.Table.Where(a => a.DescAr == model.DescAr || a.DescEn == model.DescEn).Count();
                    if (regionCount > 0)
                    {
                        ErrorNotification("المدينة مستخدمه مسبقا");
                        return View(FillRegionModel(model));
                    }

                    _UnitOfWork.RegionRepository.Insert(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index", new { cityid = model.CityId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillRegionModel(model));
        }

        public IActionResult Edit(int id)
        {
            Region region = _regionRepository.GetById(id);
            if (region == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "المدن";
            return View(FillRegionModel(region.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(RegionModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.RegionRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index", new { cityid = model.CityId });
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(FillRegionModel(model));
        }

        public IActionResult Delete(int id)
        {
            Region region = _regionRepository.GetById(id);
            if (region == null)
                return Json("السجل غير معرف");

            _UnitOfWork.RegionRepository.Delete(region);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
