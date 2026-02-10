using MazraeatiBackOffice.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("Farmer")]
    public class Farmer : BaseEntity
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string MobileNumber { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }

        public string NameEn { get; set; }

        public string Description { get; set; }
        public string DescriptionEn { get; set; }

        public string Owner { get; set; }
        public string LocationDesc { get; set; }

        public string LocationDescEn { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int EStateArea { get; set; }
        public int Room { get; set; }
        public string RoomDetails { get; set; }
        public string RoomDetailsEn { get; set; }

        public int Bathroom { get; set; }
        public string BathroomDetails { get; set; }
        public string BathroomDetailsEn { get; set; }

        public int LandArea { get; set; }
        public int Floor { get; set; }
        public int InDoor { get; set; }
        public string InDoorDescription { get; set; }
        public string InDoorDescriptionEn { get; set; }

        public int OutDoor { get; set; }
        public string OutDoorDescription { get; set; }
        public string OutDoorDescriptionEn { get; set; }

        public int kitchens { get; set; }
        public string kitchensDescription { get; set; }
        public string kitchensDescriptionEn { get; set; }

        public string ExtraDetails { get; set; }
        public string ReservationDetails { get; set; }
        public string Family { get; set; }
        public string Location { get; set; }
        public decimal InsuranceAmt { get; set; }
        public decimal DepositAmt { get; set; }
        public int MaxPerson { get; set; }
        public string ConfidentialMessageEn { get; set; }
        public string ConfidentialMessageAr { get; set; }

        public  string ? Image3DLink { get; set; }
        public bool IsTrust { get; set; }
        public bool IsVIP { get; set; }
        public bool IsOffer { get; set; }
        public bool IsWinter { get; set; }
        public bool IsApprove { get; set; }
        public string? UserName { get; internal set; }
        public FarmAppUserStatus? statusFarmAppUser { get; set; } = FarmAppUserStatus.Active;

    }
}
