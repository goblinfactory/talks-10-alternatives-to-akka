namespace QuoteService.Models
{
    // todo swap out the configured serializer so that we can use immutable dto's
    // for now, allow them to be modified.
    public class CarInsuranceRequest
    {
        public CarInsuranceRequest()
        {

        }

        public CarInsuranceRequest(string driver, int age, string regNo, string car)
        {
            Driver = driver; Age = age; RegNo = regNo; Car = car;
        }
        public string Driver { get; set; }
        public int Age { get; set; }
        public string RegNo { get; set; }
        public string Car { get; set; }
    }
}

