using Microsoft.VisualBasic.Devices;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace 贪吃蛇3
{
    public enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }


    public partial class Form1 : Form
    {
        int _blockWith = 10;
        int _blockHeight = 10;

        static int x;
        static int y;
        Snake _snake = new Snake();
        Food _food;
        Keyboard keyb = new Keyboard();
        public int X { get { return x = this.panel1.Width / _blockWith; } }
        public int Y { get { return y = this.panel1.Height / _blockHeight; } }
        Label[,] _labels;
        public Form1()
        {
            InitializeComponent();
            this.timer1.Stop();
            DrawingMap();
            Adjust();
            this._food = new Food(this._snake, this);
            this._labels[_food.Position[0], _food.Position[1]].BackColor = Color.Black;
            //_snake.direction = Direction.UP;
            KeyDown += this.KeyDownAction;
            this.timer1.Interval = Convert.ToInt32(this.textBox1.Text);
        }



        public void KeyDownAction(object? sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.W || e.KeyCode == Keys.D || e.KeyCode == Keys.A || e.KeyCode == Keys.S)
            {

                switch (e.KeyCode)
                {
                    case Keys.Space:
                        this.timer1.Stop(); break;
                    case Keys.W:
                        if (this._snake.direction == Direction.DOWN) break;
                        this._snake.direction = Direction.UP; break;
                    case Keys.S:
                        if (this._snake.direction == Direction.UP) break; this._snake.direction = Direction.DOWN; break;
                    case Keys.A: if (this._snake.direction == Direction.RIGHT) break; this._snake.direction = Direction.LEFT; break;
                    case Keys.D: if (this._snake.direction == Direction.LEFT) break; this._snake.direction = Direction.RIGHT; break;
                }
            }


        }

        public void DrawingMap()
        {
            var labels = new Label[this.X, this.Y];
            for (int i = 0; i < this.X; i++)
            {
                for (int j = 0; j < this.Y; j++)
                {
                    var labelCommon = new Label();
                    //labelCommon.Text=i.ToString()+","+j.ToString();
                    labelCommon.Size = new Size(this._blockWith, this._blockHeight);
                    labelCommon.Location = new Point(i * this._blockWith, j * this._blockHeight);
                    labelCommon.BackColor = Color.White;
                    this.panel1.Controls.Add(labelCommon);
                    labels[i, j] = labelCommon;
                }
            }
            this._labels = labels;
        }

        public void Adjust()
        {
            foreach (var item in _snake.Position)
            {
                this._labels[item[0], item[1]].BackColor = Color.Green;
            }
        }

        public void Adjustf()
        {
            bool flag = false;
            int x;
            int y;
            Random rand = new Random();
            do
            {
                x = rand.Next(0, X);
                y = rand.Next(0, Y);
                foreach (var item in this._snake.Position)
                {
                    if (x == item[0] && y == item[1])
                    {
                        flag = true;
                    }
                }
            } while (flag);
            _food.Position = new int[2] { x, y };
            this.textBox2.Text = (Convert.ToInt32(this.textBox2.Text) + 10).ToString();

            this._labels[_food.Position[0], _food.Position[1]].BackColor = Color.Black;
            int[] NewBody = AddBody();
            this._snake.Position.Add(NewBody);
            Adjust();

        }

        public void BackToCommon(int[] ints)
        {
            this._labels[ints[0], ints[1]].BackColor = Color.White;
        }

        public int[] AddBody()
        {
            int[] a;
            int[] tailerLast = this._snake.Position[_snake.Position.Count - 1];
            int[] tailerSecond = this._snake.Position[_snake.Position.Count - 2];
            if (tailerLast[0] == tailerSecond[0])
            {
                if (tailerLast[1] - tailerSecond[1] == 1)
                {
                    a = new int[2] { tailerLast[0], (tailerLast[1] + 1) % Y };
                }
                else
                {
                    a = new int[2] { tailerLast[0], (tailerLast[1] - 1 + Y) % Y };
                }
            }
            else
            {
                if (tailerLast[0] - tailerSecond[0] == 1)
                {
                    a = new int[2] { (tailerLast[0] + 1) % X, tailerLast[1] };
                }
                else
                {
                    a = new int[2] { (tailerLast[0] - 1 + X) % X, tailerLast[1] };
                }
            }
            return a;
        }
        public void button1_Click(object sender, EventArgs e)
        {

            if (this.button1.Text == "BEGIN")
            {
                this.timer1.Start();
                this.button1.Text = "STOP";
            }
            else
            if (this.button1.Text == "STOP")
            {
                this.timer1.Stop();
                this.button1.Text = "BEGIN";
            }
            if (SnakeHead())
            {
                Adjustf();
            }
        }

        public bool SnakeHead()
        {
            bool a = false;
            if (this._snake.Position[0][0] == _food.Position[0] && this._snake.Position[0][1] == this._food.Position[1])
            {
                a = true;
            }

            return a;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {


            //this._labels[1, 2].BackColor = Color.Green;


            this._snake.Move(this.Adjust, this.SnakeHead, Adjustf, this.AddBody, this.BackToCommon, this.X, this.Y);
            IsBite();

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            //this._food = new Food(this._snake, this);
            KeyDown += this.KeyDownAction;

            this.KeyPreview = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(textBox1.Text, out int value))
            {
                this.timer1.Interval = value;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.Focus();
                this.ActiveControl = null;
            }
        }

        public void IsBite()
        {
            for(int i = 1; i < this._snake.Position.Count; i++)
            {
                if (this._snake.Position[0][0] == this._snake.Position[i][0] && this._snake.Position[0][1] == this._snake.Position[i][1])
                {
                    this.timer1.Stop();
                    if (MessageBox.Show("好似，开香槟咯") == DialogResult.OK)
                    {
                        MessageBox.Show("你的得分是{0}", this.textBox2.Text);
                    }

            
                    this.timer1.Stop();
                    this.button1.Text = "BEGIN";
                    foreach(var item in this._labels)
                    {
                        item.BackColor = Color.White;
                    }
                    this._snake = new Snake();
                    Adjust();
                    this._food = new Food(this._snake, this);
                    this._labels[_food.Position[0], _food.Position[1]].BackColor = Color.Black;
                    this.textBox2.Text = "0";
                    //_snake.direction = Direction.UP;
                    //KeyDown += this.KeyDownAction;
                    //this.timer1.Interval = Convert.ToInt32(this.textBox1.Text);
                }
            }
         
            

        }

        
    }

    
    public class Snake
    {
        private List<int[]> _position = new List<int[]>();
        private Direction _direction = Direction.LEFT;
        public List<int[]> Position { get { return this._position; } }
        public Direction direction { get { return this._direction; } set { _direction = value; } }
        public Snake() { _position.Add(new int[2] { 10, 10 }); _position.Add(new int[2] { 11, 10 }); }


        public void Move(Action action, Func<bool> func, Action Adjustf, Func<int[]> AddBody, Action<int[]> action1, int width, int height)
        {
            int[] temp;
            temp = this.Position[0];
            var tempr = this.Position[Position.Count - 1];
            switch (this.direction)
            {
                case Direction.LEFT:

                    this.Position.Remove(tempr);

                    this.Position.Insert(0, (new int[2] { (temp[0] - 1 + width) % width, temp[1] })); break;
                case Direction.RIGHT:

                    this.Position.Remove(tempr);
                    this.Position.Insert(0, (new int[2] { (temp[0] + 1) % width, temp[1] })); break;
                case Direction.DOWN:
                    this.Position.Remove(tempr);
                    this.Position.Insert(0, (new int[2] { temp[0], (temp[1] + 1) % height })); break;
                case Direction.UP:
                    this.Position.Remove(tempr);
                    this.Position.Insert(0, (new int[2] { temp[0], (temp[1] - 1 + height) % height })); break;
            }
            if (func.Invoke())
            {
                Adjustf();
                this.Position.Add(AddBody());
            }
            action1.Invoke(tempr);
            action.Invoke();
        }
    }
}