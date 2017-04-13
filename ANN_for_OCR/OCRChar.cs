using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANN_for_OCR
{
    public class OCRChar
    {
        public static char[] CharMap = new char[10]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'
        };
        
        public static byte[][] DrawMap = new byte[][]
        {
            new byte[] {1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1},
            new byte[] {1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0},
            new byte[] {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            new byte[] {1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 0, 0},
            new byte[] {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            new byte[] {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            new byte[] {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            new byte[] {1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1},
            new byte[] {0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0},
            new byte[] {0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0}
        };

        public static byte[][] BitMap = new byte[][]
        {
            new byte[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new byte[] {0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
            new byte[] {0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
            new byte[] {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            new byte[] {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
            new byte[] {0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            new byte[] {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
            new byte[] {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
            new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
            new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
        };

        private int index;

        public OCRChar(char _char)
        {
            for (int i = 0; i < OCRChar.CharMap.Length; i++)
            {
                if (_char.Equals(OCRChar.CharMap[i]))
                {
                    this.index = i;
                    return;
                }
            }

            throw new Exception("Char not found");
        }

        public OCRChar(double[] _vdouble)
        {
            byte[] vet = new byte[_vdouble.Length];
            for (int i = 0; i < vet.Length; i++)
            {
                vet[i] = (byte)_vdouble[i];
            }

            this.Construct(vet);
        }

        public OCRChar(byte[] _vbyte)
        {
            this.Construct(_vbyte);
        }

        private void Construct(byte[] _vbyte)
        {
            if (_vbyte.Length == 30)
            {
                for (int i = 0; i < OCRChar.DrawMap.Length; i++)
                {
                    for (int j = 0; j < OCRChar.DrawMap[i].Length; j++)
                    {
                        if (_vbyte[j] != OCRChar.DrawMap[i][j])
                        {
                            break;
                        }

                        this.index = i;
                        return;
                    }
                }
            }
            else if (_vbyte.Length == 10)
            {
                int onec = 0;
                bool invalidFormat = false;
                for (int i = 0; i < _vbyte.Length; i++)
                {
                    if (_vbyte[i] > 1 || _vbyte[i] < 0)
                    {
                        invalidFormat = true;
                        break;
                    }

                    if (_vbyte[i] == 1)
                    {
                        onec++;
                        this.index = i;
                    }
                }

                if (onec == 1 && invalidFormat == false)
                {
                    return;
                }
            }

            throw new Exception("Byte Vector not found");
        }

        public byte[] toDraw()
        {
            return OCRChar.DrawMap[this.index];
        }

        public char toChar()
        {
            return OCRChar.CharMap[this.index];
        }

        public byte[] toBitMap()
        {
            return OCRChar.BitMap[this.index];
        }

        public double[] toDrawDoubleMap()
        {
            double[] map = new double[OCRChar.DrawMap[this.index].Length];
            for (int i = 0; i < OCRChar.DrawMap[this.index].Length; i++)
            {
                map[i] = (double)OCRChar.DrawMap[this.index][i];   
            }

            return map;
        }
    }
}
