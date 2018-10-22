using System;
using System.Drawing;

namespace MyGame
{

    #region Создать собственное исключение GameObjectException, которое появляется при попытке  создать объект с неправильными характеристиками (например, отрицательные размеры, слишком большая скорость или неверная позиция).
    class CharacteristicException : Exception
    {
        public CharacteristicException()
        {
            Console.WriteLine("Ошибка заданных характеристик (отрицательные, или слишком большие(>100) размеры, слишком большая скорость (>200) или неверная позиция).");
        }
    }
    #endregion

    abstract class BaseObject : ICollision
    {
        public static bool change = true;
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        public Rectangle Rect => new Rectangle(Pos, Size);

        public Point Position { get { return Pos; } set { Pos = value; } }

        protected BaseObject (Point pos, Point dir, Size size) // protected - создать экземпляр такого класса нельзя, а унаследовать конструктор можно.
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            if (dir.X > 200 || dir.X < -100 || dir.Y > 200 || dir.Y < -100 || Size.Height < 0 || Size.Width < 0 || Size.Height > 100 || Size.Width > 100 )
                throw new CharacteristicException();
        }
        public abstract void Draw(); // т.к. метод астрактный, он не может иметь тело
                                     //{
                                     //Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
                                     //}

        #region Переделать виртуальный метод Update в BaseObject в абстрактный и реализовать его в наследниках.
        public abstract void Update();
        #endregion
        //{
        //    Pos.X = Pos.X + Dir.X;
        //    Pos.Y = Pos.Y + Dir.Y;
        //    if (Pos.X < 0) Dir.X = -Dir.X;
        //    if (Pos.X > Game.Width) Dir.X = -Dir.X;
        //    if (Pos.Y < 0) Dir.Y = -Dir.Y;
        //    if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        //}
        public static void Change()
        {
            change = !change;
        }

        public bool Collision(ICollision obj) => obj.Rect.IntersectsWith(this.Rect);
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
            if (Pos.X > Game.Width) Pos.X = 0 + Size.Width; // уотакуот
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
    class Asteroid : BaseObject, ICloneable
    {
        public int Power { get; set; }

        public Asteroid (Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X+100, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height));
            asteroid.Power = Power;
            return asteroid;
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
    }
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X > Game.Width) Pos.X = 0;
        }
    }
}