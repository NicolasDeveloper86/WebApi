using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsEmailLessThanOneHundredCharacters : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(!(entity.Email.Length <= 100))
            {
                return new ValidationError(nameof(entity.Email), $"{nameof(entity.Email)} cannot have more than 100 characters");
            }

            return null;
        }
    }
}
