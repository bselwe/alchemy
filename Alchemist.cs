using System.Collections.Generic;

namespace Alchemy
{
    public class Alchemist
    {
        private readonly IAlchemistsQueue queue;

        public AlchemistType Type { get; private set; }

        private List<Resource> neededResources;
        public IReadOnlyList<Resource> NeededResources => neededResources;

        public Alchemist(AlchemistType type, IAlchemistsQueue queue)
        {
            Type = type;
            this.queue = queue;

            InitializeNeededResources();
        }

        public void Run()
        {
            queue.AddAlchemistToQueue(Type);
            queue.WaitForResources(Type);
        }

        private void InitializeNeededResources()
        {   
            switch (Type)
            {
                case AlchemistType.A:
                    neededResources = new List<Resource>() { Resource.Lead, Resource.Mercury };
                    break;
                case AlchemistType.B:
                    neededResources = new List<Resource>() { Resource.Mercury, Resource.Sulfur };
                    break;
                case AlchemistType.C:
                    neededResources = new List<Resource>() { Resource.Lead, Resource.Sulfur };
                    break;
                case AlchemistType.D:
                    neededResources = new List<Resource>() { Resource.Mercury, Resource.Sulfur, Resource.Lead };
                    break;
            }
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