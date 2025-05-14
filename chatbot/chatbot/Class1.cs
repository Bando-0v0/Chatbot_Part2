using System;
using System.Media;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Linq;

namespace chatbot
{
    /*
     #######################################################################################################################

    The class now implements the methods created in the interface.

    ########################################################################################################################
     */


    public class workingParts : iGears
    {


        /*
    #######################################################################################################################
       i am creating this to store the user that i scurrently working with th system
   ########################################################################################################################
    */
        private User currentUser;


        /*
     #######################################################################################################################
        Creating an enum to store the responses of the user to track the coversation
    ########################################################################################################################
     */
        public enum ConversationState
        {
            Neutral,
            AskingForTopic,
            DiscussingTopic
        }
        /*
     #######################################################################################################################
        creating a response tracker so that it will control the flow of the conversation
    ########################################################################################################################
     */
        private ConversationState currentState = ConversationState.Neutral;
        private string currentTopic = "";

        /*
##################################################################################################################################
            creating a delegate to get the user input an go through the method to retrieve the bot response
####################################################################################################################################
*/
        public delegate string ResponseDelegate(string input);

        /*
##################################################################################################################################
        creating a dictionary to store bot responses to answer the users sentima
####################################################################################################################################
*/
        public Dictionary<string, int> AnalyzeConversationLog(string filePath, List<string> keywords)
        {
            Dictionary<string, int> keywordCounts = new Dictionary<string, int>();

            foreach (string keyword in keywords)
            {
                keywordCounts[keyword] = 0;
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.StartsWith("User:"))
                    {
                        string message = line.Substring(5).ToLower();

                        foreach (string keyword in keywords)
                        {
                            if (message.Contains(keyword.ToLower()))
                            {
                                keywordCounts[keyword]++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error analyzing conversation log: " + ex.Message);
            }

            return keywordCounts;
        }


        /*
##################################################################################################################################
            creating a dictionary to store bot responses to answer the users questions
####################################################################################################################################
*/
        private Dictionary<string, List<string>> keywordResponses;
        private Dictionary<string, List<string>> Sentimentkeyword;
        

        /*
##################################################################################################################################
           This is to randomize the responses that are retrieved from the list collection
####################################################################################################################################
*/
        private Random random = new Random();
        public workingParts()
        {

            Sentimentkeyword = new Dictionary<string, List<string>>
            {

            {
                    /*
         ##################################################################################################################################
                the response for curious
        ####################################################################################################################################
         */
                "curious", new List<string>
                {
                    "That's a great question! Let me explain it clearly.",
                    "I'm glad you're interested—here's how it works.",
                    "Curiosity is key in cybersecurity! Here's what you need to know.",
                    "Great thinking! Let me break that down for you.",
                    "Exploring these ideas will definitely boost your awareness.",
                    "You're asking the right questions—let's dig in.",
                    "It's awesome to see you're eager to learn!",
                    "Let me help you understand that in simple terms.",
                    "Let's explore that together—it's an important topic."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for worried
        ####################################################################################################################################
         */
                "worried", new List<string>
                {
                    "It's okay to feel concerned—you're not alone.",
                    "Cybersecurity can be scary, but you're taking the right steps.",
                    "Don't worry, I'm here to help you stay informed and safe.",
                    "Feeling uneasy is natural—let's tackle this together.",
                    "Let's make sure you're protected and understand what’s happening.",
                    "I'll guide you through it—no need to panic.",
                    "Take a deep breath—we’ve got this.",
                    "Let me show you how to stay safe, step by step.",
                    "Your safety is my priority. Let’s address your concerns."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for frustrated
        ####################################################################################################################################
         */
                "frustrated", new List<string>
                {
                    "I get it—cybersecurity can be overwhelming.",
                    "Let’s work through this together. It gets easier with time.",
                    "I'm here to support you. Let's take it one step at a time.",
                    "Don't let frustration stop you—we’ll solve this.",
                    "It's totally normal to feel stuck. Let me help.",
                    "Hang in there! You’re doing better than you think.",
                    "Let’s break it down so it’s not so confusing.",
                    "You're not alone—many people feel this way at first.",
                    "We can figure this out. Let me simplify things for you."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response fo confused
        ####################################################################################################################################
         */
                "confused", new List<string>
                {
                    "No worries—let me clarify that for you.",
                    "That part can be tricky. I’ll explain it step by step.",
                    "Confusion is the first step to understanding!",
                    "Let’s go over it again more simply.",
                    "You’re not the only one—this topic confuses many people.",
                    "I’m here to make it clearer for you.",
                    "It's okay to ask for clarity. Let me help.",
                    "Let me walk you through that carefully.",
                    "Let’s untangle that confusion together."
                }
            },
            };

            /*
        ##################################################################################################################################
               this list will store the keywords for the chat response on security topics
       ####################################################################################################################################
        */

            keywordResponses = new Dictionary<string, List<string>>
        {
            {
                    /*
         ##################################################################################################################################
                this list will store the topics that bot can respone to 
        ####################################################################################################################################
         */
                "topics", new List<string>
                {
                    "The scope of this conversation can cover 'phishing', 'password', 'malware', 'hacker', 'denial of service', 'scam' and 'socail engineering'."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for password
        ####################################################################################################################################
         */
                "password", new List<string>
                {
                    "Use at least 12 characters for a strong password.",
                    "Avoid using personal information in passwords.",
                    "Change your passwords regularly.",
                    "Don’t reuse passwords across multiple sites.",
                    "Use a mix of letters, numbers, and symbols.",
                    "Enable two-factor authentication when possible.",
                    "Consider using a trusted password manager.",
                    "Avoid writing passwords down on paper.",
                    "Make sure your password isn’t easy to guess."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for scam
        ####################################################################################################################################
         */
                "scam", new List<string>
                {
                    "Always be skeptical of offers that sound too good to be true.",
                    "Scammers often use urgency to pressure you.",
                    "Check email addresses and URLs carefully.",
                    "Never share personal info over unknown links.",
                    "Verify the source before clicking on any attachments.",
                    "Be cautious when someone asks for gift cards or wire transfers.",
                    "Use official websites to verify offers or contacts.",
                    "Report scams to proper authorities.",
                    "Keep your software and browser up to date."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for phishing
        ####################################################################################################################################
         */
                "phishing", new List<string>
                {
                    "Phishing emails often mimic trusted companies.",
                    "Look out for spelling mistakes or generic greetings.",
                    "Don't click suspicious links or download files.",
                    "Verify the sender before replying to any email.",
                    "Phishing attacks can also happen via SMS or social media.",
                    "Report phishing emails to your IT department or provider.",
                    "Don’t trust unexpected messages asking for personal info.",
                    "Use anti-phishing features in your email client.",
                    "Check the domain name carefully for slight differences."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for privacy
        ####################################################################################################################################
         */
                "privacy", new List<string>
                {
                    "Review privacy settings on your apps and social media.",
                    "Limit what you share publicly online.",
                    "Use end-to-end encrypted messaging services.",
                    "Be cautious about location sharing.",
                    "Clear your browser data regularly.",
                    "Avoid using public Wi-Fi for private activities.",
                    "Use incognito mode when needed.",
                    "Disable microphone and camera access for unused apps.",
                    "Read the privacy policy of services you use."
                }
            },
             {
                    /*
         ##################################################################################################################################
                the response for denial of service
        ####################################################################################################################################
         */
                "denial", new List<string>
                {
                    "A denial of service attack overwhelms systems with traffic.",
                    "It can make websites or services temporarily unavailable.",
                    "They’re often used to target companies or public services.",
                    "Mitigation tools can detect and block such attacks.",
                    "Firewalls and load balancers help against DoS attacks.",
                    "Always monitor your network traffic for anomalies.",
                    "Distributed DoS uses multiple systems to attack.",
                    "Cloud-based protection services can reduce the risk.",
                    "Keep your systems updated to avoid vulnerabilities."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the response for malware attack
        ####################################################################################################################################
         */
                "malware", new List<string>
                {
                    "Malware can come from email attachments or downloads.",
                    "Keep antivirus software up to date.",
                    "Never install software from untrusted sources.",
                    "Run regular scans to detect hidden malware.",
                    "Watch for sudden changes in system performance.",
                    "Use a firewall to help block malicious traffic.",
                    "Ransomware is a dangerous type of malware.",
                    "Back up your data in case of a malware attack.",
                    "Educate yourself about new malware trends."
                }
            },
            {
                    /*
         ##################################################################################################################################
                the responnse for social engineering 
        ####################################################################################################################################
         */
                "socail", new List<string>
                {
                    "Social engineering manipulates people, not systems.",
                    "Attackers often pose as someone you trust.",
                    "Be skeptical of unexpected calls or messages.",
                    "Don’t reveal sensitive info without verification.",
                    "Never let strangers access company systems or spaces.",
                    "Confirm identities through official channels.",
                    "Train staff to recognize social engineering attempts.",
                    "Pretexting and baiting are common tactics.",
                    "Security awareness is your first line of defense."
                }
            },
            {
                    /*
         ##################################################################################################################################
                The hackers response 
        ####################################################################################################################################
         */
                "hacker", new List<string>
                {
                    "Not all hackers are criminals—some are ethical hackers.",
                    "Black hat hackers exploit systems for personal gain.",
                    "White hats help improve security through testing.",
                    "Always secure your network to reduce risks.",
                    "Hackers often look for weak passwords.",
                    "Keep your software and systems patched.",
                    "Use two-factor authentication to prevent breaches.",
                    "Be alert for signs of unauthorized access.",
                    "Cybersecurity is the best defense against hackers."
                }
            },
        };
        }


        /*
##################################################################################################################################
           This method is created to capture the users name and the method calls the chat loop to start the conversation between
        the user and the chatbot
####################################################################################################################################
*/
        public void StartChat()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string greeting ="Welcome! Please enter your name: ";
            foreach (char c in greeting)
            {
                Console.Write(c);
                Thread.Sleep(40); // Delay between characters 
            }
            Console.ResetColor();

            string name = Console.ReadLine();
            currentUser = new User(name);

            string welcome = $"\nHello {currentUser.Name}, how can I assist you with cybersecurity today?";
            foreach (char c in welcome)
            {
                Console.Write(c);
                Thread.Sleep(40); 
            }
            ChatLoop(GetBotResponse);
        }

        /*
         ##############################################################################################
         ChatBot loop. while the user has not entered 'exit' the loop will contiune to prompt the user to 
        enter another question
        ################################################################################################
         */
        public void ChatLoop(ResponseDelegate responseMethod)
        {
            while (true)
            {

                /*
        ##################################################################################################################################
               this section is the design of the chat. this controls how the user will see the input area and the bot response
                it will also delay  the output so it writes out one char at a time

                it also controls the exit function and will call when the user wants to close the chat
       ####################################################################################################################################
        */


                // Prompt in green
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\n{currentUser.Name}: ");

                // Input in yellow
                Console.ForegroundColor = ConsoleColor.Yellow;
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    string farewell = $"Bot: Goodbye, {currentUser.Name}. Please stay safe online!";
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (char c in farewell)
                    {
                        Console.Write(c);
                        Thread.Sleep(40); // Delay between characters
                    }
                    Console.ResetColor();

                    LogInteraction(input, farewell);
                    break;
                }

                Dictionary<string, int> myKeywordCounts = new Dictionary<string, int>();

                // If the user asks for memory summary
                if (input.ToLower() == "memory")
                {
                    string summary = GenerateSummary();  
                    string frequentTopic = GetMostFrequentKeyword(myKeywordCounts);  
                    string sentimentResponse = RespondWithSentiment(frequentTopic);

                    string combinedResponse = summary + "\n\n" + sentimentResponse;

                    TypeOutText(combinedResponse, ConsoleColor.Yellow);
                    LogInteraction(input, combinedResponse);
                    continue;
                }
                
                if (input.ToLower() == "memory")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nBot: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string summary = EndChatSummary(); 
                    foreach (char c in summary)
                    {
                        Console.Write(c);
                        Thread.Sleep(30);
                    }
                    Console.WriteLine();
                    LogInteraction("memory", summary);
                    Console.ResetColor();
                    continue; 
                }

                // Bot label in red
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Bot: ");

                // Bot response in yellow, typed out letter by letter
                Console.ForegroundColor = ConsoleColor.Yellow;
                string response = responseMethod(input);
                foreach (char c in response)
                {
                    Console.Write(c);
                    Thread.Sleep(40); // Delay between characters
                }
                Console.WriteLine(); // New line after the response

                Console.ResetColor(); // Reset console color to default
                LogInteraction(input, response);
            }
        }




        /*
         ##########################################################################################
        the code block take the user input capture by the chatloop fuction above as a perameter.
        the user input is used as a means to give the user a response. right now the dialog s limited and
        will be expanded it the next version of the POE

        ##########################################################################################
         */


        public string GetBotResponse(string input)
        {
            input = input.ToLower();

            foreach (var entry in Sentimentkeyword)
            {
                if (input.Contains(entry.Key))
                {
                    List<string> responses = entry.Value;
                    return responses[random.Next(responses.Count)];
                }
            }


            /*
         ##########################################################################################
            The case switch accepts the enum as a perameter to start the floow of the conversation
            and manage the users state of emotions. 
        ##########################################################################################
         */
            switch (currentState)
            {
                case ConversationState.Neutral:
                    currentState = ConversationState.AskingForTopic;
                    return "What cybersecurity topic would you like help with today? (e.g., password, phishing, scam)";

                case ConversationState.AskingForTopic:
                    foreach (var keyword in keywordResponses.Keys)
                    {
                        if (input.Contains(keyword))
                        {
                            currentTopic = keyword;
                            currentState = ConversationState.DiscussingTopic;

                            var responses = keywordResponses[keyword];
                            
                            //this line when the response is retrieved from the list a random respose is selected
                            return $"Great! Let's talk about {keyword}. " + responses[random.Next(responses.Count)];
                        }
                    }
                    return "I didn’t catch that. The scope of this conversation can cover 'phishing', 'password', 'malware', 'hacker', 'denial of service'," +
                        " 'scam' and 'socail engineering'.";


                /*
                 ############################################################################################
                this code block is responsible for follow up questions. when more is typed 
                the program will retrieve another message from the same repsonses.
                #############################################################################################
                 */
                case ConversationState.DiscussingTopic:
                    if (input.Contains("more"))
                    {
                        var responses = keywordResponses[currentTopic];
                        return responses[random.Next(responses.Count)];
                    }
                    else
                    {
                        currentState = ConversationState.AskingForTopic;
                        return "Would you like to discuss another topic? Just type it!";
                    }
            }

            return "I'm not sure how to respond to that.";
        }
        /*
         ############################################################################################
        this method is to store the users questions on an extenal text file 
        #############################################################################################
         */
        private void LogInteraction(string userInput, string botResponse)
        {
            string logFileName = $"log_{currentUser.Name}_{DateTime.Now:yyyyMMdd}.txt";
            string logEntry = $"{DateTime.Now:HH:mm:ss} - {currentUser.Name} said: {userInput}\n" +
                              $"{DateTime.Now:HH:mm:ss} - Bot responded: {botResponse}\n";

            File.AppendAllText(logFileName, logEntry);
        }

        /*
        #######################################################################################################
       this method is to capture user input and capture into a variable
       ###########################################################################################################
        */
       public string GetUserInput(string prompt)
       {
           Console.Write(prompt);
           return Console.ReadLine();
       }
       /*
        #######################################################################################################
       this method is used to capture the users name....... This is for part 1 but not used in part 2 anymore
       ########################################################################################################
        */
        public void GreetUser()
        {
            currentUser.Name = GetUserInput("Enter your name: ");
            Console.WriteLine($"\nHello, {currentUser.Name}! Welcome to the Cybersecurity Awareness Bot.");
        }
        /*
         ##################################################################################################
        this play voice greeting method plays a voice recording that plays a greeting for the user when
        the bot first opens.
        ###################################################################################################
         */

        public void PlayVoiceGreeting()
        {
            try
            {
                string filePath = "greeting.wav";
                if (File.Exists(filePath))
                {
                    SoundPlayer player = new SoundPlayer(filePath);
                    player.PlaySync();
                }
                else
                {
                    Console.WriteLine("Voice greeting file not found.");
                }
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
        /*
        #######################################################################################################
       this method is used to return the most occuring key word in the log file
       ########################################################################################################
        */
        public string GetMostFrequentKeyword(Dictionary<string, int> keywordCounts)
        {
            if (keywordCounts == null || keywordCounts.Count == 0)
                return "No keywords found.";

            return keywordCounts.OrderByDescending(k => k.Value).First().Key;
        }
        /*
        #######################################################################################################
       this method is used to match the sentiment expressed by the user.
        ######################################################################################################
        */
        public string RespondWithSentiment(string topic)
        {
            string sentiment = "memory"; 

            if (Sentimentkeyword.ContainsKey(sentiment))
            {
                var responses = Sentimentkeyword[sentiment];
                return responses[random.Next(responses.Count)];

                
            }
            return $"It seem like you are really eager to learn about cyber security";
        }
        /*
        #######################################################################################################
       this method is used to show the user the keywords that have been shown the most
       ########################################################################################################
        */
        public string EndChatSummary()
        {
            List<string> keywords = new List<string> { "password", "scam", "privacy", "phishing", "denial", "malware", "social", "hacker" };

            var counts = AnalyzeConversationLog($"log_{currentUser.Name}_{DateTime.Now:yyyyMMdd}.txt", keywords);
            var topKeyword = GetMostFrequentKeyword(counts);
            RespondWithSentiment(topKeyword);
            return topKeyword;
        }

        public string GenerateSummary()
        {
            string logPath = $"log_{currentUser.Name}_{DateTime.Now:yyyyMMdd}.txt"; 
            if (!File.Exists(logPath))
            {
                return "No conversation history found.";
            }

            Dictionary<string, int> keywordCounts = new Dictionary<string, int>();
            List<string> keywords = new List<string> { "password", "scam", "privacy", "phishing", "denial of service", "malware attack", "social engineering", "hackers" };

            foreach (string keyword in keywords)
            {
                keywordCounts[keyword] = 0;
            }

            string[] lines = File.ReadAllLines(logPath);
            foreach (string line in lines)
            {
                string lowerLine = line.ToLower();
                foreach (string keyword in keywords)
                {
                    if (lowerLine.Contains(keyword))
                    {
                        keywordCounts[keyword]++;
                    }
                }
            }

            // Find the most mentioned keyword
            var mostUsed = keywordCounts.OrderByDescending(k => k.Value).First();

            if (mostUsed.Value == 0)
            {
                return "You haven’t focused on any specific topic yet.";
            }

            return $"Based on our conversation, you seem most interested in **{mostUsed.Key}**. You've asked about it {mostUsed.Value} time(s).";
        }

        public void TypeOutText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(30); 
            }

            Console.ResetColor();
            Console.WriteLine();  
        }

        public void PrintSectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n=== {title} ===");
            Console.ResetColor();
        }

        void iGears.LogInteraction(string userInput, string botResponse)
        {
            LogInteraction(userInput, botResponse);
        }

        void iGears.EndChatSummary()
        {
            throw new NotImplementedException();
        }
    }
}
