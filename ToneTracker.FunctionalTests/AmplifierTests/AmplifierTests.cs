using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToneTracker.TestLib;

namespace ToneTracker.FunctionalTests.AmplifierTests;

public class AmplifierTests
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

    [StepTest, Order(2)]
    public async Task Amplifier_CreatesAndPopulatesSettings()
    {
        await "Amplifier is created and populated".__(async () =>
        {
            var amplifierItem = new AmplifierItem()
            {
                UserId = userId,
                Name = "Fender Bassbreaker 15",
                Dials = new List<DialItem>
                {
                    new()
                    {
                        Name = "Volume",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new()
                    {
                        Name = "Treble",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new()
                    {
                        Name = "Bass",
                        Settings = Enumerable.Range(0, 12).Select(i => new TestLib.Setting()
                        {
                            SettingName = i.ToString(),
                        }).ToList()
                    },
                    new()
                    {
                        Name = "Reverb",
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
                            new()
                            {
                                SettingName = "On"
                            },
                            new()
                            {
                                SettingName = "Off"
                            }
                        }
                    }
                }
            };

            var response = await client.AmplifiersPOSTAsync(amplifierItem);
            response.Name.Should().Be(amplifierItem.Name);
            response.Dials.Count.Should().Be(amplifierItem.Dials.Count);
            response.Toggles.Count.Should().Be(amplifierItem.Toggles.Count);

            var allAmplifiers = await client.AllAsync(userId);
            var amplifierResponse = await client.AmplifiersGETAsync(response.Id.GetValueOrDefault());

            amplifierResponse.Id.Should().Be(response.Id);
            amplifierResponse.Name.Should().Be(amplifierItem.Name);
            amplifierResponse.Dials.Count.Should().Be(amplifierItem.Dials.Count);
            amplifierResponse.Toggles.Count.Should().Be(amplifierItem.Toggles.Count);
        });
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        using var scope = ApiClientFactory.Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToneTrackerDbContext>();
        dbContext.Amplifiers.RemoveRange(dbContext.Amplifiers);
        dbContext.Dials.RemoveRange(dbContext.Dials);
        dbContext.Toggles.RemoveRange(dbContext.Toggles);
        dbContext.Settings.RemoveRange(dbContext.Settings);
        dbContext.SaveChanges();
    }
}
