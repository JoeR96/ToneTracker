using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToneTracker.TestLib;
using ToneTracker.Tone;
using ToneItem = ToneTracker.TestLib.ToneItem;

namespace ToneTracker.FunctionalTests.ToneTests;

public class ToneTests
{
    public Guid userId;
    public ToneTrackerClient client;

    [OneTimeSetUp]
    public async Task InitiateClient()
    {
        var (client, _userId) = await ApiClientFactory.CreateToneTrackerClientAsync(true);
        this.client = client;
        userId = _userId;
    }
    [StepTest, Order(10)]
    public async Task Tone_CreatesAndPopulatesAmplifierAndPedals()
    {
        await "Tone is created and populated".__(async () =>
        {
            // Create an amplifier
            var amplifierItem = new AmplifierItem()
            {
                UserId = userId,
                Name = "Fender Bassbreaker 15",
                Dials = new List<DialItem>
                {
                    new DialItem()
                    {
                        Name = "Volume",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new DialItem()
                    {
                        Name = "Treble",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new DialItem()
                    {
                        Name = "Bass",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    }
                },
                Toggles = new List<ToggleItem>
                {
                    new ToggleItem()
                    {
                        Name = "Bright",
                        Settings = new List<TestLib.Setting>()
                        {
                            new TestLib.Setting()
                            {
                                SettingName = "On"
                            },
                            new TestLib.Setting()
                            {
                                SettingName = "Off"
                            }
                        }
                    }
                }
            };

            var createdAmplifier = await client.AmplifiersPOSTAsync(amplifierItem);
            createdAmplifier.Name.Should().Be(amplifierItem.Name);
            createdAmplifier.Dials.Count.Should().Be(amplifierItem.Dials.Count);
            createdAmplifier.Toggles.Count.Should().Be(amplifierItem.Toggles.Count);

            // Create a pedal
            var pedalItem = new PedalItem()
            {
                UserId = userId,
                Name = "OCD Overdrive V2",
                Dials = new List<DialItem>
                {
                    new DialItem()
                    {
                        Name = "Volume",
                        Settings = Enumerable.Range(0, 13).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new DialItem()
                    {
                        Name = "Drive",
                        Settings = Enumerable.Range(0, 13).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new DialItem()
                    {
                        Name = "Tone",
                        Settings = Enumerable.Range(0, 13).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    }
                },
                Toggles = new List<ToggleItem>
                {
                    new ToggleItem()
                    {
                        Name = "Toggle",
                        Settings = new List<TestLib.Setting>()
                        {
                            new TestLib.Setting()
                            {
                                SettingName = "Up"
                            },
                            new TestLib.Setting()
                            {
                                SettingName = "Down"
                            }
                        }
                    }
                }
            };

            var createdPedal = await client.PedalsPOSTAsync(pedalItem);
            createdPedal.Name.Should().Be(pedalItem.Name);
            createdPedal.Dials.Count.Should().Be(pedalItem.Dials.Count);
            createdPedal.Toggles.Count.Should().Be(pedalItem.Toggles.Count);

            // Create a tone with the created amplifier and pedal
            var toneItem = new ToneItem()
            {
                Name = "Arctic Monkeys",
                UserId = userId,
                AmplifierId = createdAmplifier.Id.GetValueOrDefault(),
                Pedals = new List<PedalItem>()
                {
                   createdPedal
                }
            };

            var createdTone = await client.TonesPOSTAsync(toneItem);
            createdTone.AmplifierId.Should().Be(createdAmplifier.Id.GetValueOrDefault());
            createdTone.Pedals.Count.Should().Be(1);
            createdTone.Pedals.First().Id.Should().Be(createdPedal.Id);

            // Retrieve the created tone
            var retrievedTone = await client.TonesGETAsync(createdTone.Id.GetValueOrDefault());
            retrievedTone.Should().BeEquivalentTo(createdTone);

            var newPedal = new PedalItem()
            {
                Name = "Boss DS1",
                Dials = createdPedal.Dials,
                Toggles = createdPedal.Toggles
            };

            var updateTonePedalResponse = await client.PedalsPOSTAsync(newPedal);
            // Update the tone
            var updatedToneItem = new ToneItem()
            {
                UserId = userId,
                AmplifierId = createdAmplifier.Id.GetValueOrDefault(),
                Pedals = new List<PedalItem>()
                {
                    createdPedal,
                    updateTonePedalResponse
                }
            };

            await client.TonesPUTAsync(createdTone.Id.GetValueOrDefault(), updatedToneItem);

            var updatedTone = await client.TonesGETAsync(createdTone.Id.GetValueOrDefault());
            updatedTone.AmplifierId.Should().Be(updatedToneItem.AmplifierId);
            updatedTone.Pedals.Count.Should().Be(updatedToneItem.Pedals.Count);
            updatedTone.Pedals.Any(p => p.Id == updatedToneItem.Pedals.ToList()[0].Id).Should().BeTrue();
            updatedTone.Pedals.Any(p => p.Id == updatedToneItem.Pedals.ToList()[1].Id).Should().BeTrue();

            await client.TonesDELETEAsync(createdTone.Id.GetValueOrDefault());

            var deletedTone = await client.TonesGETAsync(createdTone.Id.GetValueOrDefault());
            deletedTone.Should().BeNull();

        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        using var scope = ApiClientFactory.Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToneTrackerDbContext>();
        dbContext.Tones.RemoveRange(dbContext.Tones);
        dbContext.Amplifiers.RemoveRange(dbContext.Amplifiers);
        dbContext.Pedals.RemoveRange(dbContext.Pedals);
        dbContext.Dials.RemoveRange(dbContext.Dials);
        dbContext.Toggles.RemoveRange(dbContext.Toggles);
        dbContext.Settings.RemoveRange(dbContext.Settings);
        dbContext.SaveChanges();
    }
}
