using System;
using System.Windows.Forms;
using System.Drawing;
namespace MyGame
{
    /// <summary>
    /// Класс описывающий графическую составляющую
    /// </summary>
    static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static Form thisForm;

        /// <summary>
        /// Высота и ширина окна
        /// </summary>
        static int height = 0, width = 0;

        public static int Width
        {
            get { return width; }
            set
            {
                if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else width = value;
            }
        }

        public static int Height
        {
            get { return height; }
            set { if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else height = value; }
        }

        static Game()
        {
        }

        /// <summary>
        /// Метод для инициализации игрового окна
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            thisForm = form;
            Graphics g;            
            _context = BufferedGraphicsManager.Current; // Предоставляет доступ к главному буферу графического контекста для текущего приложения / заметка для себя
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
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height)); // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере / Creates a graphics buffer of the specified size using the pixel format of the specified / заметка для себя

            Load();
            Timer timer = new Timer { Interval = 100 }; // запуск события в заданный интервал времени
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Рисование объектов, и вызов отрисовки в объектах
        /// </summary>
        public static void Draw()
        {    // Проверяем вывод графики
            Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (BaseObject obj in _asteroids)
                obj.Draw();            
                _bullet.Draw();

        Buffer.Render();
        }

        /// <summary>
        /// Обновление состояний и действий объектов для вызова через интервал вызовов в таймере
        /// </summary>
        public static void Update()
        {
            Random rnd = new Random();

            foreach (Asteroid obj in _asteroids)
            {
                obj.Update();
                if (obj.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();

                    obj.Position = new Point(rnd.Next(Width / 2, Width), rnd.Next(0, Height));
                    _bullet.Position = new Point(rnd.Next(0, Width / 2), rnd.Next(0, Height));
                };
            }

            foreach (BaseObject obj in _objs)
                obj.Update();

            _bullet.Update();
        }

        
        public static BaseObject[] _objs;
        public static Asteroid[] _asteroids;
        public static Bullet _bullet;

        /// <summary>
        /// Создание игровых объектов
        /// </summary>
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
                    _asteroids[i] = new Asteroid(new Point(100, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(r, r));
                }
            }
            catch (CharacteristicException e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        /// <summary>
        /// Метод, вызывающийся событием таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}