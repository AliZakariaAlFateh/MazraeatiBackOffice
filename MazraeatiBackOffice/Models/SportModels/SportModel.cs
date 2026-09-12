using DocumentFormat.OpenXml.Drawing.Charts;
using MazraeatiBackOffice.Core.SportCore;
using MazraeatiBackOffice.Core.SystemCore;
using MazraeatiBackOffice.Core.UserManagementCore;
using MazraeatiBackOffice.Dto.SportDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.SportModels
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
        [DisplayName("خط الطول")]
        public string Longitude { get; set; }
        [DisplayName("خط العرض")]
        public string Latitude { get; set; }
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
        public List<Regions> Regions { get; set; } = new List<Regions>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();
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
