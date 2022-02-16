using Artin.BringAuto.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artin.BringAuto.DAL.Models
{
    public class RouteStop : IId<int>, ITenancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Route Route { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int Order { get; set; }

        public int? StationId { get; set; }
        [ForeignKey(nameof(StationId))]
        public Station Station { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

    }
}