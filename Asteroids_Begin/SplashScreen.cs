using System;
using System.Windows.Forms;
using System.Drawing;
namespace MyGame
{
    static class splash
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static Form thisForm;

        public static int Width { get; set; }
        public static int Height { get; set; }

        static splash()
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
            //Buffer.Render();
            Buffer.Graphics.FillRectangle(Brushes.Orange, new Rectangle(0, Height/2, Width, Height/2));
            //Buffer.Graphics.
            //Buffer.Graphics.Clear(Color.Black);
            foreach (NewBase obj in _objs)
                obj.Draw();
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (NewBase obj in _objs)
                obj.Update();
        }

        public static NewBase[] _objs;
        public static void Load()
        {
            Random r = new Random();

            _objs = new NewBase[90];
            for (int i = 0; i < _objs.Length; i++)
            {
                int s = r.Next(4, 20);
                int h = r.Next(5, Height);
                if (i % 2 == 0)
                    _objs[i] = new NewBase(new Point(Width / 2 + 30, h /*Height / 2*/), new Point(i, 0), new Size(s, s));
                else
                    _objs[i] = new NewBase(new Point(Width / 2 - 30, h /*Height / 2*/), new Point(-i, 0), new Size(s, s));
            }
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}