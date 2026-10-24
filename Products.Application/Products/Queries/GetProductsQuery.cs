using MediatR;
using Products.Application.DTOs;
using Products.Application.Interfaces;

namespace Products.Application.Products.Queries;

public record GetProductsQuery() : IRequest<List<ProductDto>>;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repo;

    public GetProductsHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllAsync(ct);
        return products.Select(p => new ProductDto(p.Id, p.Name, p.Price)).ToList();
    }
}