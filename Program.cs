#region Global Variables
// ----------------
// GLOBAL VARIABLES
// ----------------

bool shouldExit = false;
string exitReason = "";

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;

// Console position of the player
(int x, int y) playerPosition = (0, 0);
HashSet<(int x, int y)> playerHitBox = new();
(int x, int y) lastPosition = (0, 0);
HashSet<(int x, int y)> interpolatedPositions = new();

// Console position of the food
(int x, int y) foodPosition = (0, 0);
HashSet<(int x, int y)> allFoodPieces = new();

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;
#endregion

#region MainLoop
// ---------
// MAIN LOOP
// ---------

InitializeGame();
while (!shouldExit)
{
    int speed = player == states[1] ? 4 : 1;

    Move(speed, true);
    TerminalResized();
    SetHitbox();
    CheckFoodCollision();
    if (IsFoodGone())
    {
        ChangePlayer();
        ShowFood();
    }
}
Exit();
#endregion

#region Helpers
// -------
// HELPERS
// -------

void Exit()
{
    Console.Clear();
    Console.Write("\x1b[3J");
    Console.SetCursorPosition(0, 0);
    Console.WriteLine($"{exitReason} Program exiting.");
}

// Returns true if the Terminal was resized 
void TerminalResized()
{
    bool wasResized = height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;

    if (wasResized)
    {
        shouldExit = true;
        if (exitReason == "")
            exitReason = "Console was resized.";
    }
}

void CheckFoodCollision()
{
    foreach (var box in playerHitBox)
    {
        if (allFoodPieces.Remove(box))
        {
            Console.SetCursorPosition(box.x, box.y);
            Console.Write(" ");
            Console.SetCursorPosition(playerPosition.x, playerPosition.y);
            Console.Write(player);
        }
    }
}

bool IsFoodGone() => allFoodPieces.Count == 0;

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    bool overlaps;
    do
    {
        overlaps = false;
        foodPosition.x = random.Next(0, width - player.Length);
        foodPosition.y = random.Next(0, height - 1);

        for (int i = 0; i < foods[food].Length; i++)
        {
            if (playerHitBox.Contains((foodPosition.x + i, foodPosition.y)))
            {
                overlaps = true;
                break;
            }
        }
    }
    while (overlaps);

    if (playerHitBox.Count == 0)
    {
        if (exitReason == "")
            exitReason = "Food generation error, player hitbox invalid.";
        shouldExit = true;
    }

    // Display the food at the location
    Console.SetCursorPosition(foodPosition.x, foodPosition.y);
    Console.Write(foods[food]);

    for (int i = 0; i < foods[food].Length; i++)
        allFoodPieces.Add((foodPosition.x + i, foodPosition.y));
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerPosition.x, playerPosition.y);
    Console.Write(player);

    if (player == states[2])
        FreezePlayer();
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    Thread.Sleep(1000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(int speed, bool quitOnInvalidInput = false)
{
    interpolatedPositions.Clear();
    lastPosition = playerPosition;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            for (int i = 0; i < speed; i++)
            {
                playerPosition.y--;
                interpolatedPositions.Add(playerPosition);
            }
            break;

        case ConsoleKey.DownArrow:
            for (int i = 0; i < speed; i++)
            {
                playerPosition.y++;
                interpolatedPositions.Add(playerPosition);
            }
            break;

        case ConsoleKey.LeftArrow:
            for (int i = 0; i < speed; i++)
            {
                playerPosition.x--;
                interpolatedPositions.Add(playerPosition);
            }
            break;

        case ConsoleKey.RightArrow:
            for (int i = 0; i < speed; i++)
            {
                playerPosition.x++;
                interpolatedPositions.Add(playerPosition);
            }
            break;

        case ConsoleKey.Escape:
            shouldExit = true;
            if (exitReason == "")
                exitReason = "User pressed escape.";
            break;

        default:
            if (quitOnInvalidInput)
            {
                shouldExit = true;
                if (exitReason == "")
                    exitReason = "Invalid input.";
            }
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastPosition.x, lastPosition.y);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerPosition.x = (playerPosition.x < 0) ? 0 : (playerPosition.x >= width ? width : playerPosition.x);
    playerPosition.y = (playerPosition.y < 0) ? 0 : (playerPosition.y >= height ? height : playerPosition.y);

    // Draw the player at the new location
    Console.SetCursorPosition(playerPosition.x, playerPosition.y);
    Console.Write(player);
}

void SetHitbox()
{
    playerHitBox.Clear();
    for (int i = 0; i < player.Length; i++)
        playerHitBox.Add((playerPosition.x + i, playerPosition.y));

    foreach (var position in interpolatedPositions)
    {
        for (int i = 0; i < player.Length; i++)
            playerHitBox.Add((position.x + i, position.y));
    }
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    for (int i = 0; i < 5; i++)
        playerHitBox.Add((i, 0));
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
#endregion