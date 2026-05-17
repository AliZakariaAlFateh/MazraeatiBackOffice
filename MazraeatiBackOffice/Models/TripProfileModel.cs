using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class TripProfileModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CountryId { get; set; }

        [DisplayName("الدولة")]
        public string CountryDesc { get; set; }

        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CityId { get; set; }

        [DisplayName("المحافظة")]
        public string CityDesc { get; set; }

        [DisplayName("نوع الخدمة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int TypeId { get; set; }
        public string TypeDesc { get; set; }

        [DisplayName("اسم ملف الشركة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Name { get; set; }

        [DisplayName("وصف الشركة")]
        public string Description { get; set; }

        [DisplayName("الموقع")]
        public string Location { get; set; }

        [DisplayName("رابط الفيس بوك")]
        public string FacebookUrl { get; set; }

        [DisplayName("رابط الانستقرام")]
        public string InstgramUrl { get; set; }

        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

        [DisplayName("رقم الهاتف")]
        public string TelephoneNumber { get; set; }

        [DisplayName("رقم الهاتف المحمول")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string MobileNumber { get; set; }

        [DisplayName("الفعالية ؟")]
        public bool Active { get; set; }

        public List<TripSection> TripSections { get; set; }
        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
    }
}
