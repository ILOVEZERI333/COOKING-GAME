using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using COOKING_GAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace COOKING_GAME
{
    internal class Character
    {
        #region fields and properties
        private float defaultAccceleration = 9f;
        private int defaultWalkSpeed = 20;
        private Texture2D animationSheet;
        private Vector2 position = new Vector2();
        private AnimManager animationManager;
        private List<Rectangle> animationRects = new List<Rectangle>();
        private int frameWidth;
        private int frameHeight;
        private Rectangle currentRectangle;
        private Vector2 velocity = new Vector2();


        public Rectangle CurrentFrame { get { return currentRectangle; } }

        public Texture2D AnimationTextures { get { return animationSheet; } }

        public Vector2 Position { get { return position; } }

        #endregion



        #region constructor

        public Character(Game1 game) 
        {
            string animationName = "";
            animationManager = new AnimManager(400);
            animationSheet = game.Content.Load<Texture2D>("Main Character SpreadSheet");
            frameWidth = animationSheet.Width / 4;
            frameHeight = animationSheet.Height / 4;
            //12 is the number of animations in the walking animation

            //walking animations
            for (int n = 0; n < 4; n++)
            {
                for (int i = 2; i < 4; i++)
                {
                    animationRects.Add(new Rectangle(i * frameWidth, (n) * frameHeight, frameWidth, frameHeight));
                }
                switch (n)
                {
                    case 0:
                        animationName = "walkDown";
                        break;
                    case 1:
                        animationName = "walkUp";
                        break;
                    case 2:
                        animationName = "walkLeft";
                        break;
                    case 3:
                        animationName = "walkRight";
                        break;
                }
                animationManager.Add(animationName, new List<Rectangle>(animationRects));
                animationRects.Clear();
            }

            for (int n = 0; (n < 4); n++) 
            {
                for (int i = 0; i < 2; i++)
                {
                    animationRects.Add(new Rectangle(i * frameWidth,n * frameHeight, frameWidth, frameHeight));
                }
                switch (n)
                {
                    case 0:
                        animationName = "idleDown";
                        break;
                    case 1:
                        animationName = "idleUp";
                        break;
                    case 2:
                        animationName = "idleLeft";
                        break;
                    case 3:
                        animationName = "idleRight";
                        break;
                }
                animationManager.Add(animationName, new List<Rectangle>(animationRects));
                animationRects.Clear();
            }
            animationManager.ChangeAnimation("idleRight");
        }
        #endregion



        #region methods

        public void Update(GameTime gameTime, KeyboardState kstate)
        {
            AccelerateInDirection(kstate, gameTime);
            SmoothTurningDirections(kstate, gameTime);
            UpdateAnimations(gameTime);
        }


        
        private void SmoothTurningDirections(KeyboardState kstate, GameTime gameTime)
        {
            //if main character wants to move left but is moving right, instantly change velocity to the negative of itself for smoother movement
            if (kstate.IsKeyDown(Keys.W) && velocity.Y > 0) {
                velocity.Y = -velocity.Y;
            }
            if (kstate.IsKeyDown(Keys.S) && velocity.Y < 0)
            {
                velocity.Y = -velocity.Y;
            }
            if (kstate.IsKeyDown(Keys.A) && velocity.X > 0) {
                velocity.X = -velocity.X;
            }
            if (kstate.IsKeyDown(Keys.D) && velocity.X < 0) {  velocity.X = -velocity.X; }
        }

        public void AccelerateInDirection(KeyboardState keyState, GameTime gameTime) 
        {
            float currentSpeed = 0;
            float accelerationFactor = 5f;
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if key is down and char is moving slower than default speed then speed them up
            if (currentSpeed < defaultWalkSpeed)
            {
                if (keyState.IsKeyDown(Keys.W))
                {
                    currentSpeed += accelerationFactor;
                    accelerationFactor += 1f;
                    velocity.Y = -currentSpeed; 
                }
                if (keyState.IsKeyDown(Keys.S))
                {
                    currentSpeed += accelerationFactor;
                    accelerationFactor += 1f;
                    velocity.Y = currentSpeed;
                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    currentSpeed += accelerationFactor;
                    accelerationFactor += 1f;
                    velocity.X = -currentSpeed;
                }
                if (keyState.IsKeyDown(Keys.D))
                {
                    currentSpeed += accelerationFactor;
                    accelerationFactor += 1f;
                    velocity.X = currentSpeed;
                }
                if (velocity != Vector2.Zero) 
                {
                    velocity.Normalize();
                }
                position += velocity * 5;
            }
            
            //if all keys are not pressed apply friction when moving
            if (!keyState.IsKeyDown(Keys.S) && !keyState.IsKeyDown(Keys.W) && velocity.Y != 0)
            {
                
                velocity.Y -= velocity.Y * accelerationFactor * delta;
                if (velocity.Y < 0.2)
                {
                    velocity.Y = 0;
                }
            }
            if (!keyState.IsKeyDown(Keys.A) && !keyState.IsKeyDown(Keys.D) && velocity.X != 0)
            {
                velocity.X -= velocity.X * accelerationFactor * delta;
                if (velocity.X < 0.2)
                {
                    velocity.X = 0;
                }
            }
            if (!keyState.IsKeyDown(Keys.A) && !keyState.IsKeyDown(Keys.D) && !keyState.IsKeyDown(Keys.S) && !keyState.IsKeyDown(Keys.W) && (velocity.X != 0 || velocity.Y != 0)) 
            {
                currentSpeed -= accelerationFactor;
                accelerationFactor -= 1f;
            }
        }

        public Rectangle UpdateAnimations(GameTime gameTime)
        {
            currentRectangle = animationManager.Update(gameTime);
            return currentRectangle;
        }


        #endregion


    }
}
