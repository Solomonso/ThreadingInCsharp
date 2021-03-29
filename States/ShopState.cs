using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadingInCsharp.Game;
using ThreadingInCsharp.Game.Controls;
using ThreadingInCsharp.Game.interfaces;
using ThreadingInCsharp.Game.Items;

namespace ThreadingInCsharp.States
{
    public class ShopState : State
    {
        public List<IInventoryItem> invList;
        Button closeButton;
        private InventoryState inventory;
        SpriteFont font;
        private Thread[] liveStockThreadList;


        public ShopState(Global game, GraphicsDevice graphicsDevice, ContentManager contentManager, InventoryState inventory)
            : base(game, graphicsDevice, contentManager)
        {
            this.inventory = inventory;
            this.font = _content.Load<SpriteFont>("defaultFont");
            this.invList = new List<IInventoryItem>();
            this.liveStockThreadList = new Thread[5];

            CreateInvList();

            //i dictates how many rows should be created (number of inventory items divided by 3 rounded up), j draws 3 items every row
            for (int i = 0; i < (int)Math.Ceiling(((float)this.invList.Count / 3)); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i * 3 + j < invList.Count)
                        GenerateSlot(new Vector2(j * 250 + 105, i * 210 + 50), this.invList[i * 3 + j]);
                }
            }
            Texture2D closeButtonSprite = _content.Load<Texture2D>("CloseButton");
            this.closeButton = new Button(closeButtonSprite, this.font, new Vector2(1500, 300), 1);
            this.closeButton.Click += closeButton_Click;
            components.Add(this.closeButton);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_content.Load<Texture2D>("storeBackground"), new Vector2(250, 200), Color.White);
            spriteBatch.DrawString(this.font, "Coins " + this.inventory.Coins, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Color.White);

            foreach (Entity component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        private void GenerateSlot(Vector2 position, IInventoryItem item)
        {
            ShopSlot newSlot = new ShopSlot(_content, position, item, 1, 1f, this.inventory, this);
            components.Add(newSlot);
        }

        public void CreateInvList()
        {
            SeedItem wheatSeed = new SeedItem(_content.Load<Texture2D>("seeds_wheat"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), 500, 500, "wheat");
            SeedItem lettuceSeed = new SeedItem(_content.Load<Texture2D>("seeds_lettuce"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/22, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/22), 50, 0, "lettuce");
            SeedItem cornSeed = new SeedItem(_content.Load<Texture2D>("seeds_corn"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), 5, 0, "corn");

            this.invList.Add(wheatSeed);
            this.invList.Add(lettuceSeed);
            this.invList.Add(cornSeed);

            LiveStockItem cowItem = new LiveStockItem(_content.Load<Texture2D>("cow"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), 750, 0, "cow");
            LiveStockItem chickenItem = new LiveStockItem(_content.Load<Texture2D>("chicken"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), 300, 0, "chicken");
            this.invList.Add(cowItem);
            this.invList.Add(chickenItem);

            TileItem tileItem = new TileItem(_content.Load<Texture2D>("Sprites/land"), new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/10), 10000, 0, "farmslot");
            this.invList.Add(tileItem);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            _global.ChangeState(_global.Game);
        }

        public void addItem(IInventoryItem item)
        {
            Semaphore liveStockSemphore = new Semaphore(initialCount: 3, maximumCount: 3, name: "liveStockSemaphore");

            if (item.GetName() == "chicken" || item.GetName() == "cow")
            {
                    Task.Factory.StartNew(() =>
                    {
                        liveStockSemphore.WaitOne();
                        _global.Game.AddAnimal((LiveStockItem)item);
                        liveStockSemphore.Release();
                    });
            }
        }

        public void PrepareLand(IInventoryItem item)
        {
            if (item.GetName() == "farmslot" && (item.GetCount() < 1))
            {
                _global.Game.BuyLand();
            }
        }
    }
}
