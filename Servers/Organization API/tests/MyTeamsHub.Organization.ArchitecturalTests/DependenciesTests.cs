using FluentAssertions;

using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Organization.API.Controllers;
using MyTeamsHub.Organization.API.Models.V1.Users;
using MyTeamsHub.Organization.Persistence;
using MyTeamsHub.SignalR.Users.Hub;

using NetArchTest.Rules;

namespace MyTeamsHub.ArchitectureTests;
public class DependenciesTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Application()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(applicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var infrastructureCoreAssembly = typeof(Infrastructure.DependencyInjection).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(infrastructureCoreAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Persistance_Infrastructure()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var infrastructurePersistenceAssembly = typeof(DependencyInjection).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(infrastructurePersistenceAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Presentation_API()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var presentationApiAssembly = typeof(BaseApiController).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(presentationApiAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Presentation_API_Models()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var presentationApiModelsAssembly = typeof(UsersResponseDto).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(presentationApiModelsAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Presentation_SignalR()
    {
        var domainAssembly = typeof(OrganizationEntity).Assembly;
        var presentationSignalRAssembly = typeof(IUserNotificationHub).Assembly;

        var result = Types
            .InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOn(presentationSignalRAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Depend_On_Domain()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var ref1 = applicationAssembly.GetReferencedAssemblies();
        var domainAssembly = typeof(OrganizationEntity).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .HaveDependencyOnAny(typeof(OrganizationEntity).Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var infrastructureCoreAssembly = typeof(Infrastructure.DependencyInjection).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn(infrastructureCoreAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Persistance_Infrastructure()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var infrastructurePersistenceAssembly = typeof(DependencyInjection).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn(infrastructurePersistenceAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Presentation_API()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var presentationApiAssembly = typeof(BaseApiController).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn(presentationApiAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Presentation_API_Models()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var presentationApiModelsAssembly = typeof(UsersResponseDto).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn(presentationApiModelsAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Presentation_SignalR()
    {
        var applicationAssembly = typeof(Core.Application.DependencyInjection).Assembly;
        var presentationSignalRAssembly = typeof(IUserNotificationHub).Assembly;

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn(presentationSignalRAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
