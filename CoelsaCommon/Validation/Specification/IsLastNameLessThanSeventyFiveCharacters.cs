using CoelsaCommon.Models;

namespace CoelsaCommon.Validation.Specification
{
    public class IsLastNameLessThanSeventyFiveCharacters : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            if(!(entity.LastName.Length <= 75))
            {
                return new ValidationError(nameof(entity.LastName), $"{nameof(entity.LastName)} must not exceed 75 characters");
            }

            return null;
        }
    }
}
