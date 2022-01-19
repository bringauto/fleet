using System;

namespace Artin.BringAuto.Shared.LocationHistory
{
    public class LocationHistory : IId<int>
    {
        public int Id { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
