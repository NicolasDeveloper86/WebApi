namespace CoelsaCommon.Validation
{
    public class ValidationError
    {
        public string Property { get; set; }
        public string Reason { get; set; }

        public ValidationError(string property, string reason)
        {
            Property = property;
            Reason = reason;
        }
    }
}
