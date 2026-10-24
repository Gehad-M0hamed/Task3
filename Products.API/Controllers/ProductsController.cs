using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Products.Commands;
using Products.Application.Products.Queries;

namespace Products.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IValidator<CreateProductCommand> _validator;

    public ProductsController(ISender sender, IValidator<CreateProductCommand> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var products = await _sender.Send(new GetProductsQuery(), ct);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var product = await _sender.Send(new GetProductByIdQuery(id), ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(command, ct);
        if (!result.IsValid)
        {
            return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        }

        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }
}