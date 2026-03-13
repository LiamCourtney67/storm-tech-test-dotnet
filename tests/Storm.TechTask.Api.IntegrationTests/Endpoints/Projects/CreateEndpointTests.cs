
using Duende.IdentityModel.Client;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.Commands;
using Storm.TechTask.Core.ProjectAggregate.Exceptions;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints.Projects
{
    [Collection("Sequential")]
    public class CreateEndpointTests : BaseEndpointFixture
    {
        public CreateEndpointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }

        [Fact]
        public async Task CreatesProject()
        {
            // Arrange
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.ProjectAdmin));

            // Act
            var response = await this.HttpClient.PostAsync($"/Projects", new CreateProject.Command("name", ProjectCategory.Development, true));

            // Assert
            await response.ShouldBeCreated().WithObjectPayload(new ProjectDto(await response.GetJsonIntProp("id"), "name"));
            var expected = NewProject()
                .Set(p => p.Id, await response.GetJsonIntProp("id"))
                .Set(p => p.Name, "name")
                .Set(p => p.Category, ProjectCategory.Development)
                .Set(p => p.InternalOnly, true)
                .Set(p => p.Status, ProjectStatus.Open).Build();
            await expected.ShouldBeInDb();
        }

        [Fact]
        public async Task ReturnsBadRequestIfNameNotUnique()
        {
            // Arrange
            var existing = await NewProject().BuildAndPersist();
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.ProjectAdmin));

            // Act
            var response = await this.HttpClient.PostAsync($"/Projects", new CreateProject.Command(existing.Name, ProjectCategory.Development, true));

            // Act & Assert
            await response.ShouldBeBusinessRuleException(new ProjectWithNameAlreadyExistsException(existing.Name));
        }

        [Theory]
        [AllRolesExcept(AppRole.ProjectAdmin)]
        public async Task DoesNotCreateProjectWhenInRole(AppRole role)
        {
            // Arrange
            var projectName = "Test Project";
            HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            var data = new { Name = projectName };

            // Act
            var response = await HttpClient.PostAsync("/Projects", data);

            // Assert
            response.ShouldBeNotAuthorised();
        }

        [Fact]
        public async Task ReturnsBadRequestIfNameIsNull()
        {
            HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.ProjectAdmin));

            var data = new { Name = null as string };

            var response = await HttpClient.PostAsync("/Projects", data);

            await response.ShouldBeInvalidCommand();
        }

        [Fact]
        public async Task ShouldBeUnauthorized()
        {
            // Act
            var response = await this.HttpClient.PostAsync($"/Projects", new CreateProject.Command("name", ProjectCategory.Development, true));

            // Assert
            response.ShouldBeNotAuthenticated();
        }
    }
}
