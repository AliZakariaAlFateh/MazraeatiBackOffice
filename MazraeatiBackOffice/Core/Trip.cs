using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("Trip")]
    public class Trip : BaseEntity
    {
        public int Id { get; set; }
        public int TripSectionId { get; set; }
        public int CityId { get; set; }
        public string MobileNumber { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string LocationDesc { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ExtraDetails { get; set; }
        public string ReservationDetails { get; set; }
        public string Location { get; set; }
        public decimal InsuranceAmt { get; set; }
        public decimal DepositAmt { get; set; }
        public int MaxPerson { get; set; }
        public bool IsTrust { get; set; }
        public bool IsVIP { get; set; }
        public bool IsOffer { get; set; }
        public bool IsApprove { get; set; }
        public bool IsWinter { get; set; }
    }
}
