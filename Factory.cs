using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Factory
    {
        public Resource Resource { get; private set; }
        public IStore Store { get; private set; }
        public int Curses { get; set; }
        
        public SemaphoreSlim SemCurses { get; private set; } // Blocking access to curses
        public SemaphoreSlim SemClean { get; private set; } // Indicates whether the factory is cursed
        
        public Factory(Resource resource, IStore store)
        {
            Resource = resource;
            Store = store;
            SemClean = new SemaphoreSlim(1, 1);
            SemCurses = new SemaphoreSlim(1, 1);
        }

        public void Run()
        {
            var random = new Random();

            while (true)
            {
                Print("waiting");
                Store.SemCapacity[Resource].Wait();
                SemClean.Wait();

                Store.SemResources.Wait();
                Print($"producing ({Store.Resources[Resource] + 1})");
                Store.Resources[Resource]++;
                Store.SemResources.Release();
                Store.SemNewResources.Release();
                
                SemClean.Release();

                var timeToSleep = random.Next(Configuration.FactoryMinSleepTimeMs, Configuration.FactoryMaxSleepTimeMs);
                Thread.Sleep(timeToSleep);
            }
        }

        private void Print(string message)
        {
            Console.WriteLine($" [FACTORY] {Resource} factory: {message}");
        }
    }

    public enum Resource
    {
        Lead,
        Sulfur,
        Mercury
    }
}