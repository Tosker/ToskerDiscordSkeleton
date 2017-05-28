using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using ToskersDiscordSkeleton.Services;

namespace ToskersDiscordSkeleton.Modules
{
    public class AudioModule : ModuleBase
    {
        private AudioService audioService;

        public AudioModule(AudioService audioService)
        {
            this.audioService = audioService;
        }

        [Command("join", RunMode = RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        public async Task Join()
            => await audioService.JoinAudio(Context.Guild, (Context.User as IGuildUser).VoiceChannel);

        [Command("stop", RunMode = RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        public async Task Stop()
            => await audioService.LeaveAudio(Context.Guild);

        [Command("play", RunMode = RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        public async Task Play([Remainder] string song)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\resources\\Audio\\{song}.mp3";
            if (path == null) { return; }
            await audioService.JoinAudio(Context.Guild, (Context.User as IGuildUser).VoiceChannel);
            await audioService.SendAudioAsync(Context.Guild, path);
            await audioService.LeaveAudio(Context.Guild);
        }
    }
}