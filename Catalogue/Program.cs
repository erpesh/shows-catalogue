using Catalogue.Models;

namespace Catalogue
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineInterface CLI = new CommandLineInterface();

            CLI.ExecuteCommand(args);
        }
    }
}