using MediatR;
using Microsoft.Extensions.Logging;
using Products.Application.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _repo;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IProductRepository repo,
        ILogger<CreateProductHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
    {
        _logger.LogInformation(
            "Creating product {ProductName} with price {ProductPrice}",
            request.Name, request.Price);

        try
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            await _repo.AddAsync(product, ct);

            _logger.LogInformation(
                "Product created successfully. ProductId: {ProductId}",
                product.Id);

            return product.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create product {ProductName}",
                request.Name);
            throw;
        }
    }
}