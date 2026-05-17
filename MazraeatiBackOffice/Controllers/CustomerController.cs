using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;
using MazraeatiBackOffice.Extenstion;

namespace MazraeatiBackOffice.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        public CustomerController(IUnitOfWork unitOfWork, IRepository<Customer> customerRepository,
            IWebHostEnvironment hostEnvironment,IConfiguration configuration)
        {
            _UnitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            webHostEnvironment = hostEnvironment;
            _configuration = configuration;

        }

        public CustomerModel FillModel(CustomerModel model)
        {
            return model;
        }

        public IActionResult Index()
        {

            var model = _customerRepository.Table.OrderByDescending(a => a.Id).Select(c => c.ToModel());
            ViewBag.activePage = "العملاء";
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _customerRepository.Table.OrderByDescending(a => a.Id).Where(a => a.MobileNumber.Contains(search) || a.FullName.Contains(search)).Select(c => c.ToModel());

            ViewBag.activePage = "العملاء";
            ViewBag.search = search;
            return View(model);
        }


        public IActionResult Create()
        {
            ViewBag.activePage = "العملاء";
            return View(FillModel(new CustomerModel()));
        }

        [HttpPost]
        public IActionResult Create(CustomerModel model, IFormFile formFile)
        {
            try
            {
                var exist_1 = _customerRepository.Table.FirstOrDefault(U => U.MobileNumber == model.MobileNumber);
                if (exist_1 != null)
                {
                    ErrorNotification($" هذا الرقم مسجل من قبل مع العميل  {exist_1.FullName}  و لا يمكن تكراره");
                    return View(FillModel(model));
                }
                if (ModelState.IsValid)
                {
                    var CustomerEntity = model.ToEntity();
                    _UnitOfWork.CustomerRepository.Insert(CustomerEntity);
                    SuccessNotification("تم اضافة السجل بنجاح");
                    _UnitOfWork.Save();
                   
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                //ErrorNotification(e.Message);
                ErrorNotification(e.InnerException.ToString());
                return View(FillModel(model));
            }
            return View(FillModel(model));
        }

        public IActionResult Edit(int id)
        {

            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "العملاء";
            return View(FillModel(customer.ToModel()));
        }

        [HttpPost]
        public IActionResult Edit(CustomerModel model, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.CustomerRepository.Update(model.ToEntity());
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
            Customer Customer = _customerRepository.GetById(id);

            if (Customer == null)
                return Json("السجل غير معرف");

            _UnitOfWork.CustomerRepository.Delete(Customer);
            _UnitOfWork.Save();
            return Json(1);
        }
    }
}
