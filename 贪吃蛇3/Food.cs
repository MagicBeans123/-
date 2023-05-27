using System.Security.Cryptography.X509Certificates;

namespace 贪吃蛇3
{
    public class Food
    {
        private Snake _snake;
        private Form1 _form1;
        public int[] Position { get; set; }
        public Snake snake { get { return _snake; } set { _snake = value; } }
        public Food(Snake snake,Form1 form)
        {
            this._snake = snake;
            this._form1 = form;
            bool flag = false;
            int x;
             int y;
            Random rand=new Random();
            do
            {
                x = rand.Next(0, this._form1.X );
                y=rand.Next(0,this._form1.Y );
                foreach (var item in snake.Position)
                {
                    if (x == item[0] && y== item[1])
                    {
                        flag = true; 
                    }
                }
            }while(flag);
            this.Position = new int[2] { x, y };
        }
    }
}