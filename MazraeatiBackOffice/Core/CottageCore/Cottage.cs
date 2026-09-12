using DocumentFormat.OpenXml.Drawing.Charts;
using MazraeatiBackOffice.Core.SystemCore;
using MazraeatiBackOffice.Core.UserManagementCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MazraeatiBackOffice.Core.CottageCore
{
    [Table("Cottages")]
    public class Cottage:BaseEntity
    {
        
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
        public string ConfidentialMessageAr { get; set; }
        public string ConfidentialMessageEn { get; set; }
        public string ReservationDetails { get; set; }
        public string ExtraDetails {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string GeographicLocation { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        // ===== تفاصيل الحجز (مثل Farmer) =====
        public decimal InsuranceAmt { get; set; }
        public decimal DepositAmt { get; set; }
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
        public int statusCottageAppUser { get; set; }

        // ===== حقول إضافية (مثل Farmer) =====
        public string SerialCottageKey { get; set; }
        public string MobileOwnerAppUser { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        // ===== Navigation Properties =====
        public virtual Country Country { get; set; }
        //[ForeignKey("CityId")]
        public virtual City City { get; set; }
        public virtual Regions Region { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<CottagePriceList> PriceList { get; set; }
        public virtual ICollection<CottageImage> CottageImages { get; set; }
        public virtual ICollection<CottageVideo> CottageVideos { get; set; }
        //public virtual ICollection<CottageGeneralFacility> GeneralFacilities { get; set; }
        //public virtual ICollection<SportSportFeature> SportFeatures { get; set; }
        //public virtual ICollection<CottageBlackList> CottageBlackLists { get; set; }
        //public virtual ICollection<CottagetatusLog> CottageStatusLogs { get; set; }
    }
}
