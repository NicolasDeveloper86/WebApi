using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsPhoneNumberANumber : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if (!(long.TryParse(entity.PhoneNumber, out long result)))
            {
                return new ValidationError(nameof(entity.PhoneNumber), $"{nameof(entity.PhoneNumber)} must be a number");
            }

            return null;
        }
    }
}
