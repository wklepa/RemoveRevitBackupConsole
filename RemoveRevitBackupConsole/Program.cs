using System;
using System.IO;

// Print header
string[] header = {"The script to remove Revit files from the given folder.",
                   "The files in the subfolders are also included.",
                   "developed by wojciech.klepacki@grimshaw.global 2018-2022" };

foreach (string line in header)
{
    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line.Length / 2)) + "}", line));
}

// Define variables to be used globally
string? get_input = "";
string backup_name = "_RemovedBackupFiles.txt";
List<string> get_output = new List<string>();
List<string> get_report = new List<string>();
List<int> get_length = new List<int>();

while (true)
{
    Console.Write("\nEnter the directory name: ");
    get_input = Console.ReadLine();

    if (get_input == "")
    {
        Console.WriteLine("The input can't be blank!");
        continue;
    }
    else if (get_input != "" && Directory.Exists(get_input) == true)
    {
        Console.WriteLine("The directory exists, continue...");
        break;
    }
    else
    {
        Console.WriteLine("The directory doesn't exist!");
        continue;
    }
}

Console.WriteLine("The working directory is {0}", Path.GetFullPath(get_input.ToUpper()));

string[] filesDirs = Directory.GetFiles(get_input, "*.*", SearchOption.AllDirectories);

if (filesDirs.Length > 0)
{
    int counter = 1;
    foreach (string filename in filesDirs)
    {
        if (Path.GetExtension(filename) == ".rfa" || Path.GetExtension(filename) == ".rft")
        {
            int tmpInt;
            string getName = Path.GetFileName(filename);
            string[] words = getName.Split(".");
            string getTmp = words[words.Length - 2];
            if (Int32.TryParse(getTmp, out tmpInt))
            {
                Console.WriteLine("{0}: {1}", counter.ToString(), getName);
                get_output.Add(filename);  // Add filename (name.extension and path)
                get_length.Add(getName.Length);  // Add filename only length
                counter++;
            }
        }
    }
}
else
{
    Console.WriteLine("The directory doesn't contain any files.");
}

if (get_output.Count > 0)
{
    string? get_continue = "";
    string? save_report = "";
    int counter_true = 0;
    int counter_false = 0;
    Console.WriteLine("\n{0} Revit backup files are going to be deleted.", (get_output.Count).ToString());
    Console.Write("Do you want to proceed (Y/N)? ");
    get_continue = Console.ReadLine();
    if (get_continue is not null && get_continue.ToLower() == "y")
    {
        // Calculate maximum length
        int max_length = get_length.Max();
        int length_factor = 5;
        char pad = '-';
        // Construct ZIP with get_output and get_length
        var outputLength = get_output.Zip(get_length, (n, l) => new {Name = n, Len = l});
        Console.WriteLine("Proceeding...\n");
        foreach (var assembly in outputLength)
        {
            string filename = assembly.Name;
            int length = assembly.Len;
            string getName = Path.GetFileName(filename);
            try
            {
                // Remove backup file from folder
                File.Delete(filename);
                string suffix = "---> removed.";
                int suffix_length = suffix.Length;
                int rec_length = max_length - length + suffix_length + length_factor;
                string suffix_padded = suffix.PadLeft(rec_length, pad);
                Console.WriteLine("{0}: {1}", (counter_true + 1).ToString(), getName + suffix_padded);
                get_report.Add(filename + "\tREMOVED");
                counter_true++;
            }
            catch
            {
                string suffix = "---> failed!";
                int suffix_length = suffix.Length;
                int rec_length = max_length - length + suffix_length + length_factor;
                string suffix_padded = suffix.PadLeft(rec_length, pad);
                Console.WriteLine("{0}: {1}", (counter_false + 1).ToString(), getName + suffix_padded);
                get_report.Add(filename + "\tFAILED");
                counter_false++;
            }
        }
        Console.WriteLine("\n{0} file(s) removed, {1} failed.", counter_true.ToString(), counter_false.ToString());
        Console.Write("Do you want to save the report (Y/N)? ");
        save_report = Console.ReadLine();
        if (save_report is not null && save_report.ToLower() == "y")
        {
            string save_path_name = Path.Combine(get_input, backup_name);
            await File.WriteAllLinesAsync(save_path_name, get_report);
        }
    }
    else
    {
        Console.WriteLine("The file(s) remained intact...");
    }
}
else
{
    Console.WriteLine("There are no Revit backup files to remove.");
}

// Clear list values
get_output.Clear();
get_report.Clear();
get_length.Clear();
