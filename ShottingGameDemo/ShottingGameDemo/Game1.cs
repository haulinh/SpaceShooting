using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShottingGameDemo
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private Sprite titleScreen;
		//private Sprite background;
		private StarField statField = new StarField();
		private PlayerShip playerShip;
		private SpriteFont gameFont;
		private Sprite pauseScreen;
		private Sprite gameOverScreen;
		private EnemyManager enemyManager;
		private ShotManager shotManager;
		private CollisionManager collisionManager;
		private ExplosionManager explosionManager;
		private StarField starField;
		private SoundManager soundManager;
		private StatusManager statusManager;
		private AsteroidManager asteroidManager;

		private GameState gameState;

		private bool atTitleScreen = true;

		
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perFrom any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			gameState = new TitleScreenState(this);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			titleScreen = new Sprite(Content.Load<Texture2D>("title"), Vector2.Zero, graphics.GraphicsDevice.Viewport.Bounds, 2, 2, 8);
			//background = new Sprite(Content.Load<Texture2D>("spacebackground"), Vector2.Zero, graphics.GraphicsDevice.Viewport.Bounds);
			statField.LoadContent(Content);

			pauseScreen = new Sprite(Content.Load<Texture2D>("paused"), Vector2.Zero, graphics.GraphicsDevice.Viewport.Bounds);
			gameOverScreen = new Sprite(Content.Load<Texture2D>("gameOver"), Vector2.Zero, graphics.GraphicsDevice.Viewport.Bounds);

			soundManager = new SoundManager(Content);

			var shipTexture = Content.Load<Texture2D>("playership");
			var yPositionOfShip = graphics.GraphicsDevice.Viewport.Height - shipTexture.Height - 10;
			var xPositionOfShip = (graphics.GraphicsDevice.Viewport.Width / 2) - (shipTexture.Width / 2);
			var playerBound = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height - 200,
				graphics.GraphicsDevice.Viewport.Width, 200);

			shotManager = new ShotManager(Content.Load<Texture2D>("playershipshot"), Content.Load<Texture2D>("enemyshot"), graphics.GraphicsDevice.Viewport.Bounds, soundManager);

			playerShip = new PlayerShip(shipTexture, new Vector2(xPositionOfShip, yPositionOfShip), playerBound, shotManager);
			
			enemyManager = new EnemyManager(Content.Load<Texture2D>("alien"), graphics.GraphicsDevice.Viewport.Bounds, shotManager);

			explosionManager = new ExplosionManager(Content.Load<Texture2D>("explosion1"), graphics.GraphicsDevice.Viewport.Bounds, soundManager);

			asteroidManager = new AsteroidManager(Content.Load<Texture2D>("rocks_rotated_small"), graphics.GraphicsDevice.Viewport.Bounds);

			collisionManager = new CollisionManager(playerShip, shotManager, enemyManager, explosionManager, asteroidManager);

			gameFont = Content.Load<SpriteFont>("GameFont");
			statusManager = new StatusManager(gameFont, graphics.GraphicsDevice.Viewport.Bounds, enemyManager, shipTexture)
			{
				Lives = 3,
				Score = 0
			};

			soundManager.PlayBackgroundMusic();

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			currentKeyboardState = Keyboard.GetState(PlayerIndex.One);
			gameState.Update(gameTime);
			previousKeyboardState = currentKeyboardState;
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			//background.Draw(spriteBatch);

			gameState.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public class TitleScreenState : GameState
		{
			public TitleScreenState(Game1 game) : base(game)
			{
			}

			public override void Update(GameTime gameTime)
			{
				game.titleScreen.Update(gameTime);
				if (game.currentKeyboardState.IsKeyDown(Keys.A) && !game.previousKeyboardState.IsKeyDown(Keys.A))
					game.gameState = new PlayingState(game);
			}

			public override void Draw(SpriteBatch spriteBatch)
			{
				game.titleScreen.Draw(spriteBatch);
			}
		}

		public class PauseState : PlayingState
		{
			public PauseState(Game1 game1) : base(game1)
			{
			}

			public override void Update(GameTime gameTime)
			{
				game.pauseScreen.Update(gameTime);
				if (game.currentKeyboardState.IsKeyDown(Keys.P) && !game.previousKeyboardState.IsKeyDown(Keys.P))
					game.gameState = new PlayingState(game);
			}

			public override void Draw(SpriteBatch spriteBatch)
			{
				base.Draw(spriteBatch);
				game.pauseScreen.Draw(spriteBatch);
			}
		}

		public class GameOverState : PlayingState
		{
			public GameOverState(Game1 game1) : base(game1)
			{
			}

			public override void Update(GameTime gameTime)
			{
				base.Update(gameTime);
				game.gameOverScreen.Update(gameTime);
				if (game.currentKeyboardState.IsKeyDown(Keys.P) || game.currentKeyboardState.IsKeyDown(Keys.A))
				{
					game.LoadContent();
					game.gameState = new TitleScreenState(game);
				}
			}

			public override void Draw(SpriteBatch spriteBatch)
			{
				base.Draw(spriteBatch);
				game.gameOverScreen.Draw(spriteBatch);
			}
		}


		public class PlayingState : GameState
		{
			public PlayingState(Game1 game1) : base(game1)
			{

			}

			public override void Update(GameTime gameTime)
			{
				game.playerShip.Update(gameTime);
				game.enemyManager.Update(gameTime);
				game.shotManager.Update(gameTime);
				game.explosionManager.Update(gameTime);
				game.statField.Update(gameTime);
				game.asteroidManager.Update(gameTime);
				
				
				game.collisionManager.Update(gameTime);
				game.statusManager.UpdateScore();


				if (game.currentKeyboardState.IsKeyDown(Keys.P) && !game.previousKeyboardState.IsKeyDown(Keys.P))
					game.gameState = new PauseState(game);

				if (game.playerShip.IsDead)
				{
					game.statusManager.Lives--;
					if (game.statusManager.Lives < 1)
						game.gameState = new GameOverState(game);
					else
					{
						game.playerShip.IsDead = false;
					}
				}
			}

			public override void Draw(SpriteBatch spriteBatch)
			{
				game.statField.Draw(spriteBatch);
				if (!game.playerShip.IsDead)
					game.playerShip.Draw(spriteBatch);
				game.statusManager.Draw(spriteBatch);
				game.enemyManager.Draw(spriteBatch);
				game.shotManager.Draw(spriteBatch);
				game.explosionManager.Draw(spriteBatch);
				game.asteroidManager.Draw(spriteBatch);
			}
		}
	}

	public class StatusManager
	{
		private SpriteFont gameFont;
		private readonly Rectangle bounds;
		private readonly EnemyManager enemyManager;
		private readonly Texture2D lifeTexture;
		public int Score { get; set; }
		public int Lives { get; set; }

		public StatusManager(SpriteFont gameFont, Rectangle bounds, EnemyManager enemyManager, Texture2D lifeTexture)
		{
			this.gameFont = gameFont;
			this.bounds = bounds;
			this.enemyManager = enemyManager;
			this.lifeTexture = lifeTexture;
		}

		public void UpdateScore()
		{
			var kills = enemyManager.GetKillCount();
			Score += kills * 1000;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var scale = .60f;
			for (int i = 0; i < Lives + 1; i++)
			{
				var xPosition = (lifeTexture.Width / 2) * scale * (i - 1);
				spriteBatch.Draw(lifeTexture, new Vector2(xPosition, 10),
					new Rectangle(0, 0, lifeTexture.Width / 2, lifeTexture.Height / 2), 
					Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
			}

			DrawScore(spriteBatch);
		}

		private void DrawScore(SpriteBatch spriteBatch)
		{
			var scoreText = string.Format("Score: {0}", Score);
			var scoreDimensions = gameFont.MeasureString(scoreText);

			var scoreX = bounds.Width - scoreDimensions.X - 10;
			var scoreY = 0 + 0 + 5;

			spriteBatch.DrawString(gameFont, scoreText, new Vector2(scoreX, scoreY), Color.White);
		}
	}

	public abstract class GameState
	{
		protected readonly Game1 game;

		public GameState(Game1 game)
		{
			this.game = game;
		}

		public abstract void Update(GameTime gameTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}
