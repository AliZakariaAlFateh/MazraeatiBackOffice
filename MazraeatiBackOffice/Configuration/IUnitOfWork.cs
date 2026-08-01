using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Farmer> FarmerRepository { get; }
        IRepository<FarmerExtraFeatureType> FarmerExtraFeatureTypeRepository { get; }
        IRepository<FarmerPriceList> FarmerPriceListRepository { get; }
        IRepository<FarmerImage> FarmerImageRepository { get; }
        IRepository<FarmerVideo> FarmerVideoRepository { get; }
        IRepository<Setting> SettingRepository { get; }
        IRepository<ImageSlider> ImageSliderRepository { get; }
        IRepository<Onboarding> OnboardingRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IRepository<City> CityRepository { get; }

        IRepository<TripSection> TripSectionRepository { get; }
        IRepository<TripProfile> TripProfileRepository { get; }
        IRepository<Trip> TripRepository { get; }
        IRepository<TripPriceList> TripPriceListRepository { get; }
        IRepository<TripExtraFeatureType> TripExtraFeatureTypeRepository { get; }
        IRepository<TripImage> TripImageRepository { get; }
        IRepository<TripVideo> TripVideoRepository { get; }
        IRepository<FarmerUser> FarmerUserRepository { get; }
        IRepository<FarmerReservation> FarmerReservationRepository { get; }
        IRepository<FarmerFeedback> FarmerFeedbackRepository { get; }
        IRepository<CustomerBlackList> CustomerBlackListRepository { get; }
        IRepository<FarmerBlackList> FarmerBlackListRepository { get; }
        IRepository<Lookup> LookupRepository { get; }
        IRepository<LookupValue> LookupValueRepository { get; }
        IRepository<terms> TermsRepository { get; }
        IRepository<Regions> RegionRepository { get; }
        IRepository<AppUser> UserRepository { get; }
        IRepository<AppUserBlackList> AppUserBlackListRepository { get; }
        IRepository<CommonQuestions> CommonQuestionsRepository { get; }
        IRepository<CommonQuestionsVisitors> CommonQuestionsVisitorsRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }

        #region Sport Departments ...
        public IRepository<SportType> SportTypeRepository { get; }
        public IRepository<Sport> SportRepository { get; }
        public IRepository<AdditionalService> AdditionalServiceRepository { get; }
        public IRepository<SportAdditionalService> SportAdditionalServiceRepository { get; }
        public IRepository<SportFeature> SportFeatureRepository { get; }
        public IRepository<SportSportFeature> SportSportFeatureRepository { get; }
        public IRepository<GeneralFacility> GeneralFacilityRepository { get; }
        public IRepository<SportGeneralFacility> SportGeneralFacilityRepository { get; }
        public IRepository<SportPriceList> SportPriceListRepository { get; }
        public IRepository<SportImage> SportImageRepository { get; }
        public IRepository<SportVideo> SportVideoRepository { get; }
        public IRepository<SafetyFeature> SafetyFeatureRepository { get; }
        public IRepository<SportSafetyFeature> SportSafetyFeatureRepository { get; }
        public IRepository<SportReservation> SportReservationRepository { get; }
        //properties
        public IRepository<SportPropertyValue> SportPropertyValueRepository { get; }
        public IRepository<SportPropertyOption> SportPropertyOptionRepository { get; }
        public IRepository<SportPropertyTemplate> SportPropertyTemplateRepository { get; }
        #endregion



        #region LoyaltyPoints
        public IRepository<CustomerLoyaltyAccount> CustomerLoyaltyAccountRepository { get; set; }
        public IRepository<LoyaltyActivityType> LoyaltyActivityTypeRepository { get; set; }
        public IRepository<LoyaltyBookingActivity> LoyaltyBookingActivityRepository { get; set; }
        public IRepository<LoyaltyPointRuleFarm> LoyaltyPointRuleFarmRepository { get; set; }
        public IRepository<LoyaltyPointRuleGeneral> LoyaltyPointRuleGeneralRepository { get; set; }
        public IRepository<LoyaltyPointRuleSport> LoyaltyPointRuleSportRepository { get; set; }
        public IRepository<LoyaltyRedeemRule> LoyaltyRedeemRuleRepository { get; set; }
        public IRepository<LoyaltyTier> LoyaltyTierRepository { get; set; }
        public IRepository<LoyaltyTransaction> LoyaltyTransactionRepository { get; set; }
        public IRepository<ReservationLoyaltyDiscount> ReservationLoyaltyDiscountRepository { get; set; }
        public IRepository<LoyaltyPointRule> LoyaltyPointRuleRepository { get; set; }
        #endregion
        int Save();
        // ✅ إضافة BeginTransaction مع IsolationLevel
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        // ✅ إضافة النسخة الـ Async
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
