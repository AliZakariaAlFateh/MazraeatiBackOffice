using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext Context;
        private IDbContextTransaction _transaction;
        public IRepository<Farmer> FarmerRepository { get; }
        public IRepository<FarmerExtraFeatureType> FarmerExtraFeatureTypeRepository { get; }
        public IRepository<FarmerPriceList> FarmerPriceListRepository { get; }
        public IRepository<FarmerImage> FarmerImageRepository { get; }
        public IRepository<FarmerVideo> FarmerVideoRepository { get; }
        public IRepository<Setting> SettingRepository { get; }
        public IRepository<ImageSlider> ImageSliderRepository { get; }
        public IRepository<Onboarding> OnboardingRepository { get; }
        public IRepository<Country> CountryRepository { get; }
        public IRepository<City> CityRepository { get; }
        public IRepository<TripSection> TripSectionRepository { get; }
        public IRepository<TripProfile> TripProfileRepository { get; }
        public IRepository<Trip> TripRepository { get; }
        public IRepository<TripPriceList> TripPriceListRepository { get; }
        public IRepository<TripExtraFeatureType> TripExtraFeatureTypeRepository { get; }
        public IRepository<TripImage> TripImageRepository { get; }
        public IRepository<TripVideo> TripVideoRepository { get; }
        public IRepository<FarmerUser> FarmerUserRepository { get; }
        public IRepository<FarmerReservation> FarmerReservationRepository { get; }
        public IRepository<FarmerFeedback> FarmerFeedbackRepository { get; }
        public IRepository<CustomerBlackList> CustomerBlackListRepository { get; }
        public IRepository<FarmerBlackList> FarmerBlackListRepository { get; }
        public IRepository<Lookup> LookupRepository { get; }
        public IRepository<LookupValue> LookupValueRepository { get; }
        public IRepository<terms> TermsRepository { get; }
        public IRepository<Regions> RegionRepository { get; }
        public IRepository<AppUser> UserRepository { get; }
        public IRepository<AppUserBlackList> AppUserBlackListRepository { get; }
        public IRepository<CommonQuestions> CommonQuestionsRepository { get; }
        public IRepository<CommonQuestionsVisitors> CommonQuestionsVisitorsRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }

        #region Sport Deparments
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

        //public IRepository<SportVideoBlackList> CustomerRepository { get; }
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




        public UnitOfWork(DataContext context, IRepository<Farmer> farmerRepository,
            IRepository<FarmerExtraFeatureType> farmerExtraFeatureTypeRepository,
            IRepository<FarmerPriceList> farmerPriceListRepository,
            IRepository<FarmerImage> farmerImageRepository,
            IRepository<FarmerVideo> farmerVideoRepository,
            IRepository<Setting> settingRepository,
            IRepository<ImageSlider> imageSliderRepository,
            IRepository<Onboarding> onboardingRepository,
            IRepository<Country> countryRepository,
            IRepository<City> cityRepository,
            IRepository<TripSection> tripSectionRepository,
            IRepository<TripProfile> tripProfileRepository,
            IRepository<Trip> tripRepository,
            IRepository<TripExtraFeatureType> tripExtraFeatureTypeRepository,
            IRepository<TripImage> tripImageRepository,
            IRepository<TripVideo> tripVideoRepository,
            IRepository<FarmerUser> farmerUserRepository,
            IRepository<FarmerReservation> farmerReservationRepository,
            IRepository<FarmerFeedback> farmerFeedbackRepository,
            IRepository<CustomerBlackList> customerBlackListRepository,
            IRepository<FarmerBlackList> farmerBlackListRepository,
            IRepository<Lookup> lookupRepository,
            IRepository<LookupValue> lookupValueRepository,
            IRepository<TripPriceList> tripPriceListRepository,
            IRepository<terms> termsRepository,
            IRepository<Regions> regionRepository,
            IRepository<AppUser> userRepository,
            IRepository<AppUserBlackList> appUserBlackListRepository,
            IRepository<CommonQuestions> commonQuestionsRepository,
            IRepository<CommonQuestionsVisitors> commonQuestionsVisitorsRepository,
            IRepository<Customer> customerRepository,
            IRepository<SportType> sportTypeRepository,
            IRepository<Sport> sportRepository,
            IRepository<AdditionalService> additionalServiceRepository,
            IRepository<SportAdditionalService> sportAdditionalServiceRepository,
            IRepository<SportFeature> sportFeatureRepository,
            IRepository<SportSportFeature> sportSportFeatureRepository,
            IRepository<GeneralFacility> generalFacilityRepository,
            IRepository<SportGeneralFacility> sportGeneralFacilityRepository,
            IRepository<SportPriceList> sportPriceListRepository,
            IRepository<SportImage> sportImageRepository,
            IRepository<SportVideo> sportVideoRepository,
            IRepository<SafetyFeature> safetyFeatureRepository,
            IRepository<SportSafetyFeature> sportSafetyFeatureRepository,
            IRepository<SportReservation> sportReservationRepository,
            IRepository<SportPropertyValue> sportPropertyValueRepository,
            IRepository<SportPropertyOption> sportPropertyOptionRepository,
            IRepository<SportPropertyTemplate> sportPropertyTemplateRepository,
            IRepository<CustomerLoyaltyAccount> customerLoyaltyAccountRepository,
            IRepository<LoyaltyActivityType> loyaltyActivityTypeRepository,
            IRepository<LoyaltyBookingActivity> loyaltyBookingActivityRepository,
            IRepository<LoyaltyPointRuleFarm> loyaltyPointRuleFarmRepository,
            IRepository<LoyaltyPointRuleGeneral> loyaltyPointRuleGeneralRepository,
            IRepository<LoyaltyPointRuleSport> loyaltyPointRuleSportRepository,
            IRepository<LoyaltyTier> loyaltyTierRepository,
            IRepository<LoyaltyTransaction> loyaltyTransactionRepository,
            IRepository<ReservationLoyaltyDiscount> reservationLoyaltyDiscountRepository,
            IRepository<LoyaltyPointRule> loyaltyPointRuleRepository,
            IRepository<LoyaltyRedeemRule> loyaltyRedeemRuleRepository

            )
        {
            Context = context;
            FarmerRepository = farmerRepository;
            FarmerExtraFeatureTypeRepository = farmerExtraFeatureTypeRepository;
            FarmerPriceListRepository = farmerPriceListRepository;
            FarmerImageRepository = farmerImageRepository;
            FarmerVideoRepository = farmerVideoRepository;
            SettingRepository = settingRepository;
            ImageSliderRepository = imageSliderRepository;
            OnboardingRepository = onboardingRepository;
            CountryRepository = countryRepository;
            CityRepository = cityRepository;
            TripSectionRepository = tripSectionRepository;
            TripProfileRepository = tripProfileRepository;
            TripRepository = tripRepository;
            TripExtraFeatureTypeRepository = tripExtraFeatureTypeRepository;
            TripImageRepository = tripImageRepository;
            TripVideoRepository = tripVideoRepository;
            FarmerUserRepository = farmerUserRepository;
            FarmerReservationRepository = farmerReservationRepository;
            FarmerFeedbackRepository = farmerFeedbackRepository;
            CustomerBlackListRepository = customerBlackListRepository;
            FarmerBlackListRepository = farmerBlackListRepository;
            LookupRepository = lookupRepository;
            LookupValueRepository = lookupValueRepository;
            TripPriceListRepository = tripPriceListRepository;
            TermsRepository = termsRepository;
            RegionRepository = regionRepository;
            UserRepository = userRepository;
            AppUserBlackListRepository = appUserBlackListRepository;
            CommonQuestionsRepository = commonQuestionsRepository;
            CommonQuestionsVisitorsRepository = commonQuestionsVisitorsRepository;
            CustomerRepository = customerRepository;
            SportTypeRepository = sportTypeRepository;
            SportRepository = sportRepository;
            AdditionalServiceRepository = additionalServiceRepository;
            SportAdditionalServiceRepository = sportAdditionalServiceRepository;
            SportFeatureRepository = sportFeatureRepository;
            SportSportFeatureRepository = sportSportFeatureRepository;
            GeneralFacilityRepository = generalFacilityRepository;
            SportGeneralFacilityRepository = sportGeneralFacilityRepository;
            SportPriceListRepository = sportPriceListRepository;
            SportImageRepository = sportImageRepository;
            SportVideoRepository = sportVideoRepository;
            SafetyFeatureRepository = safetyFeatureRepository;
            SportSafetyFeatureRepository = sportSafetyFeatureRepository;
            SportReservationRepository = sportReservationRepository;
            SportPropertyValueRepository = sportPropertyValueRepository;
            SportPropertyOptionRepository = sportPropertyOptionRepository;
            SportPropertyTemplateRepository = sportPropertyTemplateRepository;
            CustomerLoyaltyAccountRepository = customerLoyaltyAccountRepository;
            LoyaltyActivityTypeRepository = loyaltyActivityTypeRepository;
            LoyaltyBookingActivityRepository = loyaltyBookingActivityRepository;
            LoyaltyPointRuleFarmRepository = loyaltyPointRuleFarmRepository;
            LoyaltyPointRuleGeneralRepository = loyaltyPointRuleGeneralRepository;
            LoyaltyPointRuleSportRepository = loyaltyPointRuleSportRepository;
            LoyaltyTierRepository = loyaltyTierRepository;
            LoyaltyTransactionRepository = loyaltyTransactionRepository;
            ReservationLoyaltyDiscountRepository = reservationLoyaltyDiscountRepository;
            LoyaltyPointRuleRepository = loyaltyPointRuleRepository;
            LoyaltyRedeemRuleRepository = loyaltyRedeemRuleRepository;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Context.Database.BeginTransaction(isolationLevel);
            return _transaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = await Context.Database.BeginTransactionAsync(isolationLevel);
            return _transaction;
        }

    }
}
