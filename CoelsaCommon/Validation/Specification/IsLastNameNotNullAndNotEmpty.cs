using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsLastNameNotNullAndNotEmpty : ISpecificationNonNullNotEmpty<Contact>
    {

        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(string.IsNullOrEmpty(entity.LastName))
            {
                return new ValidationError(nameof(entity.LastName), $"{nameof(entity.LastName)} must not be null or empty");
            }

            return null;
        }
    }
}
