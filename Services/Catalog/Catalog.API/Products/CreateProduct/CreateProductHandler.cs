
namespace Catalog.API.Products.CreatProduct
{
    public record CeateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
  :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CeateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CeateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
                Name= request.Name
                
            };
            session.Store(product);
            await   session.SaveChangesAsync(cancellationToken);
            return   new  CreateProductResult(product.Id);
        }
    }
}
