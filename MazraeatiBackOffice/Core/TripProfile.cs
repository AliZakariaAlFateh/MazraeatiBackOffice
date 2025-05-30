using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("TripProfile")]
    public class TripProfile : BaseEntity
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string FacebookUrl { get; set; }
        public string InstgramUrl { get; set; }
        public string ImageUrl { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool Active { get; set; }
    }
}
