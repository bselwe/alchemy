using System;

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
            Console.WriteLine("!!! DISTRIBUTE RESOURCES !!!");
        }
    }
}