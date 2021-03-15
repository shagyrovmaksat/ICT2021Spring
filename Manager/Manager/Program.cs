using System;
using System.IO;
using System.Collections.Generic;

namespace Manager
{
    class Layer
    {
        public DirectoryInfo dir;
        public int pos;
        public Layer(DirectoryInfo dir, int pos)
        {
            this.dir = dir;
            this.pos = pos;
        }
        public long folderSize(DirectoryInfo dir)
        {
            long length = 0;
            foreach (FileInfo file in dir.GetFiles())
            {
                length += file.Length;
            }
            foreach (DirectoryInfo folder in dir.GetDirectories())
            {
                length += folderSize(folder);
            }
            return length;
        }

        public void printInfo()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(String.Format("Current directory: {0}; size = {1} bytes", dir.FullName, folderSize(dir)));
            int i = 0;
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                if (i == pos) Console.BackgroundColor = ConsoleColor.Blue;
                else Console.BackgroundColor = ConsoleColor.Black;

                Console.WriteLine((i + 1).ToString() + ") " + d.Name);
                i++;
            }

            foreach (FileInfo f in dir.GetFiles())
            {
                if (i == pos) Console.BackgroundColor = ConsoleColor.Blue;
                else Console.BackgroundColor = ConsoleColor.Black;

                Console.Write((i + 1).ToString() + ") " + f.Name);
                Console.WriteLine("   size: " + f.Length + " bytes");
                Console.ForegroundColor = ConsoleColor.White;
                i++;
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void setPosition(int x)
        {
            pos += x;
            if (pos == -1)
            {
                pos = dir.GetFileSystemInfos().Length - 1;
            }
            else if (pos == dir.GetFileSystemInfos().Length)
            {
                pos = 0;
            }
        }

        public FileSystemInfo getInfo()
        {
            FileSystemInfo[] dirs = dir.GetDirectories();
            FileSystemInfo[] files = dir.GetFiles();
            if (dirs.Length <= pos)
            {
                return files[pos - dirs.Length];
            }
            return dirs[pos];
        }
    }

    class FileManager
    {
        private Stack<Layer> history = new Stack<Layer>();
        public static bool isRunning = true;
        public FileManager()
        {
            string path = @"C:\Users\Админ\Desktop";
            history.Push(new Layer(new DirectoryInfo(path), 0));
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("If you want to create new file in this directory press N.");
            Console.ForegroundColor = ConsoleColor.White;
            history.Peek().printInfo();
        }
        public void ReadFile(FileSystemInfo file)
        {
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    // Read the stream as a string, and write the string to the console.
                    Console.WriteLine("Content of file: ");
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
            }
            System.Threading.Thread.Sleep(3000);
        }

        public void CreateFile()
        {
            Console.WriteLine("You are going ot create new file. Write the name of file.");
            string name = Console.ReadLine();
            Console.WriteLine("Write the content.");
            string content = Console.ReadLine();

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(history.Peek().dir.FullName, $"{name}.txt")))
            {
                outputFile.WriteLine(content);
            }
            Console.WriteLine("New file created!");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }

        public void KeyPressed(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    history.Peek().setPosition(-1);
                    break;
                case ConsoleKey.DownArrow:
                    history.Peek().setPosition(1);
                    break;
                case ConsoleKey.Enter:
                    if (history.Peek().dir.GetFileSystemInfos().Length == 0) break;

                    if (history.Peek().getInfo().GetType() == typeof(DirectoryInfo))
                    {
                        history.Push(new Layer(history.Peek().getInfo() as DirectoryInfo, 0));
                        Console.Clear();
                    }
                    else if (history.Peek().getInfo().Extension == ".txt")
                    {
                        ReadFile(history.Peek().getInfo());
                        Console.Clear();
                    }
                    break;
                case ConsoleKey.Escape:
                    if (history.Count != 1)
                    {
                        Console.Clear();
                        history.Pop();
                    }
                    else
                    {
                        isRunning = false;
                    }
                    break;
                case ConsoleKey.N:
                    CreateFile();
                    break;
            }
            Draw();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FileManager fileManager = new FileManager();
            fileManager.Draw();
            while (FileManager.isRunning == true)
            {
                Console.CursorVisible = false;
                fileManager.KeyPressed(Console.ReadKey(true));
            }
        }
    }
}
