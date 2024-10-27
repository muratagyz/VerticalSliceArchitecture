using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using VerticalSliceArchitecture.WebApi.Contracts;
using VerticalSliceArchitecture.WebApi.Database;
using VerticalSliceArchitecture.WebApi.Entities;
using VerticalSliceArchitecture.WebApi.Shared;

namespace VerticalSliceArchitecture.WebApi.Features.Products
{
    public static class CreateProduct
    {
        public class Command : IRequest<CustomResponse<Guid>>
        {
            public string Name { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }

            internal sealed class Handler : IRequestHandler<Command, CustomResponse<Guid>>
            {
                private readonly AppDbContext _dbContext;
                private readonly IValidator<Command> _validator;

                public Handler(AppDbContext dbContext, IValidator<Command> validator)
                {
                    _dbContext = dbContext;
                    _validator = validator;
                }

                public async Task<CustomResponse<Guid>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var validationResult = _validator.Validate(request);
                    if (validationResult.IsValid is false) return CustomResponse<Guid>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

                    var product = new Product(Guid.NewGuid(), request.Name, request.Stock, request.Price);

                    await _dbContext.Products.AddAsync(product);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return CustomResponse<Guid>.Success(product.id);
                }
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Stock).NotEmpty();
                RuleFor(x => x.Price).NotEmpty();
            }
        }
    }


    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProduct.Command>();

                var result = await sender.Send(command);

                if (result.Status is false)
                {
                    return Results.BadRequest(result);
                }

                return Results.Ok(result);
            });
        }
    }
}
