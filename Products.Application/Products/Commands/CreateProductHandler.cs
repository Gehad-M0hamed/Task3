using MediatR;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _repo;

    public CreateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        await _repo.AddAsync(product, ct);
        return product.Id;
    }
}