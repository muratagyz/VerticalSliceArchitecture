using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.WebApi.Contracts;
using VerticalSliceArchitecture.WebApi.Database;
using VerticalSliceArchitecture.WebApi.Shared;

namespace VerticalSliceArchitecture.WebApi.Features.Products
{
    public static class GetProduct
    {
        public class Query : IRequest<CustomResponse<ProductResponse>>
        {
            public Guid Id { get; set; }

            internal sealed class Handler : IRequestHandler<Query, CustomResponse<ProductResponse>>
            {
                private readonly AppDbContext _dbContext;

                public Handler(AppDbContext dbContext)
                {
                    _dbContext = dbContext;
                }

                public async Task<CustomResponse<ProductResponse>> Handle(Query request, CancellationToken cancellationToken)
                {
                    var product = await _dbContext
                        .Products
                        .Where(x => x.id == request.Id)
                        .Select(x => new ProductResponse
                        {
                            Id = x.id,
                            Name = x.name,
                            Stock = x.stock,
                            Price = x.price
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (product is null)
                    {
                        return CustomResponse<ProductResponse>.Fail("Product not found");
                    }

                    return CustomResponse<ProductResponse>.Success(product);
                }
            }
        }
    }

    public class GetProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/articles/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetProduct.Query { Id = id };

                var result = await sender.Send(query);

                if (result.Status is false)
                {
                    return Results.NotFound(result);
                }

                return Results.Ok(result);
            });
        }
    }
}
