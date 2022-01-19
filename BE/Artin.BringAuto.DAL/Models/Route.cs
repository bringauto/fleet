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
    public class Route : IId<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Name { get; set; }
        public String Color { get; set; }
        public ICollection<RouteStop> Stops{ get; set; }

    }
}
