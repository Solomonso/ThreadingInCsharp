using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ThreadingInCsharp.Game.Livestocks
{
    public class Cow : LiveStock
    {
        public Cow(Texture2D texture, Vector2 position) : base(texture, position, "cow", 4)
        {

        }
    }
}
