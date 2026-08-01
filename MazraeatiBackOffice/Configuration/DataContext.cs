using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.LoyaltyPoints;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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
        public DbSet<Regions> Region { get; set; }
        public DbSet<AppUser> User { get; set; }
        public DbSet<DeviceToken> DeviceToken { get; set; }
        public DbSet<FarmerViewes> FarmerViewes { get; set; }
        public DbSet<AppUserBlackList> AppUserBlackList { get; set; }
        public DbSet<CommonQuestions> CommonQuestions { get; set; }
        public DbSet<CommonQuestionsVisitors> CommonQuestionsVisitors { get; set; }
        public DbSet<Customer> Customers { get; set; }

        #region For Rules and Permessions ...
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Screen> Screens { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        #endregion


        #region Sport DbSets
        public DbSet<SportType> SportTypes { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<SportAdditionalService> SportAdditionalServices { get; set; }
        public DbSet<SportFeature> SportFeatures { get; set; }
        public DbSet<SportSportFeature> SportSportFeatures { get; set; }
        public DbSet<GeneralFacility> GeneralFacilities { get; set; }
        public DbSet<SportGeneralFacility> SportGeneralFacilities { get; set; }
        public DbSet<SportPriceList> SportPriceLists { get; set; }
        public DbSet<SportImage> SportImages { get; set; }
        public DbSet<SportVideo> SportVideos { get; set; }
        public DbSet<SafetyFeature> SafetyFeatures { get; set; }
        public DbSet<SportSafetyFeature> SportSafetyFeatures { get; set; }
        public DbSet<SportReservation> SportReservations { get; set; }

        //Properties
        public DbSet<SportPropertyValue> SportPropertyValues { get; set; }
        public DbSet<SportPropertyOption> SportPropertyOptions { get; set; }
        public DbSet<SportPropertyTemplate> SportPropertyTemplates { get; set; }

        #endregion

        #region Loyalty Points

        public DbSet<CustomerLoyaltyAccount> CustomerLoyaltyAccounts { get; set; }
        public DbSet<LoyaltyActivityType> LoyaltyActivityTypes { get; set; }
        public DbSet<LoyaltyBookingActivity> LoyaltyBookingActivities { get; set; }
        //public DbSet<LoyaltyPointRuleFarm> LoyaltyPointRuleFarms { get; set; }
        //public DbSet<LoyaltyPointRuleGeneral> LoyaltyPointRuleGenerals { get; set; }
        //public DbSet<LoyaltyPointRuleSport> LoyaltyPointRuleSports { get; set; }
        public DbSet<LoyaltyRedeemRule> LoyaltyRedeemRules { get; set; }
        public DbSet<LoyaltyTier> LoyaltyTiers { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<ReservationLoyaltyDiscount> ReservationLoyaltyDiscounts { get; set; }

        public DbSet<LoyaltyPointRule> LoyaltyPointRules { get; set; }


        #endregion




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Region>().HasNoKey();
            // AdminUser configuration
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(250);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(300);
            });

            // UserRole configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // UserPermission configuration - Unique constraint
            modelBuilder.Entity<UserPermission>()
                .HasIndex(up => new { up.UserId, up.ScreenId })
                .IsUnique();

            // Screen self-reference (Parent-Child)
            modelBuilder.Entity<Screen>()
                .HasOne(s => s.Parent)
                .WithMany(s => s.SubScreens)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
