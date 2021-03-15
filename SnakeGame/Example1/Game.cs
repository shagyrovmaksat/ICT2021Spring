using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Example1
{
    class Game
    {
        public bool pause = false;
        public int indLevel = 1;
        Timer wormTimer = new Timer(100);
        Timer gameTimer = new Timer(1000);
        public static int Width { get { return 40; } }
        public static int Height { get { return 40; } }

        Wall wall;
        Worm w;
        Food f;

        public bool IsRunning;
        public Game()
        {
            this.pause = false;
            IsRunning = true;
            this.wall = new Wall('#', ConsoleColor.White, @$"Levels/Level{indLevel}.txt");
            this.w = new Worm('@', ConsoleColor.Green);
            this.f = new Food('$', ConsoleColor.Red);
            wormTimer.Elapsed += Move;
            wormTimer.Start();
            gameTimer.Elapsed += GameTimer;
            gameTimer.Start();
            Console.CursorVisible = false;
            Console.SetWindowSize(Width, Width);
            Console.SetBufferSize(Width, Width);
        }
        int s = 0, m = 0;
        public void GameTimer(object sender, ElapsedEventArgs e)
        {
            Console.Title = String.Format("{0}:{1}", m.ToString(), s.ToString());
            s++;
            if(s == 60)
            {
                m++;
                s = 0;
            }
        }

        private void NextLevel()
        {
            indLevel = 2;
            wall = new Wall('#', ConsoleColor.White, @$"Levels/Level{indLevel}.txt");
            this.w = new Worm('@', ConsoleColor.Green);
            this.f = new Food('$', ConsoleColor.Red);
        }
            
        bool CheckCollisionFoodWithWorm()
        {
            return w.body[0].X == f.body[0].X && w.body[0].Y == f.body[0].Y;
        }

        public void GameOver()
        {
            gameTimer.Stop();
            wormTimer.Stop();
            this.IsRunning = false;
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You lose!");
        }

        public void Move(object sender, ElapsedEventArgs e)
        {
            if (!w.Move(wall))
            {
                GameOver();
            }
            else
            {
                Console.SetCursorPosition(10, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Food count: " + (w.body.Count - 1));

                if (CheckCollisionFoodWithWorm())
                {
                    w.Increase(w.body[0]);
                    f.Generate(w, wall);
                }
                if (w.body.Count == 4)
                {
                    Console.Clear();
                    this.NextLevel();
                }
            }
            
        }

        public void KeyPressed(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    w.ChangeDirection(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    w.ChangeDirection(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    w.ChangeDirection(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    w.ChangeDirection(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    if(!pause)
                    {
                        wormTimer.Stop();
                        pause = true;
                    }
                    else
                    {
                        wormTimer.Start();
                        pause = false;
                    }
                    break;
                case ConsoleKey.S:
                    wormTimer.Stop();
                    w.Save();
                    wormTimer.Start();
                    break;
                case ConsoleKey.L:
                    wormTimer.Stop();
                    w.Clear();
                    wall.Clear();
                    f.Clear();
                    w = Worm.Load();
                    wall = new Wall('#', ConsoleColor.White, @$"Levels/Level{indLevel}.txt");
                    f = new Food('$', ConsoleColor.Red);
                    wormTimer.Start();
                    break;
                case ConsoleKey.Escape:
                    IsRunning = false;
                    wormTimer.Stop();
                    gameTimer.Stop();
                    break;
            }
        }
    }
}
