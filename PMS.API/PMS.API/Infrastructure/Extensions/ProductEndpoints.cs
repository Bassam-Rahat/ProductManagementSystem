using MediatR;
using PMS.API.Application.Commands;
using PMS.API.Application.Queries;

namespace PMS.API.Infrastructure.Extensions
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            app.MapGet("/api/products", async (IMediator mediator) =>
                await mediator.Send(new GetAllProductsQuery()));

            app.MapGet("/api/products/{id}", async (IMediator mediator, Guid id) =>
                await mediator.Send(new GetProductByIdQuery { Id = id}));

            app.MapPost("/api/products", async (IMediator mediator, CreateProductCommand command) =>
                Results.Ok(await mediator.Send(command)));

            app.MapPut("/api/products", async (IMediator mediator, UpdateProductCommand command) =>
            {
                await mediator.Send(command);
                return Results.Ok();
            });

            app.MapDelete("/api/products/{id}", async (IMediator mediator, Guid id) =>
            {
                await mediator.Send(new DeleteProductCommand { Id = id });
                return Results.Ok();
            });
        }
    }
}