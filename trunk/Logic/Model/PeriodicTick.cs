using System;
using System.Threading.Tasks;
using System.Threading;

namespace PongController
{
    public class PeriodicTick
    {
        private int tickInterval; // Defines how often you want the Tick event to be raised in milliseconds
        private readonly IGameController gameController;
        public CancellationTokenSource CancellationTokenSrc { get; set; }

        public PeriodicTick(int tickInterval, IGameController gameController)
        {
            this.tickInterval = tickInterval;
            CancellationTokenSrc = new CancellationTokenSource();
            this.gameController = gameController;
        }

        public void Start()
        {
            var token = CancellationTokenSrc.Token;
            Task.Factory.StartNew(
                () => 
                {
                    Thread.CurrentThread.Name = "TickThread";
                    while (true)
                    {
                        if (token.IsCancellationRequested) { break; }
                        Task.Delay(tickInterval).Wait(); // Wait for tickInterval milliseconds...
                        gameController.Refresh(); // ...Then tick!
                    }
                },
                token);
        }
    }
}
