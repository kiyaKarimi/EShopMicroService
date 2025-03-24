

namespace Catalog.API.Products.GetProduct
{
    public record GetProductQuery() : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductQueryHandler(IDocumentSession session, ILogger<GetProductQueryHandler> logger) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"get info {request}");
            var product = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(product);
        }
    }
}
