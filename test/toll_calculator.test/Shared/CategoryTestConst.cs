namespace Rax.IocTools.Test.Shared;

internal static class CategoryTestConst
{
    internal static class Name
    {
        internal const string ClassTest = nameof(ClassTest);
        internal const string AcceptanceTest = nameof(AcceptanceTest);
        internal const string ClassIntegrationTest = nameof(ClassIntegrationTest);
        internal const string ClassCompositeTest = nameof(ClassCompositeTest);
        internal const string EdgeCaseTest = nameof(EdgeCaseTest);
    }
    internal static class Assembly
    {
        internal const string This = $"{nameof(Rax)}.{nameof(IocTools)}.{nameof(Test)}";
        internal const string TraitDiscoverer = $"{This}.{nameof(Shared)}.{nameof(TestCategoryDiscoverer)}";
    }
}