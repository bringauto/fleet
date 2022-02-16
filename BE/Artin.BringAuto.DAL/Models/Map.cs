using Artin.BringAuto.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.DAL.Models
{
    public class Map : IId<int>, ITenancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Double MinLongitude { get; set; }
        public Double MaxLongitude { get; set; }
        public Double MinLatitude { get; set; }
        public Double MaxLatitude { get; set; }

        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

    }
}
