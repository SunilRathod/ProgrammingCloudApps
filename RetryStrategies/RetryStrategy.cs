using System;
using System.Threading;

namespace RetryStrategies
{
    public static class RetryStrategy
    {
        private const int Second = 1000;

        public static void TightLoop(int retryAttempts, Action executeLogic)
        {           
            int currentAttemptNumber = 0;
            do
            {
                try
                {
                    executeLogic();
                    break;
                }
                catch (Exception ex)
                {                    
                    if (++currentAttemptNumber >= retryAttempts) break;
                    Console.WriteLine("Attempt failed.");                   
                    Console.WriteLine(string.Format("Tight Loop: Retrying attempt: {0}/{1} at time: {2} seconds.", currentAttemptNumber, retryAttempts, DateTime.Now));
                }
            } while (true);
        }

        public static void ConstantTimeInterval(int retryAttempts, int interval, Action executeLogic)
        {                       
            int currentAttemptNumber = 0;
            do
            {
                try
                {
                    executeLogic();
                    break;
                }
                catch (Exception ex)
                {                    
                    if (++currentAttemptNumber >= retryAttempts) break;
                    Console.WriteLine("Attempt failed.");
                    Console.WriteLine(string.Format("Constant Loop: Retrying attempt {0}/{1} at time {2} with an interval of {3} seconds.", currentAttemptNumber, retryAttempts, DateTime.Now, interval));
                    Thread.Sleep(interval * Second);
                }
            } while (true);
        }

        public static void RandomInterval(int retryAttempts, int intervalMin, int intervalMax, Action executeLogic)
        {
            Random randomValueGenerator = new Random();           
            int minInterval = intervalMin;
            int maxInterval = intervalMax;
            int interval = 0;
            int currentAttemptNumber = 0;

            do
            {
                try
                {
                    executeLogic();
                    break;
                }
                catch (Exception ex)
                {                    
                    if (++currentAttemptNumber >= retryAttempts) break;
                    Console.WriteLine("Attempt failed.");
                    Console.WriteLine(string.Format("Random interval Loop: Retrying attempt {0}/{1} at time {2} with an interval of {3} seconds.", currentAttemptNumber, retryAttempts, DateTime.Now, interval));
                    interval = randomValueGenerator.Next(minInterval, maxInterval);
                    Thread.Sleep(interval * Second);
                }
            } while (true);
        }

        public static void ExponentialBackOff(int retryAttempts, int minInterval, int maxInterval, Action executeLogic)
        {
                    
            int interval = minInterval;
            int exponent = 2;
            int currentAttemptNumber = 0;
            do
            {
                try
                {
                    executeLogic();
                    break;
                }
                catch (Exception ex)
                {
                    if (++currentAttemptNumber >= retryAttempts) break;
                    Console.WriteLine("Attempt failed.");
                    Console.WriteLine(string.Format("Exponential Backoff Loop: Retrying attempt {0}/{1} at time {2} with an interval of {3} seconds.", currentAttemptNumber, retryAttempts, DateTime.Now, interval));
                    Thread.Sleep(interval * Second);
                    interval = Math.Min(maxInterval, interval * exponent);
                }
            } while (true);

        }
    }
}
