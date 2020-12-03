namespace CoelsaCommon.Validation.Specification
{
    public interface ISpecification<T>
    {
        ValidationError IsSatisfiedBy(T entity);
    }
}
