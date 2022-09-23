using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SymbolsMapRenderer
{
    static class Program
    {
        static T ReadArgument<T>(string argumentName)
        {
            Console.WriteLine($"Input {argumentName} : {typeof(T).Name}");
            return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
        }
        static void Main()
        {
            int width = ReadArgument<int>("map width"); 
            int height = ReadArgument<int>("map height");
            string encodingName = ReadArgument<string>("ASCII encoding name");
            string fontName = ReadArgument<string>("font name");
            int fontSize = ReadArgument<int>("font size");
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.PageUnit = GraphicsUnit.Pixel;

            byte[] symbol = new byte[1];
            var encoding = System.Text.Encoding.GetEncoding(encodingName);

            if (fontName.Equals("all"))
            {
                foreach (var family in FontFamily.Families)
                {
                    fontName = family.Name;
                    CreateMap();
                }
            }
            else
            {
                CreateMap();
            }

            void CreateMap()
            {
                graphics.Clear(Color.FromArgb(0));
                float eW = width / 16f;
                float eH = height / 16f;
                for (int i = 0; i < 256; i++)
                {
                    int x = i % 16;
                    int y = i / 16;
                    symbol[0] = (byte)i;
                    string text = encoding.GetString(symbol);
                    Font font = new Font(fontName, fontSize);
                    RectangleF rect = new RectangleF(eW * x, eH * y, eW, eH);
                    SizeF textSize = graphics.MeasureString(text, font);
                    graphics.Transform = new System.Drawing.Drawing2D.Matrix(rect.Width / textSize.Width, 0f, 0f, rect.Height / textSize.Height, rect.X, rect.Y);
                    graphics.DrawString(text, font, Brushes.White, new PointF());
                }
                if (!System.IO.Directory.Exists("./Maps"))
                {
                    System.IO.Directory.CreateDirectory("./Maps");
                }
                bitmap.Save($"./Maps/SymbolsMap_{encodingName}_{fontName}.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
