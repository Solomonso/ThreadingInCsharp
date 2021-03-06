using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadingInCsharp.Game
{
    public abstract class Entity : Animation
    {
        public Color tintColor;
        public int spriteWidth;
        public int spriteHeight;
        public float scale;
        public bool flaggedForDeletion;

        public Entity(Texture2D texture, Vector2 position, int frameCount, float scale) : base(texture, frameCount)
        {
            this.Position = position;
            this.tintColor = Color.White;
            this.scale = scale;
            this.flaggedForDeletion = false;
        }

        public Entity(Texture2D texture, Vector2 position, int frameCount) : base(texture, frameCount)
        {
            this.Position = position;
            this.tintColor = Color.White;
            this.scale = 1f;
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActivateAnimator)
            {
                DrawAnimation(spriteBatch);
            }
            else
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }
    }
}
