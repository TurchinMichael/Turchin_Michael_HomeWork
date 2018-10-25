using System.Drawing;

namespace MyGame
{
    #region 1. Добавить космический корабль, как описано в уроке
    /// <summary>
    /// Объект - круг, вписанный в прямоугольник, представляющий космический корабль
    /// </summary>
    class Ship : BaseObject
    {
        private int _energy = 100;
        private int _score = 0;

        /// <summary>
        /// Энергия корабля
        /// </summary>
        public int Energy => _energy; // только аксессор, без мутатора / заметка для себя

        # region 4. Добавить подсчет очков за сбитые астероиды.
        /// <summary>
        /// Счет количества сбитых астероидов
        /// </summary>
        public int Score { get { return _score; } set { _score = value; } }
        #endregion

        /// <summary>
        /// Событие изменения энергии корабля для отображения в журнале
        /// </summary>
        public static event EnergyMessage _energyMessage;

        /// <summary>
        /// Событие уничтожения корабля
        /// </summary>
        public static event Message MessageDie;

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size) // описание конструктора в базовом классе
        {
            _energyMessage += Energy__journalMessage;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Метод для изменения энергии космического корабля
        /// </summary>
        public void GetEnergy(int n)
        {
            Energy__journalMessage(-n);
            _energy += n;
        }

        /// <summary>
        /// Метод для переноса корабля выше
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        /// <summary>
        /// Метод для переноса корабля ниже
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        /// <summary>
        /// Метод, который обозначает уничтожение корабля
        /// </summary>
        public void Die()
        {
            string temp = "Корабль был уничтожен";
            if (Program.streamWriter.BaseStream != null)
                Program.streamWriter?.WriteLine(temp);
            MessageDie?.Invoke();
        }
    }
    #endregion
}