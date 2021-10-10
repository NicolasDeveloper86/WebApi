using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsFirstNameLessThanSeventyFiveCharacters : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(!(entity.FirstName.Length <= 75))
            {
                return new ValidationError(nameof(entity.FirstName), $"{nameof(entity.FirstName)} must not exceed 75 characters long");
            }

            return null;
        }
    }
}
