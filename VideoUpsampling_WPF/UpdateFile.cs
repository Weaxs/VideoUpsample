using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpsampling_WPF
{
    class UpdateFile
    {

        String others =  "LoadPlugin(\"FFT3dGPU.dll\") \n" 
            + "FFT3dGPU(mode= 1, precision= 2, sigma= 3)\n"
            + "LoadPlugin(\"flash3kyuu_deband.dll\")\n"
            + "flash3kyuu_deband(Y= 70, Cb= 40, Cr= 40, grainY= 0, grainC= 0)\n"
            + "Import(\"LimitedSharpenFaster.avsi\")\n" 
            + "LoadPlugin(\"RgTools.dll\")\n" 
            + "LoadPlugin(\"masktools2.dll\")\n"
            + "LimitedSharpenFaster()\n";

        /**
         * 修改AVS脚本文件中，源地址的路径（nnedi3算法
         * 
         * */
        public String UpdateAvsFile_nnedi(String originalPath,int px)
        {
            String result;
            try
            {
                StreamReader sr = new StreamReader(@"avs\nnedi.avs", true);
                StringBuilder context = new StringBuilder();

                
                int length = 12;
                for (int i = 0; i < length; i++)
                {
                    if (i == 10)
                    {
                        sr.ReadLine();
                        context.Append("FFVideoSource(\"" + originalPath + "\")\n");
                    }else if(i == 11){
                        if(px == 720)
                        {
                            sr.ReadLine();
                            context.Append("nnedi3_resize16(1280,720)\n");
                        }else if(px == 1080)
                        {
                            sr.ReadLine();
                            context.Append("nnedi3_resize16(1920,1080)\n");
                        }
                        else if (px == 480)
                        {
                            sr.ReadLine();
                            context.Append("nnedi3_resize16(720,480)\n");
                        }
                    }
                    else
                    {
                        context.Append(sr.ReadLine() + "\n");
                    }
                    
                }
                sr.Close();
                StreamWriter sw = new StreamWriter(@"avs\nnedi.avs", false);
                sw.Write(context);
                sw.Close();
                result = "avs脚本修改视频地址成功。";
                return result;
            }
            catch(Exception e)
            {
                result = "avs脚本修改视频地址失败。" + e.Message;
                return result;
            }
        }

        /**
       * 修改AVS脚本文件中，源地址的路径（bilinear算法
       * 
       * */
        public String UpdateAvsFile_bilinear(string originalPath, int px)
        {
            String result;
                StreamReader sr = new StreamReader(@"avs\bilinear.avs", true);
                StringBuilder context = new StringBuilder();


                int length = 11;
                for (int i = 0; i < length; i++)
                {
                    if (i == 10)
                    {
                        if (px == 720)
                        {
                            sr.ReadLine();
                            context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BilinearResize(1280,720)");
                            context.Append("\n");
                    }
                        else if (px == 1080)
                        {
                            sr.ReadLine();
                            context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BilinearResize(1920,1080)");
                            context.Append("\n");
                    }
                    else if(px == 480)
                          {
                        sr.ReadLine();
                        context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BilinearResize(720,480)");
                        context.Append("\n");
                    }
                    }
                    else
                    {
                        context.Append(sr.ReadLine() + "\n");
                    }

                }
                sr.Close();
                StreamWriter sw = new StreamWriter(@"avs\bilinear.avs", false);
                sw.Write(context);
                sw.Close();
                result = "avs脚本修改视频地址成功。";
                return result;
            }

     

        /**
         * 修改AVS脚本文件中，源地址的路径（spline算法
         * 
         * */
        public String UpdateAvsFile_spline(String originalPath, int px)
        {
            String result;
            try
            {
                StreamReader sr = new StreamReader(@"avs\spline.avs", true);
                StringBuilder context = new StringBuilder();


                int length = 11;
                for (int i = 0; i < length; i++)
                {
                    if (i == 10)
                    {
                        if (px == 720)
                        {
                            sr.ReadLine();
                            context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.Spline64Resize(1280,720)");
                            context.Append("\n");
                        }
                        else if (px == 1080)
                        {
                            sr.ReadLine();
                            context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.Spline64Resize(1920,1080)");
                            context.Append("\n");
                        }
                        else if (px == 480)
                        {
                            sr.ReadLine();
                            context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.Spline64Resize(720,480)");
                            context.Append("\n");
                        }
                    }
                    else
                    {
                        context.Append(sr.ReadLine() + "\n");
                    }

                }
                sr.Close();
                StreamWriter sw = new StreamWriter(@"avs\spline.avs", false);
                sw.Write(context);
                sw.Close();
                result = "avs脚本修改视频地址成功。";
                return result;
            }
            catch (Exception e)
            {
                result = "avs脚本修改视频地址失败。" + e.Message;
                return result;
            }
        }
        

        /**
    * 修改AVS脚本文件中，源地址的路径（bicubic算法
    * 
    * */
        public String UpdateAvsFile_bicubic(string originalPath, int px)
        {
            String result;
            StreamReader sr = new StreamReader(@"avs\bicubic.avs", true);
            StringBuilder context = new StringBuilder();


            int length = 11;
            for (int i = 0; i < length; i++)
            {
                if (i == 10)
                {
                    if (px == 720)
                    {
                        sr.ReadLine();
                        context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BicubicResize(1280,720)");
                        context.Append("\n");
                    }
                    else if (px == 1080)
                    {
                        sr.ReadLine();
                        context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BicubicResize(1920,1080)");
                        context.Append("\n");
                    }
                    else if (px == 480)
                    {
                        sr.ReadLine();
                        context.Append("FFVideoSource(\"" + originalPath + "\").ConvertToYV12.BicubicResize(720,480)");
                        context.Append("\n");
                    }
                }
                else
                {
                    context.Append(sr.ReadLine() + "\n");
                }

            }
            sr.Close();
            StreamWriter sw = new StreamWriter(@"avs\bicubic.avs", false);
            sw.Write(context);
            sw.Close();
            result = "avs脚本修改视频地址成功。";
            return result;
        }

        /**
      * 修改bat文件中的，目的地址路径,原视频路径和算法
      * */
        public String UpdateBatFile(string outputPath, string originalPath,string algorithm)
        {
            String result;

            try
            {
                StreamReader sr = new StreamReader(@"start.bat", true);
                StringBuilder sb = new StringBuilder();

                int length = 5;
                for (int i = 0; i < length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append("avs\\X264  -o avs\\temp.mp4 avs\\" + algorithm + ".avs\n");
                        sr.ReadLine();
                    }
                    else if (i == 1)
                    {
                        sr.ReadLine();
                        sb.Append("avs\\ffmpeg.exe -i \"" + originalPath + "\" -y -f wav avs\\temp.wav\n");
                    }
                    else if (i == 4)
                    {
                        sr.ReadLine();
                        sb.Append("avs\\mp4box.exe -new \"" + outputPath + "\" -add avs\\temp.mp4 -add avs\\temp.aac\n");
                    }
                    else
                    {
                        sb.Append(sr.ReadLine() + "\n");
                    }

                }
                sr.Close();
                StreamWriter sw = new StreamWriter(@"start.bat", false);
                sw.Write(sb);
                sw.Close();

                result = "bat文件修改成功。";
                return result;
            }
            catch (Exception e)
            {

                result = "bat文件修改失败。" + e.Message;
                return result;
            }
        }

        //添加其他算法滤镜
        public void UpdateOther(String algorithm)
        {
            StreamWriter sw = new StreamWriter(@"avs\" + algorithm + ".avs", true);
            sw.Write(others);
            sw.Close();
        }
    }
}
