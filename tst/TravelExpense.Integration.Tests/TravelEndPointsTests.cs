using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TravelExpense.Application.Commands.AproveExpense;
using TravelExpense.Application.Commands.RegisterExpense;
using TravelExpense.Application.Commands.RejectExpense;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Domain.Events;

namespace TravelExpense.Integration.Tests
{
    public class TravelEndPointsTests : IClassFixture<TravelExpenseWebApplicationFactory>
    {
        private readonly TravelExpenseWebApplicationFactory _webAppFactory;
        public TravelEndPointsTests(TravelExpenseWebApplicationFactory webAppFactory)
        {
            _webAppFactory = webAppFactory;
        }

        [Fact]
        public async Task RegisterExpenseShouldBeCreatedCorrectly()
        {
            // Arrange
            HttpClient? client = _webAppFactory.CreateClient();
            await _webAppFactory.SeedDatabase();

            var command = new RegisterExpenseCommand()
            {
                RelatedTo = "Hospedagem em Hotel",
                Description = "05 diárias no Hotel Astron Suítes",
                Date = DateTime.Now.AddDays(1),
                Value = 2550.65m
            };
            string body = JsonConvert.SerializeObject(command);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            // Act
            var registerResponse = await client.PostAsync("travels/1/expenses", stringContent);
            var travelResponse =  await client.GetFromJsonAsync<QueryResult<TravelDto>>("travels/1");
            var expenseResponse = await client.GetFromJsonAsync<QueryResult<ExpenseDto>>($"travels/1/expenses/{travelResponse!.Data!.Expenses.FirstOrDefault()!.Id}");

            var @event = _webAppFactory.AmqpClient.ListenFirst<ExpenseRegisteredEvent>(nameof(ExpenseRegisteredEvent));

            // Assert
            Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
            Assert.Equal(2550.65m, travelResponse!.Data!.TotalExpenses);
            Assert.Equal(command.RelatedTo, expenseResponse!.Data!.RelatedTo);
            Assert.Equal(command.Description, expenseResponse!.Data!.Description);
            Assert.Equal(command.Date, expenseResponse!.Data!.Date);
            Assert.Equal(command.Value, expenseResponse!.Data!.Value);

            Assert.Equal(command.RelatedTo, @event!.RelatedTo);
            Assert.Equal(command.Description, @event!.Description);
            Assert.Equal(command.Date, @event!.Date);
            Assert.Equal(command.Value, @event!.Value);
        }

        [Fact]
        public async Task AproveExpenseShouldBeAprovedCorrectly()
        {
            // Arrange
            HttpClient? client = _webAppFactory.CreateClient();
            await _webAppFactory.SeedDatabase();
            var command = new AproveExpenseCommand()
            {
                Id = 2,
                ExpenseId = 1,
                VoucherId = "X-123-4b",
                Comment = "Aprovado. Centro de Custos 10.001.0099- TI Eventos e Palestras"
            };
            string body = JsonConvert.SerializeObject(command);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            // Act
            var aproveResponse = await client.PutAsync("/travels/2/expenses/1/aprove", stringContent);
            var expenseResponse = await client.GetFromJsonAsync<QueryResult<ExpenseDto>>("/travels/2/expenses/1");
            var @event = _webAppFactory.AmqpClient.ListenFirst<ExpenseAprovedEvent>(nameof(ExpenseAprovedEvent));


            // Assert
            Assert.Equal(HttpStatusCode.OK, aproveResponse.StatusCode);
            Assert.Equal(command.ExpenseId, expenseResponse!.Data!.Id);
            Assert.Equal(command.VoucherId, expenseResponse!.Data!.VoucherId);
            Assert.Contains(command.Comment, expenseResponse!.Data!.Comments);

            Assert.Equal(command.ExpenseId, @event!.ExpenseId);
            Assert.Equal(command.VoucherId, @event!.VoucherId);
            Assert.Equal(command.Comment, @event!.Comment);
            //Comentário
        }

        [Fact]
        public async Task RejectExpenseShouldBeRejectedCorrectly()
        {
            // Arrange
            HttpClient? client = _webAppFactory.CreateClient();
            await _webAppFactory.SeedDatabase();
            var command = new RejectExpenseCommand()
            {
                Id = 2,
                ExpenseId = 1,
                Comment = "Aprovado. Centro de Custos 10.001.0099- TI Eventos e Palestras"
            };
            string body = JsonConvert.SerializeObject(command);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            // Act
            var rejectResponse = await client.PutAsync("/travels/2/expenses/1/reject", stringContent);
            var expenseResponse = await client.GetFromJsonAsync<QueryResult<ExpenseDto>>("/travels/2/expenses/1");
            var @event = _webAppFactory.AmqpClient.ListenFirst<ExpenseRejectedEvent>(nameof(ExpenseRejectedEvent));


            // Assert
            Assert.Equal(HttpStatusCode.OK, rejectResponse.StatusCode);
            Assert.Equal(command.ExpenseId, expenseResponse!.Data!.Id);
            Assert.Contains(command.Comment, expenseResponse!.Data!.Comments);

            Assert.Equal(command.ExpenseId, @event!.ExpenseId);
            Assert.Equal(command.Comment, @event!.Comment);
        }

    }
}
