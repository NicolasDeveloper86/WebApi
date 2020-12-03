
namespace CoelsaCommon.Validation
{
    public class ValidationExceptionExtraData
    {
        public object AdditionalData { get; set; }
        public ValidationExceptionExtraData(object additionalData)
        {
            AdditionalData = additionalData;
        }
    }
}