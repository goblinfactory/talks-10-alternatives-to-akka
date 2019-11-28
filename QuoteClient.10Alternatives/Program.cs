using System;

namespace QuoteClient._10Alternatives
{
    class Program
    {
        static void Main(string[] args)
        {
            // we want to process quotes as fast as possible,
            // so we take an "un-ending" stream of incoming quotes, and we fire them off 
            // against a panel of providers that will take between 100 to 3000ms to respond. 
            // some will give us errors
            // some will tell us to slow down (back off)
            // and others will simply timeout, become unavailable.
        }
    }
}
