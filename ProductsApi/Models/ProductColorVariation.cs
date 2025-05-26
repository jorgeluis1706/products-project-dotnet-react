namespace ProductsApi.Models;

public class ProductColorVariation
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}