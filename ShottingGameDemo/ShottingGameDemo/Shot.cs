using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class Shot : Sprite
	{
		public Shot(Texture2D texture, Vector2 position, Rectangle movementBounds) : base(texture, position, movementBounds)
		{
			Speed = 400;
		}
	}
}