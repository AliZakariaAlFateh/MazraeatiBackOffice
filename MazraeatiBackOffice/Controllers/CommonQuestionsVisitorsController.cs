using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Extenstion;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace MazraeatiBackOffice.Controllers
{
    public class CommonQuestionsVisitorsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<CommonQuestionsVisitors> _CommonQuestionsVisitorsRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public CommonQuestionsVisitorsController(IRepository<CommonQuestionsVisitors> commonQuestionsVisitorsRepository,
            IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _CommonQuestionsVisitorsRepository = commonQuestionsVisitorsRepository;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }
        public CommonQuestionsVisitorsModel FillModel(CommonQuestionsVisitorsModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _CommonQuestionsVisitorsRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel());
            ViewBag.activePage = "الأسئلة الشائعة بالزوار";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _CommonQuestionsVisitorsRepository.Table.OrderByDescending(a => a.Id).Where(
                a => a.QuestAr.Contains(search) || a.QuestEn.Contains(search) ||
                a.AnswerAr.Contains(search) || a.AnswerEn.Contains(search)
                ).Select(c => c.ToModel());
            ViewBag.activePage = "الأسئلة الشائعة";
            ViewBag.search = search;
            return View(model);
        }



        public IActionResult Create()
        {
            ViewBag.activePage = "الأسئلة الشائعة بالزوار";
            return View(FillModel(new CommonQuestionsVisitorsModel()));
        }

        [HttpPost]
        public IActionResult Create(CommonQuestionsVisitorsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "CommonQuestionsVisitorsImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CommonQuestionsVisitorsImage");

                    _UnitOfWork.CommonQuestionsVisitorsRepository.Insert(model.ToEntity());
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
            CommonQuestionsVisitors commonQuestionsVisitors = _CommonQuestionsVisitorsRepository.GetById(id);
            if (commonQuestionsVisitors == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "الأسئلة الشائعة بالزوار";
            return View(FillModel(commonQuestionsVisitors.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CommonQuestionsVisitorsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "CommonQuestionsImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CommonQuestionsImage");

                    _UnitOfWork.CommonQuestionsVisitorsRepository.Update(model.ToEntity());
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

            CommonQuestionsVisitors commonQuestionsVisitors = _CommonQuestionsVisitorsRepository.GetById(id);
            if (commonQuestionsVisitors == null)
                return Json("السجل غير معرف");

            _UnitOfWork.CommonQuestionsVisitorsRepository.Delete(commonQuestionsVisitors);
            _UnitOfWork.Save();
            return Json(1);
        }

    }
}
