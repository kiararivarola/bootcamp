using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BootcampCLT.Api
{
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IMediator mediator, ILogger<ProductoController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el detalle de un producto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Producto encontrado.</returns>
        [HttpGet("v1/api/productos")]
        [ProducesResponseType(typeof(IEnumerable<ProductoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProducto()
        {
            _logger.LogInformation("Inicia la petición para obtener productos");

            var result = await _mediator.Send(new GetProductosQuery());

            if (result == null)
            {
                _logger.LogInformation("No se encontraron productos");
                return NoContent();
            }

            return Ok(result);
        }


        /// <summary>
        /// Obtiene el detalle de un producto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Producto encontrado.</returns>
        [HttpGet("v1/api/productos/{id:int}")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> GetProductoById([FromRoute] int id)
        {
            _logger.LogInformation("Inicia la peticion con ProductoId={ProductoId}", id);
            //int aa = 0;
            //var das = 5 / aa;
            var result = await _mediator.Send(new GetProductoByIdQuery(id));

            if (result is null)
            {
                _logger.LogWarning("Esto no se pudo encontrar ProductoId={ProductoId}", id);
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="request">Datos del producto a crear.</param>
        /// <returns>Producto creado.</returns>
        [HttpPost("v1/api/productos")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> CreateProducto([FromBody] CreateProductoRequest request)
        {
            // Podés agregar validaciones rápidas si querés:
            // if (request is null) return BadRequest();
            

            var command = new CreateProductoCommand(
                Codigo: request.Codigo,
                Nombre: request.Nombre,
                Descripcion: request.Descripcion,
                Precio: request.Precio,
                Activo: request.Activo,
                CategoriaId: request.CategoriaId
            );

            var result = await _mediator.Send(command);

            return Created(string.Empty, result);
        }

        /// <summary>
        /// Actualiza completamente un producto existente.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="request">Datos completos a actualizar.</param>
        [HttpPut("v1/api/productos/{id:int}")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> UpdateProducto(
            [FromRoute] int id,
            [FromBody] UpdateProductoRequest request)
        {
            if (request == null)
                return BadRequest();

            _logger.LogInformation("Actualizando producto ProductoId={ProductoId}", id);

            var command = new UpdateProductoCommand(
                Id: id,
                Codigo: request.Codigo,
                Nombre: request.Nombre,
                Descripcion: request.Descripcion,
                Precio: request.Precio,
                Activo: request.Activo,
                CategoriaId: request.CategoriaId
            );

            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Producto no encontrado ProductoId={ProductoId}", id);
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Actualiza parcialmente un producto existente.
        /// Solo se modificarán los campos enviados.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="request">Campos a actualizar.</param>
        [HttpPatch("v1/api/productos/{id:int}")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> PatchProducto(
            [FromRoute] int id,
            [FromBody] PatchProductoRequest request)
        {
            if (request == null)
                return BadRequest();

            _logger.LogInformation("Actualización parcial del producto ProductoId={ProductoId}", id);

            var command = new PatchProductoCommand(
                Id: id,
                Codigo: request.Codigo,
                Nombre: request.Nombre,
                Descripcion: request.Descripcion,
                Precio: request.Precio,
                Activo: request.Activo,
                CategoriaId: request.CategoriaId
            );

            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Producto no encontrado ProductoId={ProductoId}", id);
                return NotFound();
            }

            return Ok(result);
        }


        /// <summary>
        /// Elimina un producto existente.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        [HttpDelete("v1/api/productos/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProducto([FromRoute] int id)
        {
            _logger.LogInformation("Eliminando producto ProductoId={ProductoId}", id);

            var result = await _mediator.Send(new DeleteProductoCommand(id));

            if (!result)
            {
                _logger.LogWarning("Producto no encontrado ProductoId={ProductoId}", id);
                return NotFound();
            }

            return NoContent();
        }

    }


}
