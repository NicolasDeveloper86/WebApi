using CoelsaCommon.Models;
using CoelsaCommon.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoelsaTests
{
    public class ContactValidatorTests
    {
        private ContactValidator _contactValidator;

        private void Init()
        {
            _contactValidator = new ContactValidator();
        }

        [Fact]
        public void Validate_Should_Failed_All_Fields_NotNull_Empty()
        {
            //Arrange
            Init();

            //Act
            Action validate = () => _contactValidator.Validate(new Contact(null, null, null, null, null));

            
            ValidationException exception = Assert.Throws<ValidationException>(validate);
            var listOfParamsFailed = exception.ExtraData.AdditionalData as List<ValidationError>;

            //Assert
            Assert.Equal(5, listOfParamsFailed.Count);
        }

        #region Validate Company
        [Fact]
        public void Validate_Should_Return_Exception_For_Null_Company()
        {
            //Arrange
            Init();

            Contact contactNullCompany = new Contact("First name", "Last name", null, "email", "1234567890");

            Action act = () => _contactValidator.Validate(contactNullCompany);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Equal("Company", extraData[0].Property);
            Assert.Equal("Company cannot be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Empty_Company()
        {
            //Arrange
            Init();

            Contact contactEmptyCompany = new Contact("", "Last name", "", "email", "123456789");

            Action act = () => _contactValidator.Validate(contactEmptyCompany);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Equal("Company", extraData[0].Property);
            Assert.Equal("Company cannot be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Company_With_More_Than_75_Characters()
        {
            //Arrange
            Init();

            string companyName = "";
            for (int i = 0; i < 76; i++)
            {
                companyName += "a";
            }

            Contact contactCompany = new Contact("First Name", "Last name", companyName, "FirstName@A.com", "1234567890");

            Action act = () => _contactValidator.Validate(contactCompany);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("Company", extraData[0].Property);
            Assert.Equal("Company cannot have more than 75 characters", extraData[0].Reason);
        }
        #endregion

        #region Email Validation
        [Fact]
        public void Validate_Should_Return_Exception_For_Null_Email()
        {
            //Arrange
            Init();

            Contact contactNullEmail = new Contact("First name", "Last name", "A", null, "1234567890");

            Action act = () => _contactValidator.Validate(contactNullEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("Email", extraData[0].Property);
            Assert.Equal("Email cannot be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Empty_Email()
        {
            //Arrange
            Init();

            Contact contactEmptyEmail = new Contact("First name", "Last name", "A", "", "1234567890");

            Action act = () => _contactValidator.Validate(contactEmptyEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("Email", extraData[0].Property);
            Assert.Equal("Email cannot be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Email_With_More_Than_100_Characters()
        {
            //Arrange
            Init();

            string email = "";
            for (int i = 0; i < 101; i++)
            {
                email += "a";
            }

            email += "@A.com";

            Contact contactEmail = new Contact("First Name", "Last name", "A", email, "1234567890");

            Action act = () => _contactValidator.Validate(contactEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("Email", extraData[0].Property);
            Assert.Equal("Email cannot have more than 100 characters", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Email_That_Is_Not_Formatted_As_Email()
        {
            //Arrange
            Init();

            Contact contactEmail = new Contact("First Name", "Last name", "A", "firstname#A.com", "1234567890");

            Action act = () => _contactValidator.Validate(contactEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("Email", extraData[0].Property);
            Assert.Equal("Email is not correctly formatted", extraData[0].Reason);
        }
        #endregion

        #region FirstName Validation
        [Fact]
        public void Validate_Should_Return_Exception_For_Null_FirstName()
        {
            //Arrange
            Init();

            Contact contactNullEmail = new Contact(null, "Last name", "A", "LastName@A.com", "1234567890");

            Action act = () => _contactValidator.Validate(contactNullEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("FirstName", extraData[0].Property);
            Assert.Equal("FirstName must not be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_Empty_FirstName()
        {
            //Arrange
            Init();

            Contact contactEmptyEmail = new Contact(string.Empty, "Last name", "A", "LastName@A.com", "1234567890");

            Action act = () => _contactValidator.Validate(contactEmptyEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("FirstName", extraData[0].Property);
            Assert.Equal("FirstName must not be null or empty", extraData[0].Reason);
        }

        [Fact]
        public void Validate_Should_Return_Exception_For_FirstName_With_More_Than_75_Characters()
        {
            //Arrange
            Init();

            string firstName = "";
            for (int i = 0; i < 76; i++)
            {
                firstName += "a";
            }

            Contact contactEmail = new Contact(firstName, "Last name", "A", "LastName@A.com", "1234567890");

            Action act = () => _contactValidator.Validate(contactEmail);

            //Act
            ValidationException exception = Assert.Throws<ValidationException>(act);

            //Assert
            Assert.NotNull(exception.ExtraData);
            Assert.Equal("Errors were detected, read the extra data for more information", exception.Message);

            List<ValidationError> extraData = exception.ExtraData.AdditionalData as List<ValidationError>;
            Assert.Single(extraData);
            Assert.Equal("FirstName", extraData[0].Property);
            Assert.Equal("FirstName must not exceed 75 characters long", extraData[0].Reason);
        }
        #endregion
    }
}
