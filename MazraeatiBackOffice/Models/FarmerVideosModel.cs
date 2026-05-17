using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerVideosModel
    {
        public FarmerVideosModel()
        {
            Videos = new List<FarmerVideo>();
        }
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }
        public List<FarmerVideo> Videos { get; set; }
    }
}
