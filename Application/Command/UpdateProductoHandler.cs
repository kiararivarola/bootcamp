using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class UpdateProductoHandler
        : IRequestHandler<UpdateProductoCommand, ProductoResponse?>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public UpdateProductoHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse?> Handle(
            UpdateProductoCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _postgresDbContext.Productos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return null;

            entity.Codigo = request.Codigo;
            entity.Nombre = request.Nombre;
            entity.Descripcion = request.Descripcion;
            entity.Precio = request.Precio;
            entity.Activo = request.Activo;
            entity.CategoriaId = request.CategoriaId;
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
