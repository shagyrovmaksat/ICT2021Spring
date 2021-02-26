using System;
using System.Collections.Generic;
using System.Text;

namespace Example1
{
    class Game
    {
        public static int Width { get { return 40; } }
        public static int Height { get { return 40; } }

        Wall wall;
        Worm w;
        Food f;

        public bool IsRunning { get; set; }

        public Game()
        {
            IsRunning = true;
            this.wall = new Wall('#', ConsoleColor.Black, @"Levels/Level1.txt");
            this.w = new Worm('@', ConsoleColor.Green);
            this.w.setWall(this.wall);
            this.f = new Food('$', ConsoleColor.Red);
        }

        private void NextLevel(String level)
        {
            this.wall = new Wall('#', ConsoleColor.Black, @$"Levels/{level}.txt");
            this.w = new Worm('@', ConsoleColor.Green);
            this.w.setWall(this.wall);
            this.f = new Food('$', ConsoleColor.Red);
        }

        bool CheckCollisionFoodWithWorm()
        {
            return w.body[0].X == f.body[0].X && w.body[0].Y == f.body[0].Y;
        }

        public void KeyPressed(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    w.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    w.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    w.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    w.Move(1, 0);
                    break;
                case ConsoleKey.Escape:
                    IsRunning = false;
                    break;
            }
            if (CheckCollisionFoodWithWorm())
            {
                w.Increase(w.body[0]);
                f.Generate(w, wall);
            }
            if(w.body.Count == 10)
            {
                Console.Clear();
                this.NextLevel("Level2");
            }
        }
    }
}
