using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class SettingController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IRepository<Setting> _settingRepository;
        public SettingController(IRepository<Setting> settingRepository, IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var model = _settingRepository.Table.Where(a => a.Name != "setting.otp.length" && a.Name != "setting.otp.expiry.time").Select(a => a.ToModel());
            ViewBag.activePage = "متغيرات النظام";
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index");

            var model = _settingRepository.Table.Where(a => a.Name != "setting.otp.length"  && a.Name != "setting.otp.expiry.time" && (a.DisplayName.Contains(search) || a.Value.Contains(search))).Select(a => a.ToModel());

            ViewBag.search = search;
            ViewBag.activePage = "متغيرات النظام";
            return View(model);
        }
        public IActionResult Edit(int id)
        {

            Setting Setting = _settingRepository.GetById(id);

            if (Setting == null)
                return RedirectToAction("Index");

            ViewBag.activePage = "متغيرات النظام";
            return View(Setting.ToModel());
        }
        [HttpPost]
        public IActionResult Edit(SettingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _UnitOfWork.SettingRepository.Update(model.ToEntity());
                    _UnitOfWork.Save();
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ErrorNotification(e.Message);
            }
            return View(model);
        }
    }
}
