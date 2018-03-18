using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alchemy
{
    public class Store : IStore
    {
        private readonly IDispatcher dispatcher;
        
        private SemaphoreSlim semNewResources; // Used to inform about a new product

        public Dictionary<Resource, SemaphoreSlim> SemCapacity { get; private set; } // Current capacity of factories
        public Dictionary<Resource, int> Resources { get; private set; }
        public SemaphoreSlim SemResources { get; private set; } // Blocking access to resources

        public Store(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            Initialize();
        }

        public void Run()
        {
            while (true)
            {
                semNewResources.Wait();
                
                Print("NEW RESOURCE");
                Print($"{Resource.Lead}: {Resources[Resource.Lead]}, {Resource.Sulfur}: {Resources[Resource.Sulfur]}, {Resource.Mercury}: {Resources[Resource.Mercury]}");

                dispatcher.DispatchDistribution();
            }
        }

        public int AddResource(Resource resource)
        {
            SemResources.Wait();
            int resources = ++Resources[resource];
            SemResources.Release();
            semNewResources.Release();
            return resources;
        }

        public void WaitForCapacity(Resource resource)
        {
            SemCapacity[resource].Wait();
        }

        private void Initialize()
        {
            Resources = new Dictionary<Resource, int>();
            SemCapacity = new Dictionary<Resource, SemaphoreSlim>();
            SemResources = new SemaphoreSlim(1, 1);
            semNewResources = new SemaphoreSlim(0);
            
            foreach (Resource resource in Enum.GetValues(typeof(Resource)))
            {
                Resources[resource] = 0;
                SemCapacity[resource] = new SemaphoreSlim(2, 2);
            }
        }

        private void Print(string message)
        {
            Console.WriteLine($"{"[STORE]", Configuration.EntityNameLength} {message}");
        }
    }

    public interface IStore
    {
        int AddResource(Resource resource);
        void WaitForCapacity(Resource resource);
    }
}