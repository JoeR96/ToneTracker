using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToneTracker.Amplifier;
using ToneTracker.Filters;
using ToneTracker.Pedal;
using ToneTracker.Toggle;

namespace ToneTracker.Tone;

internal static class ToneApi
{
    public static RouteGroupBuilder MapTones(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/tones");

        group.WithTags("Tones");

        group.WithParameterValidation(typeof(ToneItem));
        
        group.MapGet("/{userId:guid}/all", async Task<Results<Ok<List<ToneItem>>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid userId) =>
        {
            var tones = await context.Tones
                .Where(t => t.UserId == userId)
                .Include(t => t.Amplifier)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Dials)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Toggles)
                .Select(t => t.AsToneItem())
                .ToListAsync();

            return tones.Any() ? TypedResults.Ok(tones) : TypedResults.NotFound();
        });

        group.MapGet("/{id:guid}", async Task<Results<Ok<ToneItem>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid id) =>
        {
            var tone = await context.Tones
                .Where(t => t.Id == id)
                .Include(t => t.Amplifier)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Dials)
                .ThenInclude(d => d.Settings)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Toggles)
                .ThenInclude(t => t.Settings)
                .Select(t => t.AsToneItem())
                .FirstOrDefaultAsync();

            return tone != null ? TypedResults.Ok(tone) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<ToneItem>, NotFound>> (ToneTrackerDbContext context, ToneItem toneItem) =>
        {
            var amplifier = await context.Amplifiers.FindAsync(toneItem.AmplifierId);
            if (amplifier == null)
                return TypedResults.NotFound();

            var pedals = new List<Pedal.Pedal>();
            foreach (var pedalItem in toneItem.Pedals)
            {
                var pedal = await context.Pedals
                    .Include(p => p.Dials)
                    .ThenInclude(d => d.Settings)
                    .Include(p => p.Toggles)
                    .ThenInclude(t => t.Settings)
                    .FirstOrDefaultAsync(p => p.Id == pedalItem.Id);

                if (pedal == null)
                    return TypedResults.NotFound();

                pedals.Add(pedal);
            }

            var tone = new Tone
            {
                Name = toneItem.Name,
                UserId = toneItem.UserId,
                Amplifier = amplifier,
                Pedals = pedals
            };

            context.Tones.Add(tone);
            await context.SaveChangesAsync();

            var createdResult = TypedResults.Created($"/tones/{tone.Id}", tone.AsToneItem());
            return createdResult;
        });



        group.MapPut("/{id:guid}", async Task<Results<NoContent, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid id, ToneItem toneItem) =>
        {
            var tone = await context.Tones
                .Where(t => t.Id == id)
                .Include(t => t.Amplifier)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Dials)
                .Include(t => t.Pedals)
                        .ThenInclude(p => p.Toggles)
                .FirstOrDefaultAsync();

            if (tone == null)
                return TypedResults.NotFound();

            var amplifier = await context.Amplifiers.FindAsync(toneItem.AmplifierId);
            if (amplifier == null)
                return TypedResults.NotFound();

            tone.Amplifier = amplifier;

            var existingPedalIds = tone.Pedals.Select(p => p.Id).ToList();
            var newPedalIds = toneItem.Pedals.Select(p => p.Id).ToList();

            var pedalsToRemove = tone.Pedals.Where(p => !newPedalIds.Contains(p.Id)).ToList();
            var pedalsToAdd = newPedalIds.Where(id => !existingPedalIds.Contains(id.GetValueOrDefault())).ToList();

            foreach (var pedalToRemove in pedalsToRemove)
                tone.Pedals.Remove(pedalToRemove);

            foreach (var pedalIdToAdd in pedalsToAdd)
            {
                var pedal = await context.Pedals.FindAsync(pedalIdToAdd);
                if (pedal == null)
                    return TypedResults.NotFound();

                tone.Pedals.Add(pedal);
            }

            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id:guid}", async Task<Results<NoContent, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid id) =>
        {
            var tone = await context.Tones.FindAsync(id);
            if (tone == null)
                return TypedResults.NotFound();

            context.Tones.Remove(tone);
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        return group;
    }
}
