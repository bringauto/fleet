using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artin.BringAuto.DAL.Models
{
    public class Car : IId<int>, ITenancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string HwId { get; set; }

        public string Token { get; set; }
        public string Name { get; set; }

        public string SessionId { get; set; }

        /// <summary>
        /// Datetime of last status in current session
        /// </summary>
        public DateTime SessionLogged { get; set; }

        public string CompanyName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Fuel { get; set; }
        public double Speed { get; set; }
        public CarStatus Status { get; set; }

        public ButtonStatus Button { get; set; }
        public ICollection<Order> Orders { get; set; }

        public ICollection<LocationHistory> LocationHistory { get; set; }
        public bool UnderTest { get; set; }

        public string CarAdminPhone { get; set; }
        public string CallTwiml { get; set; } = "<say>New Order</say>";

        public int? RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public Route Route { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
