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
        public int proceed()
        {
            int val = 0;
            int adresse= Tools.boolArray2Int(A)- 0x4000;
            
            //0x20 pour chaque ligne
            if ((cl == true) && (st == true))
            {
                int line = adresse / 0x20;
                int posInLine= adresse % 0x20;
                for(int i=0;i<16;i++) 
                {
                    SetPixel(posInLine + i, line, X[15-i]);
                }
            }
            return val;
        }

        public void setImage(Bitmap _image)
        {
            image = _image;
        }
        public void SetPixel(int x,int y, bool color)
        {
            Color c = Color.FromArgb(0, 51, 0);
            if (color)
                c = Color.FromArgb(155, 200, 155);

            //380x190
            image.SetPixel(27+x, 27+y, c);
        }
    }
}
