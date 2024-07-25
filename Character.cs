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
using System.Drawing.Text;

namespace COOKING_GAME
{
    internal class Character
    {
        #region fields and properties
        private float defaultAccceleration = 9f;
        float currentSpeed = 0;
        private int defaultWalkSpeed = 100;
        private Texture2D animationSheet;
        private Vector2 position = new Vector2();
        private AnimManager animationManager;
        private List<Rectangle> animationRects = new List<Rectangle>();
        private int frameWidth;
        private string directionOfLastMovement;
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
            animationManager = new AnimManager(100);
            animationSheet = game.Content.Load<Texture2D>("walk");
            frameWidth = animationSheet.Width / 8;
            frameHeight = animationSheet.Height / 4;
            //12 is the number of animations in the walking animation

            //walking animations
            for (int n = 0; n < 4; n++)
            {
                for (int i = 0; i < 8; i++)
                {
                    animationRects.Add(new Rectangle(i * frameWidth, (n) * frameHeight, frameWidth, frameHeight));
                }
                switch (n)
                {
                    case 0:
                        animationName = "walkRight";
                        break;
                    case 1:
                        animationName = "walkLeft";
                        break;
                    case 2:
                        animationName = "walkDown";
                        break;
                    case 3:
                        animationName = "walkUp";
                        break;
                }
                animationManager.Add(animationName, new List<Rectangle>(animationRects));
                animationRects.Clear();
            }

            animationSheet = game.Content.Load<Texture2D>("idle");
            frameHeight = animationSheet.Height / 4;
            frameWidth = animationSheet.Width / 4;
            for (int n = 0; n < 4; n++) 
            {
                for (int i = 0; i < 4; i++) {
                    animationRects.Add(new Rectangle(i * frameWidth, (n) * frameHeight, frameWidth, frameHeight));
                }

                switch (n) {
                    case 0:
                        animationName = "idleRight";
                        break;
                    case 1:
                        animationName = "idleLeft";
                        break;
                    case 2:
                        animationName = "idleDown";
                        break;
                    case 3:
                        animationName = "idleUp";
                        break;
                }
                animationManager.Add(animationName, new List<Rectangle>(animationRects));
                animationRects.Clear();
            }

            animationManager.ChangeAnimation("idleRight");
        }
        #endregion



        #region methods

        public void Update(GameTime gameTime, KeyboardState kstate, Game1 game)
        {
            AccelerateInDirection(kstate, gameTime);
            SmoothTurningDirections(kstate, gameTime);
            UpdateAnimations(kstate, gameTime, game);
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
            float accelerationFactor = 0.1f;
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //if key is down and char is moving slower than default speed then speed them up
            if (keyState.IsKeyDown(Keys.W))
            {
                directionOfLastMovement = "north";
                currentSpeed += accelerationFactor;
                accelerationFactor += 0.1f;
                velocity.Y = -currentSpeed; 
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                directionOfLastMovement = "south";
                currentSpeed += accelerationFactor;
                accelerationFactor += 0.1f;
                velocity.Y = currentSpeed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                directionOfLastMovement = "west";
                currentSpeed += accelerationFactor;
                accelerationFactor += 0.1f;
                velocity.X = -currentSpeed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                directionOfLastMovement = "east";
                currentSpeed += accelerationFactor;
                accelerationFactor += 0.1f;
                velocity.X = currentSpeed;
            }
            if (velocity != Vector2.Zero) 
            {
                velocity.Normalize();
            }
            if (currentSpeed > defaultWalkSpeed) 
            {
                currentSpeed = defaultWalkSpeed;
            }
            position += velocity * 1.17f;
            //if all keys are not pressed apply friction when moving
            if (!keyState.IsKeyDown(Keys.S) && !keyState.IsKeyDown(Keys.W) && velocity.Y != 0)
            {
                velocity.Y =0;
            }
            if (!keyState.IsKeyDown(Keys.A) && !keyState.IsKeyDown(Keys.D) && velocity.X != 0)
            {
                velocity.X =0;
            }
            if (!keyState.IsKeyDown(Keys.A) && !keyState.IsKeyDown(Keys.D) && !keyState.IsKeyDown(Keys.S) && !keyState.IsKeyDown(Keys.W)) 
            {
                currentSpeed -= currentSpeed /3 * accelerationFactor;
                accelerationFactor -= 1f;
            }
        }

        public Rectangle UpdateAnimations(KeyboardState kstate, GameTime gameTime, Game1 game)
        {
            if (velocity == Vector2.Zero)
            {
                animationSheet = game.Content.Load<Texture2D>("idle");
            }
            else {
                animationSheet = game.Content.Load<Texture2D>("walk");
            }



            if (velocity.X > 0)
            {
                animationManager.ChangeAnimation("walkRight");
            }
            else if (velocity.X < 0)
            {
                animationManager.ChangeAnimation("walkLeft");
            }
            else if (velocity.Y > 0)
            {
                animationManager.ChangeAnimation("walkDown");
            }
            else if (velocity.Y < 0) {
                animationManager.ChangeAnimation("walkUp");
            }

            if (velocity == Vector2.Zero) { 
                switch (directionOfLastMovement) {
                    case "east":
                        animationManager.ChangeAnimation("idleRight");
                        break;
                    case "west":
                        animationManager.ChangeAnimation("idleLeft");
                        break;
                    case "north":
                        animationManager.ChangeAnimation("idleUp");
                        break;
                    case "south":
                        animationManager.ChangeAnimation("idleDown");
                        break;
                    default:
                        break;
                }
            }

            currentRectangle = animationManager.Update(gameTime);
            return currentRectangle;
        }


        #endregion


    }
}
