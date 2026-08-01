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
    public class CommonQuestionsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<CommonQuestions> _CommonQuestionsRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public CommonQuestionsController(IRepository<CommonQuestions> commonQuestionsRepository,
            IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _CommonQuestionsRepository = commonQuestionsRepository;
            _UnitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;
        }
        public CommonQuestionsModel FillModel(CommonQuestionsModel model)
        {
            return model;
        }

        public IActionResult Index()
        {
            var model = _CommonQuestionsRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel());
            ViewBag.activePage = "الأسئلة الشائعة لأصحاب المزارع";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _CommonQuestionsRepository.Table.OrderByDescending(a => a.Id).Where(
                a => a.QuestAr.Contains(search) || a.QuestEn.Contains(search) ||
                a.AnswerAr.Contains(search) || a.AnswerEn.Contains(search)
                ).Select(c => c.ToModel());
            ViewBag.activePage = "الأسئلة الشائعة";
            ViewBag.search = search;
            return View(model);
        }



        public IActionResult Create()
        {
            ViewBag.activePage = "الأسئلة الشائعة لأصحاب المزارع";
            return View(FillModel(new CommonQuestionsModel()));
        }

        [HttpPost]
        public IActionResult Create(CommonQuestionsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "CommonQuestionsImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CommonQuestionsImage");

                    _UnitOfWork.CommonQuestionsRepository.Insert(model.ToEntity());
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
            CommonQuestions commonQuestions = _CommonQuestionsRepository.GetById(id);
            if (commonQuestions == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "الأسئلة الشائعة لأصحاب المزارع";
            return View(FillModel(commonQuestions.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CommonQuestionsModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                        model.ImageUrl = "CommonQuestionsImage/" + GenericFunction.UploadedFile(formFile, webHostEnvironment, "CommonQuestionsImage");

                    _UnitOfWork.CommonQuestionsRepository.Update(model.ToEntity());
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

            CommonQuestions CommonQuestions = _CommonQuestionsRepository.GetById(id);
            if (CommonQuestions == null)
                return Json("السجل غير معرف");

            _UnitOfWork.CommonQuestionsRepository.Delete(CommonQuestions);
            _UnitOfWork.Save();
            return Json(1);
        }


    }
}
