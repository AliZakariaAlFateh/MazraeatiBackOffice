using DocumentFormat.OpenXml.Drawing.Charts;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SportModel
    {
        public SportModel()
        {
            SportFeatures = new List<SportFeatureDto>();
            GeneralFacilities = new List<GeneralFacilityDto>();
            AdditionalServices = new List<AdditionalServiceDto>();
            PriceList = new List<SportPriceList>();
            SportImages = new List<SportImage>();
            SportVideos = new List<SportVideo>();
        }

        public int Id { get; set; }

        // ===== المالك =====

        // ===== نوع الرياضة =====
        [Required(ErrorMessage = "نوع الرياضة مطلوب")]
        public int SportTypeId { get; set; }
        [DisplayName("القسم الرياضي")]
        public string SportTypeDesc { get; set; }

        // ===== معلومات الموقع =====
        [Required(ErrorMessage = "الدولة مطلوبة")]
        public int CountryId { get; set; }
        [DisplayName("الدولة")]
        public string CountryDesc { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public int CityId { get; set; }
        [DisplayName("المدينة")]
        public string CityDesc { get; set; }

        [Required(ErrorMessage = "المنطقة مطلوبة")]
        public int RegionId { get; set; }
        [DisplayName("المنظقة")]
        public string RegionDesc { get; set; }
        
        public int? UserId { get; set; }
        [DisplayName("المستخدم")]
        public string UserDesc { get; set; }

        // ===== المعلومات الأساسية =====
        [DisplayName("رقم الجوال")]
        public string MobileNumber { get; set; }

        [DisplayName("رقم الإعلان")]
        public long Number { get; set; }

        [DisplayName("الاسم بالعربي")]
        [Required(ErrorMessage = "الاسم بالعربي مطلوب")]
        public string NameAr { get; set; }

        [DisplayName("الاسم بالإنجليزي")]
        public string NameEn { get; set; }

        [DisplayName("الوصف بالعربي")]
        public string DescriptionAr { get; set; }

        [DisplayName("الوصف بالإنجليزي")]
        public string DescriptionEn { get; set; }

        [DisplayName("نوع المالك")]
        public string Owner { get; set; }

        [DisplayName("المنطقة بالعربي")]
        public string LocationDesc { get; set; }

        [DisplayName("المنطقة بالإنجليزي")]
        public string LocationDescEn { get; set; }
        [DisplayName("الموقع الجغرافي")]
        public string GeographicLocation { get; set; }
        [DisplayName("رسالة التوثيق بالعربي")]
        public string ConfidentialMessageAr { get; set; }
        [DisplayName("رسالة التوثيق بالإنجليزي")]
        public string ConfidentialMessageEn { get; set; }
        [DisplayName("تفاصيل الحجز إن وجد")]
        public string ReservationDetails { get; set; }
        [DisplayName("تفاصيل إضافية")]
        public string ExtraDetails { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        // ===== تفاصيل العقار (حسب نوع الرياضة) =====

        // كرة القدم
        //[DisplayName("نوع أرضية الملعب")]
        //public string FootballFloorType { get; set; }

        //[DisplayName("أبعاد الملعب")]
        //public string FootballCourtDimensions { get; set; }

        //[DisplayName("عدد اللاعبين (مثل 5، 7، 11)")]
        //public int? FootballPlayerCount { get; set; }

        //[DisplayName("نوع الملعب")]
        //public string FootballPitchType { get; set; }

        //[DisplayName("ملعب مغلق (مغطى)")]
        //public bool? FootballIsIndoor { get; set; }

        //[DisplayName("ملعب مكشوف (مفتوح)")]
        //public bool? FootballIsOutdoor { get; set; }

        //[DisplayName("نظام الإضاءة والكشافات")]
        //public string FootballLightingSystem { get; set; }

        //[DisplayName("مناسب للمباريات الرسمية")]
        //public bool? FootballSuitableForOfficial { get; set; }

        //[DisplayName("مناسب للتدريب")]
        //public bool? FootballSuitableForTraining { get; set; }

        //// البادل
        //[DisplayName("نوع الملعب (بادل)")]
        //public string PadelPitchType { get; set; }

        //[DisplayName("عدد ملاعب البادل")]
        //public int? PadelNumberOfCourts { get; set; }

        //[DisplayName("يحتوي على سقف")]
        //public bool? PadelHasCeiling { get; set; }

        //[DisplayName("إضاءة ليلية")]
        //public bool? PadelNightLighting { get; set; }

        //[DisplayName("نوع الزجاج")]
        //public string PadelGlassType { get; set; }

        //[DisplayName("مستوى الملعب")]
        //public string PadelCourtLevel { get; set; }

        //// التنس
        //[DisplayName("نوع أرضية ملعب التنس")]
        //public string? TennisPitchType { get; set; }

        //[DisplayName("عدد ملاعب التنس")]
        //public int? TennisNumberOfCourts { get; set; }

        //[DisplayName("إضاءة ليلية")]
        //public bool? TennisNightLighting { get; set; }

        //[DisplayName("مناسب للعب الفردي")]
        //public bool? TennisIsSingles { get; set; }

        //[DisplayName("مناسب للعب الزوجي")]
        //public bool? TennisIsDoubles { get; set; }

        //[DisplayName("مناسب للبطولات")]
        //public bool? TennisSuitableForTournaments { get; set; }

        //// كرة السلة
        //[DisplayName("عدد السلال")]
        //public int? BasketBallNumberOfBaskets { get; set; }

        //[DisplayName("حجم/أبعاد ملعب السلة")]
        //public string BasketBallCourtSize { get; set; }

        //// كرة الطائرة
        //[DisplayName("ملعب مغلق (كرة طائرة)")]
        //public bool? VollyBallIsIndoor { get; set; }

        //[DisplayName("ملعب مكشوف (كرة طائرة)")]
        //public bool? VollyBallIsOutdoor { get; set; }

        //[DisplayName("إضاءة ليلية")]
        //public bool? VollyBallNightLighting { get; set; }

        //[DisplayName("نوع أرضية ملعب الطائرة")]
        //public string? VollyBallPitchType { get; set; }

        //[DisplayName("عدد ملاعب الطائرة")]
        //public int? VollyBallNumberOfCourts { get; set; }

        //[DisplayName("إمكانية تعديل ارتفاع الشبكة")]
        //public bool? VollyBallNetHeightAdjustable { get; set; }

        //// المسابح
        //[DisplayName("نوع المسبح")]
        //public string SwimmingPoolType { get; set; }

        //[DisplayName("طول المسبح (بالمتر)")]
        //public decimal? SwimmingPoolLength { get; set; }

        //[DisplayName("عرض المسبح (بالمتر)")]
        //public decimal? SwimmingPoolWidth { get; set; }

        //[DisplayName("عمق المسبح (بالمتر)")]
        //public decimal? SwimmingPoolDepth { get; set; }

        //[DisplayName("عدد المسابح")]
        //public int? SwimmingPoolNumberOfPools { get; set; }

        //[DisplayName("يتوفر مسبح للأطفال")]
        //public bool? SwimmingPoolHasChildrenPool { get; set; }

        //[DisplayName("يتوفر مسبح للبالغين")]
        //public bool? SwimmingPoolHasAdultsPool { get; set; }

        //[DisplayName("درجة حرارة الماء")]
        //public decimal? SwimmingPoolWaterTemperature { get; set; }

        //[DisplayName("نظام تعقيم المسبح")]
        //public string SwimmingPoolSterilizationSystem { get; set; }

        //[DisplayName("توجد ألعاب مائية / زحاليق")]
        //public bool? SwimmingPoolHasWaterSlides { get; set; }

        //[DisplayName("يتوفر جاكوزي")]
        //public bool? SwimmingPoolHasJacuzzi { get; set; }

        //[DisplayName("مناسب للتدريب والسباحة")]
        //public bool? SwimmingPoolSuitableForTraining { get; set; }

        //[DisplayName("مناسب للحفلات والفعاليات")]
        //public bool? SwimmingPoolSuitableForEvents { get; set; }

        //// الفروسية
        //[DisplayName("نوع نشاط الفروسية")]
        //public string EquestrianismActivityType { get; set; }

        //[DisplayName("عدد الخيول المتاحة")]
        //public int? EquestrianismNumberOfHorses { get; set; }

        //[DisplayName("مستوى التدريب المتاح")]
        //public string EquestrianismTrainingLevel { get; set; }

        //[DisplayName("مدة الجولة (بالدقائق)")]
        //public int? EquestrianismTourDuration { get; set; }

        //[DisplayName("العمر المسموح به")]
        //public int? EquestrianismAllowedAge { get; set; }

        //[DisplayName("الوزن المسموح به (كجم)")]
        //public int? EquestrianismAllowedWeight { get; set; }

        //[DisplayName("يتوفر مدرب مرافق")]
        //public bool? EquestrianismHasAccompanyingTrainer { get; set; }

        //[DisplayName("مضمار مغلق")]
        //public bool? EquestrianismTrackIndoor { get; set; }

        //[DisplayName("مضمار مكشوف")]
        //public bool? EquestrianismTrackOutdoor { get; set; }

        //// الرماية
        //[DisplayName("ميدان رماية مغلق")]
        //public bool? ShootingIndoor { get; set; }

        //[DisplayName("ميدان رماية مكشوف")]
        //public bool? ShootingOutdoor { get; set; }

        //[DisplayName("رماية بالضغط الهوائي")]
        //public bool? ShootingAirShooting { get; set; }

        //[DisplayName("رماية بالنار / الحي")]
        //public bool? ShootingFireShooting { get; set; }

        //[DisplayName("رماية بالقوس والسهم")]
        //public bool? ShootingBowAndArrow { get; set; }

        //[DisplayName("يتوفر مدرب رماية")]
        //public bool? ShootingShootingTrainer { get; set; }

        //[DisplayName("المعدات متوفرة")]
        //public bool? ShootingEquipmentAvailable { get; set; }

        //[DisplayName("تأجير المعدات متوفر")]
        //public bool? ShootingEquipmentRental { get; set; }

        //// Pickleball
        //[DisplayName("ملعب بيكل بول مغلق")]
        //public bool? PickleballIndoor { get; set; }

        //[DisplayName("ملعب بيكل بول مكشوف")]
        //public bool? PickleballOutdoor { get; set; }

        //[DisplayName("مضارب البيكل بول متوفرة")]
        //public bool? PickleballPaddles { get; set; }

        //[DisplayName("كرات البيكل بول متوفرة")]
        //public bool? PickleballBalls { get; set; }

        //[DisplayName("إضاءة ملعب البيكل بول")]
        //public bool? PickleballLighting { get; set; }

        //// تنس الطاولة
        //[DisplayName("طاولات احترافية")]
        //public bool? TableTennisProfessional { get; set; }

        //[DisplayName("مضارب تنس طاولة متوفرة")]
        //public bool? TableTennisPaddles { get; set; }

        //[DisplayName("كرات تنس طاولة متوفرة")]
        //public bool? TableTennisBalls { get; set; }

        //[DisplayName("يتوفر تدريب تنس طاولة")]
        //public bool? TableTennisTraining { get; set; }

        //// الاسكواش
        //[DisplayName("ملعب اسكواش زجاجي")]
        //public bool? SquashGlassCourt { get; set; }

        //[DisplayName("ملعب اسكواش عادي")]
        //public bool? SquashNormalCourt { get; set; }

        //[DisplayName("مضارب اسكواش متوفرة")]
        //public bool? SquashPaddles { get; set; }

        //[DisplayName("كرات اسكواش متوفرة")]
        //public bool? SquashBalls { get; set; }

        //[DisplayName("يتوفر تدريب اسكواش")]
        //public bool? SquashTraining { get; set; }

        //// الريشة الطائرة
        //[DisplayName("صالة ريشة طائرة مغلقة")]
        //public bool? BadmintonIndoor { get; set; }

        //[DisplayName("أرضية احترافية للريشة الطائرة")]
        //public bool? BadmintonProfessionalFloor { get; set; }

        //[DisplayName("مضارب ريشة طائرة متوفرة")]
        //public bool? BadmintonPaddles { get; set; }

        //[DisplayName("كرات ريشة (Shuttlecocks) متوفرة")]
        //public bool? BadmintonShuttlecocks { get; set; }

        //[DisplayName("يتوفر تدريب ريشة طائرة")]
        //public bool? BadmintonTraining { get; set; }

        //// الكرة الطائرة (شاطئية)
        //[DisplayName("طائرة شاطئية مغلقة")]
        //public bool? BeachVolleyballIndoor { get; set; }

        //[DisplayName("طائرة شاطئية مكشوفة")]
        //public bool? BeachVolleyballOutdoor { get; set; }

        //[DisplayName("أرضية رملية")]
        //public bool? BeachVolleyballSandFloor { get; set; }

        //[DisplayName("أرضية مغطاة")]
        //public bool? BeachVolleyballIndoorFloor { get; set; }

        //[DisplayName("إضاءة ملعب الطائرة الشاطئية")]
        //public bool? BeachVolleyballLighting { get; set; }

        //[DisplayName("شبكة احترافية متوفرة")]
        //public bool? BeachVolleyballProfessionalNet { get; set; }

        //[DisplayName("كرات متوفرة")]
        //public bool? BeachVolleyballBalls { get; set; }

        //// كرة السلة (إضافات)
        //[DisplayName("ملعب سلة مغلق")]
        //public bool? BasketballIndoor { get; set; }

        //[DisplayName("ملعب سلة مكشوف")]
        //public bool? BasketballOutdoor { get; set; }

        //[DisplayName("أرضية باركيه (خشب)")]
        //public bool? BasketballWoodFloor { get; set; }

        //[DisplayName("أرضية مطاطية (EPU/Rubber)")]
        //public bool? BasketballRubberFloor { get; set; }

        //[DisplayName("إضاءة ملعب السلة")]
        //public bool? BasketballLighting { get; set; }

        //[DisplayName("مدرجات للجمهور")]
        //public bool? BasketballStands { get; set; }

        //[DisplayName("شاشة/لوحة نتائج الإلكترونية")]
        //public bool? BasketballScoreboard { get; set; }

        //[DisplayName("كرات سلة متوفرة")]
        //public bool? BasketballBalls { get; set; }

        //[DisplayName("يتوفر تدريب كرة سلة")]
        //public bool? BasketballTraining { get; set; }

        //// التنس (إضافات)
        //[DisplayName("ملعب تنس مغلق")]
        //public bool? TennisIndoor { get; set; }

        //[DisplayName("ملعب تنس مكشوف")]
        //public bool? TennisOutdoor { get; set; }

        //[DisplayName("أرضية أكريليكصلبة")]
        //public bool? TennisAcrylicFloor { get; set; }

        //[DisplayName("أرضية ترابية/ترابية حُمرة (Clay)")]
        //public bool? TennisClayFloor { get; set; }

        //[DisplayName("أرضية عشبيّة")]
        //public bool? TennisGrassFloor { get; set; }

        //[DisplayName("إضاءة ملعب التنس")]
        //public bool? TennisLighting { get; set; }

        //[DisplayName("مضارب تنس متوفرة")]
        //public bool? TennisPaddles { get; set; }

        //[DisplayName("كرات تنس متوفرة")]
        //public bool? TennisBalls { get; set; }

        //[DisplayName("يتوفر مدرب تنس")]
        //public bool? TennisTrainer { get; set; }

        //[DisplayName("تتوفر أكاديمية تدريب تنس")]
        //public bool? TennisAcademy { get; set; }

        //[DisplayName("إقامة بطولات تنس")]
        //public bool? TennisTournaments { get; set; }

        //// البادل (إضافات)
        //[DisplayName("ملعب بادل مغلق")]
        //public bool? PadelIndoor { get; set; }

        //[DisplayName("ملعب بادل مكشوف")]
        //public bool? PadelOutdoor { get; set; }

        //[DisplayName("ملعب بانورامي")]
        //public bool? PadelPanoramic { get; set; }

        //[DisplayName("ملعب بادل عادي")]
        //public bool? PadelNormal { get; set; }

        //[DisplayName("إضاءة ملعب البادل")]
        //public bool? PadelLighting { get; set; }

        //[DisplayName("تأجير مضارب بادل")]
        //public bool? PadelPaddlesRental { get; set; }

        //[DisplayName("كرات بادل متوفرة")]
        //public bool? PadelBallsAvailable { get; set; }

        //[DisplayName("يتوفر مدرب بادل")]
        //public bool? PadelTrainer { get; set; }

        //[DisplayName("تتوفر أكاديمية بادل")]
        //public bool? PadelAcademy { get; set; }

        //[DisplayName("تنظيم بطولات بادل")]
        //public bool? PadelTournaments { get; set; }

        //[DisplayName("إمكانية بالحجز بالساعة")]
        //public bool? PadelHourlyBooking { get; set; }

        // ===== تفاصيل الحجز =====
        //[DisplayName("مبلغ التأمين")]
        //public decimal InsuranceAmt { get; set; }

        //[DisplayName("مبلغ العربون")]
        //public decimal DepositAmt { get; set; }

        [DisplayName("الحد الأقصى للأشخاص")]
        public int MaxPerson { get; set; }

        [DisplayName("رابط ثلاثي الأبعاد")]
        public string Image3DLink { get; set; }

        // ===== الخصائص العامة =====
        [DisplayName("موثوق")]
        public bool IsTrust { get; set; }

        [DisplayName("مميز VIP")]
        public bool IsVIP { get; set; }

        [DisplayName("ضمن العروض")]
        public bool IsOffer { get; set; }

        [DisplayName("شتوي")]
        public bool IsWinter { get; set; }

        [DisplayName("موافق عليه")]
        public bool IsApprove { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;

        [DisplayName("محظور")]
        public bool IsBlocked { get; set; }
        //Do not Forget to repcae or change this name ....
        [DisplayName("حالة النشاط")]
        public int statusSportAppUser { get; set; } = 0;

        // ===== حقول إضافية =====
        [DisplayName("الرقم الكودي")]
        public string SerialSportKey { get; set; }

        public string MobileOwnerAppUser { get; set; }
        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // ===== Lists =====
        public List<Country> Countries { get; set; } = new List<Country>();
        public List<City> Cities { get; set; } = new List<City>();
        public List<Regions>? Regions { get; set; } = new List<Regions>();
        public List<AppUser>? Users { get; set; } = new List<AppUser>();
        public List<SportType> SportTypes { get; set; } = new List<SportType>();

        // ===== المرفقات والخدمات =====
        public List<SportFeatureDto> SportFeatures { get; set; }= new List<SportFeatureDto>();
        public List<GeneralFacilityDto> GeneralFacilities { get; set; }
        public List<AdditionalServiceDto> AdditionalServices { get; set; }
        public List<SafetyFeatureDto> SafetyFeatures { get; set; } = new List<SafetyFeatureDto>();
        public List<SportPropertyTemplateDto> PropertyTemplates { get; set; } = new List<SportPropertyTemplateDto>();
        public List<SportPropertyValueDto> PropertyValues { get; set; } = new List<SportPropertyValueDto>();
        // ===== الصور والفيديوهات =====
        public List<SportImage> SportImages { get; set; }
        public List<SportVideo> SportVideos { get; set; }

        // ===== للرفع =====
        public List<IFormFile> Images { get; set; }
        public List<IFormFile> Videos { get; set; }

        // ===== جدول الأسعار =====
        public List<SportPriceList> PriceList { get; set; }

        // ===== إحصائيات =====
        [DisplayName("عدد الحجوزات")]
        public int ReservationCount { get; set; }
        [DisplayName("عدد التعليقات")]
        public int FeedbackCount { get; set; }
    }

}
