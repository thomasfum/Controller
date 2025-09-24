using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.InOut
{
    public class Display
    {
        public bool[] A = new bool[16];
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        Bitmap image;
        int posX = 0;
        int posY = 0;

        public int proceed()
        {
            int val = 0;
            int adresse= Tools.boolArray2Int(A)- 0x4000;
            
            //0x20 pour chaque ligne
            if ((cl == true) && (st == true))
            {
                if (adresse == 0x1FFD)
                {
                    Clear();
                }
                else if (adresse == 0x1FFE)
                {
                    bool[] bposX = new bool[16];
                    bool[] bposY = new bool[16];
                    for (int i = 0; i < 8; i++)
                    {
                        bposX[i] = X[i];
                        bposY[i] = X[i+8];
                    }
                    posX = Tools.boolArray2Int(bposX);
                    posY = Tools.boolArray2Int(bposY);
                }
                else if (adresse == 0x1FFF)
                {
                    //SetText(0,0, '\u2588');
                    char chara = (char)Tools.boolArray2Int(X);
                    SetText(posX*8, posY * 16, chara);
                    //image.SetPixel(27 + 8, 27 + 16, Color.FromArgb(255, 155, 155));
                }
                else
                {
                    int line = adresse / 0x20;
                    int posInLine = adresse % 0x20;
                    for (int i = 0; i < 16; i++)
                    {
                        SetPixel(posInLine + i, line, X[i]);
                    }
                }
            }
            return val;
        }

        public void setImage(Bitmap _image)
        {
            image = _image;
        }
        public void Clear()
        {
            for (int x = 0; x < 380; x++)
                for (int y = 0; y < 190; y++)
                    SetPixel(x, y, false);
        }
        public void SetPixel(int x,int y, bool color)
        {
            Color c = Color.FromArgb(0, 51, 0);
            if (color)
                c = Color.FromArgb(155, 200, 155);

            //380x190 =>47 x 11 
            image.SetPixel(27+x, 27+y, c);
        }

        public void SetText(int x, int y, char ch)
        {
            Brush brush = new SolidBrush(Color.FromArgb(155, 200, 155));
            PointF location = new PointF(27+x, 27 + y);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                //using (Font font = new Font("Arial", 10))
                using (Font font = new Font("Courier New", 10))
                {
                    graphics.DrawString(""+ch, font, brush, location);
                }
            }

        }
    }
}
