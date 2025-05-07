namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductsResult
    {
        public int TotalCount { get; set; }
        public IEnumerable<GetProductResult> Items { get; set; }
    }
}
