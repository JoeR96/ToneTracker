using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToneTracker.Dial;
using ToneTracker.Toggle;

namespace ToneTracker.Amplifier;


public class Amplifier
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }
    [Required]
    public Guid UserId { get; set; } = default!;
    [Required]
    public virtual List<Dial.Dial> Dials { get; set; }
    public virtual List<Toggle.Toggle> Toggles { get; set; }
    public string Name { get; set; }
}

public class AmplifierItem
{
    [Required] public string Name { get; set; }
    public List<Dial.DialItem> Dials { get; set; }
    public List<Toggle.ToggleItem> Toggles { get; set; }
    public Guid? Id { get; set; }
    public Guid UserId { get; set; } = default!;
}

public static class AmplifierMappingExtensions
{
    public static AmplifierItem AsAmplifierItem(this Amplifier amplifier)
    {
        return new AmplifierItem
        {
            Name = amplifier.Name,
            Dials = amplifier.Dials.Select(d => d.AsDialItem()).ToList(),
            Toggles = amplifier.Toggles.Select(t => t.AsToggleItem()).ToList(),
            Id = amplifier.Id,
            UserId = amplifier.UserId
        };
    }

    public static Amplifier AsAmplifier(this AmplifierItem amplifierItem)
    {
        return new Amplifier
        {
            Name = amplifierItem.Name,
            Dials = amplifierItem.Dials?.Select(d => d.AsDial()).ToList(),
            Toggles = amplifierItem.Toggles?.Select(t => t.AsToggle()).ToList(),
            Id = amplifierItem.Id,
            UserId = amplifierItem.UserId
        };
    }
}