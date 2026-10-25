using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repo;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(
        IProductRepository repo,
        ILogger<GetProductByIdHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting product {ProductId}", request.Id);

        var p = await _repo.GetByIdAsync(request.Id, ct);

        if (p is null)
        {
            _logger.LogWarning("Product {ProductId} was not found", request.Id);
            return null;
        }

        return new ProductDto(p.Id, p.Name, p.Price);
    }
}