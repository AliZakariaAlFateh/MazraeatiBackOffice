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
    public class termsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<terms> _termsRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public termsController(IUnitOfWork unitOfWork,IRepository<terms> termsRepository, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _termsRepository = termsRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        public termsModel FillModel(termsModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _termsRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel());
            ViewBag.activePage = "بنود العقد";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _termsRepository.Table.OrderByDescending(a => a.Id).Where(a => a.DescEn.Contains(search) || a.DescAr.Contains(search) ).Select(c => c.ToModel());
            ViewBag.activePage = "بنود العقد";
            ViewBag.search = search;
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.activePage = "بنود العقد";
            return View(FillModel(new termsModel()));
        }

        [HttpPost]
        public IActionResult Create(termsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   

                    _UnitOfWork.TermsRepository.Insert(model.ToEntity());
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
            terms terms = _termsRepository.GetById(id);
            if (terms == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "بنود العقد";
            return View(FillModel(terms.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(termsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.TermsRepository.Update(model.ToEntity());
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
            terms terms = _termsRepository.GetById(id);
            if (terms == null)
                return Json("السجل غير معرف");

            _UnitOfWork.TermsRepository.Delete(terms);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
