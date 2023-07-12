using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Domain.Interfaces;
using MediatR;

namespace MauzoHub.Application.CQRS.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            await _userRepository.DeleteAsync(user);

            return Unit.Value;
        }
    }
}
