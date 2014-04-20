using System;
using System.Threading.Tasks;
using System.Threading;

namespace Pong
{
    class PeriodicTick
    {
        private int tickInterval; // Defines how often you want the Tick event to be raised in milliseconds
        public CancellationTokenSource CancellationTokenSrc { get; set; }
        public Task TickTask { get; private set; }
        public event TickEventHandler Tick;

        public PeriodicTick(int tickInterval)
        {
            this.tickInterval = tickInterval;
            CancellationTokenSrc = new CancellationTokenSource();
        }

        public void Start()
        {
            var token = CancellationTokenSrc.Token;
            TickTask = Task.Factory.StartNew(
                () => 
                {
                    Thread.CurrentThread.Name = "TickThread";
                    while (true)
                    {
                        if (token.IsCancellationRequested) { break; }
                        Task.Delay(tickInterval).Wait(); // Wait for tickInterval milliseconds...
                        Tick(); // ...Then tick!
                    }
                },
                token);
        }
    }
}
