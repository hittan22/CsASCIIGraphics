using System;
using System.Drawing;
using System.IO;

namespace ConsoleApp3
{
    class Program
    {
        static public string imageToASCII(Image thing)
        {
            Bitmap img_bit = new Bitmap(thing);
            char[] ASCIIChars = { ' ', ',', ':', '+', '*', '?', '%', '#', '@' }; //replacing pixels with these characters
            string result = "";
            for (int j = 0; j < thing.Height; j++)
            {
                for (int i = 0; i < thing.Width; i++)
                {
                    var pixel = img_bit.GetPixel(i, j);
                    if (pixel.A == 0) // RGBA, A is alpha, so "if pixel is absolute transparent" and will be replaced with whitespace
                    {

                        result += ASCIIChars[0];
                    }
                    else
                    {
                        var avg = (pixel.R + pixel.G + pixel.B) / 3; // average value mainly means brightness of a pixel in grayscale
                        result += ASCIIChars[(int)Map(avg, 0, 255, 0, ASCIIChars.Length - 1)];
                    }
                }
            }
            return result;
        }
        static public float Map(float value, float start1, float stop1, float start2, float stop2)
        {

            return ((value-start1)/(stop1-start1))*(stop2-start2)+start2; // to convert number from one range to another
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Input amount of frames: ");
            int length = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input path to directory with frames (e.g. \"C:\\\\dir1\\\\frames\\\\\", frames in png): "); // project contains start example of video
            string path = Console.ReadLine();
            string[] video = new string[length];
            for (int i = 1; i < length+1; i++)
            { 
                video[i-1] = imageToASCII(Image.FromStream(File.OpenRead($"{path}{i}.png"))); // array of strings (one string is one frame)
            }
            while (true)
            {
                for (int i = 0; i < length; i++)
                {
                    Console.Write(video[i]);
                    System.Threading.Thread.Sleep(32); // should be adjusted by framerate of your video (1000/framerate)
                }
            }
        }
    }
}
