using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ToneTracker.Dial;
using ToneTracker.Toggle;
using System.Text.Json.Serialization;

namespace ToneTracker.Pedal;

public class Pedal
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; } = default!;
    [Required]
    public string Name { get; set; }
    public virtual List<Dial.Dial> Dials { get; set; }
    public virtual List<Toggle.Toggle> Toggles { get; set; }
}

public class PedalItem
{
    [Required] public string Name { get; set; }
    [Required]
    public Guid UserId { get; set; } = default!;
    public List<Dial.DialItem> Dials { get; set; }
    public List<Toggle.ToggleItem> Toggles { get; set; }
    public Guid? Id { get; set; }
}

public static class PedalMappingExtensions
{
    public static PedalItem AsPedalItem(this Pedal pedal)
    {
        return new PedalItem
        {
            Name = pedal.Name,
            Dials = pedal.Dials?.Select(p => p.AsDialItem()).ToList(), 
            Toggles = pedal.Toggles?.Select(t => t.AsToggleItem()).ToList(),
            Id = pedal.Id,
            UserId = pedal.UserId
        };
    }
    
    public static Pedal AsPedal(this PedalItem pedalItem)
    {
        return new Pedal
        {
            Name = pedalItem.Name,
            Dials = pedalItem.Dials?.Select(d => d.AsDial()).ToList(),
            Toggles = pedalItem.Toggles?.Select(t => t.AsToggle()).ToList(),
            UserId = pedalItem.UserId
        };
    }
}