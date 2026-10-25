using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries;

public record GetProductsQuery() : IRequest<List<ProductDto>>;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly ILogger<GetProductsHandler> _logger;

    public GetProductsHandler(
        IProductRepository repo,
        ILogger<GetProductsHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Getting all products");

        var products = await _repo.GetAllAsync(ct);

        _logger.LogInformation("Retrieved {ProductCount} products", products.Count);

        return products.Select(p => new ProductDto(p.Id, p.Name, p.Price)).ToList();
    }
}