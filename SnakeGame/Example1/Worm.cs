using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Xml.Serialization;

namespace Example1
{
    public class Worm : GameObject
    {
        //public Wall wall;
        public int Dx { get; set; }
        public int Dy { get; set; }

        public Worm()
        {

        }

        public Worm(char sign, ConsoleColor color) : base(sign, color)
        {
            Point head = new Point { X = 20, Y = 20 };
            body.Add(head);
            Draw();
        }

        public bool Collision(int x, int y, Wall wall)
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

        public void ChangeDirection(int dx, int dy)
        {
            Dx = dx;
            Dy = dy;
        }

        public bool Move(Wall wall)
        {
            if(!Collision(body[0].X+Dx, body[0].Y+Dy, wall))
            {
                Clear();

                for (int i = body.Count - 1; i > 0; --i)
                {
                    body[i].X = body[i - 1].X;
                    body[i].Y = body[i - 1].Y;
                }

                body[0].X += Dx;
                body[0].Y += Dy;

                Draw();
                return true;
            }
            return false;
        }

        public void Increase(Point point)
        {
            body.Add(new Point { X = point.X, Y = point.Y });
        }

        public void Save()
        {
            using (FileStream fs = new FileStream("worm.xml", FileMode.OpenOrCreate, FileAccess.Write))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Worm));
                xs.Serialize(fs, this);
            }
        }
        public static Worm Load()
        {
            Worm res = null;
            using (FileStream fs = new FileStream("worm.xml", FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Worm));
                res = xs.Deserialize(fs) as Worm;
            }
            return res;
        }
    }
}
