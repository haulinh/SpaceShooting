using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class ExplosionManager
	{
		private Rectangle bounds;
		private readonly SoundManager soundManager;
		private Texture2D texture;
		private List<Explosion> explosions = new List<Explosion>();

		public ExplosionManager(Texture2D texture, Rectangle bounds, SoundManager soundManager)
		{
			this.texture = texture;
			this.bounds = bounds;
			this.soundManager = soundManager;
		}

		public void CreateExplosion(Sprite sprite)
		{
			var centerOfSprite = new Vector2(sprite.Position.X + (sprite.Width / 2), sprite.Position.Y + (sprite.Height / 2));
			var explosion = new Explosion(texture, centerOfSprite, bounds);
			explosion.Position -= new Vector2(explosion.Width / 2, explosion.Height / 2);
			explosions.Add(explosion);
			soundManager.PlayExplosionSound();
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < explosions.Count; i++)
			{
				explosions[i].Update(gameTime);
				if (explosions[i].IsDone())
					explosions.Remove(explosions[i]);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var explosion in explosions)
			{
				explosion.Draw(spriteBatch);
			}
		}
	}
}