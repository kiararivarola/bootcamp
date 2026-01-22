using MediatR;

namespace BootcampCLT.Application.Command
{
    public record DeleteProductoCommand(int Id) : IRequest<bool>;
}
