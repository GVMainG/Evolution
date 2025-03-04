using System;

namespace Evolution.Core.Models
{
    public class Genome
    {
        public Guid Id { get; private set; }  // Уникальный ID генома
        public int[] Genes { get; private set; }
        public int GenerationsSurvived { get; private set; } = 0;
        public int UsageCount { get; private set; } = 0;  // Сколько раз ген использовался
        public int CreatedInGeneration { get; private set; }  // В каком поколении появился ген
        public int? ExtinctInGeneration { get; private set; }  // Когда ген вымер (null, если ещё жив)

        public Genome(int[] genes, int createdInGeneration)
        {
            Id = Guid.NewGuid();
            Genes = genes;
            CreatedInGeneration = createdInGeneration;
        }

        public void IncreaseSurvival() => GenerationsSurvived++;
        public void IncreaseUsage() => UsageCount++;
        public void MarkExtinct(int generation) => ExtinctInGeneration = generation; // Фиксируем вымирание
    }
}
