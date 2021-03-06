using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ThreadingInCsharp.Game.Crops;
using ThreadingInCsharp.Game.Items;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp.Game.Map
{
    public class FarmTile : Entity
    {
        #region Fields

        private MouseState _currentMouse;

        private bool _isHovering;

        private MouseState _previousMouse;

        public Crop plantedSeed;

        private ContentManager content;

        private GameState game;

        private SpriteFont font;
        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }
        public Vector2 Size { get; internal set; }
        public Color BackgroundColor { get; internal set; }
        public Vector2 Location { get; internal set; }
        public bool ready;


        #endregion

        #region Methods

        public FarmTile(Texture2D texture, Vector2 position, int frameCount, ContentManager content, GameState game) : base(texture, position, frameCount)
        {
            PenColour = Color.Black;
            this.font = content.Load<SpriteFont>("defaultFont");
            this.content = content;
            this.game = game;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(Texture, Rectangle, colour);

            //check if there is seed planted in the tile, if there is show remaining grow time
            if (plantedSeed != null)
            {
                plantedSeed.Draw(gameTime, spriteBatch);
                spriteBatch.DrawString(font, plantedSeed.timeTillNextStage.TotalSeconds.ToString(), plantedSeed.Position, Color.White);

            }

        }

        // when mouse hover over tile
        void Hover()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());

                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            Hover();

            if (plantedSeed != null)
            {
                plantedSeed.Update(gameTime);
            }

        }

        //planting seed in selected tile
        public void addSeed(SeedItem seed)
        {
            if (seed.GetCount() > 0 && plantedSeed == null)
            {
                switch (seed.GetName())
                {
                    case "corn":
                        plantedSeed = new Corn(content, Position, this, game);
                        seed.Plant();
                        break;
                    case "lettuce":
                        plantedSeed = new Lettuce(content, Position, this, game);
                        seed.Plant();
                        break;
                    case "wheat":
                        plantedSeed = new Wheat(content, Position, this, game);
                        seed.Plant();
                        break;
                    default:
                        break;
                }
            }
        }

        //remove seed from selected tile
        public void removeCrop()
        {
            this.plantedSeed = null;
        }

        //havest crop from selected tile
        public void harvestCrop()
        {
            if (plantedSeed.CurrentFrame == plantedSeed.FrameCount - 1)
            {
                if (game.AddCropToInventory(plantedSeed))
                {
                    this.plantedSeed = null;
                }

            }

        }

        #endregion
    }
}
