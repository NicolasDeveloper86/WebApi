namespace CoelsaCommon.Models
{
    public class ContactFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public ContactFilterModel()
        {
            Limit = 5;
            Term = string.Empty;
        }
    }
}
