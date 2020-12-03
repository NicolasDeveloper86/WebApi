using CoelsaCommon.Models;
using CoelsaCommon.Validation.Specification;
using System.Collections.Generic;

namespace CoelsaCommon.Validation
{
    public class ContactValidator : IValidator<Contact>
    {
        private readonly ISpecificationNonNullNotEmpty<Contact> companyNotNullNotEmpty = new IsCompanyNotNullAndNotEmpty();
        private readonly ISpecification<Contact> companyIsLessThanSeventyFiveCharacters = new IsCompanyLessThanSeventyFiveCharacters();
        
        private readonly ISpecificationNonNullNotEmpty<Contact> emailNotNullNotEmpty = new IsEmailNotNullAndNotEmpty();
        private readonly ISpecification<Contact> emailFormattedCorrectly = new IsEmailFormattedCorrectly();
        private readonly ISpecification<Contact> emailIsLessThanOneHundredCharacters = new IsEmailLessThanOneHundredCharacters();

        private readonly ISpecificationNonNullNotEmpty<Contact> firstNameNotNullNotEmpty = new IsFirstNameNotNullAndNotEmpty();
        private readonly ISpecification<Contact> firstNameIsLessThanSeventyFiveCharacters = new IsFirstNameLessThanSeventyFiveCharacters();

        private readonly ISpecificationNonNullNotEmpty<Contact> lastNameNotNullNotEmpty = new IsLastNameNotNullAndNotEmpty();
        private readonly ISpecification<Contact> lastNameIsLessThanSeventyFiveCharacters = new IsLastNameLessThanSeventyFiveCharacters();

        private readonly ISpecificationNonNullNotEmpty<Contact> phoneNumberNotNullNotEmpty = new IsPhoneNumberNotNullAndNotEmpty();
        private readonly ISpecification<Contact> phoneNumberIsANumber = new IsPhoneNumberANumber();
        private readonly ISpecification<Contact> phoneNumberHaveTenDigits = new IsPhoneNumberTenDigitsLong();

        /// <summary>
        /// Validates if the Contact Entity is in a valid state
        /// </summary>
        /// <param name="entity">The Contact to validate</param>
        public void Validate(Contact entity)
        {
            var ruleCompany = companyNotNullNotEmpty.AfterNonNullableOrEmpty(new ISpecification<Contact>[] { companyIsLessThanSeventyFiveCharacters });
            var ruleEmail = emailNotNullNotEmpty.AfterNonNullableOrEmpty(new ISpecification<Contact>[] { emailFormattedCorrectly, emailIsLessThanOneHundredCharacters });
            var ruleFirstName = firstNameNotNullNotEmpty.AfterNonNullableOrEmpty(new ISpecification<Contact>[] { firstNameIsLessThanSeventyFiveCharacters });
            var ruleLastName = lastNameNotNullNotEmpty.AfterNonNullableOrEmpty(new ISpecification<Contact>[] { lastNameIsLessThanSeventyFiveCharacters });
            var rulePhoneNumber = phoneNumberNotNullNotEmpty.AfterNonNullableOrEmpty(new ISpecification<Contact>[] { phoneNumberIsANumber, phoneNumberHaveTenDigits });

            var isCompanyValid = ruleCompany.IsSatisfiedBy(entity);
            var isEmailValid = ruleEmail.IsSatisfiedBy(entity);
            var isFirstNameValid = ruleFirstName.IsSatisfiedBy(entity);
            var isLastNameValid = ruleLastName.IsSatisfiedBy(entity);
            var isPhoneNumberValid = rulePhoneNumber.IsSatisfiedBy(entity);

            var errors = new List<ValidationError>().AppendAllErrors(new List<ValidationError>[] { isCompanyValid, isEmailValid, isFirstNameValid, isLastNameValid, isPhoneNumberValid });

            if(errors.Count > 0)
            {
                var extraData = new ValidationExceptionExtraData(errors);
                throw new ValidationException("Errors were detected, read the extra data for more information", string.Empty, extraData);
            }

        }
    }
}
