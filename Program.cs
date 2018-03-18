using System;
using System.Threading;
using static Alchemy.Alchemist;

namespace Alchemy
{
    class Program
    {
        private static int numberOfWarlocks = 3;
        private static int numberOfSorcerers = 3;

        private static void Main(string[] args)
        {
            HandleArguments(args);

            var (store, queue) = InitializeDistribution();
            var factories = InitializeFactories(store);
            InitializeWarlocks(factories);
            InitializeSorcerers(factories);
            InitializeAlchemists(queue);
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

        private static (Store, AlchemistsQueue) InitializeDistribution()
        {
            var dispatcher = new Dispatcher();
            var store = new Store(dispatcher);
            var queue = new AlchemistsQueue(dispatcher);
            var distributor = new Distributor(dispatcher, store, queue);

            new Thread(() => store.Run()).Start();
            new Thread(() => queue.Run()).Start();
            new Thread(() => distributor.Run()).Start();

            return (store, queue);
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

        private static void InitializeAlchemists(IAlchemistsQueue queue)
        {
            var random = new Random();
            var alchemistTypes = Enum.GetValues(typeof(AlchemistType));

            for (int i = 0; i < Configuration.NumberOfAlchemists; i++)
            {
                var type = (AlchemistType) alchemistTypes.GetValue(random.Next(alchemistTypes.Length));
                var alchemist = new Alchemist(type, queue);
                new Thread(() => alchemist.Run()).Start();
            }
        }
    }
}
