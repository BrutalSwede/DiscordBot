using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DiscordBot.SettingsService;

namespace DiscordBot.Modules
{
    class InfoModule : ModuleBase<SocketCommandContext>
    {

        [Command("shoutout")]
        [Summary("Shout out a user")]
        public async Task ShoutOutAsync(SocketUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;

            await ReplyAsync($"{user.Mention} is a dummy!");
        }


        [Command("newJson")]
        public async Task NewXml()
        {
            await ReplyAsync("Creating new settings xml");
            SettingsController.Init();
        }

        [Command("readJson")]
        public async Task ReadXml()
        {
            Settings s = await SettingsController.GetSettingsAsync();
            await ReplyAsync($"Current Settings:\n" +
                             $"PostRSS = \t {s.PostRSS}\n" +
                             $"TestBool = \t {s.TestBool}");
        }

        [Command("galnet")]
        [Summary("Enables/Disables Galnet RSS feed")]
        public async Task rss([Summary("Whether or not the bot will post RSS updates.")]bool setting)
        {
            Settings s = await SettingsController.GetSettingsAsync();

            if(setting == s.PostRSS)
            {
                await ReplyAsync($"RSS already set to {setting}");
                return;
            }
            else
            {
                await ReplyAsync($"Changing RSS setting.");
                s.PostRSS = setting;
                SettingsController.SaveSettings(s);
            }


        }
    }
}
