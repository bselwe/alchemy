using System;
using System.Linq;
using static Alchemy.Alchemist;

namespace Alchemy
{
    public class Distributor
    {
        private readonly IDispatcher dispatcher;
        private readonly Store store;
        private readonly AlchemistsQueue queue;

        public Distributor(IDispatcher dispatcher, Store store, AlchemistsQueue queue)
        {
            this.dispatcher = dispatcher;
            this.store = store;
            this.queue = queue;
        }

        public void Run()
        {
            while (true)
            {
                dispatcher.WaitForDispatch();
                DistributeResources();
            }
        }

        public void DistributeResources()
        {
            queue.SemAlchemists.Wait();
            store.SemResources.Wait();

            Print("TRYING TO DISTRIBUTE RESOURCES");

            TrySatisfyAlchemists(AlchemistType.D);
            TrySatisfyAlchemists(AlchemistType.A);
            TrySatisfyAlchemists(AlchemistType.B);
            TrySatisfyAlchemists(AlchemistType.C);

            store.SemResources.Release();
            queue.SemAlchemists.Release();
        }

        private void TrySatisfyAlchemists(AlchemistType alchemist)
        {
            while (queue.Alchemists[alchemist] > 0)
            {
                if (CanSatisfyAlchemist(alchemist))
                    SatisfyAlchemist(alchemist);
                else
                    break;
            }
        }

        private void SatisfyAlchemist(AlchemistType alchemist)
        {
            foreach (var resource in Alchemist.NeededResources[alchemist])
            {
                store.Resources[resource]--;
                store.SemCapacity[resource].Release();
            }

            queue.SemAlchemistsQueue[alchemist].Release();
            queue.Alchemists[alchemist]--;
        }

        private bool CanSatisfyAlchemist(AlchemistType alchemist)
        {
            var neededResources = Alchemist.NeededResources[alchemist];
            return neededResources.All(resource => store.Resources[resource] > 0);
        }

        private void Print(string message)
        {
            Console.WriteLine($"{"[DISTRIBUTOR]", Configuration.EntityNameLength} {message}");
        }
    }
}