using TravelExpense.Application.Commands.AproveExpense;
using TravelExpense.Application.Commands.CreateTravel;
using TravelExpense.Application.Commands.RegisterExpense;
using TravelExpense.Application.Commands.RejectExpense;
using TravelExpense.Application.Commands.RemoveExpense;
using TravelExpense.Application.Dtos;
using TravelExpense.Core;
using TravelExpense.Core.Communications;
using TravelExpense.Application.Queries.GetTravelById;
using TravelExpense.Application.Queries.GetExpenseById;
using Microsoft.AspNetCore.Mvc;

namespace TravelExpense.Presentation
{
    public static class TravelEndPoints
    {
        public static WebApplication MapTravelEndPoints(this WebApplication app)
        {
            app.MapPost("/travels", async (CreateTravelCommand command, IMediatorHandler mediator) =>
            {
                CommandResult<TravelDto> commandResult = await mediator.SendCommandAsync<CreateTravelCommand, CommandResult<TravelDto>>(command);
                return commandResult.IsSuccess ? 
                Results.Created($"/travels/{commandResult.Data?.Id}", commandResult) : 
                Results.BadRequest(commandResult);
            });

            app.MapGet("/travels/{id}", async (long id, IMediatorHandler mediator) =>
            {
                QueryResult<TravelDto> queryResult = await mediator.SendQueryAsync<GetTravelByIdQuery, QueryResult<TravelDto>>(new GetTravelByIdQuery { Id = id });
                return queryResult.IsSuccess ?
                Results.Ok(queryResult) :
                Results.BadRequest(queryResult);
            });

            app.MapGet("/travels/{travelId}/expenses/{id}", async (long travelId, long id, IMediatorHandler mediator) =>
            {
                QueryResult<ExpenseDto> queryResult = await mediator
                     .SendQueryAsync<GetExpenseByIdQuery, QueryResult<ExpenseDto>>(new GetExpenseByIdQuery { Id = id, TravelId = travelId });
                return queryResult.IsSuccess ?
                Results.Ok(queryResult) :
                Results.BadRequest(queryResult);
            });


            app.MapPost("/travels/{id}/expenses", async (long id, RegisterExpenseCommand command, IMediatorHandler mediator) => 
            {
                command.Id = id;
                CommandResult<TravelDto> commandResult = await mediator.SendCommandAsync<RegisterExpenseCommand, CommandResult<TravelDto>>(command);
                if (commandResult.IsFailure)
                {
                    if (commandResult.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase))
                        return Results.NotFound(commandResult);
                    return Results.BadRequest(commandResult);
                }
                return Results.Created($"/travels/{id}/expenses/{commandResult.Data!.Id}", commandResult);
            });

            app.MapDelete("/travels/{id}/expenses/{expenseId}", async (long id, long expenseId, IMediatorHandler mediator) =>
            {
                var command = new RemoveExpenseCommand
                {
                    Id = id,
                    ExpenseId = expenseId
                };
                CommandResult<TravelDto> commandResult = await mediator.SendCommandAsync<RemoveExpenseCommand, CommandResult<TravelDto>>(command);
                if (commandResult.IsFailure)
                {
                    if (commandResult.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase))
                        return Results.NotFound(commandResult);
                    return Results.BadRequest(commandResult);
                }
                return Results.Ok(commandResult);
            });

            app.MapPut("/travels/{id}/expenses/{expenseId}/aprove", async (long id, long expenseId, AproveExpenseCommand command, IMediatorHandler mediator) =>
            {
                command.Id = id;
                command.ExpenseId = expenseId;
                CommandResult<TravelDto> commandResult = await mediator.SendCommandAsync<AproveExpenseCommand, CommandResult<TravelDto>>(command);
                if (commandResult.IsFailure)
                {
                    if (commandResult.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase))
                        return Results.NotFound(commandResult);
                    return Results.BadRequest(commandResult);
                }
                return Results.Ok(commandResult);
            });

            app.MapPut("/travels/{id}/expenses/{expenseId}/reject", async (long id, long expenseId, [FromBody] RejectExpenseCommand command, IMediatorHandler mediator) =>
            {
                command.Id = id;
                command.ExpenseId = expenseId;
                CommandResult<TravelDto> commandResult = await mediator.SendCommandAsync<RejectExpenseCommand, CommandResult<TravelDto>>(command);
                if (commandResult.IsFailure)
                {
                    if (commandResult.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase))
                        return Results.NotFound(commandResult);
                    return Results.BadRequest(commandResult);
                }
                return Results.Ok(commandResult);
            });

            return app;
        }
    }
}
