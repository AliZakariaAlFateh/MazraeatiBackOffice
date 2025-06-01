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
    public class FarmerBlackListController : BaseController
    {
        private readonly IRepository<FarmerBlackList> _FarmerBlackListRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public FarmerBlackListController(IRepository<FarmerBlackList> farmerBlackListRepository
            , IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _FarmerBlackListRepository = farmerBlackListRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public FarmerBlackListModel NewFillModel(FarmerBlackListModel model)
        {
            return model;
        }

        public FarmerBlackListModel EditFillModel(FarmerBlackListModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _FarmerBlackListRepository.Table.Select(c => c.ToModel());
            ViewBag.activePage = "قائمة المزارع المحظوريين";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            search = string.IsNullOrEmpty(search) ? "" : search;
            var model = _FarmerBlackListRepository.Table.Where(t => (t.Reason.Contains(search) || t.FarmerMobNum.Contains(search) || t.FarmerName.Contains(search) 
            || t.FarmerNameEn.Contains(search)|| t.ReasonEn.Contains(search)))
                .OrderByDescending(a=>a.Id).Select(c => c.ToModel());

            ViewBag.activePage = "قائمة المزارع المحظوريين";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked==true)
                      .Select(a => a.FarmerId).ToList();
            ViewBag.activePage = "قائمة المزارع المحظوريين";
            return View(NewFillModel(new FarmerBlackListModel()
            {
                Farmers = _UnitOfWork.FarmerRepository.Table.Where(a => a.CountryId == 2 && !farmerBlackListIds.Contains(a.Id)).ToList()
            }));
        }

        [HttpPost]
        public IActionResult Create(FarmerBlackListModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");

                    _UnitOfWork.FarmerBlackListRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving , please contact to administrator");
            }
            var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null&& a.IsBlocked == true)
                      .Select(a => a.FarmerId).ToList();
            return View(NewFillModel(new FarmerBlackListModel()
            {
                Farmers = _UnitOfWork.FarmerRepository.Table.Where(a => a.CountryId == 2 && !farmerBlackListIds.Contains(a.Id)).ToList()
            }));
        }

        public IActionResult Edit(int id)
        {
            FarmerBlackList farmerBlackList = _UnitOfWork.FarmerBlackListRepository.GetById(id);
            if (farmerBlackList == null)
                return RedirectToAction("Index", new { farmerId = 0 });


            ViewBag.activePage = "قائمة المزارع المحظوريين";
            var res = farmerBlackList.ToModel();
            //var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
            //          .Select(a => a.FarmerId).ToList();
            res.Farmers = _UnitOfWork.FarmerRepository.Table.Where(a => a.CountryId == 2).ToList();
            return View(EditFillModel(res));
        }

        [HttpPost]
        public IActionResult Edit(FarmerBlackListModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "blacklist/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "blacklist");

                    _UnitOfWork.FarmerBlackListRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving  , please contact to administrator");
            }
            //var farmerBlackListIds = _UnitOfWork.FarmerBlackListRepository.Table.Where(a => a.FarmerId != null && a.IsBlocked == true)
            //          .Select(a => a.FarmerId).ToList();

            model.Farmers = _UnitOfWork.FarmerRepository.Table.Where(a => a.CountryId == 2).ToList();

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            string result = "1";
            FarmerBlackList farmerBlackList = _FarmerBlackListRepository.GetById(id);
            if (farmerBlackList == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.FarmerBlackListRepository.Delete(farmerBlackList);
                _UnitOfWork.Save();
                SuccessNotification("Delete Succesfuly");
            }
            catch (Exception)
            {
                result = "There is data associated with this record";
            }

            return Json(result);
        }

    }
}
