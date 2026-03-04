using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Commands.StartJourney;

public class StartJourneyCommandHandler : IRequestHandler<StartJourneyCommand, ApiResponse<StartJourneyResponse>>
{
    private readonly IUserJourneyRepository _journeyRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IRoutingService _routingService;

    public StartJourneyCommandHandler(
        IUserJourneyRepository journeyRepository,
        IIncidentRepository incidentRepository,
        IRoutingService routingService)
    {
        _journeyRepository = journeyRepository;
        _incidentRepository = incidentRepository;
        _routingService = routingService;
    }

    public async Task<ApiResponse<StartJourneyResponse>> Handle(StartJourneyCommand request, CancellationToken cancellationToken)
    {
        // Auto-complete any stale active journey so the user can start a fresh one
        var activeJourney = await _journeyRepository.GetActiveByUserIdAsync(request.UserId);
        if (activeJourney != null)
        {
            activeJourney.Status = JourneyStatus.Completed;
            await _journeyRepository.UpdateAsync(activeJourney);
        }

        var routeResult = await _routingService.GetRouteAsync(
            request.StartLatitude, request.StartLongitude,
            request.EndLatitude, request.EndLongitude);

        if (routeResult == null)
            throw new BadRequestException("Could not calculate route. Please check your coordinates.");

        var journey = new UserJourney
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RoutePath = routeResult.RoutePath,
            Status = JourneyStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await _journeyRepository.AddAsync(journey);

        var incidentsOnRoute = await _incidentRepository.GetAlongRouteAsync(routeResult.RoutePath, 10);

        var response = new StartJourneyResponse
        {
            JourneyId = journey.Id,
            DistanceInKm = Math.Round(routeResult.DistanceInMeters / 1000, 1),
            EstimatedMinutes = (int)Math.Ceiling(routeResult.DurationInSeconds / 60),
            IncidentsOnRoute = incidentsOnRoute.Count(),
            Incidents = incidentsOnRoute.Select(i => new RouteIncidentDto
            {
                Id = i.Id,
                Title = i.Title,
                CategoryName = i.Category.Name,
                Latitude = i.Location.Y,
                Longitude = i.Location.X,
                DistanceFromRouteMeters = Math.Round(i.Location.Distance(routeResult.RoutePath), 0),
                Status = i.Status.ToString()
            }).ToList(),
            Message = incidentsOnRoute.Any()
                ? $"Warning: {incidentsOnRoute.Count()} incident(s) found on your route. Drive carefully!"
                : "No incidents reported on your route. Have a safe trip!"
        };

        return ApiResponse<StartJourneyResponse>.Success(response, "Journey started successfully.");
    }
}