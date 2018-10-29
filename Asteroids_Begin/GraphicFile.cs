using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyGame
{
    /// <summary>
    /// Класс описывающий графическую составляющую
    /// </summary>
    static class Game
    {
        static BufferedGraphicsContext _context;
        static Timer timer = new Timer();
        static int height = 0;
        static int width = 0;
        static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
        public static Random rnd = new Random();
        public static BufferedGraphics Buffer;
        public static Form thisForm;

        /// <summary>
        /// Ширина окна
        /// </summary>
        public static int Width
        {
            get { return width; }
            set
            {
                if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else width = value;
            }
        }

        /// <summary>
        /// Высота окна
        /// </summary>
        public static int Height
        {
            get { return height; }
            set
            {
                if (value <= 0 || value >= 2000) throw new ArgumentOutOfRangeException();
                else height = value;
            }
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

            Ship.MessageDie += Finish;
            BaseObject.JournalCollisionMessage += BaseObject.BaseObject__journalCollisionMessage;
            BaseObject.MessageShot += BaseObject.Bullet_MessageShot;


            form.KeyDown += Form_KeyDown;
            Load();
            timer = new Timer { Interval = 100 }; // запуск события в заданный интервал времени
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(50, 0), new Size(20, 1));
                _ship.GetEnergy(rnd.Next(-3, -1));
            }
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }

        /// <summary>
        /// Рисование объектов, и вызов отрисовки в объектах
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject bs in _objs)
                bs?.Draw();
            foreach (Asteroid ast in _asteroids)
                ast?.Draw();
            foreach (Aid aid in _aids)
                aid?.Draw();

            _bullet?.Draw();
            _ship?.Draw();

            if (_ship != null)
            {
                Buffer.Graphics.DrawString($"Energy: {_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString($"Score: {_ship.Score}", SystemFonts.DefaultFont, Brushes.White, 0, 20);
            }

            Buffer.Render();
        }

        /// <summary>
        /// Обновление состояний и действий объектов для вызова через интервал вызовов в таймере
        /// </summary>
        public static void Update()
        {
            for (int i = 0; i < _asteroids.Length; i++) //foreach (Asteroid astr in _asteroids)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();

                if (!(_bullet is null) && _asteroids[i].Collision(_bullet))
                {
                    _ship.Score++;
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    continue;
                };
                if (_ship.Collision(_asteroids[i]))
                {
                    _ship?.GetEnergy(_asteroids[i].Power);
                    System.Media.SystemSounds.Asterisk.Play();
                }
                if (_ship.Energy <= 0) _ship?.Die();
            }

            for (int i = 0; i < _aids.Length; i++) //foreach (Asteroid astr in _asteroids)
            {
                if (_aids[i] == null) continue;
                _aids[i].Update();

                if (!(_bullet is null) && _aids[i].Collision(_bullet))
                {
                    System.Media.SystemSounds.Question.Play();
                    _aids[i] = null;
                    _bullet = null;
                    continue;
                }
                if (_ship.Collision(_aids[i]))
                {
                    _ship?.GetEnergy(_aids[i].Power);
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }

            foreach (BaseObject obj in _objs)
                obj.Update();

            _bullet?.Update();
        }

        public static Aid[] _aids;
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
                _asteroids = new Asteroid[4];
                _aids = new Aid[2];

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
                for (int i = 0; i < _aids.Length; i++)
                {
                    int r = rnd.Next(20, 50);
                    _aids[i] = new Aid(new Point(100, rnd.Next(0, Game.Height)), new Point(-r / 2, r), new Size(r, r));
                }
            }
            catch (CharacteristicException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Метод заканчивающий игру
        /// </summary>
        public static void Finish()
        {
            timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 202, 100);
            Buffer.Render();
            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.Close();
        }

        /// <summary>
        /// Метод, вызывающийся событием срабатывания таймера
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