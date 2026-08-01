using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Extenstion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{
    public class CustomerLoyaltyAccountsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerLoyaltyAccountsController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX - عرض جميع حسابات العملاء
        // ============================================================
        public IActionResult Index(string search, int? tierId)
        {
            ViewBag.activePage = "حسابات العملاء";

            var query = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                .Include(a => a.Customer)
                .Include(a => a.CurrentTier)
                .AsQueryable();

            // فلتر حسب المستوى
            if (tierId.HasValue && tierId.Value > 0)
            {
                query = query.Where(a => a.CurrentTierId == tierId.Value);
            }

            // فلتر البحث
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.Customer.FullName.Contains(search) ||
                    a.Customer.MobileNumber.Contains(search));
            }

            var accounts = query.OrderByDescending(a => a.AvailablePoints).ToList();

            var model = accounts.Select(a => a.ToModel()).ToList();

            // ربط أسماء العملاء والمستويات
            foreach (var item in model)
            {
                var customer = _UnitOfWork.CustomerRepository.GetById(item.CustomerId);
                item.CustomerName = customer?.FullName ?? "";

                if (item.CurrentTierId.HasValue)
                {
                    var tier = _UnitOfWork.LoyaltyTierRepository.GetById(item.CurrentTierId.Value);
                    item.CurrentTierName = tier?.NameAr ?? "برونز";
                }
                else
                {
                    item.CurrentTierName = "برونز";
                }
            }

            // ===== الإحصائيات =====
            ViewBag.TotalAccounts = model.Count();
            ViewBag.TotalPoints = model.Sum(a => a.AvailablePoints);
            ViewBag.ActiveAccounts = model.Count(a => a.AvailablePoints > 0);

            // ===== للفلاتر =====
            ViewBag.Tiers = _UnitOfWork.LoyaltyTierRepository.Table
                .Where(t => t.IsActive == true)
                .OrderBy(t => t.MinPoints)
                .ToList();

            ViewBag.SelectedTierId = tierId;

            return View(model);
        }

        // ============================================================
        // INDEX - POST
        // ============================================================
        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var search = form["search"];
            var tierId = string.IsNullOrEmpty(form["tierId"]) ? (int?)null : int.Parse(form["tierId"]);

            return RedirectToAction("Index", new
            {
                search = search,
                tierId = tierId
            });
        }

        // ============================================================
        // GET CUSTOMER ACCOUNT DETAILS (AJAX)
        // ============================================================
        [HttpGet]
        public IActionResult GetCustomerAccountDetails(int customerId)
        {
            var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            if (account == null)
                return Json(new { success = false, message = "الحساب غير موجود" });

            var customer = _UnitOfWork.CustomerRepository.GetById(customerId);
            var tier = account.CurrentTierId.HasValue
                ? _UnitOfWork.LoyaltyTierRepository.GetById(account.CurrentTierId.Value)
                : null;

            return Json(new
            {
                success = true,
                customerName = customer?.FullName ?? "",
                totalPoints = account.TotalPoints,
                availablePoints = account.AvailablePoints,
                redeemedPoints = account.RedeemedPoints,
                expiredPoints = account.ExpiredPoints,
                tier = tier?.NameAr ?? "برونز",
                tierIcon = tier?.IconClass ?? "fa-medal",
                discountPercent = tier?.DiscountPercent ?? 0,
                expireDate = account.ExpireDate?.ToString("yyyy-MM-dd")
            });
        }
    }
}
