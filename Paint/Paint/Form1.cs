using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    enum Tool
    {
        Line, 
        Rectangle,
        Pen,
        Fill,
        Circle,
        Eraser,
        Star,
        Pentagon,
        Octagon
    }

    public partial class Form1 : Form
    {
        Bitmap bitmap = default(Bitmap);
        Bitmap colorBitmap = default(Bitmap);
        Graphics graphics = default(Graphics);
        Graphics colorGraphics = default(Graphics);
        Pen pen = new Pen(Color.Black, 5);
        Pen eraserPen = new Pen(Color.White, 5);
        Point prevPoint = default(Point);
        Point currentPoint = default(Point);
        Color color = default(Color);
        Image star = Image.FromFile(@"C:\Users\Бекзат\Desktop\ICT\Paint\star.png");
        Image pentagon = Image.FromFile(@"C:\Users\Бекзат\Desktop\ICT\Paint\pentagon-outline-shape.png");
        Image octagon = Image.FromFile(@"C:\Users\Бекзат\Desktop\ICT\Paint\octagon-outline-shape.png");
        bool isMousePressed = false;
        Tool currentTool = Tool.Pen;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            colorBitmap = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            graphics = Graphics.FromImage(bitmap);
            colorGraphics = Graphics.FromImage(colorBitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            pictureBox1.Image = bitmap;
            pictureBox2.Image = colorBitmap;
            graphics.Clear(Color.White);
            colorGraphics.Clear(Color.Black);
            открытьToolStripMenuItem.Click += ОткрытьToolStripMenuItem_Click;
            сохранитьToolStripMenuItem.Click += СохранитьToolStripMenuItem_Click;
        }

        private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(saveFileDialog1.FileName);
            }
        }

        private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap = Bitmap.FromFile(openFileDialog1.FileName) as Bitmap;
                pictureBox1.Image = bitmap;
                graphics = Graphics.FromImage(bitmap);
            }
        }

        Rectangle getMyRectangle(Point prevPoint, Point currentPoint)
        { 
            return new Rectangle { X = Math.Min(prevPoint.X, currentPoint.X), Y = Math.Min(prevPoint.Y, currentPoint.Y), Width = Math.Abs(prevPoint.X - currentPoint.X), Height = Math.Abs(prevPoint.Y - currentPoint.Y)};
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Location.ToString();

            if (isMousePressed)
            {
                switch (currentTool)
                {
                    case Tool.Pen:
                    case Tool.Eraser:
                        switch (currentTool)
                        {
                            case Tool.Eraser:
                                graphics.DrawLine(eraserPen, prevPoint, e.Location);
                                break;
                            case Tool.Pen:
                                graphics.DrawLine(pen, prevPoint, e.Location);
                                break;
                            default:
                                break;
                        }
                        prevPoint = currentPoint;
                        currentPoint = e.Location;
                        break;
                    case Tool.Line: 
                    case Tool.Rectangle:
                    case Tool.Circle:
                    case Tool.Star:
                    case Tool.Octagon:
                    case Tool.Pentagon:
                        currentPoint = e.Location;
                        break;
                    default:
                        break;
                } 
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prevPoint = e.Location;
            currentPoint = e.Location;
            isMousePressed = true;
            if(currentTool == Tool.Fill)
            {
                //my fill
                //bitmap = Utils.Fill(bitmap, currentPoint, bitmap.GetPixel(e.X, e.Y), Color.Red);
                //pictureBox1.Image = bitmap;
                //pictureBox1.Refresh();
                MapFill mf = new MapFill();
                mf.Fill(graphics, currentPoint, color, ref bitmap);
                graphics = Graphics.FromImage(bitmap);
                pictureBox1.Image = bitmap;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
            switch (currentTool)
            {
                case Tool.Pen:
                    break;
                case Tool.Line:
                    graphics.DrawLine(pen, prevPoint, currentPoint);
                    break;
                case Tool.Star:
                    Rectangle r = getMyRectangle(prevPoint, currentPoint);
                    graphics.DrawImage(star, r);
                    break;
                case Tool.Rectangle:
                    graphics.DrawRectangle(pen, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Circle:
                    graphics.DrawEllipse(pen, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Pentagon:
                    graphics.DrawImage(pentagon, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Octagon:
                    graphics.DrawImage(octagon, getMyRectangle(prevPoint, currentPoint));
                    break;
                default:
                    break;
            }
            prevPoint = e.Location;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            switch (currentTool)
            {
                case Tool.Pen:
                    break;
                case Tool.Line:
                    e.Graphics.DrawLine(pen, prevPoint, currentPoint); 
                    break;
                case Tool.Rectangle:
                    e.Graphics.DrawRectangle(pen, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Star:
                    e.Graphics.DrawImage(star, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Pentagon:
                    e.Graphics.DrawImage(pentagon, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Octagon:
                    e.Graphics.DrawImage(octagon, getMyRectangle(prevPoint, currentPoint));
                    break;
                case Tool.Circle:
                    e.Graphics.DrawEllipse(pen, getMyRectangle(prevPoint, currentPoint));
                    break;
                default:
                    break;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                pen.Color = color;
                colorGraphics.Clear(color);
                pictureBox2.Refresh();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = Convert.ToSingle(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            eraserPen.Width = Convert.ToSingle(numericUpDown2.Value);
        }

        private void buttonChangeTool(object sender, EventArgs e)
        {
            Button b = sender as Button;
            switch (b.Name)
            {
                case "buttonEraser":
                    currentTool = Tool.Eraser;
                    break;
                case "buttonLine":
                    currentTool = Tool.Line;
                    break;
                case "buttonCircle":
                    currentTool = Tool.Circle;
                    break;
                case "buttonRectangle":
                    currentTool = Tool.Rectangle;
                    break;
                case "buttonPentagon":
                    currentTool = Tool.Pentagon;
                    break;
                case "buttonOctagon":
                    currentTool = Tool.Octagon;
                    break;
                case "buttonStar":
                    currentTool = Tool.Star;
                    break;
                case "buttonFill":
                    currentTool = Tool.Fill;
                    break;
                case "buttonPen":
                    currentTool = Tool.Pen;
                    break;
                default:
                    break;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            pen.Color = b.BackColor;
            color = b.BackColor;
            colorGraphics.Clear(color);
            pictureBox2.Refresh();
        }
    }
}
