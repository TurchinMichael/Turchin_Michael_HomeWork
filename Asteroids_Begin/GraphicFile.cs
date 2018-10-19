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

        public static int Width { get; set; }
        public static int Height { get; set; }
        
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
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
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
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
        } 

        public static BaseObject[] _objs;
        public static void Load()
        {
            Random r = new Random();
            _objs = new BaseObject[90];

            #region Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон, похожий на полет в звездном пространстве.
            for (int i = 0; i < _objs.Length; i++)
            {
                int s = r.Next(4, 20);
                int h = r.Next(5, Height);
                if (i % 2 == 0)
                    _objs[i] = new TrueStar(new Point(Width / 2 + 30, h /*Height / 2*/), new Point( i, 0), new Size(s, s));
                else
                    _objs[i] = new TrueStar(new Point(Width / 2 - 30, h /*Height / 2*/), new Point(-i, 0), new Size(s, s));
            }
            #endregion
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}