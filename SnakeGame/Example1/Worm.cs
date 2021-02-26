using System;
using System.Collections.Generic;
using System.Text;

namespace Example1
{
    class Worm : GameObject
    {
        private Wall wall;

        public void setWall(Wall wall)
        {
            this.wall = wall;
        }

        public Worm(char sign, ConsoleColor color) : base(sign, color)
        {
            Point head = new Point { X = 20, Y = 20 };
            body.Add(head);
            Draw();
        }

        public bool Collision(int x, int y)
        {
            for (int i = wall.body.Count - 1; i > 0; --i)
            {
                if (wall.body[i].X == x && wall.body[i].Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Move(int dx, int dy)
        {
            if(!Collision(body[0].X+dx, body[0].Y+dy))
            {
                Clear();

                for (int i = body.Count - 1; i > 0; --i)
                {
                    body[i].X = body[i - 1].X;
                    body[i].Y = body[i - 1].Y;
                }

                body[0].X += dx;
                body[0].Y += dy;

                Draw();
            }
        }

        public void Increase(Point point)
        {
            body.Add(new Point { X = point.X, Y = point.Y });
        }
    }
}
