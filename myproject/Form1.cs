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
using System.IO;

namespace myproject
{
    public partial class Form1 : Form
    {
        Graphics formGraphics;
        Graphics formGraphics2;
        public Game game;
        public double currentTick = 800;
        Font font1 = new Font("Times New Roman", 28, FontStyle.Bold, GraphicsUnit.Pixel);
        public string filePath = "Record.txt";
        public StreamWriter writer;
        public StreamReader reader;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!game.gameOn)
            {
                timer.Stop();
                currentTick = 800;
                Bitmap resizedImg = new Bitmap(Properties.Resources.tyan, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = resizedImg;
                UpdateRecord();
            } 
            else
            {
                if (game.Tick(formGraphics, formGraphics2))
                {
                    currentTick *= 0.99;
                }
                UpdateRecord();
                timer.Interval = (int)(currentTick);
                labelPlayerScore.Text = "Points\n" + game.score.ToString();
                labelPlayerRecord.Text = "Record\n" + GetRecord().ToString();
            }
        }

        private void UpdateRecord()
        {
            int rec = GetRecord();
            writer = new StreamWriter(filePath);
            writer.Write(Math.Max(rec, game.score));
            writer.Close();
        }

        private int GetRecord()
        {
            reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            int rec;
            int.TryParse(line, out rec);
            reader.Close();
            return rec;
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
                case Keys.P:
                    if (timer.Enabled)
                    {
                        timer.Stop();
                        Bitmap resizedImg = new Bitmap(Properties.Resources.tyan2, pictureBox1.Width, pictureBox1.Height);
                        pictureBox1.Image = resizedImg;
                    }
                    else
                    {
                        formGraphics.Clear(game.backColor);
                        timer.Start();
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
                    }
                    break;
                case Keys.Escape:
                    this.Close();
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
            labelPlayerRecord.Text = "Record\n" + GetRecord().ToString();
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
            UpdateRecord();
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
