using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsPhoneNumberTenDigitsLong : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(!(entity.PhoneNumber.Length == 10))
            {
                return new ValidationError(nameof(entity.PhoneNumber), $"{nameof(entity.PhoneNumber)} must be exactly 10 digits");
            }

            return null;
        }
    }
}
