using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace ListMyPerms
{
    [ApiVersion(2, 1)]
    public class ListMyPerms : TerrariaPlugin
    {
        public override Version Version { get { return new Version(1,0); } }
        public override string Name { get { return "ListMyPerms"; } }
        public override string Author { get { return "magnusi"; } }
        public override string Description { get { return "Lists permissions of all commands."; } }
        public ListMyPerms(Main game) : base(game) { }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("magnusi.perms", ListPerms, "listperms"));
            Commands.ChatCommands.Add(new Command("magnusi.findcmd", CmdFinder, "cmdfinder"));
        }

        public void ListPerms(CommandArgs args)
        {
            var i = 0;
            var index = 0;

            Console.WriteLine("****LIST OF PERMISSIONS FOUND****");
            Console.WriteLine(Commands.ChatCommands.Count.ToString() + " commands found. ");
            try
            {
                i = 0 + ((Convert.ToInt16(args.Parameters[0].ToString()) - 1) * 50);
                Console.WriteLine("****Page " + args.Parameters[0] + " out of " + (Commands.ChatCommands.Count / 50).ToString() + "****");
            }
            catch { Console.WriteLine("****Page 1 out of " + (Commands.ChatCommands.Count / 50).ToString() + "****"); }

            foreach (var command in Commands.ChatCommands)
            {
                index++;
                if (index > i && index < i + 50)
                    switch (command.Permissions.Count)
                    {
                        default:
                            string str = String.Join(", ", command.Permissions);
                            Console.WriteLine(str + ":" + command.Name);
                            break;
                        case 0:
                            Console.WriteLine("no permissions :" + command.Name);
                            break;
                    }
            }
        }
        public void CmdFinder(CommandArgs args)
        {
            try
            {
                var i = false;
                foreach (var command in Commands.ChatCommands)
                    if (command.Name.IndexOf(args.Parameters[0]) != -1)
                    {
                        if (i)
                        {
                            args.Player.SendInfoMessage("Following commands contain searched term:");
                            i = false;
                        }
                        try
                        {
                                string str = String.Join(", ", command.Permissions);
                                args.Player.SendInfoMessage(str + ":" + command.Name);
                        }
                        catch { args.Player.SendInfoMessage("no permissions :" + command.Name); }
                    }
            }
            catch(Exception potato)
            {
                args.Player.SendErrorMessage("Command not found. Check your spelling and try again.");
                if (args.Parameters.Contains("debug"))
                    Console.WriteLine(potato);
            }
        }
    }
}
