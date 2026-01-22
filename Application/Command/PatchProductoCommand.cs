using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Command
{
    public record PatchProductoCommand(
        int Id,
        string? Codigo,
        string? Nombre,
        string? Descripcion,
        decimal? Precio,
        bool? Activo,
        int? CategoriaId
    ) : IRequest<ProductoResponse>;
}
