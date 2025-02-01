using MyTeamsHub.IntegrationTests.Core;
using MyTeamsHub.IntegrationTests.Core.Extensions;
using MyTeamsHub.Organization.API.Models.V1.Organizations;
using MyTeamsHub.Organization.API.Models.V1.Teams;
using System.Net;

namespace MyTeamsHub.IntegrationTests
{
    public class OrganizationsIntegrationTests
    {
        //[Fact]
        public void GetOrganizationsShouldReturn20Items()
        {
            MyTestHub
                .ApiEngine
                .Get<GetTeamsRequestDto>("api/organizations")
                .Should(response =>
                {
                    response.HttpStatusCode.ShouldBeOk();
                    response.Data.PageSize.ShouldBe(1);
                    response.Data.PageSize.ShouldBe(20);
                });
        }
    }
}