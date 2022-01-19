using System;

namespace Artin.BringAuto.Shared.Butons
{
    public class Button : NewButton, IId<int>
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
    }
}
