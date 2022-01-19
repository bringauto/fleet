using Artin.BringAuto.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artin.BringAuto.DAL.Models
{
    public class Station : IId<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Phone]
        public string ContactPhone { get; set; }
    }
}
