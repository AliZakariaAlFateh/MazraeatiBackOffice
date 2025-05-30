using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class TripSectionModel
    {
        public int Id { get; set; }

        [DisplayName("الدولة")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int CountryId { get; set; }

        [DisplayName("الدولة")]
        public string CountryDesc { get; set; }

        [DisplayName("العنوان الرئيسي ")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Title { get; set; }

        [DisplayName("صورة الخدمة")]
        public string MainImage { get; set; }

        [DisplayName("نص اضافي فوق الصورة")]
        public string ExtraText { get; set; }

        [DisplayName("فعالية الاعلان")]
        public bool Active { get; set; }

        [DisplayName("ترتيب الظهور")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int OrderId { get; set; }

        public IFormFile FileMainImage { get; set; }
        public List<Country> Countries { get; set; }
    }
}
