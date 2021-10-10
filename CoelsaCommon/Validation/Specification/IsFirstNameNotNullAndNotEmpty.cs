using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    class IsFirstNameNotNullAndNotEmpty : ISpecificationNonNullNotEmpty<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(string.IsNullOrEmpty(entity.FirstName))
            {
                return new ValidationError(nameof(entity.FirstName), $"{nameof(entity.FirstName)} must not be null or empty");
            }

            return null;
        }
    }
}
