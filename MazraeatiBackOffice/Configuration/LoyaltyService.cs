using MazraeatiBackOffice.Core.LoyaltyPoints;
using MazraeatiBackOffice.Dto.LoyaltyPoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class LoyaltyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpContext _httpContext;
        public LoyaltyService(IUnitOfWork unitOfWork, HttpContext httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        // ============================================================
        // جلب نقاط العميل
        // ============================================================
        public async Task<int> GetCustomerPointsAsync(int customerId)
        {
            var account = await _unitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefaultAsync(a => a.CustomerId == customerId);

            return account?.AvailablePoints ?? 0;
        }

        // ============================================================
        // جلب معلومات الولاء للعميل
        // ============================================================
        public async Task<CustomerLoyaltyInfo> GetCustomerLoyaltyInfoAsync(int customerId)
        {
            var account = await _unitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefaultAsync(a => a.CustomerId == customerId);

            if (account == null)
            {
                return new CustomerLoyaltyInfo
                {
                    CustomerId = customerId,
                    AvailablePoints = 0,
                    TotalPoints = 0,
                    TierName = "برونز"
                };
            }

            var tier = await _unitOfWork.LoyaltyTierRepository.Table
                .FirstOrDefaultAsync(t => t.Id == account.CurrentTierId);

            return new CustomerLoyaltyInfo
            {
                CustomerId = customerId,
                AvailablePoints = account.AvailablePoints,
                TotalPoints = account.TotalPoints,
                TierName = tier?.NameAr ?? "برونز",
                TierIcon = tier?.IconClass ?? "fa-medal",
                DiscountPercent = tier?.DiscountPercent ?? 0
            };
        }

        // ============================================================
        // حساب النقاط المستحقة (مع الأولوية)
        // ============================================================
        public int CalculatePoints(int activityTypeId, string referenceType, int? referenceId = null)
        {
            // ===== 1. قاعدة خاصة بعقار معين =====
            if (referenceId.HasValue && referenceId.Value > 0)
            {
                var specificRule = _unitOfWork.LoyaltyPointRuleRepository.Table
                    .FirstOrDefault(r => r.ActivityTypeId == activityTypeId &&
                                         r.ReferenceType == referenceType &&
                                         r.ReferenceId == referenceId.Value &&
                                         r.IsActive == true);

                if (specificRule != null)
                    return specificRule.Points;
            }

            // ===== 2. قاعدة عامة (IsDefault = true) =====
            var defaultRule = _unitOfWork.LoyaltyPointRuleRepository.Table
                .FirstOrDefault(r => r.ActivityTypeId == activityTypeId &&
                                     r.ReferenceType == referenceType &&
                                     r.ReferenceId == null &&
                                     r.IsDefault == true &&
                                     r.IsActive == true);

            if (defaultRule != null)
                return defaultRule.Points;

            return 0;
        }

        // ============================================================
        // إضافة نقاط للعميل (عند تأكيد الحجز)
        // ============================================================
        public async Task AddPointsAsync(int customerId, int activityTypeId, string referenceType, int? referenceId, int reservationId, string reservationType)
        {
            //لو العميل مش مسجل عندي على السيستم مش هيتحسب له نقاط
            if (customerId == null  || customerId <= 0) return;
            
            var points = CalculatePoints(activityTypeId, referenceType, referenceId);
            if (points <= 0) return;

            var activity = _unitOfWork.LoyaltyActivityTypeRepository.GetById(activityTypeId);
            var activityName = activity?.NameAr ?? "نشاط";

            string propertyName = "";
            if (referenceType == "Farmer" && referenceId.HasValue)
            {
                var farm = _unitOfWork.FarmerRepository.GetById(referenceId.Value);
                propertyName = farm?.Name ?? "";
            }
            else if (referenceType == "Sports" && referenceId.HasValue)
            {
                var sport = _unitOfWork.SportRepository.GetById(referenceId.Value);
                propertyName = sport?.NameAr ?? "";
            }

            var account = _unitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);
            //انشاء حساب للعميل خاص بالنقاط لمعرفت مستواه و ما الى ذلك و مجموع النقاط و النقاط الخالية
            if (account == null)
            {
                account = new CustomerLoyaltyAccount
                {
                    CustomerId = customerId,
                    TotalPoints = 0,
                    AvailablePoints = 0,
                    ExpireDate = DateTime.Now.AddMonths(12)
                };
                _unitOfWork.CustomerLoyaltyAccountRepository.Insert(account);
                _unitOfWork.Save();
            }

            account.TotalPoints += points;
            account.AvailablePoints += points;
            account.UpdatedDate = DateTime.Now;

            _unitOfWork.CustomerLoyaltyAccountRepository.Update(account);
            _unitOfWork.Save();
            var transaction = new LoyaltyTransaction
            {
                CustomerId = customerId,
                TransactionType = TransactionTypeEnum.Earn,
                Points = points,
                ReferenceId = reservationId,
                ReferenceType = reservationType,
                Description = $"حجز {activityName}" + (!string.IsNullOrEmpty(propertyName) ? $" - {propertyName}" : "") + $" (+{points} نقطة)",
                ExpireDate = DateTime.Now.AddMonths(12),
                CreatedBy = GetCurrentAdminId()
            };
            //This not add any thing in the DB
            _unitOfWork.LoyaltyTransactionRepository.Insert(transaction);
            _unitOfWork.Save();

            await UpdateCustomerTierAsync(customerId);
        }

        // ============================================================
        // خصم نقاط من العميل (عند استخدام الخصم)
        // ============================================================
        //public async Task<bool> RedeemPointsAsync(int customerId, int points, int reservationId, string reservationType)
        //{
        //    var account = _unitOfWork.CustomerLoyaltyAccountRepository.Table
        //        .FirstOrDefault(a => a.CustomerId == customerId);

        //    if (account == null || account.AvailablePoints < points)
        //        return false;

        //    var redeemRule = _unitOfWork.LoyaltyRedeemRuleRepository.Table
        //        .Where(r => r.IsActive == true && r.Points <= points)
        //        .OrderByDescending(r => r.Points)
        //        .FirstOrDefault();

        //    if (redeemRule == null)
        //        return false;

        //    var discountAmount = (points / redeemRule.Points) * redeemRule.DiscountAmount;

        //    account.AvailablePoints -= points;
        //    account.RedeemedPoints += points;
        //    account.UpdatedDate = DateTime.Now;

        //    _unitOfWork.CustomerLoyaltyAccountRepository.Update(account);

        //    var transaction = new LoyaltyTransaction
        //    {
        //        CustomerId = customerId,
        //        TransactionType = TransactionTypeEnum.Redeem,
        //        Points = -points,
        //        ReferenceId = reservationId,
        //        ReferenceType = reservationType,
        //        Description = $"خصم نقاط من الحجز {reservationId} (-{points} نقطة) - خصم {discountAmount} دينار",
        //        ExpireDate = DateTime.Now.AddMonths(12)
        //    };

        //    _unitOfWork.LoyaltyTransactionRepository.Insert(transaction);

        //    var discount = new ReservationLoyaltyDiscount
        //    {
        //        ReservationId = reservationId,
        //        ReservationType = reservationType,
        //        CustomerId = customerId,
        //        PointsUsed = points,
        //        DiscountAmount = discountAmount
        //    };

        //    _unitOfWork.ReservationLoyaltyDiscountRepository.Insert(discount);
        //    _unitOfWork.Save();

        //    await UpdateCustomerTierAsync(customerId);

        //    return true;
        //}

        // ============================================================
        // تحديث مستوى العميل
        // ============================================================
        public async Task UpdateCustomerTierAsync(int customerId)
        {
            var account = _unitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            if (account == null) return;

            var tier = _unitOfWork.LoyaltyTierRepository.Table
                .Where(t => t.IsActive == true &&
                            t.MinPoints <= account.TotalPoints &&
                            (t.MaxPoints == null || t.MaxPoints >= account.TotalPoints))
                .OrderByDescending(t => t.MinPoints)
                .FirstOrDefault();

            if (tier != null && account.CurrentTierId != tier.Id)
            {
                account.CurrentTierId = tier.Id;
                account.UpdatedDate = DateTime.Now;

                _unitOfWork.CustomerLoyaltyAccountRepository.Update(account);
                _unitOfWork.Save();
            }
        }

        // ============================================================
        // إلغاء الحجز (استرجاع النقاط)
        // ============================================================
        public async Task ReversePointsOnCancellationAsync(int reservationId, string reservationType)
        {
            var transactions = _unitOfWork.LoyaltyTransactionRepository.Table
                .Where(t => t.ReferenceId == reservationId &&
                            t.ReferenceType == reservationType &&
                            t.TransactionType == TransactionTypeEnum.Earn &&
                            t.Points > 0)
                .ToList();

            foreach (var transaction in transactions)
            {
                var reverseTransaction = new LoyaltyTransaction
                {
                    CustomerId = transaction.CustomerId,
                    TransactionType = TransactionTypeEnum.Adjust,
                    Points = -transaction.Points,
                    ReferenceId = reservationId,
                    ReferenceType = reservationType,
                    Description = $"إلغاء حجز - استرجاع {transaction.Points} نقطة",
                    ExpireDate = DateTime.Now.AddMonths(12),
                    CreatedBy = GetCurrentAdminId()
                };

                _unitOfWork.LoyaltyTransactionRepository.Insert(reverseTransaction);

                var account = _unitOfWork.CustomerLoyaltyAccountRepository.Table
                    .FirstOrDefault(a => a.CustomerId == transaction.CustomerId);

                if (account != null)
                {
                    account.TotalPoints -= transaction.Points;
                    account.AvailablePoints -= transaction.Points;
                    account.UpdatedDate = DateTime.Now;
                    _unitOfWork.CustomerLoyaltyAccountRepository.Update(account);
                }
            }

            _unitOfWork.Save();
        }
        //private int GetCurrentAdminId()
        //{
        //    // لو عندك Session
        //    return HttpContext.Session.GetInt32("AdminId") ?? 0;

        //    // أو من الـ Claims
        //    // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    // return !string.IsNullOrEmpty(userId) ? int.Parse(userId) : 0;
        //}
        //protected int GetCurrentUserIdFromSession()
        //{
        //    return HttpContext.Session.GetInt32("UserId") ?? 0;
        //}
        // ============================================================
        // جلب معرف المستخدم من Session
        // ============================================================
        private int GetCurrentAdminId()
        {
            if (_httpContext == null) return 0;
            // جلب الـ UserId من Session
            var userId = _httpContext.Session.GetInt32("UserId");
            return userId ?? 0;
        }


        public async Task<bool> RedeemPointsAsync(int customerId, int points, int reservationId, string reservationType)
        {
            var account = _unitOfWork.CustomerLoyaltyAccountRepository.Table
                .FirstOrDefault(a => a.CustomerId == customerId);

            if (account == null || account.AvailablePoints < points)
                return false;

            // جلب قاعدة الصرف المناسبة
            var redeemRule = _unitOfWork.LoyaltyRedeemRuleRepository.Table
                .Where(r => r.IsActive == true && r.Points <= points)
                .OrderByDescending(r => r.Points)
                .FirstOrDefault();

            if (redeemRule == null)
                return false;

            //var discountAmount = (points / redeemRule.Points) * redeemRule.DiscountAmount;
            decimal ratio = (decimal)points / redeemRule.Points;
            decimal discountAmount = ratio * redeemRule.DiscountAmount;
            // تحديث رصيد العميل
            account.AvailablePoints -= points;
            account.RedeemedPoints += points;
            account.UpdatedDate = DateTime.Now;

            _unitOfWork.CustomerLoyaltyAccountRepository.Update(account);

            // تسجيل الحركة
            var transaction = new LoyaltyTransaction
            {
                CustomerId = customerId,
                TransactionType = TransactionTypeEnum.Redeem,
                Points = -points,
                ReferenceId = reservationId,
                ReferenceType = reservationType,
                Description = $"خصم نقاط من الحجز {reservationId} (-{points} نقطة) - خصم {discountAmount} دينار",
                ExpireDate = DateTime.Now.AddMonths(12),
                CreatedBy = GetCurrentAdminId()
            };

            _unitOfWork.LoyaltyTransactionRepository.Insert(transaction);

            // تسجيل الخصم في جدول ReservationLoyaltyDiscount
            var discount = new ReservationLoyaltyDiscount
            {
                ReservationId = reservationId,
                ReservationType = reservationType,
                CustomerId = customerId,
                PointsUsed = points,
                DiscountAmount = discountAmount
            };

            _unitOfWork.ReservationLoyaltyDiscountRepository.Insert(discount);
            _unitOfWork.Save();

            await UpdateCustomerTierAsync(customerId);

            return true;
        }

        // ============================================================
        // حساب نقاط الحجز حسب نوعه
        // ============================================================
        public int CalculateReservationEarnedPoints(int customerId, string bookingType, int referenceId)
        {
            try
            {
                // 1. جلب الأنشطة المرتبطة بنوع الحجز
                var bookingActivities = _unitOfWork.LoyaltyBookingActivityRepository
                    .Table
                    .Where(b => b.BookingType == bookingType && b.IsActive)
                    .Select(b => b.ActivityTypeId)
                    .ToList();

                if (!bookingActivities.Any())
                    return 0;

                int totalPoints = 0;

                // 2. لكل نشاط، جلب النقاط حسب القاعدة
                foreach (var activityTypeId in bookingActivities)
                {
                    // جلب القاعدة الخاصة (ReferenceId)
                    var specificRule = _unitOfWork.LoyaltyPointRuleRepository
                        .Table
                        .FirstOrDefault(r => r.ActivityTypeId == activityTypeId &&
                                             r.ReferenceType == bookingType &&
                                             r.ReferenceId == referenceId &&
                                             r.IsActive);

                    if (specificRule != null)
                    {
                        totalPoints += specificRule.Points;
                        continue;
                    }

                    // جلب القاعدة الافتراضية
                    var defaultRule = _unitOfWork.LoyaltyPointRuleRepository
                        .Table
                        .FirstOrDefault(r => r.ActivityTypeId == activityTypeId &&
                                             r.ReferenceType == bookingType &&
                                             r.ReferenceId == null &&
                                             r.IsDefault &&
                                             r.IsActive);

                    if (defaultRule != null)
                    {
                        totalPoints += defaultRule.Points;
                    }
                }

                return totalPoints;
            }
            catch
            {
                return 0;
            }
        }



    }
}
