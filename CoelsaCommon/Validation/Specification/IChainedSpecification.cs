using System.Collections.Generic;

namespace CoelsaCommon.Validation.Specification
{
    public interface IChainedSpecification<T>
    {
        public List<ValidationError> IsSatisfiedBy(T entity);
    }
}