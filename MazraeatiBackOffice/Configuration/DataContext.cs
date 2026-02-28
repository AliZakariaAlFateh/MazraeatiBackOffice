using Microsoft.EntityFrameworkCore;
using MazraeatiBackOffice.Core;

namespace MazraeatiBackOffice.Configuration
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<FarmerPriceList> FarmerPriceList { get; set; }
        public DbSet<FarmerExtraFeatureType> FarmerExtraFeatureType { get; set; }
        public DbSet<FarmerImage> FarmerImage { get; set; }
        public DbSet<FarmerVideo> FarmerVideo { get; set; }
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<LookupValue> LookupValue { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<ImageSlider> ImageSlider { get; set; }
        public DbSet<Onboarding> Onboarding { get; set; }

        public DbSet<TripSection> TripSection { get; set; }
        public DbSet<TripProfile> TripProfile { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<TripPriceList> TripPriceList { get; set; }
        public DbSet<TripExtraFeatureType> TripExtraFeatureType { get; set; }
        public DbSet<TripImage> TripImage { get; set; }
        public DbSet<TripVideo> TripVideo { get; set; }
        public DbSet<FarmerUser> FarmerUser { get; set; }
        public DbSet<FarmerReservation> FarmerReservation { get; set; }
        public DbSet<FarmerFeedback> FarmerFeedback { get; set; }
        public DbSet<CustomerBlackList> CustomerBlackList { get; set; }
        public DbSet<FarmerBlackList> FarmerBlackList { get; set; }
        public DbSet<NotificationsFarm> NotificationsFarm { get; set; }
        public DbSet<terms> terms { get; set; }
        public DbSet<Region> Region { get; set; }

    }
}
