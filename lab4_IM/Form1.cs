using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_IM
{
    public partial class Form1 : Form
    {
        private Graphics graph;
        private bool[,] field;
        private int resol;
        private int rows;
        private int columns;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nextGen();
        }
        private void nextGen()
        {
            graph.Clear(Color.White);
            var newField = new bool[columns, rows];

            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    var neighbours = cntNeighbours(x, y);
                    var hasLife = field[x, y];

                    if(!hasLife && neighbours == 3) {
                        newField[x, y] = true;
                    }
                    else if(hasLife && (neighbours < 2 || neighbours > 3)) {
                        newField[x, y] = false;
                    }
                    else {
                        newField[x, y] = hasLife;
                    }

                    if(hasLife)
                    {
                        graph.FillRectangle(Brushes.Black, x * resol, 
                            y * resol, resol - 1, resol - 1);
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
        }

        private void startGoL()
        {
            if(timer1.Enabled) {
                return;
            }
            edResolution.Enabled = false;
            edDensity.Enabled = false;
            resol = (int)edResolution.Value;
            rows = pictureBox1.Height / resol;
            columns = pictureBox1.Width / resol;
            field = new bool[columns, rows];
            Random random = new Random();
            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)edDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graph = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private int cntNeighbours(int x, int y)
        {
            int count = 0;
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    var col = (x + i + columns) % columns;
                    var row = (y + j + rows) % rows;
                    var self = col == x && row == y;
                    var hasLife = field[col, row];
                    if(hasLife && !self)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            startGoL();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            edResolution.Enabled = true;
            edDensity.Enabled = true;
        }
    }
}
