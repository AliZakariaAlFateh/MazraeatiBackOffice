using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerPriceModel
    {
        public FarmerPriceModel()
        {
            PriceList = new List<FarmerPriceList>();
        }

        [DisplayName("الحد الاقصى للاشخاص")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int Person { get; set; }
        public int FarmerId { get; set; }
        public List<FarmerPriceList> PriceList { get; set; }
    }
}
