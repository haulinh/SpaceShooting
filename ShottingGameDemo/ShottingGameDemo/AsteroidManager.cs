using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class AsteroidManager
	{
		private readonly Texture2D texture;
		private readonly Rectangle bounds;
		private readonly SoundManager soundManager;
		private List<Asteroid> asteroids = new List<Asteroid>();

		public AsteroidManager(Texture2D texture, Rectangle bounds)
		{
			this.texture = texture;
			this.bounds = bounds;
		
			CreateAsteroid();
		}

		public IList<Asteroid> Asteroids
		{
			get { return asteroids; }
		}

		private void CreateAsteroid()
		{
			var random = new Random();
			var inflateBounds = bounds;
			inflateBounds.Inflate(5, 5);
			Vector2 position = RandomPosition();
			for (int i = 0; i < 10; i++)
			{
				var asteroidx = new Asteroid(texture, new Vector2(i  * random.Next(100, 150), random.Next(200)), inflateBounds);
				asteroids.Add(asteroidx);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var asteroid in asteroids)
			{
				asteroid.Draw(spriteBatch);
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < asteroids.Count; i++)
			{
				if (!bounds.Contains(asteroids[i].BoundingBox))
					asteroids.Remove(asteroids[i]);
			}

			for (int i = 0; i < asteroids.Count; i++)
			{
				if (asteroids[i].IsDead)
					asteroids.Remove(asteroids[i]);
			}

			foreach (var asteroid in asteroids)
			{
				asteroid.Update(gameTime);
			}

			if (!asteroids.Any())
				CreateAsteroid();
		}

		private Vector2 RandomPosition()
		{
			var random = new Random();
			var xPosition = random.Next(10);
			return new Vector2(xPosition, 10);
		}

		public void RemoveAsteroid(Asteroid asteroid)
		{
			asteroids.Remove(asteroid);
		}
	}
}
