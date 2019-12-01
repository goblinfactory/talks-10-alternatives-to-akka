namespace QuoteShared
{
    // RFQ - Request for Quotation
    public class RFQ
    {
        public RFQ()
        {

        }

        public RFQ(string driver, int age, string regNo)
        {
            Driver = driver; Age = age; RegNo = regNo;
        }
        public string Driver { get; set; }
        public int Age { get; set; }
        public string RegNo { get; set; }
        public override string ToString()
        {
            return $"{Driver,-20} {Age,-3} {RegNo,-8}";
        }
    }
}