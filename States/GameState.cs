using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThreadingInCsharp.Game;
using ThreadingInCsharp.Game.Controls;
using ThreadingInCsharp.Game.Crops;
using ThreadingInCsharp.Game.Items;
using ThreadingInCsharp.Game.Livestocks;
using ThreadingInCsharp.Game.Map;

namespace ThreadingInCsharp.States
{
    public class GameState : State
    {
        //declaring all textures and assests needed
        Texture2D rainTexture;
        Texture2D buttonTexture;
        Texture2D slotTexture;
        Texture2D littleCow;
        Texture2D chickenGrow;
        Texture2D littleChicken;
        Texture2D cowGrow;
        Texture2D deadChicken;
        Texture2D liveStockFencetile;
        Texture2D farmTileTexture;

        SpriteFont buttonFont;
        SpriteFont font;

        InventoryState inventory;
        ShopState shop;
        Weather weather;
        SeedItem selectedSeed = null;
        List<FarmTile> farmTiles;
        List<FarmTile> Tiles;
        List<Texture2D> chickenSprites;
        List<Texture2D> cowSprites;

        SoundEffect rainSfx, buttonSfx;
        SoundEffectInstance rainSound, buttonSound;

        public int currTemp;
        public int currHum;
        public int currSun;
        public int currRain;
        public int chickenCount;
        public int cowCount;

        TimeSpan timeTillNextWeatherUpdate;
        TimeSpan timeTillNextRain;

        public GameState(Global game, GraphicsDevice graphicsDevice, ContentManager content, InventoryState inventory, MouseState mouseState, ShopState shop) : base(game, graphicsDevice, content)
        {
            this.chickenCount = 0;
            this.cowCount = 0;
            this.chickenSprites = new List<Texture2D>();
            this.cowSprites = new List<Texture2D>();
            this.inventory = inventory;
            this.shop = shop;

            this.weather = new Weather();
            this.LoadGameStateAssets();
            farmTiles = new List<FarmTile>();
            Tiles = new List<FarmTile>();
            this.buttonSound = buttonSfx.CreateInstance();

            this.currHum = 40;
            this.currSun = 30;
            this.currTemp = 15;
            this.currRain = 10;
            this.timeTillNextWeatherUpdate = new TimeSpan(0, 0, 10);
            this.timeTillNextRain = new TimeSpan(0, 0, 10);

            this.rainSfx = content.Load<SoundEffect>("Sound/rain");
            this.rainSound = rainSfx.CreateInstance();
            this.rainSound.IsLooped = true;
            if (currRain == 100)
            {
                rainSound.Play();
            }
            else
            {
                rainSound.Stop();
            }

            //creates a fencetile barn where the animals will be displayed in
            var fencetile = new FarmTile(liveStockFencetile, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 2 / 3, 210), 1, content, this);//fencetile

            for (int i = 0; i < 9; i++)//creates 9 farmtile where the seeds are plant
            {
                farmTiles.Add(new FarmTile(farmTileTexture, new Vector2(-100, -100), 1, content, this));
                Tiles.Add(new FarmTile(farmTileTexture, new Vector2(-100, -100), 1, content, this));
            }

            //calculate the totalFarm tile and the one been clicked on
            for (int i = 0; i < (int)Math.Ceiling(((float)farmTiles.Count / 3)); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i * 3 + j < farmTiles.Count)
                    {
                        farmTiles[i * 3 + j].Position = new Vector2(j * 60, i * 55 + 40);
                        farmTiles[i * 3 + j].Click += farmTile_Click;
                    }
                }
            }

            var menuButton = new Button(buttonTexture, buttonFont, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 1 / 6, 950), 1)
            {
                Text = "Menu",
            };

            menuButton.Click += menuButton_Click;

            var inventoryButton = new Button(buttonTexture, buttonFont, new Vector2(900, 950), 1)
            {
                Text = "Inventory",
            };

            inventoryButton.Click += inventoryButton_Click;

            var shopButton = new Button(buttonTexture, buttonFont, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 4 / 5, 950), 1)
            {
                Text = "Shop",
            };

            shopButton.Click += shopButton_Click;

            components = new List<Entity>()//adding the farmtile and buttons to components
            {
                fencetile,//fenceTile
                farmTiles[0],
                farmTiles[1],
                farmTiles[2],
                farmTiles[3],
                farmTiles[4],
                farmTiles[5],
                farmTiles[6],
                farmTiles[7],
                farmTiles[8],
                Tiles[0],
                Tiles[1],
                Tiles[2],
                Tiles[3],
                Tiles[4],
                Tiles[5],
                Tiles[6],
                Tiles[7],
                Tiles[8],
                menuButton,
                inventoryButton,
                shopButton,
            };
        }

        /// <summary>
        /// To buy an extra farm time land
        /// </summary>
        public void BuyLand()
        {
            for (int i = 0; i < (int)Math.Ceiling(((float)farmTiles.Count / 3)); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i * 3 + j < farmTiles.Count)
                    {
                        Tiles[i * 3 + j].Position = farmTiles[i * 3 + j].Position + new Vector2(0, 200);
                        Tiles[i * 3 + j].Click += farmTile_Click;
                    }
                }
            }
        }

        private void GameState_Click(object sender, EventArgs e)
        {
            if (selectedSeed != null)
                ((FarmTile)sender).addSeed(selectedSeed);
        }

        /// <summary>
        /// Load all game assests from the content pipeline
        /// </summary>
        private void LoadGameStateAssets()
        {
            this.font = _content.Load<SpriteFont>("defaultFont");
            this.farmTileTexture = _content.Load<Texture2D>("dirt");
            this.rainTexture = _content.Load<Texture2D>("rain");
            this.buttonTexture = _content.Load<Texture2D>("Button");
            buttonFont = _content.Load<SpriteFont>("defaultFont");

            liveStockFencetile = _content.Load<Texture2D>("Sprites/dirt2");
            slotTexture = _content.Load<Texture2D>("ItemSlot");

            //animal sprites
            littleChicken = _content.Load<Texture2D>("chicken");
            littleCow = _content.Load<Texture2D>("cow");
            chickenGrow = _content.Load<Texture2D>("cowgrow");
            cowGrow = _content.Load<Texture2D>("chickGrow2");
            deadChicken = _content.Load<Texture2D>("Sprites/deadChicken");

            this.buttonSfx = _content.Load<SoundEffect>("Sound/selectionClick");
        }

        /// <summary>
        /// Drawing all the spirites needed when game state loads
        /// </summary>
        /// <param name="gameTime">GameTime of the game</param>
        /// <param name="spriteBatch">Sprites that are been drawn</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D grass = _content.Load<Texture2D>("Grass");

            spriteBatch.Begin();

            //normal time(dayTime)
            spriteBatch.Draw(grass, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);

            DateTime dateTime = DateTime.Now;
            string time = dateTime.ToString("h:mm tt");

            if (Night() == true)
            {
                //nightTime
                //this gets drawn
                spriteBatch.Draw(grass, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), new Color(50, 50, 125));
                spriteBatch.DrawString(font, "Time: " + time, new Vector2(640, 15), Color.White);
            }

            if (this.selectedSeed != null)
                spriteBatch.Draw(selectedSeed.GetTexture(), new Vector2(200, 20), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);


            if (currRain == 100)
            {
                spriteBatch.Draw(rainTexture, new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White * 0.7f);
            }

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(font, "Time: " + time, new Vector2(640, 15), Color.White);

            spriteBatch.DrawString(font, "Temperature:" + currTemp.ToString(), new Vector2(640, 35), Color.White);
            spriteBatch.DrawString(font, "Humidity:" + currHum.ToString(), new Vector2(640, 55), Color.White);
            spriteBatch.DrawString(font, "Sunshine:" + currSun.ToString(), new Vector2(640, 75), Color.White);
            spriteBatch.DrawString(font, "Chance of Rain:" + currRain.ToString() + "%", new Vector2(640, 95), Color.White);

            spriteBatch.Draw(littleChicken, new Vector2(280, 5), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(littleCow, new Vector2(280, 30), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);

            spriteBatch.Draw(slotTexture, new Vector2(195, 15), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "X " + chickenCount, new Vector2(320, 15), Color.White);
            spriteBatch.DrawString(font, "X " + cowCount, new Vector2(320, 40), Color.White);

            if (this.selectedSeed != null)
            {
                spriteBatch.Draw(selectedSeed.GetTexture(), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// This function use the actual time of the day
        ///  When its 7PM the Game state gets dark
        /// Return true or false
        /// </summary>
        public bool Night()
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan endDayTime = new TimeSpan(19, 0, 0);
            TimeSpan startDayTime = new TimeSpan(06, 0, 0);
            TimeSpan now = dateTime.TimeOfDay;

            if (endDayTime < startDayTime)
                return endDayTime <= now && now <= startDayTime;

            return !(startDayTime < now && now < endDayTime);
        }

        /// <summary>
        /// if a seed as been selected from the inventory slot
        ///     it prepare all seeds for planting
        /// </summary>
        void PrepareSeed()
        {
            foreach (SeedItem seeds in inventory.seeds)
            {
                if (seeds.IsSelected() == false)
                {
                    this.selectedSeed = null;
                }
            }

            foreach (SeedItem seeds in inventory.seeds)
            {
                if (seeds.IsSelected() && (seeds.GetCount() > 0))
                {
                    this.selectedSeed = seeds;
                }
            }
        }

        /// <summary>
        /// add animals to game when you buy them
        /// </summary>
        public void AddAnimal(LiveStockItem animal)
        {

            Random random = new Random();
            int i = 1;
            if (animal.GetName() == "chicken")
            {
                float xPosition = random.Next(1300, 1450);
                float yPosition = random.Next(350, 550);
                Chicken chick = new Chicken(cowGrow, new Vector2(xPosition, yPosition));
                components.Add(chick);
                chick.Click += Livestock_Click;
                chickenCount++;
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }

            if (animal.GetName() == "cow")
            {
                float xPosition = random.Next(1300, 1450);
                float yPosition = random.Next(350, 550);
                Cow cow = new Cow(chickenGrow, new Vector2(xPosition, yPosition));
                components.Add(cow);
                cow.Click += Livestock_Click;
                i++;
                cowCount++;
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }

        /// <summary>
        /// For adding crops to farmtile
        ///  event clicker for crops
        /// </summary>
        private void farmTile_Click(object sender, EventArgs e)
        {
            if (selectedSeed != null && ((FarmTile)sender).plantedSeed == null)
            {

                ((FarmTile)sender).addSeed(selectedSeed);
            }
            else if (((FarmTile)sender).plantedSeed != null)
            {
                ((FarmTile)sender).harvestCrop();
            }
        }

        void MouseMethod()
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed && selectedSeed != null)
            {
                selectedSeed.Select(false);
                selectedSeed = null;
            }
        }

        public bool AddCropToInventory(Crop crop)
        {
            for (int i = 0; i < inventory.Inventory.Count; i++)
            {
                if (crop.GetName() == inventory.Inventory[i].GetName())
                {
                    inventory.Inventory[i].SetCount();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Update the game state at runtime
        /// </summary>
        public override void Update(GameTime gameTime)
        {

            //Using TPL randomize weather conditions
            Task.Factory.StartNew(() => updateWeather(gameTime));
            Task.Factory.StartNew(() => makeItRain(gameTime));

            if (currRain == 100)//plays rain sounf when the % is 100
            {
                rainSound.Play();
            }
            else
            {
                rainSound.Pause();
            }

            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(gameTime);
                if (components[i].flaggedForDeletion)
                {
                    components.RemoveAt(i);
                }
            }
            MouseMethod();
            PrepareSeed();
        }

        //for changing the game to shop state
        private void shopButton_Click(object sender, EventArgs e)
        {
            this.buttonSound.Play();
            _global.ChangeState(_global.shop);
        }
        //for changing the game to inventory state
        private void inventoryButton_Click(object sender, EventArgs e)
        {
            this.buttonSound.Play();
            _global.ChangeState(_global.inventory);
        }
        //for changing the game to menu state
        private void menuButton_Click(object sender, EventArgs e)
        {
            this.buttonSound.Play();
            _global.ChangeState(_global.menu);
        }

        //event clicker for harvesting animals
        private void Livestock_Click(object sender, EventArgs e)
        {
            if (((LiveStock)sender).GetName() == "cow")
            {
                if (((LiveStock)sender).CurrentFrame > 2)
                {
                    this.cowCount -= 1;
                    for (int i = 0; i < inventory.Inventory.Count; i++)
                    {
                        if ("cow" == inventory.Inventory[i].GetName())
                        {
                            inventory.Inventory[i].SetCount();
                        }
                    }
                    ((Entity)sender).flaggedForDeletion = true;
                }
            }
            else if (((LiveStock)sender).GetName() == "chicken")
            {
                if (((LiveStock)sender).CurrentFrame > 2)
                {
                    this.chickenCount -= 1;

                    for (int i = 0; i < inventory.Inventory.Count; i++)
                    {
                        if ("chicken" == inventory.Inventory[i].GetName())
                        {
                            inventory.Inventory[i].SetCount();

                        }
                    }
                    ((Entity)sender).Texture = deadChicken;
                    ((Entity)sender).flaggedForDeletion = true;
                }
            }
        }

        private void updateWeather(GameTime gameTime)
        {
            this.timeTillNextWeatherUpdate = timeTillNextWeatherUpdate - gameTime.ElapsedGameTime;

            if (this.timeTillNextWeatherUpdate < TimeSpan.Zero)
            {
                currTemp = weather.randomTemp();
                Thread.Sleep(3000); //Suspend the currTemp(current thread) for 3 seconds
                currHum = weather.randomHumidity();
                Thread.Sleep(3000); //Suspend the currHum(current thread) for 3 seconds
                currSun = weather.randomSun();
                this.timeTillNextWeatherUpdate = new TimeSpan(0, 0, 10);

            }
        }

        private void makeItRain(GameTime gameTime)
        {
            this.timeTillNextRain = timeTillNextRain - gameTime.ElapsedGameTime;

            if (this.timeTillNextRain < TimeSpan.Zero)
            {
                currRain = weather.randomRain();
                this.timeTillNextRain = new TimeSpan(0, 0, 20);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //Implement an update if need arises later
        }

    }
}