using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Categories;
public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description",
        };

        var datetimeBefore = DateTime.Now;

        var category = new Category(
            validData.Name, 
            validData.Description);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.True(category.IsActive);
        Assert.NotEqual(DateTime.MinValue, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description",
            IsActive = isActive
        };

        var datetimeBefore = DateTime.Now;

        var category = new Category(
            validData.Name,
            validData.Description,
            validData.IsActive);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.Equal(isActive, category.IsActive);
        Assert.NotEqual(DateTime.MinValue, category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    { 
        Action action =
            () => new Category(name!, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action =
            () => new Category("Category Name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("12")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action =
            () => new Category(invalidName, "Category Valid Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreatherThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreatherThan255Characters()
    {
        var invalidName = String.Join(null, 
            Enumerable.Range(0, 256).Select(_ => "a").ToList());

        Action action =
            () => new Category(invalidName, "Category Valid Description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreatherThan10_000Characters()
    {
        var invalidDescription = String.Join(null,
            Enumerable.Range(0, 10_001).Select(_ => "a").ToList());

        Action action =
            () => new Category("Category Valid Name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should be less or equal 10_000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        
        var category = new Category(validData.Name, validData.Description, false);

        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };

        var category = new Category(validData.Name, validData.Description, true);

        category.Deactivate();

        Assert.False(category.IsActive);
    }
}
