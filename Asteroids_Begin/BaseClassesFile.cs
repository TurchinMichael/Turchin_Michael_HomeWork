﻿using System;
using System.Drawing;

namespace MyGame
{
    /// <summary>
    /// Исключение при задании неподходящих характеристик
    /// </summary>
    class CharacteristicException : Exception
    {
        public CharacteristicException()
        {
            Console.WriteLine("Ошибка заданных характеристик (отрицательные, или слишком большие(>100) размеры, слишком большая скорость (>200) или неверная позиция).");
        }
    }

    /// <summary>
    /// Базовый класс для игровых объектов
    /// </summary>
    abstract class BaseObject : ICollision
    {
        #region 5. Добавить в пример обобщенный делегат.
        /// <summary>
        /// Событие коллизии для отображения в журнале
        /// </summary>
        public static event Action<object, object> JournalCollisionMessage;
        #endregion

        /// <summary>
        /// Событие выстрела для отображения в журнале
        /// </summary>
        public static event Message MessageShot;

        /// <summary>
        /// Координаты объекта
        /// </summary>
        protected Point Pos;

        /// <summary>
        /// Изменение положения и скорость объекта
        /// </summary>
        protected Point Dir;

        /// <summary>
        /// Размер объекта
        /// </summary>
        protected Size Size;

        public Rectangle Rect => new Rectangle(Pos, Size);

        public Point Position { get { return Pos; } set { Pos = value; } }

        protected BaseObject(Point pos, Point dir, Size size) // protected - создать экземпляр такого класса нельзя, а унаследовать конструктор можно. / заметка для себя
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            if (dir.X > 200 || dir.X < -100 || dir.Y > 200 || dir.Y < -100 || Size.Height < 0 || Size.Width < 0 || Size.Height > 100 || Size.Width > 100)
                throw new CharacteristicException();
        }

        public static void BaseObject__journalCollisionMessage(object sender, object receiver)
        {
            Console.WriteLine($"\n{receiver.ToString()} попал в {sender.ToString()}");
        }

        public static void Energy__journalMessage(int h)
        {
            string temp = String.Empty;
            if (h > 0)
                temp = $"Было потеряно {h.ToString()} энергии";
            else
                temp = $"Было добавлено {Math.Abs(h).ToString()} энергии";

            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.WriteLine(temp);

            Console.WriteLine(temp);
        }

        public static void Bullet_MessageShot()
        {
            string temp = "\nБыл совершён выстрел";
            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.WriteLine(temp);
            Console.WriteLine(temp);
        }

        /// <summary>
        /// Метод для отрисовки объектов и задания характеристик формы
        /// </summary>
        public abstract void Draw(); // т.к. метод астрактный, он не может иметь тело / заметка для себя

        /// <summary>
        /// Метод для обновления изменения расположения / скорости / размеров объектов
        /// </summary>
        public abstract void Update();


        /// <summary>
        /// Метод для отслеживания столкновений объектов
        /// </summary>
        public bool Collision(ICollision obj)
        {
            if (obj.Rect.IntersectsWith(this.Rect))
                BaseObject__journalCollisionMessage(this, obj);
            return obj.Rect.IntersectsWith(this.Rect);
        }
    }

    /// <summary>
    /// Объект - звезда в виде двух пересеченных линий
    /// </summary>
    class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
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
            if (Pos.X > Game.Width) Pos.X = 0 + Size.Width;
        }
    }

    /// <summary>
    /// Объект - пятиконечная звезда, состоящая из 5 линий
    /// </summary>
    class TrueStar : BaseObject
    {
        public TrueStar(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
        {
        }

        public override void Draw()
        {
            /*перечисление линий для отрисовки пятиконечной звезды*/
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

        int i = 0;
        public override void Update()
        {
            i++;
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            Random g = new Random();
            int j = g.Next(5, Game.Width / 8);

            if (Pos.X > Game.Width) Pos.X = Game.Width / 2 + j;

            if (Pos.X < 0) Pos.X = Game.Width / 2 - j;
        }
    }

    /// <summary>
    /// Объект - астероид, круг вписанный в прямоугольник
    /// </summary>
    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        public int Power { get; set; }

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
        {
            Power = Game.rnd.Next(-9, -1);
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X + 100, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height)) { Power = Power };
            asteroid.Power = Power;
            return asteroid;
        }

        /// <summary>
        /// Компаратор - сравнивает два объекта по параметру силы - Power
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable<Asteroid>.CompareTo(Asteroid obj)
        {
            if (Power > obj.Power)
                return 1;
            if (Power < obj.Power)
                return -1;
            else
                return 0;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
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

    #region 3. Разработать аптечки, которые добавляют энергию
    /// <summary>
    /// Объект - аптечка, круг вписанный в прямоугольник
    /// </summary>
    class Aid : BaseObject
    {
        public int Power { get; set; }

        public Aid(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
        {
            Power = Game.rnd.Next(1, 9);
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Green, Pos.X, Pos.Y, Size.Width, Size.Height);
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
    #endregion

    /// <summary>
    /// Объект - пуля, прямоугольник
    /// </summary>
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
        {
            Bullet_MessageShot();
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            //if (Pos.X > Game.Width)
            //{
            //    Console.WriteLine("out");
            //}
        }
    }
}