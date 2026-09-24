# Microsoft Learn Foundational C#: Create Methods in C# Console Applications
## Challenge Project: Create a Mini-Game

## SUMMARY
My solution to the mini-game challenge in Learn's Foundational C# course.<br>
Initial commit contains the base code distributed for the assignment.<br>
Branch: learnSolution contains the code given by Learn as an example of a valid solution.<br>
Check history or compare branches to see my modifications and improvements.<br>

## LICENSE
Distributed under the MIT License.

## ASSIGNMENT INSTRUCTIONS
### Terminate on resize
This feature must:<br>
Determine if the terminal was resized before allowing the game to continue<br>
Clear the Console and end the game if the terminal was resized<br>
Display the following message before ending the program: Console was resized. Program exiting.<br>

### Add optional termination
Modify the existing Move method to support an optional parameter<br>
If enabled, the optional parameter should detect nondirectional key input<br>
If nondirectional input is detected, allow the game to terminate<br>

### Check if the player consumed the food
Create a method that uses the existing position variables of the player and food<br>
The method should return a value<br>
After the user moves the character, call your method to determine the following:<br>
- Whether or not to use the existing method that changes player appearance<br>
- Whether or not to use the existing method to redisplay the food<br>

### Check if the player should freeze
Create a method that checks if the current player appearance is (X_X)<br>
The method should return a value<br>
Before allowing the user to move the character, call your method to determine the following:<br>
Whether or not to use the existing method that freezes character movement<br>
Make sure the character is only frozen temporarily and the player can still move afterwards<br>
Add an option to increase player speed<br>
Modify the existing Move method to support an optional movement speed parameter<br>
Use the parameter to increase or decrease right and left movement speed by 3<br>
Create a method that checks if the current player appearance is (^-^)<br>
The method should return a value<br>
Call your method to determine if Move should use the movement speed parameter<br>

## COMMENTS
Besides implementing the above requirements:<br>
    - I created two `HashSet<(int, int)>` tuples, one to store coordinates of all positions currently populated by the player, and the other to store the individual positions of each piece of food. Comparing both sets of coordinates allows any part of the player to interact with any individual piece of food. This system is also used to prevent food from spawning in any position populated by the player. The HashSets are cleared and reused to avoid runtime allocation. HashSets were preferred over arrays in case the developer wanted to change the length of the player or food icons: since the assignment was broken down into separate modules, I wanted something that wouldn't be broken by a change of spec in later modules. I also switch to using tuples to store index positions of food and player for consistency and to prevent possible mismatches.<br>
    - `Move()` now increments positions `speed` number of times. This allows me to store every step in `interpolatedPositions`, which I add to the `playerHitbox` for collision detecting. This prevents the player tunneling through food.<br>
    - I created an `exitReason` string that stores the reason for exiting the first time `shouldExit` is set to true. This is used to generate an exit log when the main loop is terminated.