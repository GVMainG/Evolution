﻿namespace Evolution.Core.Entities
{
    public class Food
    {
        public int NutritionalValue { get; private set; }

        public Food(int nutritionalValue = 11) { NutritionalValue = nutritionalValue; }
    }
}
