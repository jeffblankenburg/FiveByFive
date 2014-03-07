using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveByFiveLogic
{
    public class Die
    {
        public int Value { get; set; }
        public bool IsHeld = false;

        public Die()
        {

        }
        
        public Die(int value, bool isheld)
        {
            Value = value;
            IsHeld = isheld;
        }

    }
}
