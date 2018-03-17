using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alchemy
{
    public class Store : IStore
    {
        public Dictionary<Resource, int> Resources { get; private set; }
        public Dictionary<Resource, SemaphoreSlim> SemCapacity { get; private set; } // Current capacity of factories
        public SemaphoreSlim SemResources { get; private set; } // Blocking access to resources
        public SemaphoreSlim SemNewResources { get; private set; } // Used to inform about a new product

        public Store()
        {
            InitializeResources();
        }

        public void Run()
        {
            while (true)
            {
                SemNewResources.Wait();
                Console.WriteLine("   [STORE] NEW RESOURCE!");
                Console.WriteLine($"   [STORE] {Resource.Lead}: {Resources[Resource.Lead]}, {Resource.Sulfur}: {Resources[Resource.Sulfur]}, {Resource.Mercury}: {Resources[Resource.Mercury]}");
            }
        }

        private void InitializeResources()
        {
            Resources = new Dictionary<Resource, int>();
            SemCapacity = new Dictionary<Resource, SemaphoreSlim>();
            SemResources = new SemaphoreSlim(1, 1);
            SemNewResources = new SemaphoreSlim(0);
            
            foreach (Resource resource in Enum.GetValues(typeof(Resource)))
            {
                Resources[resource] = 0;
                SemCapacity[resource] = new SemaphoreSlim(2, 2);
            }
        }
    }

    public interface IStore
    {
        Dictionary<Resource, int> Resources { get; }
        Dictionary<Resource, SemaphoreSlim> SemCapacity { get; } // Current capacity of factories
        SemaphoreSlim SemResources { get; } // Blocking access to resources
        SemaphoreSlim SemNewResources { get; } // Used to inform about a new product
    }
}