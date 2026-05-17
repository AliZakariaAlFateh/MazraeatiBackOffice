using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class TripImagesModel
    {
        public TripImagesModel()
        {
            Images = new List<TripImage>();
        }
        public string TripName { get; set; }
        public List<TripImage> Images { get; set; }
    }
}
