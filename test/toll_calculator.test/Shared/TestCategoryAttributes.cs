using Xunit.Abstractions;
using Xunit.Sdk;

namespace Rax.IocTools.Test.Shared;

[TraitDiscoverer(CategoryTestConst.Assembly.TraitDiscoverer, CategoryTestConst.Assembly.This)]
internal class AcceptanceTestAttribute: TestCategoryBaseAttribute
{
    public AcceptanceTestAttribute(string id) : base(id)
    {
    }

    public override string CategoryName => CategoryTestConst.Name.AcceptanceTest;
    public override string? Id { get; set; }
}

[TraitDiscoverer(CategoryTestConst.Assembly.TraitDiscoverer,CategoryTestConst.Assembly.This)]
internal class ClassTestAttribute: TestCategoryBaseAttribute
{
    public override string CategoryName => CategoryTestConst.Name.ClassTest;
    public override string? Id { get; set; }

    public ClassTestAttribute(string id) : base(id)
    {
    }
}

[TraitDiscoverer(CategoryTestConst.Assembly.TraitDiscoverer,CategoryTestConst.Assembly.This)]
internal class EdgeCaseTestAttribute: TestCategoryBaseAttribute
{
    public override string CategoryName => CategoryTestConst.Name.EdgeCaseTest;
    public override string? Id { get; set; }

    public EdgeCaseTestAttribute(string id) : base(id)
    {
    }
}

[TraitDiscoverer(CategoryTestConst.Assembly.TraitDiscoverer,CategoryTestConst.Assembly.This)]
internal class ClassCompositeTestAttribute: TestCategoryBaseAttribute
{
    public override string CategoryName => CategoryTestConst.Name.ClassCompositeTest;
    public override string? Id { get; set; }

    public ClassCompositeTestAttribute(string id) : base(id)
    {
    }
}

[TraitDiscoverer(CategoryTestConst.Assembly.TraitDiscoverer,CategoryTestConst.Assembly.This)]
internal class ClassIntegrationTestAttribute: TestCategoryBaseAttribute
{
    public override string CategoryName => CategoryTestConst.Name.ClassIntegrationTest;
    public override string? Id { get; set; }

    public ClassIntegrationTestAttribute(string id) : base(id)
    {
    }
}

internal abstract class TestCategoryBaseAttribute:Attribute, ITraitAttribute
{
    public abstract string CategoryName { get; }
    public abstract string? Id { get; set; }

    protected TestCategoryBaseAttribute(string id)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Id = id; // Needed for templating
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal class TestCategoryDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        var id = traitAttribute.GetNamedArgument<string>(nameof(TestCategoryBaseAttribute.Id));
        var categoryName = traitAttribute.GetNamedArgument<string>(nameof(TestCategoryBaseAttribute.CategoryName));
        
        yield return new KeyValuePair<string, string>(categoryName, id);
    }
}