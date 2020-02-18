using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRename
{
    class Program
    {
        readonly static IList<Tuple<string, string>> Filters = new List<Tuple<string, string>> {
            new Tuple<string, string>( @"php", @"mp4" ),
            new Tuple<string, string>( @"linkfile", "mp4" ),
        };

        static bool HasErrors = false;

        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"D:\");
            foreach (var file in files)
            {
                FilterFile(file);
            }

            if (HasErrors)
            {
                Console.ReadKey();
            }
        }

        static void FilterFile(string file)
        {
            var path = Path.GetDirectoryName(file);
            var file_name = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);

            foreach (var filter in Filters)
            {
                if (!extension.Contains(filter.Item1))
                {
                    continue;
                }

                while (File.Exists(Path.Combine(path, file_name + "." + filter.Item2)))
                {
                    file_name += "_";
                }

                while (true)
                {
                    try
                    {
                        File.Move(file, Path.Combine(path, file_name + "." + filter.Item2));
                        break;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Unable to access file: " + file);
                        HasErrors = true;
                        return;
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Unable to access file: " + file);
                        HasErrors = true;
                        return;
                    }
                }

            }
        }
    }
}
