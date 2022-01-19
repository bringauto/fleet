using Artin.BringAuto.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Cars
{
    public class UpdateCar : IId<int>
    {
        public int Id { get; set; }
        public string HwId { get; set; }

        public string Name { get; set; }
        public string CompanyName { get; set; }
        public CarStatus Status { get; set; }

        public ButtonStatus Button { get; set; }

        public bool RequireNewToken { get; set; }
        public bool UnderTest { get; set; }
        public string CarAdminPhone { get; set; }

        public int? RouteId { get; set; }
    }
}
