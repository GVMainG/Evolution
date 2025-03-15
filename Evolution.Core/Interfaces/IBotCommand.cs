using Evolution.Core.Entities;

namespace Evolution.Core.Interfaces
{
    public interface IBotCommand
    {
        int EnergyCost { get; }
        bool IsFinalOne { get; }

        void Execute(Bot bot, IWorld field);
    }
}