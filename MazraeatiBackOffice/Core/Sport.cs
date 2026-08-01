using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MazraeatiBackOffice.Core
{
    [Table("Sports")]
    public class Sport:BaseEntity
    {
        // ===== المالك الرئيسي =====
        //public int PrimaryOwnerId { get; set; }

        // ===== نوع الرياضة ====
        public int SportTypeId { get; set; }

        // ===== معلومات الموقع (مثل Farmer) =====
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int? RegionId { get; set; }

        public int? UserId { get; set; }

        // ===== المعلومات الأساسية =====
        public string MobileNumber { get; set; }

        public long Number { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }
        public string Owner { get; set; }
        public string LocationDescAr { get; set; }
        public string LocationDescEn { get; set; }
        public string GeographicLocation { get; set; }
        public string ConfidentialMessageAr { get; set; }
        public string ConfidentialMessageEn { get; set; }

        public string ReservationDetails { get; set; }
        public string ExtraDetails {  get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        // ===== تفاصيل العقار (حسب نوع الرياضة - كلها nullable) =====
        // كرة القدم
        //public string FootballFloorType { get; set; }
        //public string FootballCourtDimensions { get; set; }
        //public int? FootballPlayerCount { get; set; }
        //public string FootballPitchType { get; set; }
        //public bool? FootballIsIndoor { get; set; }
        //public bool? FootballIsOutdoor { get; set; }
        //public string FootballLightingSystem { get; set; }

        //public bool? FootballSuitableForOfficial { get; set; }

        //public bool? FootballSuitableForTraining { get; set; }

        //// البادل
        //public string PadelPitchType { get; set; }
        //public int? PadelNumberOfCourts { get; set; }
        //public bool? PadelHasCeiling { get; set; }

        //public bool? PadelNightLighting { get; set; }
        //public string PadelGlassType { get; set; }
        //public string PadelCourtLevel { get; set; }

        //// التنس
        //public string? TennisPitchType { get; set; }
        //public int? TennisNumberOfCourts { get; set; }
        //public bool? TennisNightLighting { get; set; }
        //public bool? TennisIsSingles { get; set; }
        //public bool? TennisIsDoubles { get; set; }
        //public bool? TennisSuitableForTournaments { get; set; }

        //// كرة السلة
        //public int? BasketBallNumberOfBaskets { get; set; }
        //public string BasketBallCourtSize { get; set; }

        //// كرة الطائرة
        //public bool? VollyBallIsIndoor { get; set; }
        //public bool? VollyBallIsOutdoor { get; set; }
        //public bool? VollyBallNightLighting { get; set; }
        //public string? VollyBallPitchType { get; set; }
        //public int? VollyBallNumberOfCourts { get; set; }
        ////
        //public bool? VollyBallNetHeightAdjustable { get; set; }

        //// المسابح
        //public string SwimmingPoolType { get; set; }

        //public decimal? SwimmingPoolLength { get; set; }

        //public decimal? SwimmingPoolWidth { get; set; }

        //public decimal? SwimmingPoolDepth { get; set; }

        //public int? SwimmingPoolNumberOfPools { get; set; }

        //public bool? SwimmingPoolHasChildrenPool { get; set; }

        //public bool? SwimmingPoolHasAdultsPool { get; set; }

        //public decimal? SwimmingPoolWaterTemperature { get; set; }
        //public string SwimmingPoolSterilizationSystem { get; set; }

        //public bool? SwimmingPoolHasWaterSlides { get; set; }
        //public bool? SwimmingPoolHasJacuzzi { get; set; }

        //public bool? SwimmingPoolSuitableForTraining { get; set; }

        //public bool? SwimmingPoolSuitableForEvents { get; set; }

        //// الفروسية
        //public string EquestrianismActivityType { get; set; }

        //public int? EquestrianismNumberOfHorses { get; set; }
        //public string EquestrianismTrainingLevel { get; set; }

        //public int? EquestrianismTourDuration { get; set; }

        //public int? EquestrianismAllowedAge { get; set; }

        //public int? EquestrianismAllowedWeight { get; set; }

        //public bool? EquestrianismHasAccompanyingTrainer { get; set; }

        //public bool? EquestrianismTrackIndoor { get; set; }

        //public bool? EquestrianismTrackOutdoor { get; set; }

        //// الرماية
        //public bool? ShootingIndoor { get; set; }

        //public bool? ShootingOutdoor { get; set; }

        //public bool? ShootingAirShooting { get; set; }

        //public bool? ShootingFireShooting { get; set; }

        //public bool? ShootingBowAndArrow { get; set; }

        //public bool? ShootingShootingTrainer { get; set; }

        //public bool? ShootingEquipmentAvailable { get; set; }

        //public bool? ShootingEquipmentRental { get; set; }

        //// Pickleball
        //public bool? PickleballIndoor { get; set; }

        //public bool? PickleballOutdoor { get; set; }

        //public bool? PickleballPaddles { get; set; }

        //public bool? PickleballBalls { get; set; }

        //public bool? PickleballLighting { get; set; }

        //// تنس الطاولة
        //public bool? TableTennisProfessional { get; set; }

        //public bool? TableTennisPaddles { get; set; }

        //public bool? TableTennisBalls { get; set; }

        //public bool? TableTennisTraining { get; set; }

        //// الاسكواش
        //public bool? SquashGlassCourt { get; set; }

        //public bool? SquashNormalCourt { get; set; }

        //public bool? SquashPaddles { get; set; }

        //public bool? SquashBalls { get; set; }
        //public bool? SquashTraining { get; set; }

        //// الريشة الطائرة
        //public bool? BadmintonIndoor { get; set; }

        //public bool? BadmintonProfessionalFloor { get; set; }

        //public bool? BadmintonPaddles { get; set; }

        //public bool? BadmintonShuttlecocks { get; set; }

        //public bool? BadmintonTraining { get; set; }

        //// الكرة الطائرة (شاطئية)
        //public bool? BeachVolleyballIndoor { get; set; }

        //public bool? BeachVolleyballOutdoor { get; set; }

        //public bool? BeachVolleyballSandFloor { get; set; }

        //public bool? BeachVolleyballIndoorFloor { get; set; }

        //public bool? BeachVolleyballLighting { get; set; }

        //public bool? BeachVolleyballProfessionalNet { get; set; }

        //public bool? BeachVolleyballBalls { get; set; }

        //// كرة السلة (إضافات)
        //public bool? BasketballIndoor { get; set; }
        //public bool? BasketballOutdoor { get; set; }
        //public bool? BasketballWoodFloor { get; set; }
        //public bool? BasketballRubberFloor { get; set; }
        //public bool? BasketballLighting { get; set; }
        //public bool? BasketballStands { get; set; }
        //public bool? BasketballScoreboard { get; set; }
        //public bool? BasketballBalls { get; set; }
        //public bool? BasketballTraining { get; set; }

        //// التنس (إضافات)
        //public bool? TennisIndoor { get; set; }
        //public bool? TennisOutdoor { get; set; }
        //public bool? TennisAcrylicFloor { get; set; }
        //public bool? TennisClayFloor { get; set; }
        //public bool? TennisGrassFloor { get; set; }
        //public bool? TennisLighting { get; set; }
        //public bool? TennisPaddles { get; set; }
        //public bool? TennisBalls { get; set; }
        //public bool? TennisTrainer { get; set; }
        //public bool? TennisAcademy { get; set; }
        //public bool? TennisTournaments { get; set; }

        //// البادل (إضافات)
        //public bool? PadelIndoor { get; set; }
        //public bool? PadelOutdoor { get; set; }
        //public bool? PadelPanoramic { get; set; }
        //public bool? PadelNormal { get; set; }
        //public bool? PadelLighting { get; set; }
        //public bool? PadelPaddlesRental { get; set; }
        //public bool? PadelBallsAvailable { get; set; }
        //public bool? PadelTrainer { get; set; }
        //public bool? PadelAcademy { get; set; }
        //public bool? PadelTournaments { get; set; }
        //public bool? PadelHourlyBooking { get; set; }

        // ===== تفاصيل الحجز (مثل Farmer) =====
        //public decimal InsuranceAmt { get; set; }

        //public decimal DepositAmt { get; set; }
        public int MaxPerson { get; set; }
        public string Image3DLink { get; set; }
        // ===== الخصائص العامة (مثل Farmer) =====
        public bool IsTrust { get; set; }
        public bool IsVIP { get; set; }
        public bool IsOffer { get; set; }
        public bool IsWinter { get; set; }
        public bool IsApprove { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public int statusSportAppUser { get; set; }

        // ===== حقول إضافية (مثل Farmer) =====
        public string SerialSportKey { get; set; }
        public string MobileOwnerAppUser { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        // ===== Navigation Properties =====
        //[ForeignKey("PrimaryOwnerId")]
        //public virtual AppUser PrimaryOwner { get; set; }

        //[ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }
        //[ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        //[ForeignKey("CityId")]
        public virtual City City { get; set; }
        public virtual Regions Region { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<SportPriceList> PriceList { get; set; }
        public virtual ICollection<SportImage> SportImages { get; set; }
        public virtual ICollection<SportVideo> SportVideos { get; set; }
        //public virtual ICollection<SportGeneralFacility> GeneralFacilities { get; set; }
        //public virtual ICollection<SportAdditionalService> AdditionalServices { get; set; }
        //public virtual ICollection<SportSportFeature> SportFeatures { get; set; }
        //public virtual ICollection<SportBlackList> SportBlackLists { get; set; }
        //public virtual ICollection<SportStatusLog> SportStatusLogs { get; set; }
    }
}
