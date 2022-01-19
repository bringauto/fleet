using Artin.BringAuto.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Artin.BringAuto.Shared.Orders
{
    public class NewOrder
    {
        public int CarId { get; set; }
        public int? FromStationId { get; set; }
        public int ToStationId { get; set; }
        public DateTime? Arrive { get; set; }
        public OrderPriority Priority { get; set; }
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
