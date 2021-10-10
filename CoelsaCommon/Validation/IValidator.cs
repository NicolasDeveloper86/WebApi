using System.Collections.Generic;

namespace CoelsaCommon.Validation
{
    public interface IValidator<T>
    {
        void Validate(T entity);
    }
}