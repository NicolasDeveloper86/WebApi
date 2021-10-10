using System;

namespace CoelsaCommon.Validation
{
    public class ValidationException : Exception
    {
        public string Field { get; set; }
        public ValidationExceptionExtraData ExtraData { get; set; }

        public ValidationException(string message, string field, ValidationExceptionExtraData extraData = null) : base(message)
        {
            Field = field;
            ExtraData = extraData;
        }
    }
}
