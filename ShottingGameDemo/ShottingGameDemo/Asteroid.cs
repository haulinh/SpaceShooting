using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class Asteroid : Sprite
	{
		public Asteroid(Texture2D texture, Vector2 position, Rectangle movementBounds) : base(texture, position, movementBounds, 8, 8, 10)
		{
			var random = new Random();
			Speed = random.Next(100, 300);
		}

		public override void Update(GameTime gameTime)
		{
			Velocity = new Vector2(0, 1);
			
			base.Update(gameTime);
		}

		public void Hit()
		{
			IsDead = true;
		}

		public bool IsDead { get; private set; }
	}
}
