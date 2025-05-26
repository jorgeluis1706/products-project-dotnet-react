using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;
using ProductsApi.Models.Dtos;
using ProductsApi.Contexts;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductContext context) : ControllerBase
    {
        private readonly ProductContext _context = context;

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.ColorVariations)
                .Select(p => ProductToDTO(p))
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(long id)
        {
            var product = await _context.Products
                                        .Include(p => p.ColorVariations)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return ProductToDTO(product);
        }

        // PUT/PATCH: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            // Fetch the existing product with its color variations
            var existingProduct = await _context.Products
                                        .Include(p => p.ColorVariations)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = productDTO.Name;
            existingProduct.Description = productDTO.Description;
            existingProduct.Price = productDTO.Price; 
            existingProduct.InStock = productDTO.InStock;

            var existingColorVariationIds = existingProduct.ColorVariations?.Select(cv => cv.Id).ToList() ?? [];
            var incomingColorVariationIds = productDTO.ColorVariations?.Select(cv => cv.Id).ToList() ?? [];


            // Remove variations that are in existing but not in incoming DTO
            foreach (var existingVariation in existingProduct.ColorVariations?.ToList() ?? [])
            {
                if (!incomingColorVariationIds.Contains(existingVariation.Id))
                {
                    _context.ProductColorVariations.Remove(existingVariation);
                }
            }

            // Add or update variations that are in incoming DTO
            if (productDTO.ColorVariations != null)
            {
                foreach (var incomingVariationDto in productDTO.ColorVariations)
                {
                    var existingVariation = existingProduct.ColorVariations?
                        .FirstOrDefault(cv => cv.Id == incomingVariationDto.Id);

                    if (existingVariation == null)
                    {
                        existingProduct.ColorVariations?.Add(PrdtColorVariationFromDTO(incomingVariationDto));
                    }
                    else
                    {
                        existingVariation.Name = incomingVariationDto.Name;
                        existingVariation.Price = incomingVariationDto.Price;
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDTO)
        {
            productDTO.ColorVariations ??= [];

            Product productToPost = new()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price, // Include Price when creating
                InStock = productDTO.InStock,
                ColorVariations = productDTO.ColorVariations.Select(PrdtColorVariationFromDTO).ToList()
            };

            _context.Products.Add(productToPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = productToPost.Id }, ProductToDTO(productToPost)); // Return the DTO based on the saved product
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // --- DTO Mappers ---

        private static ProductDTO ProductToDTO(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            InStock = product.InStock,
            ColorVariations = product.ColorVariations?.Select(PrdtColorVariationToDTO).ToList() ?? []
        };

        private static ProductColorVariationDTO PrdtColorVariationToDTO(ProductColorVariation variation) => new()
        {
            Id = variation.Id,
            Name = variation.Name,
            Price = variation.Price,
        };

        private static ProductColorVariation PrdtColorVariationFromDTO(ProductColorVariationDTO variation) => new()
        {
            Id = variation.Id, 
            Name = variation.Name,
            Price = variation.Price
        };
    }
}