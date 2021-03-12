using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using ThreadingInCsharp.Game.Map;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp.Game.Crops
{
	class Lettuce : Crop
	{
		public Lettuce(ContentManager content, Vector2 position, FarmTile farmTile, GameState game) : base(content.Load<Texture2D>("lettuceCrop"), position, "lettuce", 5, 30, 50, farmTile, game, 10, 30, 45, 65)
		{

		}
	}
}
