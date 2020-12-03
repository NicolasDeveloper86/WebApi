using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsPhoneNumberNotNullAndNotEmpty : ISpecificationNonNullNotEmpty<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(string.IsNullOrEmpty(entity.PhoneNumber))
            {
                return new ValidationError(nameof(entity.PhoneNumber), $"{nameof(entity.PhoneNumber)} must not me null or empty");
            }

            return null;
        }
    }
}
