using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class PatchProductoHandler
        : IRequestHandler<PatchProductoCommand, ProductoResponse?>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public PatchProductoHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse?> Handle(
            PatchProductoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _postgresDbContext.Productos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return null;

            if (request.Codigo is not null)
                entity.Codigo = request.Codigo;

            if (request.Nombre is not null)
                entity.Nombre = request.Nombre;

            if (request.Descripcion is not null)
                entity.Descripcion = request.Descripcion;

            if (request.Precio.HasValue)
                entity.Precio = request.Precio.Value;

            if (request.Activo.HasValue)
                entity.Activo = request.Activo.Value;

            if (request.CategoriaId.HasValue)
                entity.CategoriaId = request.CategoriaId.Value;

            entity.FechaActualizacion = DateTime.UtcNow;

            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            return new ProductoResponse(
                Id: entity.Id,
                Codigo: entity.Codigo,
                Nombre: entity.Nombre,
                Descripcion: entity.Descripcion ?? string.Empty,
                Precio: (double)entity.Precio,
                Activo: entity.Activo,
                CategoriaId: entity.CategoriaId,
                FechaCreacion: entity.FechaCreacion,
                FechaActualizacion: entity.FechaActualizacion,
                CantidadStock: entity.CantidadStock
            );
        }
    }
}
