using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShottingGameDemo
{ 
	public class Sprite
	{
		private Vector2 position;
		private readonly Texture2D texture;
		private readonly Rectangle movementBounds;
		private readonly int rows;
		private readonly int columns;
		private readonly double framesPerSecond;
		private readonly int totalFrames;
		private double timeSinceLastFrame;
		private int currentFrame;
		protected bool animationPlayedOnce;

		public Sprite(Texture2D texture, Vector2 position, Rectangle movementBounds) : this(texture, position, movementBounds, 1, 1, 1)
		{
			
		}

		public Sprite(Texture2D texture, Vector2 position, Rectangle movementBounds, int rows, int columns, double framesPerSecond)
		{
			this.position = position;
			this.movementBounds = movementBounds;
			this.rows = rows;
			this.columns = columns;
			this.framesPerSecond = framesPerSecond;
			this.texture = texture;
			totalFrames = rows * columns;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var imageWith = texture.Width / columns;
			var imageHeight = texture.Height / rows;

			var currentRow = currentFrame / columns;
			var currentColumn = currentFrame % columns;

			var sourceRectangle = new Rectangle(imageWith*currentColumn, imageHeight*currentRow, imageWith, imageHeight);

			var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, imageWith, imageHeight);

			spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
		}

		public virtual void Update(GameTime gameTime)
		{
			UpdateAnimation(gameTime);

			var newPosition = position + (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed);
			if (Blocked(newPosition))
				return;

			position = newPosition;
		}

		private void UpdateAnimation(GameTime gameTime)
		{
			timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
			if (timeSinceLastFrame > SecondsBetweenFrames())
			{
				currentFrame++;
				timeSinceLastFrame = 0;
			}

			if (currentFrame == totalFrames)
			{
				currentFrame = 0;
				animationPlayedOnce = true;
			}
		}

		private double SecondsBetweenFrames()
		{
			return 1 / framesPerSecond;
		}

		private bool Blocked(Vector2 newPositin)
		{
			var boundingBox = CreateBoundingBoxFromPosition(newPositin);
			return !movementBounds.Contains(boundingBox);
		}

		private Rectangle CreateBoundingBoxFromPosition(Vector2 position) 
		{
			return new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);
		}
																																																																						
		public int Height => texture.Height / columns;           

		public int Width => texture.Width / rows;

		public float Speed { get; set; }

		public Vector2 Velocity { get; set; }

		public Rectangle BoundingBox => CreateBoundingBoxFromPosition(position);

		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}
	}
}