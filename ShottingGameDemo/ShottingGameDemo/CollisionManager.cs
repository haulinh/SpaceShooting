using System.Linq;
using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework;

namespace ShottingGameDemo
{
	public class CollisionManager
	{
		private readonly EnemyManager enemyManager;
		private readonly ExplosionManager explosionManager;
		private readonly PlayerShip playerShip;
		private readonly ShotManager shotManager;
		private AsteroidManager asteroidManager;

		public CollisionManager(PlayerShip playerShip, ShotManager shotManager, EnemyManager enemyManager, ExplosionManager explosionManager, AsteroidManager asteroidManager)
		{
			this.playerShip = playerShip;
			this.shotManager = shotManager;
			this.enemyManager = enemyManager;
			this.explosionManager = explosionManager;
			this.asteroidManager = asteroidManager;
		}

		public void Update(GameTime gameTime)
		{
			CheckCollision();
		}

		private void CheckCollision()
		{
			CheckShotToPlayer();
			CheckShotToEnemy();
			CheckAsteroidToPlayer();
			CheckShotToAsteroid();
		}

		private void CheckAsteroidToPlayer()
		{
			for (int i = 0; i < asteroidManager.Asteroids.Count(); i++)
			{
				var asteroid = asteroidManager.Asteroids[i];
				if (!playerShip.IsDead && asteroid.BoundingBox.Intersects(playerShip.BoundingBox))
				{
					playerShip.Hit();
					if (playerShip.IsDead)
						explosionManager.CreateExplosion(playerShip);
					asteroidManager.RemoveAsteroid(asteroid);
				}
			}
		}

		private void CheckShotToAsteroid()
		{
			for (int i = 0; i < shotManager.PlayerShots.Count(); i++)
			{
				var shot = shotManager.PlayerShots[i];
				foreach (var asteroid in asteroidManager.Asteroids)
				{
					if (!asteroid.IsDead && shot.BoundingBox.Intersects(asteroid.BoundingBox))
					{
						asteroid.Hit();
						if (asteroid.IsDead)
							explosionManager.CreateExplosion(asteroid);
						shotManager.RemovePlayerShot(shot);
					}
				}
			}
		}

		private void CheckShotToEnemy()
		{
			for (int i = 0; i < shotManager.PlayerShots.Count(); i++)
			{
				var shot = shotManager.PlayerShots[i];
				foreach (var enemy in enemyManager.Enemies)
				{
					if (!enemy.IsDead && shot.BoundingBox.Intersects(enemy.BoundingBox))
					{
						enemy.Hit();
						if (enemy.IsDead)
							explosionManager.CreateExplosion(enemy);
						shotManager.RemovePlayerShot(shot);
					}
				}
			}
		}

		private void CheckShotToPlayer()
		{
			for (int i = 0; i < shotManager.EnemyShots.Count(); i++)
			{
				var shot = shotManager.EnemyShots[i];
				if (!playerShip.IsDead && shot.BoundingBox.Intersects(playerShip.BoundingBox))
				{
					playerShip.Hit();
					if (playerShip.IsDead)
						explosionManager.CreateExplosion(playerShip);
					shotManager.RemoveEnemyShots(shot);
				}
			}
		}
	}
}