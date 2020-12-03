using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsCompanyNotNullAndNotEmpty : ISpecificationNonNullNotEmpty<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(string.IsNullOrEmpty(entity.Company))
            {
                return new ValidationError(nameof(entity.Company), $"{nameof(entity.Company)} cannot be null or empty");
            }

            return null;
        }
    }
}
