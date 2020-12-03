using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsCompanyLessThanSeventyFiveCharacters : ISpecification<Contact>
    {
        public ValidationError IsSatisfiedBy(Contact entity)
        {
            if(!(entity.Company.Length <= 75))
            {
                return new ValidationError(nameof(entity.Company), $"{nameof(entity.Company)} cannot have more than 75 characters");
            }

            return null;
        }
    }
}
