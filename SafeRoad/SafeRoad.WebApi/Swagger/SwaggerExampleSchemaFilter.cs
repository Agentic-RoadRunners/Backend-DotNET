using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

// Request DTOs
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.DTOs.WatchedArea;

// Response DTOs
using SafeRoad.Core.DTOs.Auth;
using SafeRoad.Core.DTOs.IncidentCategory;
using SafeRoad.Core.DTOs.Verification;

// Commands used directly as [FromBody]
using SafeRoad.Core.Features.Auth.Commands.Login;
using SafeRoad.Core.Features.Auth.Commands.Register;
using SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentStatus;

// Wrapper
using SafeRoad.Core.Wrappers;

namespace SafeRoad.WebApi.Swagger;

/// <summary>
/// Populates the "Example Value" section in Swagger UI with realistic,
/// pre-filled JSON for every request and response schema.
/// </summary>
public class SwaggerExampleSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        // ── ApiResponse<T> wrapper ──────────────────────────────────
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
        {
            var innerType = type.GetGenericArguments()[0];

            // Unwrap Nullable<T>
            var nullable = Nullable.GetUnderlyingType(innerType);
            if (nullable != null) innerType = nullable;

            IOpenApiAny? dataExample = null;

            if (innerType == typeof(string))
                dataExample = Str("Operation completed successfully.");
            else if (innerType == typeof(int))
                dataExample = Int(85);
            else if (innerType.IsGenericType && innerType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = innerType.GetGenericArguments()[0];
                var item = GetResponseExample(itemType);
                dataExample = item != null ? Arr(item) : null;
            }
            else
                dataExample = GetResponseExample(innerType);

            if (dataExample != null)
            {
                schema.Example = Obj(
                    ("succeeded", Bool(true)),
                    ("message", Str("Success")),
                    ("data", dataExample),
                    ("errors", Arr())
                );
            }
            return;
        }

        // ── Request-body types ──────────────────────────────────────
        var req = GetRequestExample(type);
        if (req != null) { schema.Example = req; return; }

        // ── Response DTO types ($ref schemas) ───────────────────────
        var res = GetResponseExample(type);
        if (res != null) schema.Example = res;
    }

    // ================================================================
    //  REQUEST  EXAMPLES
    // ================================================================
    private static IOpenApiAny? GetRequestExample(Type type)
    {
        if (type == typeof(RegisterCommand))
            return Obj(
                ("email", Str("john.doe@example.com")),
                ("password", Str("SecurePass123!")),
                ("fullName", Str("John Doe"))
            );

        if (type == typeof(LoginCommand))
            return Obj(
                ("email", Str("john.doe@example.com")),
                ("password", Str("SecurePass123!"))
            );

        if (type == typeof(CreateCommentRequest))
            return Obj(
                ("content", Str("I can confirm this pothole is still there. Be careful driving through this area."))
            );

        if (type == typeof(CreateIncidentRequest))
            return Obj(
                ("categoryId", Int(1)),
                ("municipalityId", Int(3)),
                ("title", Str("Large pothole on Main Street")),
                ("description", Str("Deep pothole near the intersection of Main St and Oak Ave. Approximately 30cm wide and 10cm deep.")),
                ("latitude", Dbl(39.9208)),
                ("longitude", Dbl(32.8541))
            );

        if (type == typeof(UpdateIncidentStatusCommand))
            return Obj(
                ("newStatus", Int(1))
            );

        if (type == typeof(UpdateProfileRequest))
            return Obj(
                ("fullName", Str("Jane Doe")),
                ("avatarUrl", Str("https://example.com/avatars/jane-doe.jpg"))
            );

        if (type == typeof(ChangePasswordRequest))
            return Obj(
                ("currentPassword", Str("OldSecurePass123!")),
                ("newPassword", Str("NewSecurePass456!"))
            );

        if (type == typeof(StartJourneyRequest))
            return Obj(
                ("startLatitude", Dbl(39.9208)),
                ("startLongitude", Dbl(32.8541)),
                ("endLatitude", Dbl(39.9334)),
                ("endLongitude", Dbl(32.8597))
            );

        if (type == typeof(CreateWatchedAreaRequest))
            return Obj(
                ("label", Str("My Neighborhood")),
                ("latitude", Dbl(39.9208)),
                ("longitude", Dbl(32.8541)),
                ("radiusInMeters", Int(2000))
            );

        if (type == typeof(UpdateWatchedAreaRequest))
            return Obj(
                ("label", Str("Updated Area Name")),
                ("latitude", Dbl(39.9250)),
                ("longitude", Dbl(32.8600)),
                ("radiusInMeters", Int(3000))
            );

        return null;
    }

    // ================================================================
    //  RESPONSE  EXAMPLES
    // ================================================================
    private static IOpenApiAny? GetResponseExample(Type type)
    {
        // ── Auth ────────────────────────────────────────────────────
        if (type == typeof(AuthResponse))
            return Obj(
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("email", Str("john.doe@example.com")),
                ("fullName", Str("John Doe")),
                ("accessToken", Str("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhMWIyYzNkNCIsInJvbGUiOiJVc2VyIiwiZXhwIjoxNzA4OTg3NjU0fQ.signature")),
                ("roles", Arr(Str("User")))
            );

        // ── Comment ────────────────────────────────────────────────
        if (type == typeof(CommentResponse))
            return Obj(
                ("id", Int(42)),
                ("incidentId", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("userName", Str("John Doe")),
                ("content", Str("I can confirm this pothole is still there.")),
                ("createdAt", Str("2026-02-21T14:30:00Z"))
            );

        // ── Incident ───────────────────────────────────────────────
        if (type == typeof(IncidentResponse))
            return IncidentResponseExample();

        if (type == typeof(IncidentDetailResponse))
            return IncidentDetailResponseExample();

        if (type == typeof(CommentDto))
            return Obj(
                ("id", Int(42)),
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("userName", Str("John Doe")),
                ("content", Str("I saw this too, it's quite dangerous.")),
                ("createdAt", Str("2026-02-21T15:00:00Z"))
            );

        if (type == typeof(VerificationDto))
            return Obj(
                ("id", Int(10)),
                ("userId", Str("c3d4e5f6-7890-abcd-ef12-345678901234")),
                ("userName", Str("Jane Smith")),
                ("isPositive", Bool(true)),
                ("createdAt", Str("2026-02-21T16:00:00Z"))
            );

        // ── Incident Category ──────────────────────────────────────
        if (type == typeof(IncidentCategoryResponse))
            return Obj(
                ("id", Int(1)),
                ("name", Str("Pothole"))
            );

        // ── User ───────────────────────────────────────────────────
        if (type == typeof(UserProfileResponse))
            return Obj(
                ("id", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("email", Str("john.doe@example.com")),
                ("fullName", Str("John Doe")),
                ("avatarUrl", Str("https://example.com/avatars/john-doe.jpg")),
                ("trustScore", Int(85)),
                ("status", Str("Active")),
                ("roles", Arr(Str("User"))),
                ("createdAt", Str("2025-01-15T10:30:00Z"))
            );

        if (type == typeof(PublicProfileResponse))
            return Obj(
                ("id", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("fullName", Str("John Doe")),
                ("avatarUrl", Str("https://example.com/avatars/john-doe.jpg")),
                ("trustScore", Int(85)),
                ("createdAt", Str("2025-01-15T10:30:00Z"))
            );

        if (type == typeof(UserStatsResponse))
            return Obj(
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("totalIncidents", Int(12)),
                ("totalComments", Int(34)),
                ("totalVerifications", Int(56)),
                ("trustScore", Int(85)),
                ("memberSince", Str("2025-01-15T10:30:00Z"))
            );

        // ── Journey ────────────────────────────────────────────────
        if (type == typeof(StartJourneyResponse))
            return Obj(
                ("journeyId", Str("d4e5f6a7-8901-bcde-f234-567890abcdef")),
                ("distanceInKm", Dbl(5.2)),
                ("estimatedMinutes", Int(12)),
                ("incidentsOnRoute", Int(2)),
                ("incidents", Arr(
                    Obj(
                        ("id", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                        ("title", Str("Road construction ahead")),
                        ("categoryName", Str("Construction")),
                        ("latitude", Dbl(39.9256)),
                        ("longitude", Dbl(32.8563)),
                        ("distanceFromRouteMeters", Dbl(45)),
                        ("status", Str("Verified"))
                    )
                )),
                ("message", Str("Warning: 2 incident(s) found on your route. Drive carefully!"))
            );

        if (type == typeof(RouteIncidentDto))
            return Obj(
                ("id", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                ("title", Str("Road construction ahead")),
                ("categoryName", Str("Construction")),
                ("latitude", Dbl(39.9256)),
                ("longitude", Dbl(32.8563)),
                ("distanceFromRouteMeters", Dbl(45)),
                ("status", Str("Verified"))
            );

        if (type == typeof(EndJourneyResponse))
            return Obj(
                ("journeyId", Str("d4e5f6a7-8901-bcde-f234-567890abcdef")),
                ("distanceInKm", Dbl(5.2)),
                ("durationMinutes", Int(18)),
                ("message", Str("Journey completed. Thank you for using SafeRoad!")),
                ("askForIncidentReport", Bool(true)),
                ("incidentPrompt", Str("Did you notice any road incidents during your trip? Help others by reporting them!"))
            );

        if (type == typeof(UserJourneyResponse))
            return Obj(
                ("id", Str("d4e5f6a7-8901-bcde-f234-567890abcdef")),
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("startLatitude", Dbl(39.9208)),
                ("startLongitude", Dbl(32.8541)),
                ("endLatitude", Dbl(39.9334)),
                ("endLongitude", Dbl(32.8597)),
                ("status", Str("Completed")),
                ("startedAt", Str("2026-02-21T08:00:00Z")),
                ("endedAt", Str("2026-02-21T08:18:00Z"))
            );

        // ── Verification ───────────────────────────────────────────
        if (type == typeof(VerificationResponse))
            return Obj(
                ("id", Int(10)),
                ("incidentId", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                ("userName", Str("John Doe")),
                ("isPositive", Bool(true)),
                ("createdAt", Str("2026-02-21T16:00:00Z"))
            );

        if (type == typeof(VerificationSummaryResponse))
            return Obj(
                ("incidentId", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                ("positiveCount", Int(5)),
                ("negativeCount", Int(1)),
                ("verifications", Arr(
                    Obj(
                        ("id", Int(10)),
                        ("incidentId", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
                        ("userId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
                        ("userName", Str("John Doe")),
                        ("isPositive", Bool(true)),
                        ("createdAt", Str("2026-02-21T16:00:00Z"))
                    )
                ))
            );

        // ── Watched Area ───────────────────────────────────────────
        if (type == typeof(WatchedAreaResponse))
            return Obj(
                ("id", Int(7)),
                ("label", Str("My Neighborhood")),
                ("latitude", Dbl(39.9208)),
                ("longitude", Dbl(32.8541)),
                ("radiusInMeters", Int(2000))
            );

        return null;
    }

    // ================================================================
    //  COMPOSITE HELPERS
    // ================================================================
    private static OpenApiObject IncidentResponseExample() => Obj(
        ("id", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
        ("reporterUserId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
        ("reporterName", Str("John Doe")),
        ("categoryId", Int(1)),
        ("categoryName", Str("Pothole")),
        ("municipalityId", Int(3)),
        ("municipalityName", Str("Çankaya")),
        ("title", Str("Large pothole on Main Street")),
        ("description", Str("Deep pothole near the intersection. Approximately 30cm wide.")),
        ("latitude", Dbl(39.9208)),
        ("longitude", Dbl(32.8541)),
        ("status", Str("Verified")),
        ("positiveVerifications", Int(5)),
        ("negativeVerifications", Int(1)),
        ("commentCount", Int(3)),
        ("photoUrls", Arr(Str("https://storage.saferoad.com/photos/incident-123.jpg"))),
        ("createdAt", Str("2026-02-20T09:15:00Z"))
    );

    private static OpenApiObject IncidentDetailResponseExample() => Obj(
        ("id", Str("b7e3c2a1-4d5f-6789-0abc-def123456789")),
        ("reporterUserId", Str("a1b2c3d4-e5f6-7890-abcd-ef1234567890")),
        ("reporterName", Str("John Doe")),
        ("categoryId", Int(1)),
        ("categoryName", Str("Pothole")),
        ("municipalityId", Int(3)),
        ("municipalityName", Str("Çankaya")),
        ("title", Str("Large pothole on Main Street")),
        ("description", Str("Deep pothole near the intersection. Approximately 30cm wide.")),
        ("latitude", Dbl(39.9208)),
        ("longitude", Dbl(32.8541)),
        ("status", Str("Verified")),
        ("positiveVerifications", Int(5)),
        ("negativeVerifications", Int(1)),
        ("commentCount", Int(3)),
        ("photoUrls", Arr(Str("https://storage.saferoad.com/photos/incident-123.jpg"))),
        ("createdAt", Str("2026-02-20T09:15:00Z")),
        ("comments", Arr(
            Obj(
                ("id", Int(42)),
                ("userId", Str("c3d4e5f6-7890-abcd-ef12-345678901234")),
                ("userName", Str("Jane Smith")),
                ("content", Str("I saw this too, it's quite dangerous.")),
                ("createdAt", Str("2026-02-20T10:00:00Z"))
            )
        )),
        ("verifications", Arr(
            Obj(
                ("id", Int(10)),
                ("userId", Str("c3d4e5f6-7890-abcd-ef12-345678901234")),
                ("userName", Str("Jane Smith")),
                ("isPositive", Bool(true)),
                ("createdAt", Str("2026-02-20T09:45:00Z"))
            )
        ))
    );

    // ================================================================
    //  SHORTHAND  BUILDERS
    // ================================================================
    private static OpenApiObject Obj(params (string key, IOpenApiAny value)[] props)
    {
        var obj = new OpenApiObject();
        foreach (var (key, value) in props)
            obj[key] = value;
        return obj;
    }

    private static OpenApiArray Arr(params IOpenApiAny[] items)
    {
        var arr = new OpenApiArray();
        arr.AddRange(items);
        return arr;
    }

    private static OpenApiString Str(string v) => new(v);
    private static OpenApiInteger Int(int v) => new(v);
    private static OpenApiDouble Dbl(double v) => new(v);
    private static OpenApiBoolean Bool(bool v) => new(v);
}
