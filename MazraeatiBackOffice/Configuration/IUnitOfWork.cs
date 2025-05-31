using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MazraeatiBackOffice.Core;

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


        int Save();
    }
}
