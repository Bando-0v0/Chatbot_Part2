using System;
using System.Collections.Generic;
using static chatbot.workingParts;

namespace chatbot
{
    /*
     ##########################################################################################################################

    Using an interface to track all the methods that in need to  create the chatBot.

    ##############################################################################################################################
     */
    interface iGears
    {
        void PlayVoiceGreeting();
        string GetUserInput(string prompt);
        void GreetUser();
        string GetBotResponse(string userInput);
        void ChatLoop(ResponseDelegate responseMethod);
        void PrintSectionHeader(string title);
        /*
         ####################################################################################################################
        these method were created for part 2 of the POE
        ########################################################################################################################
         */
        void StartChat();
        void LogInteraction(string userInput, string botResponse);
        string GetMostFrequentKeyword(Dictionary<string, int> keywordCounts);
        string RespondWithSentiment(string topic);
        void EndChatSummary();
    }
    /*
     ########################################################################################################

    ############################################################################################################
     */
}
