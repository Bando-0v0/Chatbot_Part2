using System;
using System.Drawing;
using System.IO;
using System.Media;


namespace chatbot
{
    /*
        #######################################################################################################
       this code block is too draw the ASCII art work. the inner loop is designed soo the each character will print
       in one line and the outer loop prints next line. the resulting pattenr of characters. prints the logo
       #######################################################################################################
        */
    public class Logo
    {
        //constructor

        public Logo()
        {
           
            try
            {
                // get the full path
                string path_project = AppDomain.CurrentDomain.BaseDirectory;


                // then replace the bin\\debug\\
                string new_path_project = path_project.Replace("bin\\debub\\", "");


                /*
                 then combine the project full path and the image name in the format
                 */
                // string full_path = Path.Combine(new_path_project, "images.png");

                //then start working on loop
                // with the Ascii
                Bitmap image = new Bitmap("images2.jpg");
                image = new Bitmap(image, new Size(50, 20));

                //loop for, inner, and the outer 
                //nested 
                for (int height = 0; height < image.Height; height++)
                {
                    //then now work on the width
                    for (int width = 0; width < image.Width; width++)
                    {
                        //now lets work on the acsii design
                        Color pixelColor = image.GetPixel(width, height);
                        int color = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                        Console.ForegroundColor = ConsoleColor.Blue;

                        //now make use of the char
                        char ascii_design = color > 200 ? '.' : color > 150 ? '*' : color > 100 ? '0' :
                            color > 50 ? '#' : '@';
                        Console.Write(ascii_design);


                    }// end of the for inner
                    Console.WriteLine();
                }// end of loop for outer
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
                Console.ResetColor();
                // Optional: log to a file
                File.AppendAllText("error_log.txt", $"{DateTime.Now} - {ex.Message}\n");
            }
        }
    }
}