using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace myproject
{
    public partial class Form1 : Form
    {
        Graphics formGraphics;
        Graphics formGraphics2;
        public Game game;
        public double currentTick = 800;
        Font font1 = new Font("Times New Roman", 28, FontStyle.Bold, GraphicsUnit.Pixel);
        public int bestScore = 0;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!game.gameOn)
            {
                timer.Stop();
                currentTick = 800;
                bestScore = Math.Max(bestScore, game.score);
            } 
            else
            {
                if (game.Tick(formGraphics, formGraphics2))
                {
                    currentTick *= 0.99;
                }
                timer.Interval = (int)(currentTick);
                labelPlayerScore.Text = "Best Score\n" + game.score.ToString();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    game.moveLeft(formGraphics);
                    break;
                case Keys.D:
                    game.moveRight(formGraphics);
                    break;
                case Keys.S:
                    game.moveDown(formGraphics);
                    break;
                case Keys.W:
                    game.moveUp(formGraphics);
                    break;
            }
        }

        public Form1()
        {
            InitializeComponent();
            timer.Stop();
            formGraphics = pictureBox1.CreateGraphics();
            formGraphics2 = pictureBox2.CreateGraphics();
            game = new Game(formGraphics2);
            game.sizeCellX = Math.Min((int)(tableLayoutPanel1.Width * 0.7) / game.field.sizeX,
                    (int)(tableLayoutPanel1.Height * 0.8) / game.field.sizeY) - 1;
            game.sizeCellY = game.sizeCellX;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(game.backColor);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formGraphics.Clear(game.backColor);
            game = new Game(formGraphics2);
            game.sizeCellX = Math.Min((int)(tableLayoutPanel1.Width * 0.7) / game.field.sizeX,
                    (int)(tableLayoutPanel1.Height * 0.8) / game.field.sizeY) - 1;
            game.sizeCellY = game.sizeCellX;
            game.gameOn = true;
            timer.Start();
            currentTick = 800;
            for (int i = 0; i < game.field.sizeX; ++i)
            {
                for (int j = 0; j < game.field.sizeY; ++j)
                {
                    formGraphics.FillRectangle(game.myBrush, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                    formGraphics.DrawRectangle(game.myPen, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                }
            }
            this.Select();
        }

        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.sizeCellX = Math.Min((int)(tableLayoutPanel1.Width * 0.7) / game.field.sizeX,
                    (int)(tableLayoutPanel1.Height * 0.8) / game.field.sizeY) - 1;
                game.sizeCellY = game.sizeCellX;
                pictureBox1.Width = game.sizeCellX * game.field.sizeX + 5;
                pictureBox1.Height = game.sizeCellY * game.field.sizeY + 5;
                formGraphics.Clear(game.backColor);
                for (int i = 0; i < game.field.sizeX; ++i)
                {
                    for (int j = 0; j < game.field.sizeY; ++j)
                    {
                        if (game.field.cells[j][i].blocked)
                        {
                            formGraphics.FillRectangle(game.field.cells[j][i].figure.color, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                            formGraphics.DrawRectangle(game.myPen, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                        }
                        else
                        {
                            formGraphics.FillRectangle(game.myBrush, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                            formGraphics.DrawRectangle(game.myPen, i * game.sizeCellX, j * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                        }
                    }
                }
                formGraphics2.Clear(game.backColor);
                int currentY = 1;
                foreach (Figure figureInQueue in game.queueOfFigure)
                {
                    foreach (Point p in figureInQueue.cells)
                    {
                        formGraphics2.FillRectangle(figureInQueue.color, (p.X - game.field.sizeX / 2 + 3) * game.sizeCellX, (currentY + p.Y) * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                        formGraphics2.DrawRectangle(game.myPen, (p.X - game.field.sizeX / 2 + 3) * game.sizeCellX, (currentY + p.Y) * game.sizeCellY, game.sizeCellX, game.sizeCellY);
                    }
                    if (figureInQueue.GetType() == typeof(FigureI))
                    {
                        currentY += 2;
                    }
                    else
                    {
                        currentY += 3;
                    }
                }
                if (game.haveFigurenow)
                {
                    game.drawFigure(formGraphics, game.getProjection());
                }
            }  
        }
    }
}
