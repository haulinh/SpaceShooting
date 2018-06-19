using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class EnemyManager
	{
		private readonly Rectangle bounds;
		private readonly Texture2D texture;
		private readonly ShotManager shotManager;

		List<Enemy> enemies = new List<Enemy>();

		public EnemyManager(Texture2D texture, Rectangle bounds, ShotManager shotManager)
		{
			this.texture = texture;
			this.bounds = bounds;
			this.shotManager = shotManager;

			CreateEnemy();
		}

		public IList<Enemy> Enemies
		{
			get { return enemies; }
		}

		private void CreateEnemy()
		{
			var position = RandomPosition();
			var enemy = new Enemy(texture, position, bounds, shotManager);
			enemies.Add(enemy);
		}

		private Vector2 RandomPosition()
		{
			var random = new Random();
			var xPosition = random.Next(bounds.Width - texture.Width + 1);
			return new Vector2(xPosition, 20);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var enemy in enemies)
			{
				enemy.Draw(spriteBatch);
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < enemies.Count; i++)
			{
				if (enemies[i].IsDead)
				{
					enemies.Remove(enemies[i]);
						CreateEnemy();
				}
				else
					enemies[i].Update(gameTime);
			}
		}

		public int GetKillCount()
		{
			return enemies.Where(e => e.IsDead).Count();
		}
	}
}