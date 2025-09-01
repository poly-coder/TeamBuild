namespace TeamBuild.Core.Domain.Testing;

public record ModelValidationTestData<TModel>(string Rule, TModel Instance);
