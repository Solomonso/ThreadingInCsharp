using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using ThreadingInCsharp.Game.Controls;
using ThreadingInCsharp.Game.Items;
using ThreadingInCsharp.States;
using ThreadingInCsharp.Game.interfaces;

namespace ThreadingInCsharp.Game
{
    /// <summary>
    /// The InventorySlot class keep track of items in inventory an inherit from the Entity class
    /// And also responsible for displaying all the items in the inventory
    /// </summary>
    public class InventorySlot : Entity
    {
        //declaring all the variables needed
        IInventoryItem item;
        SeedItem seeditem;
        Texture2D slotTexture;
        Texture2D itemCount;
        SpriteFont font;
        bool isSeed;
        Button selectButton;
        Button sellButton;
        InventoryState inventory;
        SoundEffect buttonSfx;
        SoundEffectInstance buttonSound;

        /// <summary>
        /// This constructor is use for generating all the items that are harvested which will be for sale in the inventory
        /// </summary>
        /// <param name="content">The content</param>
        /// <param name="position">The x and y position</param>
        /// <param name="item"> the itesm to add</param>
        /// <param name="scale">the scale of it</param>
        /// <param name="inventory">The Inventory</param>
        public InventorySlot(ContentManager content, Vector2 position, IInventoryItem item, float scale, InventoryState inventory) : base(item.GetTexture(), position, 1)
        {
            this.Position = position;
            this.item = item;
            this.scale = scale;
            this.isSeed = false;
            this.inventory = inventory;
            Texture2D buttonTexture = content.Load<Texture2D>("itemCount");
            slotTexture = content.Load<Texture2D>("ItemSlot");
            font = content.Load<SpriteFont>("defaultFont");
            itemCount = content.Load<Texture2D>("itemCount");
            var buttonFont = content.Load<SpriteFont>("defaultFont");

            this.buttonSfx = content.Load<SoundEffect>("Sound/selectionClick");
            this.buttonSound = buttonSfx.CreateInstance();

            sellButton = new Button(buttonTexture, buttonFont, this.Position + new Vector2(570, 450), 1)
            {
                Text = "sell"
            };
            sellButton.Click += SellButton_Click;
        }
        
        /// <summary>
        /// This constructor is use for generating seeds that will be selected for planting (Lettuce, corn, wheat)
        /// </summary>
        /// <param name="content">The content</param>
        /// <param name="position"> The position</param>
        /// <param name="seeditem">the seeds to add</param>
        /// <param name="scale">the scale of it</param>
        public InventorySlot(ContentManager content, Vector2 position, SeedItem seeditem, float scale) : base(seeditem.GetTexture(), position, 1)
        {
            this.Position = position;
            this.seeditem = seeditem;
            this.scale = scale;
            this.isSeed = true;
            Texture2D buttonTexture = content.Load<Texture2D>("Button");
            slotTexture = content.Load<Texture2D>("ItemSlot");
            font = content.Load<SpriteFont>("defaultFont");
            itemCount = content.Load<Texture2D>("itemCount");
            this.buttonSfx = content.Load<SoundEffect>("Sound/selectionClick");
            var buttonFont = content.Load<SpriteFont>("defaultFont");

            selectButton = new Button(buttonTexture, buttonFont, this.Position + new Vector2(510, 490), 1)
            {
                Text = "select",
            };
            selectButton.Click += SelectButton_Click;
        }

        //Sell button in inventory
        private void SellButton_Click(object sender, EventArgs e)
        {
            this.buttonSound.Play();
            if (this.item.GetCount() > 0)
            {
                this.item.Sell();
                this.inventory.Coins += this.item.GetSellingPrice();
            }
        }

    
        /// <summary>
        ///This method is used to set status of seed
        ///When a seed is selected from the inventory 
        /// it set its seed status to true or false
        /// </summary>
        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (!this.seeditem.IsSelected())
            {

                this.seeditem.Select(true);
            }
            else
            {
                this.seeditem.Select(false);
            }

        }

        /// <summary>
        /// Drawing all the inventory spirites needed
        /// </summary>
        /// <param name="gameTime">GameTime of the gam</param>
        /// <param name="spriteBatch">Sprites that are been drawn</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (this.isSeed)
            {
                spriteBatch.Draw(slotTexture, Position + new Vector2(550, 350), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(itemCount, Position + new Vector2(577, 440), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(Texture, Position + new Vector2(560, 360), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "x " + seeditem.GetCount(), Position + new Vector2(560, 360), Color.Black);
                selectButton.Draw(gameTime, spriteBatch);
            }
            else
            {
                spriteBatch.Draw(slotTexture, Position + new Vector2(550, 350), null, Color.White, 0f, Vector2.Zero, scale * 0.75f, SpriteEffects.None, 0f);
                spriteBatch.Draw(itemCount, Position + new Vector2(565, 405), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "x " + item.GetCount(), Position + new Vector2(580, 410), Color.Black);
                spriteBatch.Draw(Texture, Position + new Vector2(570, 365), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                sellButton.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Update and detect any changes at runtime
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        public override void Update(GameTime gameTime)
        {
            if (seeditem != null && seeditem.IsSelected())
                selectButton.Text = "selected";
            else if (seeditem != null)
                selectButton.Text = "select";


            if (this.isSeed)
            {

                selectButton.Update(gameTime);
            }
            else
            {

                sellButton.Update(gameTime);
            }
        }
    }
}
