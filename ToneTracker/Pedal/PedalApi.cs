using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToneTracker.Dial;
using ToneTracker.Filters;
using ToneTracker.Toggle;

namespace ToneTracker.Pedal;

internal static class PedalApi
{
     public static RouteGroupBuilder MapPedals(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/pedals");

        group.WithTags("Pedals");

        group.WithParameterValidation(typeof(PedalItem));
        
        group.MapGet("/{userId:guid}/all", async Task<Results<Ok<List<PedalItem>>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid userId) =>
        {
            var pedals = await context.Pedals
                .Where(p => p.UserId == userId)
                .Include(p => p.Dials)
                .ThenInclude(d => d.Settings)
                .Include(p => p.Toggles)
                .ThenInclude(t => t.Settings)
                .Select(p => p.AsPedalItem())
                .ToListAsync();


            return pedals.Any() ? TypedResults.Ok(pedals) : TypedResults.NotFound();
        });

        group.MapGet("/{id:guid}", async Task<Results<Ok<PedalItem>, NotFound>> (ToneTrackerDbContext context, [FromRoute] Guid id) =>
        {
            var pedal = await context.Pedals
                .Where(p => p.Id == id)
                .Include(p => p.Dials)
                .ThenInclude(d => d.Settings)
                .Include(p => p.Toggles)
                .ThenInclude(t => t.Settings)
                .Select(p => p.AsPedalItem())
                .FirstOrDefaultAsync();


            return pedal != null ? TypedResults.Ok(pedal) : TypedResults.NotFound();
        });

        
        group.MapPost("/", async Task<Created<PedalItem>> (ToneTrackerDbContext context, PedalItem pedalItem) =>
        {
            var pedal = new Pedal 
            {
                Name = pedalItem.Name,
                UserId = pedalItem.UserId,
                Dials = pedalItem.Dials?.Select(p => p.AsDial()).ToList(), 
                Toggles = pedalItem.Toggles?.Select(t => t.AsToggle()).ToList()  
            };
    
            context.Pedals.Add(pedal);
            await context.SaveChangesAsync();
            
            return TypedResults.Created($"/pedal/{pedal.Id}", pedal.AsPedalItem());
        });

        return group;
    }
}