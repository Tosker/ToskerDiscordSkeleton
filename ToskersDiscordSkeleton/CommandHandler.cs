using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ToskersDiscordSkeleton
{
    public class CommandHandler
    {
        private IServiceProvider service;
        private DiscordSocketClient client;
        private CommandService commandService;

        public CommandHandler(IServiceProvider service)
        {
            this.service = service;

            client = (DiscordSocketClient)service.GetService(typeof(DiscordSocketClient));
            commandService = (CommandService)service.GetService(typeof(CommandService));

            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;

            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

            // Create a Command Context
            var context = new CommandContext(client, message);

            // Execute the command. (result does not indicate a return value,
            // rather an object stating if the command executed succesfully)
            var result = await commandService.ExecuteAsync(context, argPos, service);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}