using System;
using System.IO;

string[] header = {"The script to remove Revit files from the given folder.",
                   "The files in the subfolders are also included.",
                   "developed by wojciech.klepacki@grimshaw.global 2018-2022" };

foreach (string line in header)
{
    Console.WriteLine(line);
}

string? get_input = "";
List<string> get_output = new List<string>();

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
                get_output.Add(filename);
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
    int counter_true = 0;
    int counter_false = 0;
    Console.WriteLine("\n{0} Revit backup files are going to be deleted.", (get_output.Count).ToString());
    Console.Write("Do you want to proceed (Y/N)? ");
    get_continue = Console.ReadLine();
    if (get_continue.ToLower() == "y")
    {
        Console.WriteLine("Proceeding...\n");
        foreach (string filename in get_output)
        {
            string getName = Path.GetFileName(filename);
            try
            {
                File.Delete(filename);
                Console.WriteLine(getName + " ---> removed.");
                counter_true++;
            }
            catch
            {
                Console.WriteLine(getName + " ---> failed!");
                counter_false++;
            }
        }
        Console.WriteLine("\n{0} file(s) removed, {1} failed.", counter_true.ToString(), counter_false.ToString());
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