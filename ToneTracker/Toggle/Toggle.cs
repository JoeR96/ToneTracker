using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToneTracker.Toggle;

public class Toggle
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? Id { get; set; }

    [Required] public Guid UserId { get; set; } = default!;
    [Required] public string Name { get; set; }
    public Guid? PedalId { get; set; }
    public Pedal.Pedal? Pedal { get; set; }
    public List<Setting> Settings { get; set; }

}

public class ToggleItem
{
    [Required] public string Name { get; set; }
    [Required]
    public List<Setting> Settings { get; set; }
}

public static class ToggleMappingExtensions
{
    public static ToggleItem AsToggleItem(this Toggle toggle)
    {
        return new ToggleItem
        {
            Name = toggle.Name,
            Settings = toggle.Settings

        };
    }

    public static Toggle AsToggle(this ToggleItem toggleItem)
    {
        return new Toggle()
        {
            Name = toggleItem.Name,
            Settings = toggleItem.Settings
        };
    }
}