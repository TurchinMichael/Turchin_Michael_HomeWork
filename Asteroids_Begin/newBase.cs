using System;
using System.Drawing;

namespace MyGame
{
    class NewBase
    {
        public static bool change = true;
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        public NewBase(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        public virtual void Draw()
        {
            if (change)
            {
                /*lines*/
                /*1 | 2 - 4*/
                splash.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height / 2, Pos.X + Size.Width, Pos.Y + Size.Height / 2);
                /*2 | 4 - 1*/
                splash.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y + Size.Height / 2, Pos.X, Pos.Y + Size.Height);
                /*3 | 1 - 3*/
                splash.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height, Pos.X + Size.Width / 2, Pos.Y);
                /*4 | 3 - 5*/
                splash.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width / 2, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
                /*5 | 5 - 2*/
                splash.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y + Size.Height, Pos.X, Pos.Y + Size.Height / 2);
            }
            else
            {
                Graphics x = splash.thisForm.CreateGraphics();
                x.DrawImage(image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
            }
        }
        Bitmap image = new Bitmap(@"whiteStar.png");
        public virtual void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > splash.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > splash.Height) Dir.Y = -Dir.Y;
        }
        public static void Change()
        {
            change = !change;
        }
    }
}