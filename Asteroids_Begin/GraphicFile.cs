using System;
using System.Windows.Forms;
using System.Drawing;
namespace MyGame
{
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static Form thisForm;
        static int height = 0, width = 0;
        //public static int Width { get; set; }

        #region Сделать проверку на задание размера экрана в классе Game. Если высота или ширина (Width, Height) больше 1000 (2000) или принимает отрицательное значение, выбросить исключение ArgumentOutOfRangeException().
        public static int Width
        {
            get { return width; }
            set
            {
                if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else width = value;
            }
        }
        //public static int Width { get { return  } set { if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException(); } }
        public static int Height
        {
            get { return height; }
            set { if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else height = value; }
        }
        #endregion

        static Game()
        {
        }
        public static void Init(Form form)
        {
            thisForm = form;
            Graphics g; // вывод графики
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current; // Предоставляет доступ к объекту основного контекста буферизованной графики для домена (тоже понятней некуда.) приложения. Чтобы это не означало.
            g = form.CreateGraphics();
            try
            {
                Width = form.ClientSize.Width;
                Height = form.ClientSize.Height;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Значения высоты, или ширины экрана заданы отрицательными, или более 2000.");
            }
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height)); // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере / Creates a graphics buffer of the specified size using the pixel format of the specified 
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
            Load();
        }

        public static void Draw()
        {    // Проверяем вывод графики
            Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (BaseObject obj in _asteroids)
                obj.Draw();            
                _bullet.Draw();
        //foreach (BaseObject obj in _objsStar)
        //    obj.Draw();
        //foreach (BaseObject obj in _objsAsteroid)
        //    obj.Draw();
        //foreach (BaseObject obj in _objsBullet)
        //    obj.Draw();
        Buffer.Render();
        }

        public static void Update()
        {
            Random rnd = new Random();
            foreach (Asteroid obj in _asteroids)
            {
                obj.Update();
                if (obj.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();

                    # region Сделать так, чтобы при столкновении пули с астероидом они регенерировались в разных концах экрана.
                    obj.Position = new Point(rnd.Next(Width / 2, Width), rnd.Next(0, Height));
                    _bullet.Position = new Point(rnd.Next(0, Width / 2), rnd.Next(0, Height));
                    #endregion
                };
            }
            foreach (BaseObject obj in _objs)
                obj.Update();
            _bullet.Update();

            //foreach (BaseObject obj in _objsStar)
            //    obj.Update();
            //foreach (BaseObject obj in _objsAsteroid)
            //    obj.Update();
            //foreach (BaseObject obj in _objsBullet)
            //    obj.Update();
        }

        public static BaseObject[] _objs;
        //public static Star[] _objsStar;
        public static Asteroid[] _asteroids;
        public static Bullet _bullet;

        public static void Load()
        {
            try
            {
                _objs = new BaseObject[30];
                _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(20, 1));
                _asteroids = new Asteroid[3];
                var rnd = new Random();
                for (int i = 0; i < _objs.Length; i++)
                {
                    int r = rnd.Next(5, 50);
                    _objs[i] = new Star(new Point(100, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
                }
                for (int i = 0; i < _asteroids.Length; i++)
                {
                    int r = rnd.Next(20, 50);
                    _asteroids[i] = new Asteroid(new Point(100, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(r, r)/*заме такой размер*/);
                }
            }
            catch (CharacteristicException e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}