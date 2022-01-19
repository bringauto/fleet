using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared
{
    public interface IId<T>
    {
        public T Id { get; set; }
    }
}
