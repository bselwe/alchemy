using System;
using System.Collections.Generic;
using System.Threading;
using static Alchemy.Alchemist;

namespace Alchemy
{
    public class AlchemistsQueue : IAlchemistsQueue
    {
        public Dictionary<AlchemistType, int> Alchemists { get; private set; }
        public Dictionary<AlchemistType, SemaphoreSlim> SemAlchemistsQueue { get; private set; }
        public SemaphoreSlim SemAlchemists { get; private set; } // Blocking access to alchemists
        public SemaphoreSlim SemNewAlchemist { get; private set; } // Used to inform about a new alchemist

        public AlchemistsQueue()
        {
            Initialize();
        }

        public void Run()
        {
            while (true)
            {
                SemNewAlchemist.Wait();

                Print("NEW ALCHEMIST!");
            }
        }

        public void AddAlchemistToQueue(AlchemistType alchemist)
        {
            SemAlchemists.Wait();
            Alchemists[alchemist]++;
            if (Alchemists[alchemist]++ == 0)
                SemNewAlchemist.Release();
            SemNewAlchemist.Release();
        }

        public void WaitForResources(AlchemistType alchemist)
        {
            SemAlchemistsQueue[alchemist].Wait();
        }

        private void Initialize()
        {
            Alchemists = new Dictionary<AlchemistType, int>();
            SemAlchemistsQueue = new Dictionary<AlchemistType, SemaphoreSlim>();
            SemAlchemists = new SemaphoreSlim(1, 1);
            SemNewAlchemist = new SemaphoreSlim(0);

            foreach (AlchemistType alchemist in Enum.GetValues(typeof(AlchemistType)))
            {
                Alchemists[alchemist] = 0;
                SemAlchemistsQueue[alchemist] = new SemaphoreSlim(0);
            }
        }

        private void Print(string message)
        {
            Console.WriteLine($"   [QUEUE] {message}");
        }
    }

    public interface IAlchemistsQueue
    {
        void AddAlchemistToQueue(AlchemistType alchemist);
        void WaitForResources(AlchemistType alchemist);
    }
}