using MazraeatiBackOffice.Core;

namespace MazraeatiBackOffice.Configuration
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext Context;
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
            IRepository<TripPriceList> tripPriceListRepository
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
        }
       
        public void Dispose()
        {
            Context.Dispose();
        }

        public int Save()
        {
            return Context.SaveChanges();
        }
    }
}
