using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using MazraeatiBackOffice.Models.LoyaltyPoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
namespace MazraeatiBackOffice.Controllers.LoyaltyPoints
{

    public class LoyaltyTransactionsController : BaseController
    {
        private readonly IUnitOfWork _UnitOfWork;
        private HttpContext httpContext;

        public LoyaltyTransactionsController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // ============================================================
        // INDEX - عرض جميع حركات النقاط مع فلتر
        // ============================================================
        public IActionResult Index(string search, int? customerId, DateTime? fromDate, DateTime? toDate, int? transactionType)
        {
            ViewBag.activePage = "حركات النقاط";

            var query = _UnitOfWork.LoyaltyTransactionRepository.Table
                .Include(t => t.Customer)
                .Include(t => t.CreatedByUser)
                .OrderByDescending(t => t.TransactionDate)
                .AsQueryable();

            // فلتر حسب العميل
            if (customerId.HasValue && customerId.Value > 0)
            {
                query = query.Where(t => t.CustomerId == customerId.Value);
            }

            // فلتر حسب التاريخ
            if (fromDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= toDate.Value);
            }

            // فلتر حسب نوع الحركة
            if (transactionType.HasValue && transactionType.Value > 0)
            {
                query = query.Where(t => (int)t.TransactionType == transactionType.Value);
            }

            // فلتر البحث
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    t.Description.Contains(search) ||
                    t.Customer.FullName.Contains(search) ||
                    t.ReferenceType.Contains(search));
            }

            var model = query.ToList();

            // ===== الإحصائيات =====
            ViewBag.TotalTransactions = model.Count();
            ViewBag.TotalEarnPoints = model.Where(t => t.Points > 0).Sum(t => t.Points);
            ViewBag.TotalRedeemPoints = model.Where(t => t.Points < 0).Sum(t => Math.Abs(t.Points));

            // ===== للفلاتر =====
            ViewBag.Customers = _UnitOfWork.CustomerRepository.Table
                .OrderBy(c => c.FullName)
                .ToList();

            ViewBag.TransactionTypes = Enum.GetValues(typeof(TransactionTypeEnum))
                .Cast<TransactionTypeEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.GetDisplayName()
                })
                .ToList();

            ViewBag.SelectedCustomerId = customerId;
            ViewBag.SelectedTransactionType = transactionType;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(model);
        }

        // ============================================================
        // INDEX - POST (بحث)
        // ============================================================
        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var search = form["search"];
            var customerId = string.IsNullOrEmpty(form["customerId"]) ? (int?)null : int.Parse(form["customerId"]);
            var fromDate = string.IsNullOrEmpty(form["fromDate"]) ? (DateTime?)null : DateTime.Parse(form["fromDate"]);
            var toDate = string.IsNullOrEmpty(form["toDate"]) ? (DateTime?)null : DateTime.Parse(form["toDate"]);
            var transactionType = string.IsNullOrEmpty(form["transactionType"]) ? (int?)null : int.Parse(form["transactionType"]);

            return RedirectToAction("Index", new
            {
                search = search,
                customerId = customerId,
                fromDate = fromDate,
                toDate = toDate,
                transactionType = transactionType
            });
        }

        // ============================================================
        // CUSTOMER TRANSACTIONS - حركات عميل معين
        // ============================================================
        public IActionResult CustomerTransactions(int customerId)
        {
            ViewBag.activePage = "حركات النقاط";

            var customer = _UnitOfWork.CustomerRepository.GetById(customerId);
            if (customer == null)
                return RedirectToAction("Index");

            ViewBag.CustomerName = customer.FullName;
            ViewBag.CustomerId = customerId;

            // ===== جلب حساب العميل =====
            var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            if (account != null)
            {
                ViewBag.TotalPoints = account.TotalPoints;
                ViewBag.AvailablePoints = account.AvailablePoints;
                ViewBag.RedeemedPoints = account.RedeemedPoints;
                ViewBag.ExpiredPoints = account.ExpiredPoints;

                if (account.CurrentTierId.HasValue)
                {
                    var tier = _UnitOfWork.LoyaltyTierRepository.GetById(account.CurrentTierId.Value);
                    ViewBag.CurrentTier = tier?.NameAr ?? "برونز";
                    ViewBag.CurrentTierIcon = tier?.IconClass ?? "fa-medal";
                    ViewBag.DiscountPercent = tier?.DiscountPercent ?? 0;
                }
                else
                {
                    ViewBag.CurrentTier = "برونز";
                    ViewBag.CurrentTierIcon = "fa-medal";
                    ViewBag.DiscountPercent = 0;
                }
            }
            else
            {
                ViewBag.TotalPoints = 0;
                ViewBag.AvailablePoints = 0;
                ViewBag.RedeemedPoints = 0;
                ViewBag.ExpiredPoints = 0;
                ViewBag.CurrentTier = "برونز";
                ViewBag.CurrentTierIcon = "fa-medal";
                ViewBag.DiscountPercent = 0;
            }

            // ===== جلب الحركات =====
            var model = _UnitOfWork.LoyaltyTransactionRepository.Table
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            return View(model);
        }

        // ============================================================
        // GET CUSTOMER POINTS (AJAX)
        // ============================================================
        [HttpGet]
        public IActionResult GetCustomerPoints(int customerId)
        {
            var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            if (account == null)
                return Json(new { points = 0, totalPoints = 0, tier = "برونز", discountPercent = 0 });

            var tier = _UnitOfWork.LoyaltyTierRepository.GetById(account.CurrentTierId ?? 0);

            return Json(new
            {
                points = account.AvailablePoints,
                totalPoints = account.TotalPoints,
                tier = tier?.NameAr ?? "برونز",
                tierIcon = tier?.IconClass ?? "fa-medal",
                discountPercent = tier?.DiscountPercent ?? 0
            });
        }

        // ============================================================
        // ADJUST POINTS (تعديل يدوي للنقاط)
        // ============================================================
        [HttpPost]
        public IActionResult AdjustPoints(int customerId, int points, string reason)
        {
            try
            {
                if (points == 0)
                    return Json(new { success = false, message = "يجب إدخال عدد النقاط" });

                if (string.IsNullOrEmpty(reason))
                    return Json(new { success = false, message = "يجب إدخال سبب التعديل" });

                var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                    .FirstOrDefault(a => a.CustomerId == customerId);

                if (account == null)
                {
                    account = new CustomerLoyaltyAccount
                    {
                        CustomerId = customerId,
                        TotalPoints = 0,
                        AvailablePoints = 0,
                        ExpireDate = DateTime.Now.AddMonths(12)
                    };
                    _UnitOfWork.CustomerLoyaltyAccountRepository.Insert(account);
                    _UnitOfWork.Save();
                }

                // تحديث الرصيد
                if (points > 0)
                {
                    account.TotalPoints += points;
                    account.AvailablePoints += points;
                }
                else
                {
                    var absPoints = Math.Abs(points);
                    if (account.AvailablePoints < absPoints)
                        return Json(new { success = false, message = "رصيد النقاط غير كافٍ" });

                    account.AvailablePoints -= absPoints;
                    account.RedeemedPoints += absPoints;
                }

                account.UpdatedDate = DateTime.Now;
                _UnitOfWork.CustomerLoyaltyAccountRepository.Update(account);
                _UnitOfWork.Save();
                // تسجيل الحركة
                var transaction = new LoyaltyTransaction
                {
                    CustomerId = customerId,
                    TransactionType = TransactionTypeEnum.Adjust,
                    Points = points,
                    Description = $"تعديل يدوي: {reason}",
                    CreatedBy = GetCurrentUserIdFromSession()
                };

                _UnitOfWork.LoyaltyTransactionRepository.Insert(transaction);
                _UnitOfWork.Save();

                // تحديث المستوى
                //private LoyaltyService ll=new LoyaltyService();
                var loyaltyService = new LoyaltyService(_UnitOfWork, httpContext);
                loyaltyService.UpdateCustomerTierAsync(customerId);

                return Json(new { success = true, message = "تم تعديل النقاط بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ============================================================
        // GET CUSTOMER ACCOUNT (عرض حساب العميل)
        // ============================================================
        public IActionResult CustomerAccount(int customerId)
        {
            ViewBag.activePage = "حسابات العملاء";

            var customer = _UnitOfWork.CustomerRepository.GetById(customerId);
            if (customer == null)
                return RedirectToAction("Index", "Customers");

            var account = _UnitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            var model = new CustomerLoyaltyAccountModel
            {
                CustomerId = customerId,
                CustomerName = customer.FullName,
                TotalPoints = account?.TotalPoints ?? 0,
                AvailablePoints = account?.AvailablePoints ?? 0,
                RedeemedPoints = account?.RedeemedPoints ?? 0,
                ExpiredPoints = account?.ExpiredPoints ?? 0,
                CurrentTierId = account?.CurrentTierId,
                ExpireDate = account?.ExpireDate,
                CreatedDate = account?.CreatedDate ?? DateTime.Now,
                UpdatedDate = account?.UpdatedDate
            };

            if (account?.CurrentTierId.HasValue == true)
            {
                var tier = _UnitOfWork.LoyaltyTierRepository.GetById(account.CurrentTierId.Value);
                model.CurrentTierName = tier?.NameAr ?? "برونز";
            }
            else
            {
                model.CurrentTierName = "برونز";
            }

            return View(model);
        }

        
    }


}
