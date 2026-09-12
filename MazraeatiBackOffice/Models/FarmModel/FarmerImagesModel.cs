using MazraeatiBackOffice.Core.FarmCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.FarmModel
{
    public class FarmerImagesModel
    {
        public FarmerImagesModel()
        {
            Images = new List<FarmerImage>();
        }
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }
        public List<FarmerImage> Images { get; set; }
    }
}
