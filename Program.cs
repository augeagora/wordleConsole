Boot();
Menu();

void Menu()
{
    TitleColor("Douglas's Wordle v0.2");
    Console.WriteLine();
    Console.WriteLine("0 -- Play");
    Console.WriteLine("1 -- Play (DevMode)");
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
        Console.WriteLine("Type your first guess!\n");
        Play();
    }

    if (selection == "1")
    {
        Console.Clear();
        Console.WriteLine("Type your first guess!");
        PlayDev();
    }

    if (selection == "2")
    {
        Console.Clear();
        Exit();
    }
}

void Play()
{
    // First we need to get the wordList and word
    List<string> wordList = GetWordList();
    String word = GetWord(wordList);
    // DevComment($"The word is: {word}");

    List<string> guessesMade = new List<string>();
    int attempts = 6;
    int tries = 0;
    while (attempts > 0)
    {
        // Add one try
        tries++;

        // Let the user guess
        UserColor();
        String guess = Console.ReadLine().ToLower();
        // DevComment($"User's guess: {guess}");

        while (wordList.Contains(guess) == false)
        {
            WarningComment("Invalid Guess");
            // DevComment("Word does not exist in: valid-wordle-words.txt");
            guess = Console.ReadLine().ToLower();
        }
        while (guessesMade.Contains(guess))
        {
            WarningComment("You've already guessed that word!");
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

        // Count each letter in the word
        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (char letter in word)
        {
            if (letterCount.ContainsKey(letter) == false)
            {
                letterCount.Add(letter, 0);
            }
            if (letterCount.ContainsKey(letter) == true)
            {
                letterCount[letter]++;
            }
        }

        // DevComment($"Letter count for word: {word}");
        //foreach (var element in letterCount)
        //{
        //    DevComment(element.Key + ":" + element.Value);
        //}


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
            if (word.Contains(guess[i]) == false)
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

        // Print out the guess with corresponding colors
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

        // After each guess subtract one from attempts
        attempts--;
    }
    if (attempts == 0)
    {
        Lose(word);
    }
}

void Win(int tries)
{
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
    Menu();
}

void Lose(String word)
{
    Console.WriteLine();
    Console.WriteLine($"You lost... The word was: {word}");
    Console.ReadLine();
    Console.Clear();
    Menu();
}


void Exit()
{
    String message = "Goodbye!";
    foreach (char c in message)
    {
        Console.Write(c);
        Thread.Sleep(50);
    }
    Thread.Sleep(100);
    Environment.Exit(0);
}

void Boot()
{
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

void DevComment(String s)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine(s);
    Console.ResetColor();
}

void WarningComment(String s)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(s);
    Console.ResetColor();
}

void TitleColor(String s)
{
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(s);
    Console.ResetColor();
}

void UserColor()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}






































// This is literally just the same thing but with some things uncommented LMAO
void PlayDev()
{
    // First we need to get the wordList and word
    List<string> wordList = GetWordList();
    String word = GetWord(wordList);
    DevComment($"The word is: {word}");

    List<string> guessesMade = new List<string>();
    int attempts = 6;
    int tries = 0;
    while (attempts > 0)
    {
        // Add one try
        tries++;

        // Let the user guess
        UserColor();
        String guess = Console.ReadLine().ToLower();
        // DevComment($"User's guess: {guess}");

        while (wordList.Contains(guess) == false)
        {
            WarningComment("Invalid Guess");
            DevComment("Word does not exist in: valid-wordle-words.txt");
            guess = Console.ReadLine().ToLower();
        }
        while (guessesMade.Contains(guess))
        {
            WarningComment("You've already guessed that word!");
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

        // Count each letter in the word
        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (char letter in word)
        {
            if (letterCount.ContainsKey(letter) == false)
            {
                letterCount.Add(letter, 0);
            }
            if (letterCount.ContainsKey(letter) == true)
            {
                letterCount[letter]++;
            }
        }

        DevComment($"Letter count for word: {word}");
        foreach (var element in letterCount)
        {
            DevComment(element.Key + ":" + element.Value);
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
            if (word.Contains(guess[i]) == false)
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

        // Print out the guess with corresponding colors
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

        // After each guess subtract one from attempts
        attempts--;
    }
    if (attempts == 0)
    {
        Lose(word);
    }
}
