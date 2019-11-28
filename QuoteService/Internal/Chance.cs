using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteService.Internal
{
    public class Chance
    {
        private readonly Availability _availability;
        private Random _rnd = new Random(12345); // try to make this repeatable

        public Chance(Availability availability)
        {
            _availability = availability;
        }

        public bool ShouldBackOff()
        {
            return Should();
        }
        
        public bool GoBoom()
        {
            // 1 in a 200 quotes will totally take down the entire service!
            // we're pushing 600 quotes through the system, therefore expect
            // approximately 6 or so services to go boom, and stop responding.
            // probably par for course?

            // iterestingly GoBoom does not care about service reliability!
            // even your very very best and most reliable upstream providers
            // WILL let you down from time to time.
            // -------------------------------------------------------------
            return (_rnd.Next(200) == 17); // one exact number
        }
        private bool Should()
        {
            switch(_availability)
            {
                case Availability.Excellent: return false;
                case Availability.Good: return (_rnd.Next(100) > 10);
                case Availability.Average: return (_rnd.Next(100) > 30);
                case Availability.Spotty: return (_rnd.Next(100) > 50);
                case Availability.Bad: return (_rnd.Next(100) > 90);
                default: return false;
            }
        }

    }
}
