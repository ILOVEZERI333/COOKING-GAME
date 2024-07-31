using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace COOKING_GAME
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Character mainCharacter;
        private Texture2D map;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mainCharacter = new Character(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            map = Content.Load<Texture2D>("KITCHEN");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mainCharacter.Update(gameTime, kstate, this);


            


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);






            try
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Matrix.CreateScale(2.8f),sortMode: SpriteSortMode.FrontToBack);
                _spriteBatch.Draw(mainCharacter.AnimationTextures, mainCharacter.Position, mainCharacter.CurrentFrame, Color.White, 0f, new Vector2(0,0), new Vector2(4,4),default,0.2f);
                _spriteBatch.Draw(map, new Vector2(0,0), null,Color.White, 0f, new Vector2(0,0), new Vector2(3.6f,3.3f), default, 0.1f);
            }
            finally 
            {
             _spriteBatch?.End();
            }
            


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
