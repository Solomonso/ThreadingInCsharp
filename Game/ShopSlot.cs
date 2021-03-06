using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThreadingInCsharp.Game.Controls;
using ThreadingInCsharp.Game.interfaces;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp.Game
{
    /// <summary>
    /// The ShopSlot class keep track of items in the shop and also responsible for drawing the items 
    /// Inherits from the Entity
    /// </summary>
    public class ShopSlot : Entity
    {
        Button buyButton;
        IInventoryItem item;
        InventoryState inventory;
        ShopState shop;
        Texture2D slotTexture;
        Texture2D seedTexture;

        /// <summary>
        /// Use for constructing a shop slot 
        /// </summary>
        /// <param name="content">The content</param>
        /// <param name="position">The position </param>
        /// <param name="item">The item to add</param>
        /// <param name="frameCount">Number of frame</param>
        /// <param name="scale">the scale</param>
        /// <param name="inv">Accepts an InventoryState</param>
        /// <param name="shop">Accepts a ShopState</param>
        public ShopSlot(ContentManager content, Vector2 position, IInventoryItem item, int frameCount, float scale, InventoryState inv, ShopState shop) : base(item.GetTexture(), position, 1)
        {
            this.shop = shop;
            this.Position = position;
            this.item = item;
            this.scale = scale;
            this.inventory = inv;

            Texture2D buttonTexture = content.Load<Texture2D>("Button");
            slotTexture = content.Load<Texture2D>("ItemSlot");
            seedTexture = content.Load<Texture2D>("seeds");

            var buttonFont = content.Load<SpriteFont>("defaultFont");

            buyButton = new Button(buttonTexture, buttonFont, this.Position + new Vector2(525, 470), frameCount)
            {
                Text = "-" + this.item.GetPrice().ToString() + " coins"
            };
            buyButton.Click += BuyItem;
        }

        /// <summary>
        /// This method is to buy items available in the shop which is seeds and livestock 
        /// And also check which current item is bought from the shop and money gets deducted 
        /// </summary>
        private void BuyItem(object sender, EventArgs e)
        {
            if (inventory.Coins >= this.item.GetPrice())
            {
                if (this.item.GetName() == "lettuce" || this.item.GetName() == "wheat" || this.item.GetName() == "corn")
                {
                    for (int i = 0; i < inventory.seeds.Count; i++)
                        if (this.item.GetName() == inventory.seeds[i].GetName() && inventory.Coins >= inventory.seeds[i].GetPrice())
                        {
                            inventory.seeds[i].SetCount();
                            inventory.Coins -= inventory.seeds[i].GetPrice();
                        }
                }
                else if (this.item.GetName() == "chicken")
                {
                    shop.addItem(item);
                    inventory.Coins -= item.GetPrice();
                }
                else if (this.item.GetName() == "cow")
                {
                    shop.addItem(item);
                    inventory.Coins -= item.GetPrice();
                }
                else if (this.item.GetName() == "farmslot" && this.item.GetCount() < 1)
                {
                    shop.PrepareLand(item);
                    this.item.SetCount();
                    this.buyButton.Text = "sold out!";
                    inventory.Coins -= this.item.GetPrice();
                }
            }
        }

        /// <summary>
        /// Drawing all the assets in the shop
        /// </summary>
        /// <param name="gameTime">GameTime of the game</param>
        /// <param name="spriteBatch">Sprites that are been drawn</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(slotTexture, Position + new Vector2(540, 340), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(item.GetTexture(), Position + new Vector2(550, 350), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            buyButton.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Update and detect any changes at runtime
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            buyButton.Update(gameTime);
        }
    }
}
