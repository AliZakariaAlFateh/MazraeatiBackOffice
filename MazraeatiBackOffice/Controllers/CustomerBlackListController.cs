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
    public class CustomerBlackListController : BaseController
    {
        private readonly IRepository<CustomerBlackList> _CustomerBlackListRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;

        public CustomerBlackListController(IRepository<CustomerBlackList> customerBlackListRepository
            , IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _CustomerBlackListRepository = customerBlackListRepository;
            _UnitOfWork = UnitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }


        public CustomerBlackListModel NewFillModel(CustomerBlackListModel model)
        {
            return model;
        }

        public CustomerBlackListModel EditFillModel(CustomerBlackListModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _CustomerBlackListRepository.Table.Select(c => c.ToModel());
            ViewBag.activePage = "قائمة العملاء المحظوريين";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            search = string.IsNullOrEmpty(search) ? "" : search;
            var model = _CustomerBlackListRepository.Table.Where(t => (t.Reason.Contains(search) || t.CustMobileNum.Contains(search) || t.CustName.Contains(search))).Select(c => c.ToModel());

            ViewBag.activePage = "قائمة العملاء المحظوريين";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "قائمة العملاء المحظوريين";
            return View(NewFillModel(new CustomerBlackListModel()));
        }

        [HttpPost]
        public IActionResult Create(CustomerBlackListModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();

            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                    {
                        var fileName = GenericFunction.UploadedFile(formFile, webHostEnvironment, "CustomerBlackListImage");
                        model.ImageUrl = $"CustomerBlackListImage/{fileName}";
                        //model.ImageUrl = "CustomerBlackListImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CustomerBlackListImage");
                    }

                    _UnitOfWork.CustomerBlackListRepository.InsertEntity(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //ErrorNotification("error while saving , please contact to administrator");
                ErrorNotification($"Error while Update CustomerBlackLis: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Create CustomerBlackLis - Exception Message ", e.Message);
                logFile.LogCustomInfo("Create CustomerBlackLis - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Create CustomerBlackLis - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Create", model);
            }
            return View(NewFillModel(new CustomerBlackListModel()));
        }

        public IActionResult Edit(int id)
        {
            CustomerBlackList customerBlackList = _UnitOfWork.CustomerBlackListRepository.GetById(id);
            if (customerBlackList == null)
                return RedirectToAction("Index", new { farmerId = 0 });


            ViewBag.activePage = "قائمة العملاء المحظوريين";
            return View(EditFillModel(customerBlackList.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CustomerBlackListModel model, IFormFile formFile)
        {
            LogFile logFile = new LogFile();
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                    {
                        //model.ImageUrl = "CustomerBlackListImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CustomerBlackListImage");
                        var fileName = GenericFunction.UploadedFile(formFile, webHostEnvironment, "CustomerBlackListImage");
                        model.ImageUrl = $"CustomerBlackListImage/{fileName}";
                    }

                    _UnitOfWork.CustomerBlackListRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification("error while saving  , please contact to administrator");
                ErrorNotification("error while saving , please contact to administrator");
                ErrorNotification($"Error while Update CustomerBlackLis: {e.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("Edit CustomerBlackLis - Exception Message ", e.Message);
                logFile.LogCustomInfo("Edit CustomerBlackLis - Stack Trace Message ", e.StackTrace);
                logFile.LogCustomInfo("Edit CustomerBlackLis - Inner Exception Message ", e.InnerException.ToString());
                return RedirectToAction("Edit", model);
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            string result = "1";
            CustomerBlackList customerBlackList = _CustomerBlackListRepository.GetById(id);
            if (customerBlackList == null)
                return Json("Record Not Exists");

            try
            {
                _UnitOfWork.CustomerBlackListRepository.Delete(customerBlackList);
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
