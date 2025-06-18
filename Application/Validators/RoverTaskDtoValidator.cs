using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class RoverTaskDtoValidator : AbstractValidator<RoverTaskDto>
    {
        public RoverTaskDtoValidator()
        {
            RuleFor(x => x.RoverName).NotEmpty().WithMessage("El nombre del rover es obligatorio.");
            RuleFor(x => x.TaskType).NotEmpty().Must(BeAValidTaskType)
                .WithMessage("Tipo de tarea inválido.");
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleFor(x => x.StartsAt).NotEmpty();
        }

        private bool BeAValidTaskType(string taskType)
        {
            return Enum.TryParse(typeof(Domain.Entities.TaskType), taskType, true, out _);
        }
    }
}
