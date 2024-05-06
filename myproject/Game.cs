using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace myproject
{
    public class Game
    {
        public Field field;
        public bool haveFigurenow = false;
        public Figure figureNow, projectionFigureNow;
        public List<Figure> queueOfFigure;
        public int sizeCellX = 30, sizeCellY = 30;
        public SolidBrush myBrush;
        public Color backColor = Color.FromArgb(64, 64, 64);
        public Pen myPen = new Pen(Color.White);
        public int minEqualityBorder = 50;
        public int countNextFigures = 3;
        public int cntDeleteRows = 0;
        public int score = 0;
        public bool gameOn = false;
        Random random;
        public Game(Graphics formGraphics2) {
            field = new Field();
            queueOfFigure = new List<Figure>();
            random = new Random();
            myBrush = new SolidBrush(backColor);
            for (int i = 0; i < countNextFigures; i++)
            {
                createRandomFigure(formGraphics2);
            }
        }
        public bool Tick(Graphics formGraphics, Graphics formGraphics2)
        {
            if (haveFigurenow)
            {
                if (!moveDown(formGraphics))
                {
                    checkAndDeleteField(formGraphics);
                    haveFigurenow = false;
                    figureNow = null;
                }

                return false;
            } 
            else
            {
                if (checkAndDeleteField(formGraphics))
                {
                    return false;
                }
                cntDeleteRows = 0;
                haveFigurenow = true;
                createRandomFigure(formGraphics2);
                bool flagLose = false;
                foreach (Point p in figureNow.cells)
                {
                    if (field.cells[p.Y][p.X].blocked)
                    {
                        flagLose = true;
                    }
                }
                if (flagLose)
                {
                    finishGame();
                } 
                else
                {
                    foreach (Point p in figureNow.cells)
                    {
                        field.cells[p.Y][p.X].blocked = true;
                        field.cells[p.Y][p.X].figure = figureNow;
                    }
                    drawFigure(formGraphics, figureNow);
                    projectionFigureNow = getProjection();
                    drawFigure(formGraphics, projectionFigureNow);
                }
                return true;
            }
        }
        public void moveLeft(Graphics formGraphics) 
        {
            if (!haveFigurenow || !gameOn)
            {
                return;
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = false;
                field.cells[p.Y][p.X].figure = null;
            }
            bool havePlace = true;
            foreach (Point c in figureNow.cells)
            {
                if (c.X == 0 || field.cells[c.Y][c.X - 1].blocked)
                {
                    havePlace = false;
                }
            }
            if (havePlace)
            {
                eraseFigure(formGraphics, figureNow);
                eraseFigure(formGraphics, projectionFigureNow);

                figureNow.moveLeft();

                drawFigure(formGraphics, figureNow);
                projectionFigureNow = getProjection();
                drawFigure(formGraphics, projectionFigureNow);
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = true;
                field.cells[p.Y][p.X].figure = figureNow;
            }
        }
        public void moveRight(Graphics formGraphics)
        {
            if (!haveFigurenow || !gameOn)
            {
                return;
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = false;
                field.cells[p.Y][p.X].figure = null;
            }
            bool havePlace = true;
            foreach (Point c in figureNow.cells)
            {
                if (c.X == field.sizeX - 1 || field.cells[c.Y][c.X + 1].blocked)
                {
                    havePlace = false;
                }
            }
            if (havePlace)
            {
                eraseFigure(formGraphics, figureNow);
                eraseFigure(formGraphics, projectionFigureNow);

                figureNow.moveRight();

                drawFigure(formGraphics, figureNow);
                projectionFigureNow = getProjection();
                drawFigure(formGraphics, projectionFigureNow);
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = true;
                field.cells[p.Y][p.X].figure = figureNow;
            }
        }
        public bool moveDown(Graphics formGraphics)
        {
            if (!haveFigurenow || !gameOn)
            {
                return false;
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = false;
                field.cells[p.Y][p.X].figure = null;
            }
            bool havePlace = true;
            foreach (Point c in figureNow.cells)
            {
                if (c.Y == field.sizeY - 1 || field.cells[c.Y + 1][c.X].blocked)
                {
                    havePlace = false;
                }
            }
            if (havePlace)
            {
                eraseFigure(formGraphics, figureNow);
                figureNow.moveDown();
                drawFigure(formGraphics, figureNow);
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = true;
                field.cells[p.Y][p.X].figure = figureNow;
            }
            return havePlace;
        }
        public void moveUp(Graphics formGraphics)
        {
            if (!haveFigurenow || !gameOn)
            {
                return;
            }
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = false;
                field.cells[p.Y][p.X].figure = null;
            }
            eraseFigure(formGraphics, figureNow);
            eraseFigure(formGraphics, projectionFigureNow);
            figureNow.rotateFigure();
            bool havePlace = true;
            foreach (Point c in figureNow.cells)
            {
                if (c.X < 0 || c.X >= field.sizeX || c.Y < 0 || c.Y >= field.sizeY || field.cells[c.Y][c.X].blocked)
                {
                    havePlace = false;
                }
            }
            if (!havePlace)
            {
                figureNow.rotateFigure();
                figureNow.rotateFigure();
                figureNow.rotateFigure();
            } 
            foreach (Point p in figureNow.cells)
            {
                field.cells[p.Y][p.X].blocked = true;
                field.cells[p.Y][p.X].figure = figureNow;
            }
            drawFigure(formGraphics, figureNow);
            projectionFigureNow = getProjection();
            drawFigure(formGraphics, projectionFigureNow);
        }
        public void paintClear(Graphics formGraphics, Point p)
        {
            formGraphics.FillRectangle(myBrush, p.X * sizeCellX, p.Y * sizeCellY, sizeCellX, sizeCellY);
            formGraphics.DrawRectangle(myPen, p.X * sizeCellX, p.Y * sizeCellY, sizeCellX, sizeCellY);
        }
        public void paintFill(Graphics formGraphics, Point p, SolidBrush color)
        {
            formGraphics.FillRectangle(color, p.X * sizeCellX, p.Y * sizeCellY, sizeCellX, sizeCellY);
            formGraphics.DrawRectangle(myPen, p.X * sizeCellX, p.Y * sizeCellY, sizeCellX, sizeCellY);
        }
        public bool checkAndDeleteField(Graphics formGraphics)
        {
            for (int i = field.sizeY - 1; i > 0; --i)
            {
                bool fullFilled = true;
                for (int j = 0; j < field.sizeX; ++j)
                {
                    if (field.cells[i][j].blocked == false)
                    {
                        fullFilled = false;
                        break;
                    }
                }
                if (fullFilled)
                {
                    deleteRow(i, formGraphics);
                    return true;
                }
            }
            return false;
        }
        public void deleteRow(int row, Graphics formGraphics)
        {
            cntDeleteRows += 1;
            switch (cntDeleteRows)
            {
                case 1:
                    score += 100;
                    break;
                case 2:
                    score += 300 - 100;
                    break;
                case 3:
                    score += 700 - 300 - 100;
                    break;
                case 4:
                    score += 1500 - 700 - 300 - 100;
                    break;
            }
            for (int j = 0; j < field.sizeX; ++j)
            {
                field.cells[row][j].blocked = false;
                field.cells[row][j].figure = null;
                paintClear(formGraphics, new Point(j, row));
            }
            for (int i = row; i > 0; --i)
            {
                for (int j = 0; j < field.sizeX; ++j)
                {
                    if (field.cells[i - 1][j].blocked)
                    {
                        (field.cells[i][j], field.cells[i - 1][j]) = (field.cells[i - 1][j], field.cells[i][j]);
                        paintClear(formGraphics, new Point(j, i - 1));
                        paintFill(formGraphics, new Point(j, i), field.cells[i][j].figure.color);
                    }
                }
            }
        }
        public void createRandomFigure(Graphics formGraphics2)
        {
            Figure nextFigure = new Figure();
            do
            {
                int randNumber = random.Next(7);
                switch (randNumber)
                {
                    case 0:
                        nextFigure = new FigureI(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 1:
                        nextFigure = new FigureL(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 2:
                        nextFigure = new FigureO(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 3:
                        nextFigure = new FigureJ(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 4:
                        nextFigure = new FigureS(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 5:
                        nextFigure = new FigureT(new Point(field.sizeX / 2 - 2, 0));
                        break;
                    case 6:
                        nextFigure = new FigureZ(new Point(field.sizeX / 2 - 2, 0));
                        break;
                }
            }
            while (!checkNewFigure(nextFigure));
            queueOfFigure.Add(nextFigure);
            if (queueOfFigure.Count == countNextFigures + 1)
            {
                figureNow = queueOfFigure[0];
                queueOfFigure.RemoveAt(0);
                formGraphics2.Clear(backColor);
                int currentY = 1;
                foreach (Figure figureInQueue in queueOfFigure)
                {
                    foreach (Point p in figureInQueue.cells)
                    {
                        formGraphics2.FillRectangle(figureInQueue.color, (p.X - field.sizeX / 2 + 3) * sizeCellX, (currentY + p.Y) * sizeCellY, sizeCellX, sizeCellY);
                        formGraphics2.DrawRectangle(myPen, (p.X - field.sizeX / 2 + 3) * sizeCellX, (currentY + p.Y) * sizeCellY, sizeCellX, sizeCellY);
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
            }
        }

        public float getSquate(float x)
        {
            return x * x;
        }
        public bool checkNewFigure(Figure nextFigure)
        {
            if (isColorsTheSame(Color.FromArgb(255, 255, 255), Color.FromArgb(nextFigure.r, nextFigure.g, nextFigure.b)))
            {
                return false;
            }
            if (isColorsTheSame(Color.FromArgb(128, 128, 128), Color.FromArgb(nextFigure.r, nextFigure.g, nextFigure.b)))
            {
                return false;
            }
            for (int i = 0; i < queueOfFigure.Count; ++i)
            {
                
                if (queueOfFigure[i].GetType() == nextFigure.GetType())
                {
                    return false;
                }
                if (isColorsTheSame(Color.FromArgb(queueOfFigure[i].r, queueOfFigure[i].g, queueOfFigure[i].b), Color.FromArgb(nextFigure.r, nextFigure.g, nextFigure.b)))
                {
                    return false;
                }
            }
            return true;
        }
        public bool isColorsTheSame(Color color1, Color color2)
        {
            List<float> list1 = RGBToLab(new List<int> { color1.R, color1.G, color1.B, color1.A});
            List<float> list2 = RGBToLab(new List<int> { color2.R, color2.G, color2.B, color2.A});
            float difr = getSquate(list1[0] - list2[0]);
            float difg = getSquate(list1[1] - list2[1]);
            float difb = getSquate(list1[2] - list2[2]);
            return Math.Sqrt(difr + difg + difb) < minEqualityBorder;
        }
        public static List<float> RGBToLab(List<int> color)
        {
            float[] xyz = new float[3];
            float[] lab = new float[3];
            float[] rgb = new float[] { color[0], color[1], color[2], color[3] };

            rgb[0] = color[0] / 255.0f;
            rgb[1] = color[1] / 255.0f;
            rgb[2] = color[2] / 255.0f;

            if (rgb[0] > .04045f)
            {
                rgb[0] = (float)Math.Pow((rgb[0] + .0055) / 1.055, 2.4);
            }
            else
            {
                rgb[0] = rgb[0] / 12.92f;
            }

            if (rgb[1] > .04045f)
            {
                rgb[1] = (float)Math.Pow((rgb[1] + .0055) / 1.055, 2.4);
            }
            else
            {
                rgb[1] = rgb[1] / 12.92f;
            }

            if (rgb[2] > .04045f)
            {
                rgb[2] = (float)Math.Pow((rgb[2] + .0055) / 1.055, 2.4);
            }
            else
            {
                rgb[2] = rgb[2] / 12.92f;
            }
            rgb[0] = rgb[0] * 100.0f;
            rgb[1] = rgb[1] * 100.0f;
            rgb[2] = rgb[2] * 100.0f;


            xyz[0] = ((rgb[0] * .412453f) + (rgb[1] * .357580f) + (rgb[2] * .180423f));
            xyz[1] = ((rgb[0] * .212671f) + (rgb[1] * .715160f) + (rgb[2] * .072169f));
            xyz[2] = ((rgb[0] * .019334f) + (rgb[1] * .119193f) + (rgb[2] * .950227f));


            xyz[0] = xyz[0] / 95.047f;
            xyz[1] = xyz[1] / 100.0f;
            xyz[2] = xyz[2] / 108.883f;

            if (xyz[0] > .008856f)
            {
                xyz[0] = (float)Math.Pow(xyz[0], (1.0 / 3.0));
            }
            else
            {
                xyz[0] = (xyz[0] * 7.787f) + (16.0f / 116.0f);
            }

            if (xyz[1] > .008856f)
            {
                xyz[1] = (float)Math.Pow(xyz[1], 1.0 / 3.0);
            }
            else
            {
                xyz[1] = (xyz[1] * 7.787f) + (16.0f / 116.0f);
            }

            if (xyz[2] > .008856f)
            {
                xyz[2] = (float)Math.Pow(xyz[2], 1.0 / 3.0);
            }
            else
            {
                xyz[2] = (xyz[2] * 7.787f) + (16.0f / 116.0f);
            }

            lab[0] = (116.0f * xyz[1]) - 16.0f;
            lab[1] = 500.0f * (xyz[0] - xyz[1]);
            lab[2] = 200.0f * (xyz[1] - xyz[2]);

            return new List<float> { lab[0], lab[1], lab[2], color[3] };
        }

        public void finishGame()
        {
            gameOn = false;
        }
        public void drawFigure(Graphics formGraphics, Figure f)
        {
            foreach (Point p in f.cells)
            {
                paintFill(formGraphics, p, f.color);
            }
        }
        public void eraseFigure(Graphics formGraphics, Figure f)
        {
            foreach (Point p in f.cells)
            {
                paintClear(formGraphics, p);
            }
        }
        public Figure getProjection()
        {
            Figure projection = new Figure();
            projection.cells = new List<Point>();
            projection.color = new SolidBrush(Color.FromArgb(100, figureNow.r, figureNow.g, figureNow.b));
            for (int i = 0; i < figureNow.cells.Count; ++i)
            {
                int x = figureNow.cells[i].X;
                int y = figureNow.cells[i].Y;
                projection.cells.Add(new Point(x, y));
            }
            while (true)
            {
                bool havePlace = true;
                foreach (Point c in projection.cells)
                {
                    if (c.Y == field.sizeY - 1 || (field.cells[c.Y + 1][c.X].blocked && field.cells[c.Y + 1][c.X].figure != figureNow))
                    {
                        havePlace = false;
                    }
                }
                if (havePlace)
                {
                    projection.moveDown();
                } 
                else
                {
                    break;
                }
            }
            return projection;
        }
    }
}