#region © 2019 JoeWare.
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.IO;
using System.Linq;
using Trinet.Core.IO.Ntfs;

namespace UnblockFolder
{
    class Program
    {
        private static string cr = Environment.NewLine;
        private static string crt = $"{cr}\t";

        // ------------------------------------------------

        static void Main(string[] args)
        {
            if(args.Count() > 0)
            {
                var path = Path.GetDirectoryName(args[0]);
                Console.WriteLine($"Unblocking Folder: {path}");

                Unblock(path);
                Console.WriteLine($"{Environment.NewLine}Finished...");
            }
        }

        // ------------------------------------------------

        private static void Unblock(string path)
        {
            try
            {
                // -----------------
                // Unblock any files

                foreach(string fileName in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(fileName);

                    if(fileInfo.AlternateDataStreamExists("Zone.Identifier"))
                    {
                        Console.WriteLine($"Unblocking File:{crt}{fileName}");

                        var stream = fileInfo.GetAlternateDataStream("Zone.Identifier", FileMode.Open);

                        // ---------------------------
                        // Delete the stream (Unblock)

                        stream.Delete();
                    }
                }

                // ---------------------
                // Deal with any folders

                foreach(string folder in Directory.GetDirectories(path))
                {
                    Console.WriteLine($"{cr}Unblocking Folder: {folder}");
                    Unblock(folder);
                }
            }
            catch(System.Exception exp)
            {
                throw new ArgumentException(exp.Message, exp);
            }
        }
    }
}
