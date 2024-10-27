namespace VerticalSliceArchitecture.WebApi.Entities
{
    public record Product(Guid id, string name, int stock, decimal price);
}
