using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using COOKING_GAME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COOKING_GAME
{
    internal class Character
    {
        #region fields and properties
        private int defaultAccceleration = 3;
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

        #endregion





        #region constructor

        public Character(Game1 game) 
        {
            string animationName = "";
            animationManager = new AnimManager();
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
                animationManager.Add(animationName, animationRects);
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
                animationManager.Add(animationName, animationRects);
                animationRects.Clear();
            }
            animationManager.ChangeAnimation("idleRight");
        }
        #endregion



        #region methods

        public void Update(GameTime gameTime)
        {
            MoveCharacter();
            UpdateAnimations(gameTime);
        }

        public void AccelerateInDirection(KeyboardState keyState) 
        {
            if (keyState.IsKeyDown(Keys.W)) 
            {
                velocity += new Vector2(0, -1) * defaultAccceleration;
            }
            if (keyState.IsKeyDown(Keys.S)) 
            {
                velocity += new Vector2(0, 1) * defaultAccceleration;
            }
            if (keyState.IsKeyDown(Keys.A)) 
            {
                velocity += new Vector2(-1, 0) * defaultAccceleration;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                velocity += new Vector2(1, 0) * defaultAccceleration;
            }
        }

        public Rectangle UpdateAnimations(GameTime gameTime)
        {
            currentRectangle = animationManager.Update(gameTime);
            return currentRectangle;
        }

        private void MoveCharacter() 
        {
            position += velocity;
        }
        #endregion


    }
}
