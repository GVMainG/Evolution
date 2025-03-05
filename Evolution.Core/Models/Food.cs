using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Core.Models
{
    public class Food
    {
        public int NutritionalValue { get; set; }

        public Food(int nutritionalValue = 11) { NutritionalValue = nutritionalValue; }
    }
}
