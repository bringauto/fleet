using Artin.BringAuto.Shared.Cars;
using Artin.BringAuto.Shared.Enums;
using Artin.BringAuto.Shared.Stops;
using Artin.BringAuto.Shared.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Orders
{
    public class Order : IId<int>
    {
        public int Id { get; set; }

        public DateTime? Arrive { get; set; }
        public User User { get; set; }
        public Car Car { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }


        public Stop From { get; set; }

        public Stop To { get; set; }

        public OrderStopStatus FromStationStatus { get; set; } = OrderStopStatus.InQueue;
        public OrderStopStatus ToStationStatus { get; set; } = OrderStopStatus.InQueue;

        /// <summary>
        /// Telefonní kontakt na osobu ve výchozí stanici
        /// </summary>
        [Phone]
        public string FromStationPhone { get; set; }

        /// <summary>
        /// Telefonní kontakt na osobu v cílové stanici
        /// </summary>
        [Phone]
        public string ToStationPhone { get; set; }
    }
}
