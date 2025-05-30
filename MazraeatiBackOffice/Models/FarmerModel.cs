using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerModel
    {
        public FarmerModel()
        {
            ExtraFeature = new List<FarmerExtraFeatureTypeDto>();
        }
        public int Id { get; set; }
        
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CityId { get; set; }

        [DisplayName("الدولة")]
        public string CountryDesc { get; set; }
       
        [DisplayName("المدينة")]
        public string CityDesc { get; set; }
        
        [DisplayName("رقم الهاتف")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string MobileNumber { get; set; }

        [DisplayName("رقم الاعلان")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public long Number { get; set; }

        [DisplayName("اسم المزرعة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Name { get; set; }

        [DisplayName("وصف المزرعة")]
        public string Description { get; set; }

        [DisplayName("اسم المالك")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Owner { get; set; }

        [DisplayName("اسم المنطقة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string LocationDesc { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [DisplayName("المساحة العقار")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int EStateArea { get; set; }

        [DisplayName("عدد الغرف")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int Room { get; set; }

        [DisplayName("تفاصيل الغرف")]
        public string RoomDetails { get; set; }

        [DisplayName("عدد الحمامات")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int Bathroom { get; set; }

        [DisplayName("تفاصيل  الحمامات")]
        public string BathroomDetails { get; set; }

        [DisplayName("مساحة العقار")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int LandArea { get; set; }

        [DisplayName("عدد الطوابق")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int Floor { get; set; }

        [DisplayName("عدد الجلسات الداخلية")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int InDoor { get; set; }

        [DisplayName("تفاصيل الجلسات الداخلية")]
        public string InDoorDescription { get; set; }

        [DisplayName("عدد الجلسات الخارجية")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int OutDoor { get; set; }

        [DisplayName("تفاصيل الجلسات الخارجية")]
        public string OutDoorDescription { get; set; }

        [DisplayName("عدد المطابخ")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int kitchens { get; set; }

        [DisplayName("تفاصيل المطبخ")]
        public string kitchensDescription { get; set; }

        [DisplayName("تفاصيل اضافية")]
        public string ExtraDetails { get; set; }

        [DisplayName("تفاصيل الحجز ان اوجد")]
        public string ReservationDetails { get; set; }

        [DisplayName("نوع الاشخاص")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Family { get; set; }

        [DisplayName("الموقع الجغرافي")]
        //[Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Location { get; set; }

        [DisplayName("مبلغ التامين")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal InsuranceAmt { get; set; }

        [DisplayName("مبلغ العربون")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal DepositAmt { get; set; }

        [DisplayName("الحد الاقصى للاشخاص")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int MaxPerson { get; set; }
        [DisplayName("رسالة التوثيق بالعربي")]

        public string ConfidentialMessageAr { get; set; }
        [DisplayName("رسالة التوثيق بالانجليزي")]

        public string ConfidentialMessageEn{ get; set; }

        [DisplayName("هل الاعلان موثوق")]
        public bool IsTrust { get; set; }

        [DisplayName("هل الاعلان مميز")]
        public bool IsVIP { get; set; }

        [DisplayName("هل الاعلان ضمن العروض")]
        public bool IsOffer { get; set; }

        [DisplayName("هل الاعلان موافق عليها")]
        public bool IsApprove { get; set; }

        [DisplayName("هل المزرعة شتوية")]
        public bool IsWinter { get; set; }
        public int ReservationCount { get; set; }
        public int FeedbackCount { get; set; }

        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
        public List<FarmerExtraFeatureTypeDto> ExtraFeature { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<IFormFile> Videos { get; set; }
        public List<FarmerPriceList> PriceList { get; set; }
        public List<FarmerFeedback> FarmerFeedback { get; set; }
    }
}
