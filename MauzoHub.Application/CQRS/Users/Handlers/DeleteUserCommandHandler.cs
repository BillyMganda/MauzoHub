using MauzoHub.Application.CQRS.Users.Commands;
using MauzoHub.Application.CustomExceptions;
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
            if (request is null)
            {
                throw new BadRequestException("Bad request!");
            }

            try
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                if (user == null)
                {
                    throw new NotFoundException($"user with id {request.Id} not found");
                }

                await _userRepository.DeleteAsync(user);

                return Unit.Value;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
