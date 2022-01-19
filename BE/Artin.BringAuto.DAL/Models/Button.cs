using Artin.BringAuto.Shared;
using Artin.BringAuto.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artin.BringAuto.DAL.Models
{
    public class Button : IId<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public ButtonStatus Status { get; set; }

        public int CarId { get; set; }
        [Column(nameof(CarId))]
        public Car Car { get; set; }
    }
}
