using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders;
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
        private Texture2D animationSheet;
        private Vector2 position = new Vector2();
        private AnimManager walkAnimationManager;
        private List<Rectangle> animationRects = new List<Rectangle>();
        private int frameWidth;
        private int frameHeight;
        private Rectangle currentRectangle;



        public Rectangle CurrentFrame { get { return currentRectangle; } }

        public Texture2D AnimationTextures { get { return animationSheet; } }

        #endregion







        public Character(Game1 game) 
        {
            animationSheet = game.Content.Load<Texture2D>("Boy_Adventure D");
            frameWidth = animationSheet.Width / 8;
            frameHeight = animationSheet.Height / 12;
            //12 is the number of animations in the walking animation
            for (int i = 0; i < 6; i++) 
            {
                animationRects.Add(new Rectangle(i * frameWidth, frameHeight, frameWidth, frameHeight));
            }
            walkAnimationManager = new AnimManager();
            walkAnimationManager.Add("walk", animationRects);
        }

        public Rectangle UpdateAnimations(GameTime gameTime)
        {
            currentRectangle = walkAnimationManager.Update(gameTime);
            return currentRectangle;
        }





    }
}
