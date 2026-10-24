using Microsoft.EntityFrameworkCore;
using Products.Application.Interfaces;
using Products.Domain.Entities;
using Products.Infrastructure.Persistence;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Product>> GetAllAsync(CancellationToken ct)
    {
        return _context.Products.AsNoTracking().ToListAsync(ct);
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct)
    {
        return _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);
    }
}