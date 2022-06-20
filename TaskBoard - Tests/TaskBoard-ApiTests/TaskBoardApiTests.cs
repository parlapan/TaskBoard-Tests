using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace TaskBoard.APITests
{
    public class ApiTests
    {
        private const string url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        
        [Test]
        public void Test_GetAllTasks_CheckFirstTask()
        {
            // Arrange
            this.request = new RestRequest(url);

            // Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks[0].title, Is.EqualTo("Project skeleton"));
            Assert.That(tasks[0].description, Is.EqualTo("Create project folders, services, controllers and views"));
        }

        [Test]
        public void Test_SearchTasks_CheckFirstResylt()
        {
            // Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "home");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));
            Assert.That(tasks[0].description, Is.EqualTo("Create the [Home] page and list tasks count by board"));
        }

        [Test]
        public void Test_SearchTasks_EmptyResults()
        {
            // Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missing{randnum}");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateTasks_CreateWithInvalidData()
        {
            // Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                description = "Alabala",
                board = "Open"
            };
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateTask_CreateWithValidData()
        {
            // Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                title = "Alabala",
                description = "Alabala" + DateTime.Now.Ticks,
                board = "Done"
            };
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allTasks = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(allTasks.Content);

            var lastTask = tasks[tasks.Count - 1];
 
            Assert.That(lastTask.title, Is.EqualTo(body.title));
            Assert.That(lastTask.description, Is.EqualTo(body.description));

        }
     }
}