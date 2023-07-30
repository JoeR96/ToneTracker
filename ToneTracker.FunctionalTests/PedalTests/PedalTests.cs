
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using ToneTracker.TestLib;

    namespace ToneTracker.FunctionalTests.PedalTests;

    public class PedalTests
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
        public async Task Pedal_CreatesAndPopulatesSettings()
        {
            // var client = ApiClientFactory.CreateToneTrackerClient()
            await "Pedal is created and populated".__(async () =>
            {
                var pedalItem = new PedalItem()
                {
                    UserId = userId,
                    Name = "OCD Overdrive V2",
                    Dials = new List<DialItem>
                    {
                        new()
                        {
                            Name = "Volume",
                            Settings = Enumerable.Range(1, 12).Select(i => new TestLib.Setting()
                            {
                                SettingName = i.ToString(),
                            }).ToList()
                            
                        },
                        new()
                        {
                            Name = "Drive",
                            Settings = Enumerable.Range(1, 12).Select(i => new TestLib.Setting()
                            {
                                SettingName = i.ToString(),
                            }).ToList()
                        },
                        new()
                        {
                            Name = "Gain",
                            Settings = Enumerable.Range(1, 12).Select(i => new TestLib.Setting()
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
                                new()
                                {
                                    SettingName = "Up"
                                },
                                new()
                                {
                                    SettingName = "Down"
                                }
                            }
                        }
                    }
                };
                
                var response = await client.PedalsPOSTAsync(pedalItem);
                response.Name.Should().Be(pedalItem.Name);
                response.Dials.Count.Should().Be(pedalItem.Dials.Count);
                response.Toggles.Count.Should().Be(pedalItem.Toggles.Count);

                var allPedals = await client.All2Async(userId);
                var pedalResponse = await client.PedalsGETAsync(response.Id.GetValueOrDefault());

                pedalResponse.Id.Should().Be(response.Id);
                pedalResponse.Name.Should().Be(pedalItem.Name);
                pedalResponse.Dials.Count.Should().Be(pedalItem.Dials.Count);
                pedalResponse.Toggles.Count.Should().Be(pedalItem.Toggles.Count);

            });
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            using var scope = ApiClientFactory.Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToneTrackerDbContext>();
            dbContext.Pedals.RemoveRange(dbContext.Pedals);
            dbContext.Dials.RemoveRange(dbContext.Dials);
            dbContext.Toggles.RemoveRange(dbContext.Toggles);
            dbContext.Settings.RemoveRange(dbContext.Settings);
            dbContext.SaveChanges();
        }
    }