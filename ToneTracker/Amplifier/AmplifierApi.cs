using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToneTracker.Amplifier;
using ToneTracker.Filters;
using ToneTracker.Dial;
using ToneTracker.Toggle;

namespace ToneTracker.Pedal;

internal static class AmplifierApi
{
    public static RouteGroupBuilder MapAmplifiers(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/amplifiers");

        group.WithTags("Amplifiers");

        group.WithParameterValidation(typeof(AmplifierItem));
        
        group.MapGet("/{userId:guid}/all", async Task<Results<Ok<List<AmplifierItem>>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid userId) =>
        {
            var amplifiers = await context.Amplifiers
                .Where(a => a.UserId == userId)
                .Include(a => a.Dials)
                .ThenInclude(d => d.Settings)
                .Include(a => a.Toggles)
                .ThenInclude(t => t.Settings)
                .Select(a => a.AsAmplifierItem())
                .ToListAsync();

            return amplifiers.Any() ? TypedResults.Ok(amplifiers) : TypedResults.NotFound();
        });

        group.MapGet("/{id:guid}", async Task<Results<Ok<AmplifierItem>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid id) =>
        {
            var amplifier = await context.Amplifiers
                .Where(a => a.Id == id)
                .Include(a => a.Dials)
                .ThenInclude(d => d.Settings)
                .Include(a => a.Toggles)
                .ThenInclude(t => t.Settings)
                .Select(a => a.AsAmplifierItem())
                .FirstOrDefaultAsync();

            return amplifier != null ? TypedResults.Ok(amplifier) : TypedResults.NotFound();
        });


        
        group.MapPost("/", async Task<Created<AmplifierItem>> (ToneTrackerDbContext context, AmplifierItem amplifierItem) =>
        {
            var amplifier = new Amplifier.Amplifier
            {
                Name = amplifierItem.Name,
                UserId = amplifierItem.UserId,
                Dials = amplifierItem.Dials?.Select(d => d.AsDial()).ToList(),
                Toggles = amplifierItem.Toggles?.Select(t => t.AsToggle()).ToList()
            };

            context.Amplifiers.Add(amplifier);
            await context.SaveChangesAsync();
            
            return TypedResults.Created($"/amplifier/{amplifier.Id}", amplifier.AsAmplifierItem());
        });

        return group;
    }
}
