using System;
using System.IO;
namespace ProgPOE_PART1
{
    class Program
    {
        static void Main(string[] args)
        {
        // Define the path to the WAV file
            //https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer?view=dotnet-plat-ext-7.0
            string wavPath = @"C:\Users\paras\source\repos\ProgPart1\ProgPart1\Welcome.wav";

            // Check if the WAV file exists at the specified path
            if (File.Exists(wavPath))
            {
                // If the file exists, create a new Music object and play the audio
                new Audio(wavPath).Play();
            }
            else
            {
                // If the file does not exist, output an error message to the console
                Console.WriteLine("WAV file not found at: " + wavPath);
            }

            // Define a string variable to hold ASCII art (content not shown in this snippet)
            //https://www.asciiart.eu/image-to-ascii#google_vignette
            string asciiArt = @"
               
          %%%%%%%%%%%%%%%%%%%%%%%%%%*+++*%%%%%%%%%%%%%%%%%%%%%%%%%%
          %%%%%%%%%%%%%%%%%%%%%%%*+++++++++*%%%%%%%%%%%%%%%%%%%%%%%
          %%%%%%%%%%%%%%%%%%%#++++++#%%%#++++++#%%%%%%%%%%%%%%%%%%%
          %%%%%%%%%%%%%%%#+++++++%%%%%%%%%%%+++++++#%%%%%%%%%%%%%%%
  %%%%%%%%%%%%%%%%%%*++++++++%%%%%%%%%%%%%%%%%%%++++++++*#%%%%%%%%%%%%%%%%%%
  %%%%%%%%%##**++++++++++#%%%%%%%%%%%+++++%%%%%%%%%%%*++++++++++**##%%%%%%%%%
  %%++++++++++++++#%%%%%%%%%%%%%%%%*+++++*%%%%%%%%%%%%%%%%#++++++++++++++%%%%
  %%+++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%+++++%%%%%%%%%%%%%%%%%%%%%%%%%%%%#+++%%%%
  %%+++%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%+++%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%+++%%%%
  %%+++%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%+++%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%+++%%%%
  %%+++%%%%%%%%%%%%%%%%%%*+++++++++++++++++++++++++*%%%%%%%%%%%%%%%%%%+++%%%%
  %%*++%%%%%%%%%%%%%%%*+++++++++++++++++++++++++++++++*%%%%%%%%%%%%%%%++*%%%%
  %%*++#%%%%%%%%%%%%%+++*%%%%%%%%%%%%%%%%%%%%%%%%%%%*+++%%%%%%%%%%%%%#++*%%%%
   %%++*%%%%%%%%%%%%*++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%#++*%%%%%%%%%%%%*++%%%%%
   %%+++%%%%%%%%%%%#+++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%#+++#%%%%%%%%%%%+++%%%%%
   %%+++%%%%%%%%%++++++#%%%%#....:%%%%%%%%%-....+%%%%#++++++%%%%%%%%%+++%%%%%
   %%+++%%%%%%%%%++++++#%%%%=.....*%%%%%%%#.....:%%%%#++++++%%%%%%%%%+++%%%%%
   %%%++**%%%%%%%%++++++#%%%%%=...+%%%%%%%%%#...:%%%%%#++++++%%%%%%%%*++%%%%%
   %%+++%%%%%%%%++++++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%#++++++%%%%%%%%+++%%%%%%
   %%%*++#%%%%%%%++++++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%#++++++%%%%%%%#++*%%%%%
    %%%+++%%%%%%%%%%+++#%%%%%%%%+++++++++++++%%%%%%%%#+++%%%%%%%%%%+++%%%%%
    %%%*++*%%%%%%%%%*++#%%%%%%%%%%%%%%%%%%%%%%%%%%%%%#++*%%%%%%%%%*++*%%%%
     %%%+++%%%%%%%%%%+++*%%%%%%%%%%%%%%%%%%%%%%%%%%%*+++%%%%%%%%%%+++%%%%
      %%%+++%%%%%%%%%%*+++++++++++++++++++++++++++++++*%%%%%%%%%%+++%%%%
       %%*++*%%%%%%%%%%%%#*+++++++++++++++++++++++*#%%%%%%%%%%%%*++*%%%
        %%+++#%%%%%%%%%%%#+++++++++%%%%%%%+++++++++*%%%%%%%%%%%#+++%%%
         %%+++#%%%%%%%%*++++++++++%%*+++*%%++++++++++*%%%%%%%%#+++%%%
          %%+++*%%%%%#+++++++++++=%%+++++%%=+++++++++++#%%%%%#+++%%%
           %%+++*%%%*++++++++++++#%%%%%%%%%%*+++++++++++*%%%*+++%%%
            %%%*+++%#++++++++++++#%%%%%%%%%%%%++++++++++++*%+++*%%
             %%#++++++++++++++++#%%%%+++%%%%%++++++++++++++++#%%
              %%+++++++++++++++%%%%%+++%%%%%+++++++++++++++%%%
                %%#+++++++++++++%%%%%#+#%%%%%+++++++++++++*%%
                 %%%*+++++++++++#%%%%%%%%%%%%++++++++++++%%
                   %%%*++++++++++###########+++++++++++%%
                    %%%*+++++++++++++++++++++++++++*%%
                      %%%%+++++++++++++++++++++++#%%
                        %%%#+++++++++++++++++#%%
                           %%%%#+++++++++++#%%%
                             %%%%#+++#%%%%
                                %%%%%%%%%
                
            ";
            // Display the ASCII art on the console
            Console.WriteLine(asciiArt);

            // Display the welcome message header
            Console.WriteLine("===========================================");
            Console.WriteLine("Welcome to the Cybersecurity Awareness Bot!");
            Console.WriteLine("===========================================");

            // Prompt the user to enter their name
            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            // Loop to ensure the name is not empty or just whitespace
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name cannot be empty. Please enter your name:");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            // Greet the user with their name in cyan text
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nHey there {name}!!! I'm your Cybersecurity Awareness Assistant.");
            Console.WriteLine($"Nice to meet you, {name}!\n");
            Console.ResetColor();

            // Use the custom TypeText method to simulate typing the bot's greeting
            Console.ForegroundColor = ConsoleColor.Green;
            BotAnswers.TypeText("Let's learn how to stay safe online!", ConsoleColor.Green);
            Console.ResetColor();

            // Instantiate a HandleUserQuestions object with an empty initial question and the user's name
            HandleUserQuestions handler = new HandleUserQuestions("", name);

            // Start an infinite loop to continuously receive and respond to user questions
            while (true)
            {
                Console.Write("\n\n");
                // Add spacing to align the user input to the right side
                Console.Write("                                                                 ");
                Console.Write("You: ");

                // Read the user's question
                string question = Console.ReadLine();

                // Check if the user wants to exit the chat
                if (string.Equals(question, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nGoodbye! Stay safe online!");
                    break; // Exit the loop and end the program
                }

                // Update the handler with the new question and process it
                handler = new HandleUserQuestions(question, name);
                handler.ProcessQuestion();
            }
        }
    }
}
//*******************************************************************************************************************************************************//
//End of Code//
//References//
//TypeWriter effect - https://www.youtube.com/watch?v=15i5ugOrXmY - BotAnswers class and line 43 to 74 in the HandleUserQuestions class
//String methods - https://www.codecademy.com/resources/docs/c-sharp/strings/contains - Line 43 to 74 and 92 to 126 in Program.cs
//ASCII Art - https://www.asciiart.eu/image-to-ascii#google_vignette - Line 27 to 67 in Program.cs
//SoundPlayer - https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer?view=dotnet-plat-ext-7.0 - Audio class and Line 10 to 21 in Prgram.cs
//ChatGPT - https://chatgpt.com/c/68094316-f630-8001-9f5a-a4000365abe9 - What the bot says depending on certain questions
//Github Copilot Chat
//Bot Response - https://wirefuture.com/post/complete-guide-to-chatbot-development-in-c-for-beginners - HandleUserQuestions.cs
//Key Responses - https://www.programiz.com/csharp-programming/library/string/contains - HandleUserQuestion.cs
//********************************************************************************************************************************************************//