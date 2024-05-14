using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidsShooting
{
    enum StateScene
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        StateScene Scene = StateScene.SplashScreen;
        KeyboardState keyboardGetState, oldKeyboardState = Keyboard.GetState();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.Background = Content.Load<Texture2D>("SpaceBackground");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashScreenFont");
            Asteroids.Initialize(_spriteBatch, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Stars.Texture2D = Content.Load<Texture2D>("Star");
            SpaceShip.Texture2D = Content.Load<Texture2D>("SpaceShip");
            FireShot.Texture2D = Content.Load<Texture2D>("Fire");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardGetState = Keyboard.GetState();
            switch (Scene)
            {
                case StateScene.SplashScreen:
                    SplashScreen.UpdateColor();
                    if (KeyCheck(Keys.Space))
                        Scene = StateScene.Game;
                    break;
                case StateScene.Game:
                    Asteroids.Update();
                    if (KeyCheck(Keys.Q))
                        Scene = StateScene.SplashScreen;
                    if (KeyCheck(Keys.Up) || KeyCheck(Keys.W)) 
                        Asteroids.SpaceShip.MoveUp();
                    if (KeyCheck(Keys.Down) || KeyCheck(Keys.S)) 
                        Asteroids.SpaceShip.MoveDown();
                    if (KeyCheck(Keys.Left) || KeyCheck(Keys.A)) 
                        Asteroids.SpaceShip.MoveLeft();
                    if (KeyCheck(Keys.Right) || KeyCheck(Keys.D)) 
                        Asteroids.SpaceShip.MoveRight();
                    if (KeyCheck(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
                        Asteroids.SpaceShipFire();
                        break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || KeyCheck(Keys.Escape))
                Exit();

            oldKeyboardState = keyboardGetState;

            base.Update(gameTime);
        }

        public bool KeyCheck(Keys key) => keyboardGetState.IsKeyDown(key);

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            switch (Scene)
            {
                case StateScene.SplashScreen:
                    SplashScreen.Draw(_spriteBatch);
                    break;
                case StateScene.Game:
                    Asteroids.Draw();
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
