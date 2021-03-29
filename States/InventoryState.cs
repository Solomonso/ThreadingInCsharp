using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThreadingInCsharp.Game;
using ThreadingInCsharp.Game.Controls;
using ThreadingInCsharp.Game.interfaces;
using ThreadingInCsharp.Game.Items;

namespace ThreadingInCsharp.States
{
   public class InventoryState : State
    {

        public List<IInventoryItem> Inventory;
        public List<SeedItem> seeds;
        public SeedItem selected = null;
        public int Coins;

        Texture2D lettuceSprite;
        Texture2D lettuceSeedSprite;
        Texture2D cornSprite;
        Texture2D cornSeedSprite;
        Texture2D wheatSprite;
        Texture2D wheatSeedSprite;
        Texture2D cowSprite;
        Texture2D chickenSprite;
        SpriteFont font;
        Button closeButton;


        public InventoryState(Global game, GraphicsDevice graphicsDevice, ContentManager contentManager)
            : base(game, graphicsDevice, contentManager)
        {
            Inventory = new List<IInventoryItem>();
            seeds = new List<SeedItem>();
            this.Coins = 1000;
            font = _content.Load<SpriteFont>("defaultFont");
            this.lettuceSprite = game.Content.Load<Texture2D>("Sprites/Lettuce-icon");
            this.lettuceSeedSprite = game.Content.Load<Texture2D>("seeds_lettuce");
            this.cornSprite = game.Content.Load<Texture2D>("Sprites/Corn");
            this.cornSeedSprite = game.Content.Load<Texture2D>("seeds_corn");
            this.wheatSprite = game.Content.Load<Texture2D>("Sprites/wheat");
            this.wheatSeedSprite = game.Content.Load<Texture2D>("seeds_wheat");
            this.cowSprite = game.Content.Load<Texture2D>("Sprites/Beef");
            this.chickenSprite = game.Content.Load<Texture2D>("Sprites/chicken_leg");
            CreateInventory();

            for (int i = 0; i < (int)Math.Ceiling(((float)Inventory.Count / 5)); i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i * 5 + j < Inventory.Count)
                        GenerateSlot(new Vector2(j * 100 + 163, i * 100 + 50), Inventory[i * 3 + j]);
                }
            }

            for (int i = 0; i < seeds.Count; i++)
            {
                GenerateSeedSlot(new Vector2(i * 200 + 150, 200), seeds[i]);
            }


            SpriteFont buttonFont = _content.Load<SpriteFont>("defaultFont");
            Texture2D closeButtonSprite = _content.Load<Texture2D>("CloseButton");
            closeButton = new Button(closeButtonSprite, buttonFont, new Vector2(730, 10), 1);
            closeButton.Click += CloseButton_Click;
            components.Add(closeButton);

        }
              
        private void CloseButton_Click(object sender, EventArgs e)
        {
            _global.ChangeState(_global.Game);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_content.Load<Texture2D>("storeBackground"), new Vector2(25, 20), Color.White);
            spriteBatch.DrawString(font, "Coins " + Coins, new Vector2(40, 40), Color.White);
            foreach (Entity component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        void CreateInventory()
        {
            CropItem wheatItem = new CropItem(wheatSprite, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100), 600, 0, "wheat", 600);
            SeedItem wheatSeed = new SeedItem(wheatSeedSprite, new Vector2(-100, -100), 100, 0, "wheat");
            CropItem lettuceItem = new CropItem(lettuceSprite, new Vector2(-100, -100), 250, 0, "lettuce", 250);
            SeedItem lettuceSeed = new SeedItem(lettuceSeedSprite, new Vector2(-100, -100), 50, 0, "lettuce");
            CropItem cornItem = new CropItem(cornSprite, new Vector2(-100, -100), 75, 0, "corn", 50);
            SeedItem cornSeed = new SeedItem(cornSeedSprite, new Vector2(-100, -100), 5, 0, "corn");
            MeatItem cowItem = new MeatItem(cowSprite, new Vector2(-100, -100), 750, 0, "cow", 1150);
            MeatItem chickenItem = new MeatItem(chickenSprite, new Vector2(-100, -100), 300, 0, "chicken", 650);

                Task.Factory.StartNew(() =>
                {
                    CropItem wheatItem = new CropItem(wheatSprite, new Vector2(-100, -100), 600, 0, "wheat", 600);
                    Inventory.Add(wheatItem);
                }),
                Task.Factory.StartNew(() =>
                {
                    SeedItem wheatSeed = new SeedItem(wheatSeedSprite, new Vector2(-100, -100), 100, 0, "wheat");
                    seeds.Add(wheatSeed);
                }),

                Task.Factory.StartNew(() =>
                {
                    CropItem lettuceItem = new CropItem(lettuceSprite, new Vector2(-100, -100), 250, 0, "lettuce", 250);
                    Inventory.Add(lettuceItem);
                }),
                Task.Factory.StartNew(() =>
                {
                    SeedItem lettuceSeed = new SeedItem(lettuceSeedSprite, new Vector2(-100, -100), 50, 0, "lettuce");
                    seeds.Add(lettuceSeed);
                }),
                Task.Factory.StartNew(() =>
                {
                   CropItem cornItem = new CropItem(cornSprite, new Vector2(-100, -100), 75, 0, "corn", 50);
                    Inventory.Add(cornItem);
                }),
                Task.Factory.StartNew(() =>
                {
                   SeedItem cornSeed = new SeedItem(cornSeedSprite, new Vector2(-100, -100), 5, 0, "corn");
                   seeds.Add(cornSeed);
                }),
                Task.Factory.StartNew(() =>
                {
                   MeatItem cowItem = new MeatItem(cowSprite, new Vector2(-100, -100), 750, 0, "cow", 1150);
                   Inventory.Add(cowItem);
                }),
                Task.Factory.StartNew(() =>
                {
                   MeatItem chickenItem = new MeatItem(chickenSprite, new Vector2(-100, -100), 300, 0, "chicken", 650);
                   Inventory.Add(chickenItem);
                }),

        };

            Task.WaitAll(allTasks);
        }

        private void GenerateSlot(Vector2 position, IInventoryItem item)
        {
            InventorySlot newSlot = new InventorySlot(_content, position, item, 1f, this);
            components.Add(newSlot);
        }

        private void GenerateSeedSlot(Vector2 position, SeedItem item)
        {
            InventorySlot newSlot = new InventorySlot(_content, position, item, 1f);
            components.Add(newSlot);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
