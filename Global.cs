﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp
{
    public class Global : Microsoft.Xna.Framework.Game
    {
        public Vector2 resize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public InventoryState inventory;
        public ShopState shop;
        public MenuState menu;
        public SettingState setting;
        public GameState Game;
        MouseState mouseState;
        private State currentState;
        private State nextState;

        public Thread shopStateThread;
        public Thread menuStateThread;
        public Thread settingStateThread;
        public Thread gameStateThread;
        public Thread inventoryStateThread;

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Global()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
                mouseState = Mouse.GetState();
                inventory = new InventoryState(this, graphics.GraphicsDevice, Content);

                shop = new ShopState(this, graphics.GraphicsDevice, Content, inventory);

                menu = new MenuState(this, graphics.GraphicsDevice, Content);

                setting = new SettingState(this, graphics.GraphicsDevice, Content);
                Game = new GameState(this, graphics.GraphicsDevice, Content, inventory, mouseState, shop);

            IsMouseVisible = true;
                IsFixedTimeStep = true;
                TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
                base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentState = menu;
        }

        void _ChangeState(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }
            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);
           base.Update(gameTime);
        }

        public void _Resize()
        {
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            _ChangeState(gameTime);
            _Resize();
        }

        protected override void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
