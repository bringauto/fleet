using Artin.BringAuto.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Orders
{
    public class OrderForCall
    {
        public int Id { get; set; }

        public bool CanCall { get; set; }
        [Phone]
        public string FromStationPhone { get; set; }

        /// <summary>
        /// Telefonní kontakt na osobu v cílové stanici
        /// </summary>
        [Phone]
        public string ToStationPhone { get; set; }

        public OrderStopStatus FromStationStatus { get; set; } = OrderStopStatus.InQueue;
        public OrderStopStatus ToStationStatus { get; set; } = OrderStopStatus.InQueue;

        public int? ToStationId { get; set; }
        public int? FromStationId { get; set; }

    }
}
