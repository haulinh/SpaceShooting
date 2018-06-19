using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShottingGameDemo
{
	public class Enemy : Sprite
	{
		private readonly ShotManager shotManager;
		private double timeSinceLastShot;
		private const double ShotDelay = 0.2;

		public Enemy(Texture2D texture, Vector2 position, Rectangle bounds, ShotManager shotManager) : base(texture, position, bounds, 2, 2, 14)
		{
			this.shotManager = shotManager;
			Speed = 200;
		}

		public override void Update(GameTime gameTime)
		{
			var random = new Random();
			if (Velocity == Vector2.Zero)
			{
				var dicection = random.Next(2);
				Velocity = new Vector2(dicection == 0 ? -1 : 1, 0); 
			}
			else if (gameTime.TotalGameTime.Seconds % 2 == 0)
			{
				if (random.Next(2) == 0)
					Velocity = new Vector2(-Velocity.X, Velocity.Y);
			}

			timeSinceLastShot += gameTime.ElapsedGameTime.TotalSeconds;
			if (timeSinceLastShot > ShotDelay)
			{
				if (random.Next(2) == 0)
					shotManager.FireEnemyShot(CalculateShotPosition());
				timeSinceLastShot = 0;
			}

			base.Update(gameTime);
		}

		private Vector2 CalculateShotPosition()
		{
			return Position + new Vector2(Width / 2 , Height - 30);
		}

		public void Hit()
		{
			IsDead = true;
		}

		public bool IsDead { get; private set; }
	}
}