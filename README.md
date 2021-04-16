# Threading In C#

This Game is a farming simulator. It is built C# in Visual Studio 2019 using UWP the MonoGame Framework. 

#### Team Members:

- Ademola Adigun
- Allan Akanyijuka
- Isaac Semackor
- Jonathan Mohamed
- Solomon Asezebhobor
- Vũ Vãn



## How to Run this game

1. Clone and open the repo in Visual Studio, right-click on the solution and select `"Set as Startup Project"`.

2. From here, you should be able to run the game; if not, continue to step 3 to rebuild the assets in the MonoGame Pipeline.

3. Download and install `MonoGame Pipeline 3.7.1 for Visual Studio`

   https://community.monogame.net/t/monogame-3-7-1/11173. 

4. Inside Visual Studio, navigate to the Content folder, and right-click on the `Content.mgcb` file and press `“Open With…”` and select `“MonoGame Pipeline”`.

5. The MonoGame Pipeline should now be launched. Click `Build` or `F6`, and after the rebuilt is successful, you can close MonoGame Pipeline and Repeat Step 2.





## Where Threading is used

- Semaphores

  - Controls the number of livestock allowed to grow at a time. [ShopState.cs] 

- Task Parallel Library

  - Used to produce random weather conditions [GameState.cs]

  - Used for creating inventory assets [ShopState.cs, InventoryState.cs ]

- Multi-Threading

  - Each livestock entity is created on separate thread [ShopState.cs

- Async and Await

  - Used for creating Shop items [ShopState.cs]

  

