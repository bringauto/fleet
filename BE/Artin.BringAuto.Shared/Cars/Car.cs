using Artin.BringAuto.Shared.Enums;
using Artin.BringAuto.Shared.Orders;
using System.Collections.Generic;

namespace Artin.BringAuto.Shared.Cars
{
    public class Car : IId<int>
    {
        public int Id { get; set; }
        public string HwId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Fuel { get; set; }
        public double Speed { get; set; }
        public CarStatus Status { get; set; }
        public ButtonStatus Button { get; set; }

        public bool UnderTest { get; set; }
        public string CarAdminPhone { get; set; }
        public string CallTwiml { get; set; }
        public int? RouteId { get; set; }
    }
}
