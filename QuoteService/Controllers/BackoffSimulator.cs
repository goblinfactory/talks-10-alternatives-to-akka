using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuoteService.Controllers
{
    internal static class BackoffSimulator
    {
        private static Random _rnd = new Random();
        private static Dictionary<Type, DateTime> _activeThrottles = new Dictionary<Type, DateTime>();

        internal static bool IsCurrentlyLimiting<T>(DateTime now)
        {
            var t = typeof(T);
            if (!_activeThrottles.ContainsKey(t)) return false;
            var throttleUntil = _activeThrottles[t];
            var isStillActive = now < throttleUntil;
            if (!isStillActive)
            {
                _activeThrottles.Remove(t);
                return false;
            }

            return true;
        }

        /// <summary>
        /// return the amount of time (random delay between 5 and 15 seconds) we need to spread the queue of customer's load across...
        /// </summary>
        internal static int HowLong<T>()
        {
            // hard code to 10 seconds
            return 5 + _rnd.Next(10);
        }

        internal static void TakeServiceOfflineUntil<T>(DateTime until)
        {
            var t = typeof(T);
            _activeThrottles.Add(t, until);
        }

    }
}
