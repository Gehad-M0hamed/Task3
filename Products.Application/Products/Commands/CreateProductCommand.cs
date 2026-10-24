using MediatR;

namespace Products.Application.Products.Commands;

public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;