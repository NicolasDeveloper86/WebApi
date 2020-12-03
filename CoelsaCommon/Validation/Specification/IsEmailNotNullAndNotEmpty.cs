using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsEmailNotNullAndNotEmpty : ISpecificationNonNullNotEmpty<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(string.IsNullOrEmpty(entity.Email))
            {
                return new ValidationError(nameof(entity.Email), $"{nameof(entity.Email)} cannot be null or empty");
            }

            return null;
        }
    }
}
