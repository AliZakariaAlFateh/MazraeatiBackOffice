using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class TripModel
    {
        public TripModel()
        {
            ExtraFeature = new List<TripExtraFeatureTypeDto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int TripSectionId { get; set; }
        [DisplayName("اسم القسم")]
        public string TripSectionDesc { get; set; }

        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CityId { get; set; }
     
        [DisplayName("المدينة")]
        public string CityDesc { get; set; }

        [DisplayName("رقم الهاتف")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string MobileNumber { get; set; }

        [DisplayName("رقم الاعلان")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public long Number { get; set; }

        [DisplayName("اسم النشاط")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Name { get; set; }

        [DisplayName("وصف النشاط")]
        public string Description { get; set; }

        [DisplayName("اسم المالك")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Owner { get; set; }

        [DisplayName("اسم المنطقة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string LocationDesc { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        [DisplayName("تفاصيل اضافية")]
        public string ExtraDetails { get; set; }

        [DisplayName("تفاصيل الحجز ان اوجد")]
        public string ReservationDetails { get; set; }

        [DisplayName("الموقع الجغرافي")]
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

        public List<City> Cities { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<IFormFile> Videos { get; set; }
        public List<TripExtraFeatureTypeDto> ExtraFeature { get; set; }
        public List<TripPriceList> PriceList { get; set; }

    }
}
