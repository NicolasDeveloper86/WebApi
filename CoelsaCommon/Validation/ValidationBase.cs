using System.Collections.Generic;

namespace CoelsaCommon.Validation
{
    public abstract class ValidationBase
    {
        public List<ValidationError> Errors { get; set; }

        public ValidationBase()
        {
            Errors = new List<ValidationError>();
        }

        public void ValidateId(int id)
        {
            if (id <= 0)
            {
                var extraData = new ValidationExceptionExtraData(id);
                throw new ValidationException("Id must have a correct value. Greater than 0", "Id", extraData);
            }
        }

    }
}
