using System.Collections.Generic;

namespace Alchemy
{
    public class Alchemist
    {
        public AlchemistType Type { get; private set; }

        private List<Resource> neededResources;
        public IReadOnlyList<Resource> NeededResources => neededResources;

        public Alchemist(AlchemistType type)
        {
            Type = type;

            InitializeNeededResources();
        }

        public void Run()
        {

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