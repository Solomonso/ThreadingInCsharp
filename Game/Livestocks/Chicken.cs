﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadingInCsharp.Game.Livestocks
{
    class Chicken : Livestock
    {
        public Chicken(Texture2D texture, Vector2 position) : base(texture, position, "chicken", 4)
        {

        }
    }
}
