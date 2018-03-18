using System;
using System.Threading;

namespace Alchemy
{
    public class Sorcerer
    {
        private Factory[] factories;
        private Random random;

        public Sorcerer(Factory[] factories)
        {
            this.factories = factories;
            this.random = new Random(Guid.NewGuid().GetHashCode());
        }

        public void Run()
        {
            while (true)
            {
                for (int i = 0; i < Configuration.NumberOfFactories; i++)
                {
                    var factory = factories[i];
                    factory.SemCurses.Wait();
                    
                    if (factory.Curses > 0)
                    {
                        factory.Curses--;
                        // Print(factory.Resource, $"{factory.Curses} CURSES");
                        
                        if (factory.Curses == 0)
                        {
                            factory.SemClean.Release();
                            Print(factory.Resource, "CLEAN");
                        }
                    }

                    factory.SemCurses.Release();
                }

                var timeToSleep = random.Next(Configuration.SorcererMinSleepTimeMs, Configuration.SorcererMaxSleepTimeMs);
                Thread.Sleep(timeToSleep);
            }
        }

        private void Print(Resource resource, string message)
        {
            Console.WriteLine($"{"[SORCERER]", Configuration.EntityNameLength} {resource} factory: {message}");
        }
    }
}