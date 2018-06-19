using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class Explosion : Sprite	
	{
		public Explosion(Texture2D texture, Vector2 centerOfSprite, Rectangle bounds) : base(texture, centerOfSprite, bounds, 3, 4, 20)
		{
		}

		public bool IsDone()
		{
			return animationPlayedOnce;
		}
	}
}