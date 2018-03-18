using System;
using System.Threading;

namespace Alchemy   
{
    public class Warlock
    {
        private Factory[] factories;
        private Random random;

        public Warlock(Factory[] factories)
        {
            this.factories = factories;
            this.random = new Random(Guid.NewGuid().GetHashCode());
        }

        public void Run() 
        {
            while (true)
            {
                var index = random.Next(0, Configuration.NumberOfFactories);
                var factory = factories[index];

                factory.SemCurses.Wait();
                factory.Curses++;
                // Print(factory.Resource, $"{factory.Curses} curses");

                if (factory.Curses == 1)
                {
                    factory.SemClean.Wait();
                    Print(factory.Resource, "CURSED");
                }

                factory.SemCurses.Release();

                var timeToSleep = random.Next(Configuration.WarlockMinSleepTimeMs, Configuration.WarlockMaxSleepTimeMs);
                Thread.Sleep(timeToSleep);
            }
        }

        private void Print(Resource resource, string message)
        {
            Console.WriteLine($" [WARLOCK] {resource} factory: {message}");
        }
    }
}