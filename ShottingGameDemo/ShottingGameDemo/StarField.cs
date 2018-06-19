using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class StarField
	{
		public Texture2D texture;
		private GraphicsDeviceManager graphics;
		public Vector2 backgroundPos1;
		public Vector2 backgroundPos2;
		public int speed;

		public StarField()
		{
			texture = null;
			backgroundPos1 = new Vector2(0, 0);
			backgroundPos2 = new Vector2(0, -480);
			speed = 2;
		}

		public void LoadContent(ContentManager Content)
		{
			texture = Content.Load<Texture2D>("space");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, backgroundPos1, Color.White);
			spriteBatch.Draw(texture, backgroundPos2, Color.White);
		}

		public void Update(GameTime gameTime)
		{
			// Setting speed for background scrolling
			backgroundPos1.Y = backgroundPos1.Y + speed;
			backgroundPos2.Y = backgroundPos2.Y + speed;

			// Scrolling background (Repeating)
			if (backgroundPos1.Y >= 480)
			{
				backgroundPos1.Y = 0;
				backgroundPos2.Y = -480;
			}
		}
	}
}
