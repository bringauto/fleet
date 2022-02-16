using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artin.BringAuto.DAL.Models
{
    public class Order : IId<int>, ITenancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? Arrive { get; set; }

        public string UserId { get; set; }
        [Column(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public int CarId { get; set; }
        [Column(nameof(CarId))]
        public Car Car { get; set; }

        public int? FromStationId { get; set; }
        [Column(nameof(FromStationId))]
        public Station FromStation { get; set; }

        public int ToStationId { get; set; }
        [Column(nameof(ToStationId))]
        public Station ToStation { get; set; }
        public OrderPriority Priority { get; set; }
        public OrderStatus Status { get; set; }

        public OrderStopStatus FromStationStatus { get; set; } = OrderStopStatus.InQueue;
        public OrderStopStatus ToStationStatus { get; set; } = OrderStopStatus.InQueue;


        [Phone]
        public string FromStationPhone { get; set; }

        [Phone]
        public string ToStationPhone { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
