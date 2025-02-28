using FluentAssertions;

using MyTeamsHub.Organization.API.Models.V1.Users;

using NetArchTest.Rules;

namespace MyTeamsHub.ArchitectureTests.Presentation;

public class ApiModelsTests
{
    [Fact]
    public void Presentation_API_Models_Should_Be_Records()
    {
        var presentationApiModelsAssembly = typeof(UsersResponseDto).Assembly;

        var result = Types
            .InAssembly(presentationApiModelsAssembly)
            .That()
            .ResideInNamespaceStartingWith("MyTeamsHub.Organization.API.Models.V1")
            .Should()
            .BeClasses()
            .And()
            .NotBeStatic()
            .And()
            .NotBeAbstract()
            .And()
            .BeSealed()
            .And()
            .Inherit(typeof(object))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
