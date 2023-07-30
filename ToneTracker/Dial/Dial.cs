using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToneTracker.Dial;

public class Dial
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public Guid UserId { get; set; } = default!;
    [Required]
    
    public string Name { get; set; }
    public Guid? PedalId { get; set; } 
    public Pedal.Pedal? Pedal { get; set; }
    public List<Setting> Settings { get; set; }

}

public class DialItem
{
    [Required] 
    public string Name { get; set; }
    [Required]
    public List<Setting> Settings { get; set; }

    
}   

public static class DialMappingExtensions
{
    public static DialItem AsDialItem(this Dial dial)
    {
        return new DialItem
        {
            Name = dial.Name,
            Settings = dial.Settings
        };
    }

    public static Dial AsDial(this DialItem dial)
    {
        return new Dial()
        {
            Name = dial.Name,
            Settings = dial.Settings
        };
    }
}