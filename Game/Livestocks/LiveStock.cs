using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ThreadingInCsharp.Game.Livestocks
{
    public abstract class LiveStock : Entity
    {
        string name;
        public TimeSpan timeTillNextStage;
        Random random;
        int minGrowTime;
        int maxGrowTime;
        private MouseState _currentMouse;
        private bool _isHovering;
        private MouseState _previousMouse;
        public event EventHandler Click;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width / FrameCount, Texture.Height);
            }
        }

        public bool Clicked { get; private set; }
        public LiveStock(Texture2D texture, Vector2 position, string name, int frameCount) : base(texture, position, frameCount)
        {
            this.name = name;
            this.random = new Random();
            int secondsTillNextStage = random.Next(1, 10);
            this.timeTillNextStage = TimeSpan.FromSeconds(secondsTillNextStage);
            this.minGrowTime = 1;
            this.maxGrowTime = 10;
        }

        public string GetName()
        {
            return this.name;
        }
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
            timeTillNextStage = timeTillNextStage.Subtract(gameTime.ElapsedGameTime);

            if (timeTillNextStage.TotalMilliseconds < 0 && CurrentFrame < FrameCount - 1)
            {
                CurrentFrame++;
                timeTillNextStage = TimeSpan.FromSeconds(random.Next(minGrowTime, maxGrowTime));
            }
            Hover();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawAnimation(spriteBatch);
        }


    }
}