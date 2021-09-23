using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using BusinessMicroservice.Models;

namespace BusinessMicroservice.IntegrationTests.Tests.Features
{
    // [TestClass] - атрибут класса, который содержит тесты
    [TestClass]
    public class Feature1
    {
        // [TestMethod] - атрибут метода, который является тестом
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange - секция исходных данных
            // запустить микросервис
            var app = new WebApplicationFactory<Startup>();
            // создать http клиент для микросервиса (аналог swagger, postman, soap ui)
            var client = app.CreateClient();

            // Act - секция действия пользователя, которое хотим проверить
            // выполнить get запрос, результат записать в переменную request
            var request = await client.GetAsync("/hello");

            // Assert - секция утверждений/проверок результата действия пользователя
            // проверить код ответа
            request.StatusCode.Should().Be(HttpStatusCode.OK);
            // прочитать содержимое ответа
            var result = await request.Content.ReadAsStringAsync();
            // проверить содержимое
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

        [TestMethod]
        public async Task TestMethod4()
        {
            // Arrange
            WebApplicationFactory<Startup> app = new();
            HttpClient client = app.CreateClient();

            var nameMessage = new NameMessage { Name = "Evgeny" };
            var nameMessageJsonString = JsonSerializer.Serialize(nameMessage);
            var httpRequestContent = new StringContent(nameMessageJsonString, Encoding.UTF8, "application/json");

            // Act
            var request = await client.PostAsync("/postjsonname", httpRequestContent);

            // Assert
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Be("{\"message\":\"Hello, Evgeny\"}");
        }

        [TestMethod]
        public async Task TestMethod5()
        {
            // Arrange
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();

            // Act
            var request = await client.GetAsync("/geterror");

            // Assert
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Contain("Error text");
        }
    }
}
