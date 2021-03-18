using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp.Game.Livestocks
{
   public class Chicken : LiveStock
    {
        public Chicken(Texture2D texture, Vector2 position) : base(texture, position, "chicken", 4)
        {

        }

     
    }
}
