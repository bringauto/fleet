using Artin.BringAuto.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Artin.BringAuto.Shared.Orders
{
    public class UpdateOrder : IId<int>
    {
        public int Id { get; set; }

        public DateTime? Arrive { get; set; }

        public int CarId { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }

        public int? FromStationId { get; set; }

        public int ToStationId { get; set; }
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
