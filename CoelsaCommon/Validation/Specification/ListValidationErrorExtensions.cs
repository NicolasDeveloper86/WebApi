using System.Collections.Generic;

namespace CoelsaCommon.Validation.Specification
{
    public static class ListValidationErrorExtensions
    {
        public static List<ValidationError> AppendAllErrors(this List<ValidationError> errors, List<ValidationError>[] arrayOfListOfValidationErrors)
        {
            foreach (var listValidationError in arrayOfListOfValidationErrors)
            {
                if(listValidationError != null)
                {
                    foreach (var validationError in listValidationError)
                    {
                        if (validationError != null)
                        {
                            errors.Add(validationError);
                        }
                    }
                }

            }

            return errors;
        }
    }
}
