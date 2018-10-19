using System;
using System.Drawing;

namespace MyGame
{
    class BaseObject
    {
        public static bool change = true;
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        public BaseObject (Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        public virtual void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public virtual void Update ()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
        public static void Change()
        {
            change = !change;
        }
    }
    class Star: BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
    #region * Заменить кружочки картинками, используя метод DrawImage.
    class TrueStar : BaseObject
    {
        public TrueStar(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            if (change)
            {
                /*lines*/
                /*1 | 2 - 4*/
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height / 2, Pos.X + Size.Width, Pos.Y + Size.Height / 2);
                /*2 | 4 - 1*/
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y + Size.Height / 2, Pos.X, Pos.Y + Size.Height);
                /*3 | 1 - 3*/
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y + Size.Height, Pos.X + Size.Width / 2, Pos.Y);
                /*4 | 3 - 5*/
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width / 2, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
                /*5 | 5 - 2*/
                Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y + Size.Height, Pos.X, Pos.Y + Size.Height / 2);
            }
            else
            {
                Bitmap image = new Bitmap(@"whiteStar.png");
                Graphics x = Game.thisForm.CreateGraphics();
                x.DrawImage(image, new Rectangle(Pos.X, Pos.Y, Size.Width, Size.Height));
            }
        }
        int i = 0;
        public override void Update()
        {
            i++;
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            //if (Pos.X > Game.Width || Pos.X < 0) Pos.X = Game.Width / 2;
            Random g = new Random();
            int j = g.Next(5, Game.Width / 8);

            if (Pos.X > Game.Width) Pos.X = Game.Width / 2 + j;

            if (Pos.X < 0) Pos.X = Game.Width / 2 - j;
        }
    }
    #endregion
}