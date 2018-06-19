using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class ShotManager
	{
		private readonly Texture2D texturePLS;
		private readonly Texture2D textureES;
		private readonly Rectangle bounds;
		private readonly SoundManager soundManager;
		private List<Shot> enemyShots = new List<Shot>();
		private List<Shot> playerShots = new List<Shot>();

		private IEnumerable<Shot> AllShots
		{
			get { return enemyShots.Union(playerShots); }
		}
		public ShotManager(Texture2D texturePLS, Texture2D textureES, Rectangle bounds, SoundManager soundManager)
		{
			this.texturePLS = texturePLS;
			this.textureES = textureES;
			this.bounds = bounds;
			this.soundManager = soundManager;
		}

		public IList<Shot> EnemyShots
		{
			get { return enemyShots; }
		}

		public IList<Shot> PlayerShots
		{
			get { return playerShots; }
		}

		public void Update(GameTime gameTime)
		{
			foreach (var shot in AllShots)	
				shot.Update(gameTime);

			for (int i = 0; i < enemyShots.Count; i++)
			{
				if (!bounds.Contains(enemyShots[i].BoundingBox))
					enemyShots.Remove(enemyShots[i]);
			}

			for (int i = 0; i < playerShots.Count; i++)
			{
				if (!bounds.Contains(playerShots[i].BoundingBox))
					playerShots.Remove(playerShots[i]);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var shot in AllShots)
				shot.Draw(spriteBatch);
		}

		public void FireShot(Texture2D texture, Vector2 shotPosition, int yDirection, List<Shot> shotList)
		{
			var inflateBounds = bounds;
			inflateBounds.Inflate(10, 10);
			var shot = new Shot(texture, shotPosition, inflateBounds);
			shot.Velocity = new Vector2(0, yDirection);
			shotList.Add(shot);
			soundManager.PlayShotSound();
		}

		public void FirePlayerShot(Vector2 shotPosition)
		{
			FireShot(texturePLS, shotPosition, -1, playerShots);
		}

		public void FireEnemyShot(Vector2 shotPosition)
		{
			FireShot(textureES, shotPosition, 1, enemyShots);
		}

		public void RemovePlayerShot(Shot shot)
		{
			playerShots.Remove(shot);
		}

		public void RemoveEnemyShots(Shot shot)
		{
			enemyShots.Remove(shot);
		}
	}
}