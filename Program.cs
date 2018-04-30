using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;

namespace MyFirstDiscordBot
{
    class DiscordBot
    {
        static DiscordClient discord; // Discord client instance used to interact with Discord API

        static CommandsNextModule commands; //enable commands next

        static InteractivityModule interactivity;

        static VoiceNextClient voice;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] arg)
        {
            discord = new DiscordClient(new DiscordConfiguration //initializes the bot!
            {
                Token = "",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true, //output all state and doings in console
                LogLevel = LogLevel.Debug,
            });


            discord.MessageCreated += async e =>

            // MessageCreated is the event, += is subscribing the method to the event. When a MessageCreate event triggers, our method will run
            // async e is an async method. Async will run non blocking
            // e => is a lambda expression. It takes in input parameter e and returns with the statement

            {

                if (e.Message.Content.ToLower().StartsWith("ping"))
                {
                    // await suspends execution of this method until the task is complete.
                    // In this example, we suspend this method and wait for our message to parse.
                    // control resumes here when e.message.content.tolower().startswith is complete
                    //got em

                    await e.Message.RespondAsync("pong!");

                }
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration //configure the prefix with the commands
            {
                StringPrefix = ".",
                EnableDms = false
            });

            commands.RegisterCommands<MyCommands>();


            interactivity = discord.UseInteractivity(new InteractivityConfiguration //default configurations
            {
                // set default to delete reactions
                PaginationBehaviour = TimeoutBehaviour.Delete,

                // default pagination timeout to 5 minutes
                PaginationTimeout = TimeSpan.FromMinutes(1),

                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(1)
            });

            voice = discord.UseVoiceNext();

            await discord.ConnectAsync(); //Have to await an async method (Also why we had to make an async main task)
            await Task.Delay(-1); // Prevent the bot from flashing and quitting immediately
            
        }
    }
}