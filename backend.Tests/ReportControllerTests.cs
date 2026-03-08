using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace backend.Tests
{
    public class ReportControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetLaunchDistribution_ReturnsOk_WithData()
        {
            // Act
            var response = await _client.GetAsync("/api/report/launch-distribution");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<dynamic>>();
            content.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserProductivity_ReturnsOk_WithData()
        {
            // Act
            var response = await _client.GetAsync("/api/report/user-productivity");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<dynamic>>();
            content.Should().NotBeNull();
        }
    }
}
