using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShottingGameDemo
{
	public class PlayerShip : Sprite
	{
		private readonly ShotManager shotManager;
		private MouseState previousMousePostition;
		private const double TimeBetweenShotsInSecond = 0.2;
		private  double timeSinceLastFireSeconds = 0;	


		public PlayerShip(Texture2D texture, Vector2 position, Rectangle movementBounds, ShotManager shotManager)
			: base(texture, position, movementBounds, 2, 2, 14)
		{
			this.shotManager = shotManager;
			Speed = 300;
		}

		private bool CanFireShot()
		{
			return timeSinceLastFireSeconds > TimeBetweenShotsInSecond;
		}

		private void CheckForShotFromKeyboard(KeyboardState keyboardState)
		{
			if (keyboardState.IsKeyDown(Keys.Space) && CanFireShot() && !IsDead)
			{
				shotManager.FirePlayerShot(CalculateShotPosition());
				timeSinceLastFireSeconds = 0;
			}
		}

		private void UpdateVelocityFromMouse()
		{
			var velocity = new Vector2(Mouse.GetState().X - previousMousePostition.X,
				Mouse.GetState().Y - previousMousePostition.Y);

			if (velocity != Vector2.Zero)
				velocity.Normalize();

			Velocity = velocity;

			previousMousePostition = Mouse.GetState();
		}

		private void UpdateVelocityFromKeyboard(KeyboardState keyboardState)
		{
			var keyDitionary = new Dictionary<Keys, Vector2>
			{
				{Keys.Left, new Vector2(-1, 0)},
				{Keys.Right, new Vector2(1, 0)},
				{Keys.Up, new Vector2(0, -1)},
				{Keys.Down, new Vector2(0, 1)},
			};

			var velocity = Vector2.Zero;

			foreach (var key in keyDitionary)
			{
				if (keyboardState.IsKeyDown(key.Key))
					velocity += key.Value;
			}


			if (velocity != Vector2.Zero)
				velocity.Normalize();

			Velocity = velocity;
		}

		private void HandleKeybroadInput()
		{
			var keyboardState = Keyboard.GetState();
			UpdateVelocityFromKeyboard(keyboardState);
			CheckForShotFromKeyboard(keyboardState);
		}

		public override void Update(GameTime gameTime)
		{
			timeSinceLastFireSeconds += gameTime.ElapsedGameTime.TotalSeconds;
			HandleKeybroadInput();
			base.Update(gameTime);
		}

		private Vector2 CalculateShotPosition()
		{
			return Position + new Vector2((Width / 2) - 2, -30);
		}

		public void Hit()
		{
			IsDead = true;
		}

		public bool IsDead { get; set; }
	}
}