using System;
using System.Threading;

namespace Alchemy
{
    public class Sorcerer
    {
        private Factory[] factories;

        public Sorcerer(Factory[] factories)
        {
            this.factories = factories;
        }

        public void Run()
        {
            var random = new Random();

            while (true)
            {
                var index = random.Next(0, 3);
                var factory = factories[index];

                factory.SemCurses.Wait();
                
                if (factory.Curses > 0)
                {
                    factory.Curses--;
                    Print(factory.Resource, $"{factory.Curses} curses");
                    
                    if (factory.Curses == 0)
                    {
                        factory.SemClean.Release();
                        Print(factory.Resource, "CLEAN");
                    }
                }

                factory.SemCurses.Release();

                var timeToSleep = random.Next(Configuration.SorcererMinSleepTimeMs, Configuration.SorcererMaxSleepTimeMs);
                Thread.Sleep(timeToSleep);
            }
        }

        private void Print(Resource resource, string message)
        {
            Console.WriteLine($"[SORCERER] {resource} factory: {message}");
        }
    }
}