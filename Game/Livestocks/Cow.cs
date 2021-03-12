using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadingInCsharp.Game.Livestocks
{
    class Cow : Livestock
    {
        public Cow(Texture2D texture, Vector2 position) : base(texture, position, "cow", 4)
        {

        }
    }
}
