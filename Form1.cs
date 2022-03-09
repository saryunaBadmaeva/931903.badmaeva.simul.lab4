using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simul_lab_4
{
    public partial class Form1 : Form
    {
        const int cellNum1 = 20, cellNum2 = 20;
        int size1_pnl, size2_pnl;
        int size1_cell, size2_cell;

        Bitmap bm;
        Graphics graph;

        bool[,] CellularMartix;

        bool inProcess = false;


        public Form1()
        {
            InitializeComponent();
            Init();
            DrawCells();
            DrawGrid();
        }
        private void bt_Start_Click(object sender, EventArgs e)
        {
            if (inProcess)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
            inProcess = !inProcess;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            graph.Clear(Color.White);
            NextStep();
            DrawCells();
            DrawGrid();
            panel1.Invalidate();
        }
        private void Init()
        {
            size1_pnl = panel1.Width;
            size2_pnl = panel1.Height;

            size1_cell = size1_pnl / cellNum1;
            size2_cell = size2_pnl / cellNum2;

            bm = new Bitmap(size1_pnl, size2_pnl);
            graph = Graphics.FromImage(bm);

            CellularMartix = new bool[cellNum2, cellNum1];

            CellularMartix[2, 2] = true;
            CellularMartix[3, 3] = true;
            CellularMartix[3, 4] = true;
            CellularMartix[2, 4] = true;
            CellularMartix[1, 4] = true;

            CellularMartix[6, 3] = true;
            CellularMartix[7, 4] = true;
            CellularMartix[7, 5] = true;
            CellularMartix[6, 5] = true;
            CellularMartix[5, 5] = true;
        }

        private void DrawGrid()
        {
            Pen pen = new Pen(Color.Black);

            for (int i = 0; i < cellNum1 + 1; i++)
            {
                graph.DrawLine(pen, i * size1_cell, 0, i * size1_cell, size2_pnl);
            }

            for (int i = 0; i < cellNum2 + 1; i++)
            {
                graph.DrawLine(pen, 0, i * size2_cell, size1_pnl, i * size2_cell);
            }
        }

        private void NextStep()
        {
            bool[,] NextMatrix = new bool[cellNum2, cellNum1];
            for (int i = 0; i < cellNum2; i++)
            {
                for (int j = 0; j < cellNum1; j++)
                {
                    if (CellularMartix[i, j])
                    {
                        int alive = AroundAliveAmount(i, j);
                        NextMatrix[i, j] = (alive == 2) || (alive == 3);
                    }
                    else
                    {
                        NextMatrix[i, j] = AroundAliveAmount(i, j) == 3;
                    }
                }
            }
            CellularMartix = NextMatrix;
        }

        private int AroundAliveAmount(int i, int j)
        {
            int aliveNeighbours = 0;
            for (int x = -1; x <= 1; x++)
            {
                int l = i + x;

                if ((l >= 0) && (l < cellNum2))
                    for (int y = -1; y <= 1; y++)
                    {
                        int r = j + y;

                        if ((r >= 0) && (r < cellNum1))
                            aliveNeighbours += CellularMartix[l, r] ? 1 : 0;
                    }
            }

            aliveNeighbours -= CellularMartix[i, j] ? 1 : 0;

            return aliveNeighbours;
        }

        private void DrawCells()
        {
            SolidBrush solidBrush = new SolidBrush(Color.OliveDrab);

            for (int i = 0; i < cellNum2; i++)
            {
                for (int j = 0; j < cellNum1; j++)
                {
                    if (CellularMartix[i, j])
                    {
                        graph.FillRectangle(solidBrush, j * size1_cell, i * size2_cell, size1_cell, size2_cell);
                    }
                }
            }
        }
    }
}
