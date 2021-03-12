using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadingInCsharp.Game.Livestocks
{
   public class Cow : LiveStock
    {
        public Cow(Texture2D texture, Vector2 position) : base(texture, position, "cow", 4)
        {

        }
    }
}
