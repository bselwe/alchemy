using System.Threading;

namespace Alchemy
{
    public class Dispatcher : IDispatcher
    {
        private readonly SemaphoreSlim SemDispatch;

        public Dispatcher()
        {
            SemDispatch = new SemaphoreSlim(0);
        }

        public void DispatchDistribution()
        {
            SemDispatch.Release();
        }

        public void WaitForDispatch()
        {
            SemDispatch.Wait();
        }
    }

    public interface IDispatcher
    {
        void DispatchDistribution();
        void WaitForDispatch();
    }
}