using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ToneTracker.Pedal;

namespace ToneTracker.Tone
{
    public class Tone
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public virtual Amplifier.Amplifier Amplifier { get; set; }
        public virtual List<Pedal.Pedal> Pedals { get; set; }
        public string Name { get; set; }
    }

    public class ToneItem
    {
        public Guid UserId { get; set; }
        public Guid AmplifierId { get; set; }
        public List<PedalItem> Pedals { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public static class ToneMappingExtensions
    {
        public static ToneItem AsToneItem(this Tone tone)
        {
            return new ToneItem
            {
                Name = tone.Name,
                UserId = tone.UserId,
                AmplifierId = tone.Amplifier?.Id ?? Guid.Empty,
                Pedals = tone.Pedals?.Select(p => p.AsPedalItem()).ToList(),
                Id = tone.Id
            };
        }

        public static Tone AsTone(this ToneItem toneItem)
        {
            return new Tone
            {
                Name = toneItem.Name,
                UserId = toneItem.UserId,
                Amplifier = null, // Will be populated separately
                Pedals = toneItem.Pedals?.Select(p => p.AsPedal()).ToList()

            };
        }
    }
}