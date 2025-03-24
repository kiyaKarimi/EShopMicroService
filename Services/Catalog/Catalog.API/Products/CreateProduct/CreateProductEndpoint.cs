
namespace Catalog.API.Products.CreatProduct
{
    public record CeateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
: IRequest<CreateProductResponse>;
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (CeateProductRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateProductCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<CreateProductResponse>();

                    return Results.Created($"products/{response.Id}", response);
                }).WithName("CreateProduct").
                Produces<CreateProductResponse>(StatusCodes.Status201Created).
                ProducesProblem(StatusCodes.Status400BadRequest).
                WithSummary("Create Product").
                WithDescription("Create Product")
                ;


        }
    }
}
