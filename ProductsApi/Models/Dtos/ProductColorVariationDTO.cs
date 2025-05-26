namespace ProductsApi.Models.Dtos;

public class ProductColorVariationDTO
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}