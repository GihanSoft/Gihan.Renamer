using Gihan.Helpers.String;
using Gihan.Renamer.Models;
using Gihan.Renamer.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gihan.Renamer.SystemIO.CLI
{
    class Program
    {
        private enum Order : sbyte
        {
            None= -1,
            Process,
            Rename
        }
        static void Main(string[] args)
        {
            Order order = Order.None;
            string rootFolderPath = null;
            string jPatterns = null;
            RenameFlags flags = RenameFlags.Default;
            string jOrders = null;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-p") order = Order.Process;
                if (arg == "-r") order = Order.Rename;
                if (arg == "--rootFolder") rootFolderPath = args[++i];
                if (arg == "--patterns") jPatterns = args[++i];
                if (arg == "--flag" || arg == "-f") flags = (RenameFlags)byte.Parse(args[++i]);
                if (arg == "--orders" || arg == "-o") jOrders = args[++i];
            }
            IEnumerable<ReplacePattern> patterns = null;
            if (jPatterns != null)
                patterns = JsonConvert.DeserializeObject<IEnumerable<ReplacePattern>>(jPatterns);
            IEnumerable<RenameOrder> orders = null;
            if (jOrders != null)
                orders = JsonConvert.DeserializeObject<IEnumerable<RenameOrder>>(jOrders);

            if (order == Order.None) return;

            switch (order)
            {
                case Order.Process:
                    var processor = new RenameProcessor();
                    var rOrders = processor.ProcessReplace(rootFolderPath, patterns, flags);
                    Console.WriteLine(JsonConvert.SerializeObject(rOrders));
                    break;
                case Order.Rename:
                    var renamer = new Renamer();
                    renamer.Rename(orders, flags);
                    break;
            }
        }
    }
}
