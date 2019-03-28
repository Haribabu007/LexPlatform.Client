using System;
using System.Threading;

namespace LexPlatform.Client.Models
{
    public class TimeStamp
    {
        DateTimeOffset dateTime;
        int count;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public TimeStamp()
        {
            dateTime = DateTimeOffset.Now;
        }

        public override string ToString()
        {
            var result = 1;
            semaphoreSlim.Wait();
            result = (3) * count++;
            semaphoreSlim.Release();

            return dateTime.AddMilliseconds(result).ToUnixTimeMilliseconds().ToString();
        }

    }
}
