using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ThreadingInCsharp.Game;

namespace ThreadingInCsharp.States
{
    public abstract class State
    {
        protected List<Entity> components;
        protected ContentManager _content;

        protected Global _global;

        protected GraphicsDevice _graphicsDevice;

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Entity component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public abstract void PostUpdate(GameTime gameTime);

        public State(Global game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _global = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            this.components = new List<Entity>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Entity component in components)
            {
                component.Update(gameTime);
            }

        }


    }
}
