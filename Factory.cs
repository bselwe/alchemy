using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Factory
    {
        private readonly IStore store;
        private Random random;

        public Resource Resource { get; private set; }
        public int Curses { get; set; }
        
        public SemaphoreSlim SemCurses { get; private set; } // Blocking access to curses
        public SemaphoreSlim SemClean { get; private set; } // Indicates whether the factory is cursed
        
        public Factory(Resource resource, IStore store)
        {
            Resource = resource;
            Curses = 0;
            SemCurses = new SemaphoreSlim(1, 1);
            SemClean = new SemaphoreSlim(1, 1);

            this.store = store;
            this.random = new Random(Guid.NewGuid().GetHashCode());
        }

        public void Run()
        {
            while (true)
            {
                Print("WAITING");
                store.WaitForCapacity(Resource);
                SemClean.Wait();

                int resources = store.AddResource(Resource);
                Print($"PRODUCED ({resources})");
                
                SemClean.Release();

                var timeToSleep = random.Next(Configuration.FactoryMinSleepTimeMs, Configuration.FactoryMaxSleepTimeMs);
                Thread.Sleep(timeToSleep);
            }
        }

        private void Print(string message)
        {
            Console.WriteLine($"{"[FACTORY]", Configuration.EntityNameLength} {Resource} factory: {message}");
        }
    }

    public enum Resource
    {
        Lead,
        Sulfur,
        Mercury
    }
}