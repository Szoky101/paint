using System;
using System.Collections.Generic;
using System.IO;

namespace vegleges
{
    internal class Program
    {
        static string libery = "rajzok";
        static char minta = '█';
        static char torles = ' ';

        static void Main(string[] args)
        {
            if (!Directory.Exists(libery))
                Directory.CreateDirectory(libery);

            Menu();
        }

        private static void Menu()
        {
            string[] menuButtons = { "Új", "Betöltés", "Kilépés" };
            int selectedIndex = 0;
            ConsoleKey key;
            ConsoleColor defaultForeground = Console.ForegroundColor;
            ConsoleColor defaultBackground = Console.BackgroundColor;
            ConsoleColor selectedForeground = ConsoleColor.Magenta;
            ConsoleColor selectedBackground = ConsoleColor.White;

            do
            {
                Console.Clear();

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth - menuButtons[i].Length) / 2, Console.WindowHeight / 2 - 1 + i);
                    Console.CursorVisible = false;
                    if (i == selectedIndex)

                    {
                        Console.ForegroundColor = selectedForeground;
                        Console.BackgroundColor = selectedBackground;
                        Console.WriteLine(menuButtons[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = defaultForeground;
                        Console.BackgroundColor = defaultBackground;
                        Console.WriteLine(menuButtons[i]);
                    }
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? menuButtons.Length - 1 : selectedIndex - 1;

                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == menuButtons.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        if (selectedIndex == 0)
                        {
                            Drawing();
                        }
                        else if (selectedIndex == 1)
                        {
                            ListDrawings();
                        }
                        else if (selectedIndex == 2)
                        {
                            Environment.Exit(0);
                        }
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        private static void Drawing(string filePath = null)
        {
            List<string> drawing = new List<string>();
            Console.Clear();

            if (filePath != null)
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var parts = line.Split(',');
                    Console.SetCursorPosition(int.Parse(parts[0]), int.Parse(parts[1]));
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), parts[3]);
                    Console.Write(parts[2]);
                    drawing.Add(line);
                }
            }

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
                Console.CursorVisible = true;

                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (Console.CursorLeft < Console.WindowWidth - 1)
                            Console.CursorLeft++;
                        if (Console.CapsLock)
                        {
                            Console.Write(minta);
                            Console.CursorLeft--;
                            drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{minta},{Console.ForegroundColor}");
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Console.CursorLeft > 0)
                            Console.CursorLeft--;
                        if (Console.CapsLock)
                        {
                            Console.Write(minta);
                            Console.CursorLeft--;
                            drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{minta},{Console.ForegroundColor}");
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop > 0)
                            Console.CursorTop--;
                        if (Console.CapsLock)
                        {
                            Console.Write(minta);
                            Console.CursorLeft--;
                            drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{minta},{Console.ForegroundColor}");
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop < Console.WindowHeight - 1)
                            Console.CursorTop++;
                        if (Console.CapsLock)
                        {
                            Console.Write(minta);
                            Console.CursorLeft--;
                            drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{minta},{Console.ForegroundColor}");
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        Console.Write(minta);
                        drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{minta},{Console.ForegroundColor}");
                        Console.CursorLeft--;
                        break;
                    case ConsoleKey.Backspace:
                        Console.Write(torles);
                        drawing.Add($"{Console.CursorLeft},{Console.CursorTop},{torles},{Console.ForegroundColor}");
                        Console.CursorLeft--;
                        break;
                    case ConsoleKey.F1:
                        minta = '█';
                        break;
                    case ConsoleKey.F2:
                        minta = '▓';
                        break;
                    case ConsoleKey.F3:
                        minta = '▒';
                        break;
                    case ConsoleKey.F4:
                        minta = '░';
                        break;
                    case ConsoleKey.W:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case ConsoleKey.A:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case ConsoleKey.S:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        break;
                    case ConsoleKey.D:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case ConsoleKey.E:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case ConsoleKey.R:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case ConsoleKey.T:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;


                }
            } while (key != ConsoleKey.Escape);

            SaveDrawing(drawing, filePath);
        }

        private static void SaveDrawing(List<string> drawing, string filePath = null)
        {
            if (filePath == null)
            {
                filePath = Path.Combine(libery, $"Paint {DateTime.Now:MMddHHmmss}.txt");
            }
            File.WriteAllLines(filePath, drawing);
            Console.WriteLine($"\nMentve: {filePath}");
            Console.ReadKey();
        }

        private static void ListDrawings()
        {
            Console.Clear();
            var files = Directory.GetFiles(libery);
            if (files.Length == 0)
            {
                Console.WriteLine("Nincsenek rajzok a könyvtárban.");
                Console.ReadKey();
                return;
            }

            int selectedIndex = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Mentett rajzok (Enter: szerkesztés, Delete: törlés):");
                ConsoleColor defaultForeground = Console.ForegroundColor;
                ConsoleColor defaultBackground = Console.BackgroundColor;
                ConsoleColor selectedForeground = ConsoleColor.Magenta;
                ConsoleColor selectedBackground = ConsoleColor.White;
                for (int i = 0; i < files.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = selectedForeground;
                        Console.BackgroundColor = selectedBackground;
                        Console.WriteLine(Path.GetFileName(files[i]));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = defaultForeground;
                        Console.BackgroundColor = defaultBackground;
                        Console.WriteLine(Path.GetFileName(files[i]));
                    }
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? files.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == files.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        Drawing(files[selectedIndex]);
                        break;
                    case ConsoleKey.Delete:
                        File.Delete(files[selectedIndex]);
                        Console.WriteLine("Rajz törölve.");
                        Console.ReadKey();
                        return;
                }

            } while (key != ConsoleKey.Escape);
        }
    }
}