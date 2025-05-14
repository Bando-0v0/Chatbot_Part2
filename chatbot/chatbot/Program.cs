using System;
using System.IO;
using System.Media;
using System.Drawing;
using static chatbot.workingParts;

namespace chatbot 
{
    class Program 
    {
        static void Main(string[] args)
        {
            
            workingParts obj = new workingParts();
            obj.PlayVoiceGreeting();
            new Logo() { };
            obj.StartChat();
            ResponseDelegate responseDelegate = new ResponseDelegate(obj.GetBotResponse);
            
            

        }
    }
}
