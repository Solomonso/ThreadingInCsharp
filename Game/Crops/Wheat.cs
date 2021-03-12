using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using ThreadingInCsharp.Game.Map;
using ThreadingInCsharp.States;

namespace ThreadingInCsharp.Game.Crops
{
	public class Wheat : Crop
	{
		public Wheat(ContentManager content, Vector2 position, FarmTile farmTile, GameState game) : base(content.Load<Texture2D>("wheatCrop"), position, "wheat", 5, 50, 70, farmTile, game, 10, 30, 50, 90)
		{

		}
	}
}
