using System;
using System.Collections.Generic;
using System.Text;

namespace Example1
{
    class Food : GameObject
    {
        Random rnd = new Random();
        public Food(char sign, ConsoleColor color) : base(sign, color)
        {
            Point location = new Point { X = 15, Y = 15 };
            body.Add(location);
            Draw();
        }

        public bool Collision(GameObject w)
        {
            for (int i = w.body.Count - 1; i > 0; --i)
            {
                if(w.body[i].X == this.body[0].X && w.body[i].Y == this.body[0].Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Generate(Worm w, Wall wall)
        {
            body[0].X = rnd.Next(1, 39);
            body[0].Y = rnd.Next(1, 39);
            while(Collision(w) || Collision(wall))
            {
                body[0].X = rnd.Next(1, 39);
                body[0].Y = rnd.Next(1, 39);
            }
            Draw();
        }
    }
}
