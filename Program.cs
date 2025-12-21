using System.IO;
using System.Media;
using System.Runtime.InteropServices;


Boot();
Menu();

void Menu()
{
    var devMode = false;

    TitleColor("Douglas's Wordle v0.3", "WRITELINE");
    Console.WriteLine();
    Console.WriteLine("0 -- Play");
    Console.WriteLine("1 -- DevMode");
    Console.WriteLine("2 -- Exit");
    Console.WriteLine();
    UserColor();
    String selection = Console.ReadLine();


    while (selection != "0" && selection != "1" && selection != "2")
    {
        selection = Console.ReadLine();
    }
    Reset();

    if (selection == "0")
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("   ");
        TitleColor("Start", "WRITELINE");
        Reset();
        Console.WriteLine("-----------");
        Play(devMode);
    }

    if (selection == "1")
    {
        Console.Clear();
        Console.Write("   ");
        TitleColor("{DEV}", "WRITELINE");
        Reset();
        Console.WriteLine("-----------");
        devMode = true;
        Play(devMode);
    }

    if (selection == "2")
    {
        Console.Clear();
        Exit();
    }
}

void Play(bool devMode)
{
    // First we need to get the wordList and word
    List<string> wordList = GetWordList();
    String word = GetWord(wordList);

    // Grab the virtual keyboard
    var vk = VirtualKeyboardLetters();

    if (devMode) { DevComment($"The word is: {word}", "WRITELINE"); } //DM
    

    List<string> guessesMade = new List<string>();
    int attempts = 6;
    int tries = 0;
    while (attempts > 0)
    {
        // Add one try
        tries++;

        // Let the user guess
        UserColor();
        Console.Write("   "); // Add spaces to sort of center
        String guess = Console.ReadLine().ToLower();
        if (devMode) { DevComment($"User's guess: {guess}", "WRITELINE"); } //DM

        while (!wordList.Contains(guess))
        {
            WarningComment("Invalid Guess");
            UserColor();
            if (devMode) { DevComment("Word does not exist in: valid-wordle-words.txt", "WRITELINE"); } //DM
            Console.Write("   ");
            guess = Console.ReadLine().ToLower();
        }

        while (guessesMade.Contains(guess))
        {
            WarningComment("You've already guessed that word!");
            UserColor();
            Console.Write("   ");
            guess = Console.ReadLine().ToLower();
        }
        Reset();

        // Add valid guess to guessesMade list
        guessesMade.Add(guess);

        // Create an array for the user's guess
        String[] guessArray = { "", "", "", "", "" };
        for (int i = 0; i < 5; i++)
        {
            guessArray[i] = guess[i].ToString();
        }
        if (devMode)
        {
            foreach (String g in guessesMade) //DM
            {
                DevComment("guessesMade:" + g, "WRITELINE");
            }
        }

        // Count each letter in the word
        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (char letter in word)
        {
            if (!letterCount.ContainsKey(letter))
            {
                letterCount.Add(letter, 0);
            }
            if (letterCount.ContainsKey(letter))
            {
                letterCount[letter]++;
            }
        }

        if (devMode && tries == 1)
        {
            DevComment($"letterCount for [{word}]: ", "WRITE"); //DM
            foreach (var element in letterCount)
            {
                DevComment(element.Key + ":" + element.Value, "WRITE");
            }
            Console.WriteLine();
        }

        // Check for green (correct letter position)
        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == word[i])
            {
                guessArray[i] = "Green";
                letterCount[guess[i]]--;
            }
        }

        // Check for gray (not in the word)
        for (int i = 0; i < 5; i++)
        {
            if (!word.Contains(guess[i]))
            {
                guessArray[i] = "Gray";
            }
        }

        // Check for yellow (exists in the word)
        for (int i = 0; i < 5; i++)
        {
            if (word.Contains(guess[i]) && guessArray[i] != "Green" && guessArray[i] != "Gray" && letterCount[guess[i]] != 0)
            {
                guessArray[i] = "Yellow";
                letterCount[guess[i]]--;
            }
            else if (word.Contains(guess[i]) && guessArray[i] != "Green" && guessArray[i] != "Gray" && letterCount[guess[i]] == 0)
            {
                guessArray[i] = "Repeat";
            }
        }

        if (devMode)
        {
            DevComment($"guessArray:", "WRITE"); //DM
            foreach (var letter in guessArray)
            {
                DevComment(letter, "WRITE");
            }
            Console.WriteLine();
        }

        // Print out the guess with corresponding colors
        Console.Write("   "); // Add spaces to sort of center
        for (int i = 0; i < 5; i++)
        {
            if (guessArray[i] == "Green")
            {
                Green($"{guess[i]}");
            }
            else if (guessArray[i] == "Gray")
            {
                Gray($"{guess[i]}");
            }
            else if (guessArray[i] == "Yellow")
            {
                Yellow($"{guess[i]}");
            }
            else if (guessArray[i] == "Repeat")
            {
                Gray($"{guess[i]}");
            }
        }
        Console.WriteLine();

        // Check to see if user has won
        if (guess == word)
        {
            Win(tries);
            break;
        }

        // After each guess subtract one from attempts and print the virtual keyboard
        PrintVirtualKeyBoard(vk, guess, guessArray);
        attempts--;
    }
    if (attempts == 0)
    {
        Lose(word);
    }
}

void Win(int tries)
{
    // Play Win Sound
    if (OperatingSystem.IsWindows())
    {
        SoundPlayer bootSound = new SoundPlayer("audio\\win.wav");
        bootSound.Load();
        bootSound.Play();
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine();
    if (tries == 1)
    {
        Console.WriteLine($"Horray! You found the word in {tries} try!");
    }
    else
    {
        Console.WriteLine($"Horray! You found the word in {tries} tries!");
    }
    Console.ReadLine();
    Console.Clear();
    Reset();
    Menu();
}

void Lose(String word)
{
    // Play Lose Sound
    if (OperatingSystem.IsWindows())
    {
        SoundPlayer bootSound = new SoundPlayer("audio\\lose.wav");
        bootSound.Load();
        bootSound.Play();
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    Console.WriteLine($"You lost... The word was: {word}");
    Console.ReadLine();
    Console.Clear();
    Reset();
    Menu();
}


void Exit()
{
    // Play Exit Sound
    if (OperatingSystem.IsWindows())
    {
        SoundPlayer bootSound = new SoundPlayer("audio\\exit.wav");
        bootSound.Load();
        bootSound.Play();
    }

    String message = "Goodbye!";
    foreach (char c in message)
    {
        TitleColor($"{c}", "WRITE");
        Thread.Sleep(50);
    }
    Thread.Sleep(800);
    Environment.Exit(0);
}

void Boot()
{   
    // Play Boot Sound
    if (OperatingSystem.IsWindows())
    {
        SoundPlayer bootSound = new SoundPlayer("audio\\boot.wav");
        bootSound.Load();
        bootSound.Play();
    }

    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    String message = "AugeAgora";
    foreach (char c in message)
    {
        Console.Write(c);
        Thread.Sleep(50);
    }
    Thread.Sleep(100);
    Reset();
    Console.Clear();
}

List<string> GetWordList()
{
    // Read File
    string filePath = @"valid-wordle-words.txt";
    List<string> wordList = new List<string>();
    wordList = File.ReadAllLines(filePath).ToList();

    return wordList;
}

String GetWord(List<string> wordList)
{
    // Randomly Pick A Word
    Random random = new Random();
    int randomIndex = random.Next(0, wordList.Count);
    String word = wordList[randomIndex];

    return word;
}

// Color functions for convenience
void Green(String s)
{
    Console.BackgroundColor = ConsoleColor.Green;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write(s);
    Console.ResetColor();
}

void Yellow(String s)
{
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write(s);
    Console.ResetColor();
}

void Gray(String s)
{
    Console.BackgroundColor = ConsoleColor.Gray;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Write(s);
    Console.ResetColor();
}

void Reset()
{
    Console.ResetColor();
}

void DevComment(String s, String writeType)
{
    if (writeType == "WRITELINE")
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(s);
        Console.ResetColor();
    }
    else if (writeType == "WRITE")
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write(s + " ");
        Console.ResetColor();
    }
}

void WarningComment(String s)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(s);
}

void TitleColor(String s, String writeType)
{
    if (writeType == "WRITELINE")
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(s);
        Console.ResetColor();
    }
    else if (writeType == "WRITE")
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(s);
        Console.ResetColor();
    }
}

void UserColor()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}


// Virtual Keyboard thing
Dictionary<char, string> VirtualKeyboardLetters()
{
    var keyboardLetters = new Dictionary<char, string>
    {
        { 'q', "none" },
        { 'w', "none" },
        { 'e', "none" },
        { 'r', "none" },
        { 't', "none" },
        { 'y', "none" },
        { 'u', "none" },
        { 'i', "none" },
        { 'o', "none" },
        { 'p', "none" },
        { 'a', "none" },
        { 's', "none" },
        { 'd', "none" },
        { 'f', "none" },
        { 'g', "none" },
        { 'h', "none" },
        { 'j', "none" },
        { 'k', "none" },
        { 'l', "none" },
        { 'z', "none" },
        { 'x', "none" },
        { 'c', "none" },
        { 'v', "none" },
        { 'b', "none" },
        { 'n', "none" },
        { 'm', "none" },
    };
    return keyboardLetters;
}


void PrintVirtualKeyBoard(Dictionary<char, string> virtualKeyboardLetters, string guess, string[] guessArray)
{   
    // This sets the colors
    foreach (KeyValuePair<char, string> letter in virtualKeyboardLetters)
    {
        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == letter.Key) { virtualKeyboardLetters[guess[i]] = guessArray[i]; }
        }
    }

    // This prints out the keyboard
    Console.WriteLine("-----------");
        foreach (KeyValuePair<char,string> letter in virtualKeyboardLetters)
    {

        if (letter.Key == 'a')
        {
            Console.Write("\n ");
        }
        if (letter.Key == 'z')
        {
            Console.Write("\n   ");
        }

        if (letter.Value == "Green")
        {
            Green($"{letter.Key}");
        }
        else if (letter.Value == "Gray")
        {
            Gray($"{letter.Key}");
        }
        else if (letter.Value == "Yellow")
        {
            Yellow($"{letter.Key}");
        }
        else if (letter.Value == "Repeat")
        {
            Gray($"{letter.Key}");
        }
        else
        {
            Console.Write(letter.Key);
        }
    }
    Console.WriteLine("\n-----------");
}