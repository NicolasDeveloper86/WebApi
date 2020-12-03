using System;
using System.Collections.Generic;

namespace CoelsaCommon.Validation.Specification
{
    public class AfterNonNullableOrEmptySpecification<T> : IChainedSpecification<T>
    {
        private readonly ISpecificationNonNullNotEmpty<T> _specNull;
        private readonly ISpecification<T>[] _specifications;

        public AfterNonNullableOrEmptySpecification(ISpecificationNonNullNotEmpty<T> specNull,
            ISpecification<T>[] specifications)
        {
            _specNull = specNull ?? throw new ArgumentException(_specNull.GetType().Name);
            _specifications = specifications ?? throw new ArgumentException(_specifications.GetType().Name);
        }

        /// <summary>
        /// Method that returns a possible List of Validation Errors
        /// </summary>
        /// <param name="entity">the entity to validate</param>
        /// <returns>A List Of Validation Errors</returns>
        public List<ValidationError> IsSatisfiedBy(T entity)
        {
            List<ValidationError> errors = new List<ValidationError>();
            var notNullEmptyRule = _specNull.IsSatisfiedBy(entity);

            if (notNullEmptyRule == null)
            {
                foreach (var spec in _specifications)
                {
                    if(spec.IsSatisfiedBy(entity) != null)
                    {
                        errors.Add(spec.IsSatisfiedBy(entity));
                    }
                }
            }
            else
            {
                errors.Add(notNullEmptyRule);
            }

            return errors;
        }
    }
}
