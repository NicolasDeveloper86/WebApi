namespace CoelsaCommon.Models
{
    public abstract class FilterModelBase
    {
        public int Page { get; set; }
        public int Limit { get; set; }

        public FilterModelBase()
        {
            Page = 1;
            Limit = 10;
        }
    }
}
