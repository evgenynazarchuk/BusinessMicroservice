using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using BusinessMicroservice.Models;
using Microsoft.AspNetCore.WebUtilities;

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
            // создать фабрику микросервиса, которая создаст сервер и клиент мкросервиса
            // BusinessMicroservice.Startup - конфигурация микросервиса
            var webAppFactory = new WebApplicationFactory<BusinessMicroservice.Startup>();
            // создать http клиент для микросервиса (аналог swagger, postman, soap ui)
            // перед созданием http клиента автоматически создаётся микросервис
            var client = webAppFactory.CreateClient();

            // Act - секция действия пользователя, которое хотим проверить
            // выполнить get запрос к микросервису
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
            var webAppFactory = new WebApplicationFactory<Startup>();
            var client = webAppFactory.CreateClient();
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
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();
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
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();
            var nameMessage = new NameMessage { Name = "Evgeny" };

            // конвертация объекта в json строку для передачи микросервису с отметкой application/json
            var nameMessageJsonString = JsonSerializer.Serialize(nameMessage);
            var httpRequestContent = new StringContent(nameMessageJsonString, Encoding.UTF8, "application/json");

            // Act
            // post запрос
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

        [TestMethod]
        public async Task TestMethod6()
        {
            // Arrange
            var app = new WebApplicationFactory<Startup>();
            var client = app.CreateClient();
            var numbers = new int[] { 1, 3 };

            // подготовка строки параметров
            // формируем строку вида ?numbers=1&numbers=2
            // чтобы в итоге получить url: localhost/getnumbers?numbers=1&numbers=2
            // не рекомендуется использовать в тестах операторы for, foreach, if, switch 
            string query = string.Empty;
            foreach (var number in numbers)
            {
                query = QueryHelpers.AddQueryString(query, "numbers", number.ToString());
            }
            
            // Act
            var request = await client.GetAsync($"/getnumbers{query}");

            // Arrange
            request.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await request.Content.ReadAsStringAsync();
            result.Should().Be("1,3");
        }
    }
}
