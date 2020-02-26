# Set Up
1. Add Prefab MazeParent to scene.  
2. Add Script MazeGridGenerator to prefab if link is lost.
3. Drag Prefab MazeTile to script's Tile slot.
4. Make sure MazeTile prefab has MazeTileSprite attach to its Image component.

# Parameters
1. Tile Width: Cell Size of Grid Layout Group.
2. ~~Tile Count: The total number tiles initiating. ( Equals Maze Width * Maze Width )~~
3. Grid Width: # of tiles spaning between opposite walls (including walls).
4. ~~Maze Width: # of tiles on one side of the maze. ( Equals Grid Width + (Grid Width - 1) * (Quandrant Per Row - 1)~~
5. Quadrant is each sub square inside the maze. 

   Quadrant Per Row: # of sub squares that made up one side of the maze
   
## Demo

Maze Created with 
* Tile Width: 90  
* Tile Count: 961 
* Grid Width: 7 
* MazeWidth: 31 
* Quadrant Per Row: 5

![Imgur](https://i.imgur.com/XJ4RoR0.png)

Maze Created with
* Tile Width: 90  
* Tile Count: 441
* Grid Width: 5 
* MazeWidth: 21 
* Quadrant Per Row: 5. 

![Imgur](https://i.imgur.com/0WWQ92H.png)
