
using FluentValidation;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.CreateWatchedArea;

public class CreateWatchedAreaCommandValidator : AbstractValidator<CreateWatchedAreaCommand>
{
    public CreateWatchedAreaCommandValidator()
    {
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

        RuleFor(x => x.RadiusInMeters)
            .GreaterThan(0).WithMessage("Radius must be greater than 0.")
            .LessThanOrEqualTo(50000).WithMessage("Radius cannot exceed 50km.");
    }
}