namespace CoelsaCommon.Validation.Specification
{
    public static class SpecificationExtensions
    {
        public static IChainedSpecification<T> AfterNonNullableOrEmpty<T>(this ISpecificationNonNullNotEmpty<T> specNonNull, ISpecification<T>[] specifications) 
        {
            return new AfterNonNullableOrEmptySpecification<T>(specNonNull, specifications);
        }
    }
}
