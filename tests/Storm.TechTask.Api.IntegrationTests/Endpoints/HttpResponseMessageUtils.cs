using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.UnitTests.Utilities.Comparison;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints
{
    public static class HttpResponseMessageUtils
    {
        #region Deserialization

        public static T? JsonDeserialise<T>(this string json)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<T>(json, jsonOptions)!;
        }

        #endregion


        #region Status Code asserts

        public static HttpResponseMessage ShouldBeSuccess(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            return response;
        }

        public static HttpResponseMessage ShouldBeCreated(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            return response;
        }

        public static HttpResponseMessage ShouldBeNotAuthenticated(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return response;
        }

        public static HttpResponseMessage ShouldBeNotAuthorised(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            return response;
        }

        public static HttpResponseMessage ShouldBeInvalidToken(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return response;
        }

        public static HttpResponseMessage ShouldBeNotFound(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            return response;
        }

        public static async Task<HttpResponseMessage> ShouldBeInvalidCommand(this HttpResponseMessage response)
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var jsonPayload = await response.Content.ReadAsStringAsync();
            var actualPayload = jsonPayload.JsonDeserialise<ProblemDetails>();
            actualPayload?.Title.Should().Be("One or more validation errors occurred.");

            return response;
        }

        public static async Task<HttpResponseMessage> ShouldBeBusinessRuleException<TException>(this HttpResponseMessage response, TException expectedException)
            where TException : BusinessRuleException
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var jsonPayload = await response.Content.ReadAsStringAsync();
            var actualPayload = jsonPayload.JsonDeserialise<ProblemDetails>();
            actualPayload?.Detail.Should().Be(expectedException.Message);

            return response;
        }

        #endregion


        #region Payload asserts

        public static async Task<HttpResponseMessage> WithObjectPayload<T>(this HttpResponseMessage response, T expectedPayload)
        {
            var jsonPayload = await response.Content.ReadAsStringAsync();
            var actualPayload = jsonPayload.JsonDeserialise<T>();
            actualPayload.ShouldHaveSameStateAs(expectedPayload);

            return response;
        }

        public static async Task<HttpResponseMessage> WithListPayload<T>(this HttpResponseMessage response, List<T> expectedPayload)
        {
            var jsonPayload = await response.Content.ReadAsStringAsync();
            var actualPayload = jsonPayload.JsonDeserialise<List<T>>();
            actualPayload?.ShouldHaveSameItemStateAs(expectedPayload);

            return response;
        }
        public static async Task<int> GetJsonIntProp(this HttpResponseMessage response, string propName)
        {
            var jsonPayload = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonPayload);
            return jsonDoc.RootElement.GetProperty(propName).GetInt32();
        }

        public static async Task<string?> GetJsonStringProp(this HttpResponseMessage response, string propName)
        {
            var jsonPayload = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonPayload);
            return jsonDoc.RootElement.GetProperty(propName).GetString();
        }

        #endregion
    }
}
