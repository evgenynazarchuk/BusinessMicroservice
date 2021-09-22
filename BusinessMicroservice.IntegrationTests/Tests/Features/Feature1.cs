using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using FluentAssertions;

namespace BusinessMicroservice.IntegrationTests.Tests.Features
{
    [TestClass]
    public class Feature1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();

            // Act
            var request = await client.GetAsync("/hello");

            // Assert
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Be("Hello world");
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            // Arrange
            WebApplicationFactory<Startup> app = new();
            HttpClient client = app.CreateClient();
            var name = "Evgeny";

            // Act
            var request = await client.GetAsync($"/helloperson?name={name}");

            // Assert
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Be($"Hello, {name}");
        }

        [TestMethod]
        public async Task TestMethod3()
        {
            // Arrange
            WebApplicationFactory<Startup> app = new();
            HttpClient client = app.CreateClient();
            var name = "Evgeny";

            // Act
            var request = await client.GetAsync($"/getjsonhello?name={name}");

            // Assert
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Be("{\"message\":\"Hello, Evgeny\"}");
        }
    }
}
