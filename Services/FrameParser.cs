using PrimeraApp.Models;
using System;
namespace PrimeraApp.Services;

public static class FrameParser
{
    public static Frame CheckFrame(string frame)
    {
        Frame result = new Frame();

        int EndOfFramePosition = frame.IndexOf('*'); 

        if(EndOfFramePosition == -1)
        {
            result.Status = false;
            result.Error = "* missing";
            return result;
        }
        string body = frame.Substring(0,frame.IndexOf('*'));
        int sum = 0;

        foreach(char c in body)
        {
           sum+= c;
        }

        sum = sum & 0xFF;
        string CheckSum = sum.ToString("X2");
        //Console.WriteLine(CheckSum);
        if ((frame.Length - EndOfFramePosition) != 3)
        {
            result.Status = false;
            result.Error = "Format error";

            return result;
        }

        if(CheckSum == frame.Substring(frame.IndexOf('*')+1,2))
        {
            //Console.WriteLine("");
        }
        else
        {
            result.Status = false;
            result.Error = "Incorrect Checksum";
            
            return result;
        }

        
        string[] parts = body.Split(',');
        if(parts.Length == 4)
        {
            if(parts[0] != "V")
            {
                result.Status = false;
                result.Error = "Format error V";
                return result;

            }
            else
            {
            }

            if(parts[3] != "64")
            {
                result.Status = false;
                result.Error = "Format error: N";
                return result;

            }
            else
            {
                //Console.WriteLine("N ok");
            }
            

            try
            {
                if(Convert.ToInt32(parts[2]) > 65472 || Convert.ToInt32(parts[2]) < 0)
                {

                    result.Status = false;
                    result.Error = "Sum out of bounds";
                    return result;
                }

                else
                {
                    //Console.WriteLine("Sum ok");
                }


            }

            catch
            {
                    result.Status = false;
                    result.Error = "Sum is not number";
                    return result;
            }

            try
            {
                Convert.ToInt32(parts[1]);
            }
            catch
            {
                result.Status = false;
                result.Error = "Seq is not number";
                return result;
            }
        }
        else
        {
            result.Status = false;
            result.Error = "Incorrect fields";
            return result;           
        }

        foreach(string s in parts)
        {
            //Console.WriteLine(s);
        }

        result.Status = true;
        result.Error = "";
        result.Seq = Convert.ToInt32(parts[1]);
        result.Sum = Convert.ToInt32(parts[2]);
        result.N = Convert.ToInt32(parts[3]);
        
        result.Voltage = Math.Round(((((double)result.Sum / (double)result.N) * 3.3) / 1023), 5);
        return result;


    }

}
