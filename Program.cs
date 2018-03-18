using System;
using System.Threading;

namespace Alchemy
{
    class Program
    {
        private static int numberOfWarlocks = 3;
        private static int numberOfSorcerers = 3;

        private static void Main(string[] args)
        {
            HandleArguments(args);

            var store = InitializeStore();
            var queue = InitializeAlchemistsQueue();
            var factories = InitializeFactories(store);
            InitializeWarlocks(factories);
            InitializeSorcerers(factories);
        }

        private static void HandleArguments(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Length != 2)
                    throw new ArgumentException("Invalid number of arguments. Arguments: n m, n - warlocks, m - sorcerers.");
                
                if (!Int32.TryParse(args[0], out numberOfWarlocks) ||
                    !Int32.TryParse(args[1], out numberOfSorcerers))
                    throw new ArgumentException("All arguments must be integers. Arguments: n m, n - warlocks, m - sorcerers.");
            }
        }

        private static Store InitializeStore()
        {
            var store = new Store();
            new Thread(() => store.Run()).Start();
            return store;
        }

        private static AlchemistsQueue InitializeAlchemistsQueue()
        {
            var queue = new AlchemistsQueue();
            new Thread(() => queue.Run()).Start();
            return queue;
        }

        private static Factory[] InitializeFactories(IStore store)
        {
            var factories = new Factory[] 
            { 
                new Factory(Resource.Lead, store), 
                new Factory(Resource.Sulfur, store), 
                new Factory(Resource.Mercury, store)
            };

            foreach (var factory in factories)
            {
                new Thread(() => factory.Run()).Start();
            }

            return factories;
        }

        private static void InitializeWarlocks(Factory[] factories)
        {
            for (int i = 0; i < numberOfWarlocks; i++)
            {
                var warlock = new Warlock(factories);
                new Thread(() => warlock.Run()).Start();
            }
        }

        private static void InitializeSorcerers(Factory[] factories)
        {
            for (int i = 0; i < numberOfSorcerers; i++)
            {
                var sorcerer = new Sorcerer(factories);
                new Thread(() => sorcerer.Run()).Start();
            }
        }
    }
}
