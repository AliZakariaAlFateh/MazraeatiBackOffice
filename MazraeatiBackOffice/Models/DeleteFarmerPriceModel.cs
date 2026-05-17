using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class DeleteFarmerPriceModel
    {
        public int Person { get; set; }
        public int FarmerId { get; set; }
    }
}
