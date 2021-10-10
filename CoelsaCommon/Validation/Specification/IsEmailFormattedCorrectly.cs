using CoelsaCommon.Models;
using System.Text.RegularExpressions;

namespace CoelsaCommon.Validation.Specification
{
    public class IsEmailFormattedCorrectly : ISpecification<Contact>
    {
        ValidationError ISpecification<Contact>.IsSatisfiedBy(Contact entity)
        {
            Regex validateMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match matches = validateMail.Match(entity.Email);

            if(!(matches.Success))
            {
                return new ValidationError(nameof(entity.Email), $"{nameof(entity.Email)} is not correctly formatted");
            }

            return null;
        }
    }
}
