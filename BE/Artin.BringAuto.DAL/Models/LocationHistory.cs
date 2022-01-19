using Artin.BringAuto.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artin.BringAuto.DAL.Models
{
    public class LocationHistory : IId<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CarId { get; set; }
        [Column(nameof(CarId))]
        public Car Car { get; set; }
        public DateTime Time { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
