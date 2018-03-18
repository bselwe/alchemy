using System;
using System.Collections.Generic;

namespace Alchemy
{
    public class Alchemist
    {
        private readonly IAlchemistsQueue queue;

        public AlchemistType Type { get; private set; }

        private static Dictionary<AlchemistType, List<Resource>> neededResources
            = new Dictionary<AlchemistType, List<Resource>>() 
        {
            { AlchemistType.A, new List<Resource>() { Resource.Lead, Resource.Mercury } },
            { AlchemistType.B, new List<Resource>() { Resource.Mercury, Resource.Sulfur } },
            { AlchemistType.C, new List<Resource>() { Resource.Lead, Resource.Sulfur } },
            { AlchemistType.D, new List<Resource>() { Resource.Mercury, Resource.Sulfur, Resource.Lead } }
        };

        public static IReadOnlyDictionary<AlchemistType, List<Resource>> NeededResources => neededResources;

        public Alchemist(AlchemistType type, IAlchemistsQueue queue)
        {
            Type = type;
            this.queue = queue;
        }

        public void Run()
        {
            queue.AddAlchemistToQueue(Type);
            queue.WaitForResources(Type);
            Consume();
        }

        private void Consume()
        {
            Print($"ALCHEMIST {Type} ACQUIRED RESOURCES");
        }

        private void Print(string message)
        {
            Console.WriteLine($"{"[ALCHEMIST]", Configuration.EntityNameLength} {message}");
        }

        public enum AlchemistType
        {
            A,
            B,
            C,
            D
        }
    }
}