using Discord;
using Discord.Audio;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ToskersDiscordSkeleton.Services
{
    ///
    /// ***READ IF ISSUES WITH  AUDIO STREAMING***
    ///
    /// *You must have FFMpeg downloaded and in the root of your application executable, e.g., Bin/Debug
    /// https://github.com/RogueException/Discord.Net/blob/dev/docs/guides/voice/sending-voice.md#transmitting-audio
    /// *You must also have opus.dll and libsodium.dll in the root of your application executable
    /// https://github.com/RogueException/Discord.Net/blob/dev/docs/guides/voice/sending-voice.md#installing
    ///
    /// This object implementation is from the kind works of Bond-009
    /// https://github.com/Bond-009/iTool.DiscordBot/blob/master/src/iTool.DiscordBot/Services/AudioService.cs
    ///
    public class AudioService
    {
        /// <summary>
        /// Collection of IAudioClient voice channel connections.
        /// </summary>
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        /// <summary>
        /// Join a voice channel.
        /// </summary>
        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            //Check if voice channel is already connected to
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client)
                || target.Guild.Id != guild.Id)
            {
                return;
            }

            //Add voice channel to a new connection
            ConnectedChannels.TryAdd(guild.Id, await target.ConnectAsync());
        }

        /// <summary>
        /// Stop current voice channel connection.
        /// </summary>
        public async Task LeaveAudio(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
            }
        }

        /// <summary>
        /// Send audio through connected voice channel.
        /// </summary>
        /// <param name="path">Audio file path.</param>
        public async Task SendAudioAsync(IGuild guild, string path)
        {
            //Audio file path does not exist, do not continue.
            if (!File.Exists(path))
            {
                return;
            }

            //Audio client is currently connected to voice channel
            if (ConnectedChannels.TryGetValue(guild.Id, out IAudioClient client))
            {
                //Create audio stream with FFMpeg binaries
                Stream output = CreateStream(path).StandardOutput.BaseStream;
                AudioOutStream stream = client.CreatePCMStream(AudioApplication.Music);

                //Copy FFMpeg stream to IAudioClient stream
                await output.CopyToAsync(stream);
                await stream.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create a new FFMpeg stream
        /// </summary>
        /// <param name="path">Audio file path.</param>
        private Process CreateStream(string path)
        {
            string filename;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                filename = "ffmpeg.exe";
            }
            else { filename = "ffmpeg"; }

            return Process.Start(new ProcessStartInfo()
            {
                FileName = filename,
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}