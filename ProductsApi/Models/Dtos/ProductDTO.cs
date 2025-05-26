namespace ProductsApi.Models.Dtos;

public class ProductDTO
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public bool InStock { get; set; } = false;
    public virtual List<ProductColorVariationDTO>? ColorVariations { get; set; }
}