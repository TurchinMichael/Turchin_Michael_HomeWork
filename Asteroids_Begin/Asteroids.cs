using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace MyGame
{
    public delegate void Message();
    public delegate void EnergyMessage(int energy);
    //public delegate void JournalCollisionMessage(object sender, object receiver); // был создан обобщенный делегат

    /// <summary>
    /// Основная программа
    /// </summary>
    class Program
    {
        public static StreamWriter streamWriter = new StreamWriter("test.txt");

        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };

            #region 2 Дана коллекция List<T>. Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции
            SecondTask.Second();
            #endregion

            #region 3 * Дан фрагмент программы
            ThirdTask.Third();
            #endregion
            
            //InitForm1(form);
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);
            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.Close();
        }
    }
}