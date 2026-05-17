using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class TripPriceModel
    {
        public TripPriceModel()
        {
            PriceList = new List<TripPriceList>();
        }

        public int Id { get; set; }

        [DisplayName("الحد الاقصى للاشخاص")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int Person { get; set; }
        public int TripId { get; set; }
        public List<TripPriceList> PriceList { get; set; }
    }
}
