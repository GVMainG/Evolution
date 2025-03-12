using Evolution.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Core.Infrastructure
{
    public class EvolutionManager
    {
        private FieldBase _field;
        private int _generationCount;
        private const int InitialBotCount = 50;
        private const int SurvivorThreshold = 10;
        private const int MaxGenerations = 5000;

        public event Action<int> OnGenerationChange; // Событие смены поколения

        public EvolutionManager(FieldBase field)
        {
            _field = field;
            _generationCount = 0;
        }

        /// <summary>
        /// Проверяет количество ботов и запускает смену поколения, если их осталось мало.
        /// </summary>
        public void CheckAndEvolve()
        {
            if (_field.Bots.Count > SurvivorThreshold) return;

            if (_generationCount >= MaxGenerations)
            {
                Console.WriteLine("Достигнуто максимальное количество поколений.");
                return;
            }

            _generationCount++;
            OnGenerationChange?.Invoke(_generationCount); // Уведомляем о смене поколения
            Console.WriteLine($"Поколение {_generationCount} завершено. Начинаем новое.");

            List<Bot> survivors = _field.Bots.OrderByDescending(b => b.Energy).Take(SurvivorThreshold).ToList();
            _field.Bots.Clear();

            List<Bot> newGeneration = GenerateNextGeneration(survivors);
            PlaceNewBots(newGeneration);
            _field.SpawnFood(); // Добавляем еду
        }

        private List<Bot> GenerateNextGeneration(List<Bot> survivors)
        {
            List<Bot> newGeneration = new();

            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    newGeneration.Add(new Bot(survivor.Genome, _generationCount, _field.GetRandomEmptyPosition(), 15));
                }

                newGeneration.Add(new Bot(Genome.CreateRandom(_generationCount), _generationCount, _field.GetRandomEmptyPosition(), 15));
                newGeneration.Add(survivor.Genome.Mutate(_generationCount, 5).CreateBot(_generationCount, _field.GetRandomEmptyPosition()));
            }

            return newGeneration;
        }

        private void PlaceNewBots(List<Bot> bots)
        {
            foreach (var bot in bots)
            {
                _field.AddBot(bot);
            }
        }
    }
}
