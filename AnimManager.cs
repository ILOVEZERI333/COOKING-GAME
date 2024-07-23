using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COOKING_GAME
{
    internal class AnimManager
    {
        #region fields and properties
        private int millisecondsPerFrame;
        private int timeSinceLastFrame;
        private Texture2D spriteSheet;
        private Dictionary<string, List<Rectangle>> animations = new Dictionary<string, List<Rectangle>>();
        private List<Rectangle> sourceRectangles = new List<Rectangle>();
        private int indexer;
        private bool active = true;
        private bool animationDone;

        public bool Active { get { return active; } }
        public List<Rectangle> SourceRectangles { get { return sourceRectangles; } }
        #endregion

        #region constructor

        public AnimManager()
        {
            millisecondsPerFrame = 100;
        }

        public AnimManager(int milliPerFrame) 
        {
            millisecondsPerFrame = milliPerFrame;
        }
        #endregion

        #region methods



        public Rectangle Update(GameTime gameTime)
        {
            if (active && sourceRectangles.Count != 0) 
            {

                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (millisecondsPerFrame < timeSinceLastFrame)
                {
                    indexer++;
                    if (indexer >= sourceRectangles.Count)
                    {
                        indexer = 0;
                    }
                    timeSinceLastFrame = 0;
                }

                try
                {
                    Debug.WriteLine($"Frame Index: {indexer}, Total Frames: {sourceRectangles.Count}");
                    return sourceRectangles[indexer];
                }
                catch (System.ArgumentOutOfRangeException) {
                    indexer = 0;
                    return sourceRectangles[indexer];
                }
                
            }
            else
                //return a rectangle that is basically nothing (null makes .Draw method draw entire sprite)
                return new Rectangle(0,0,1,1);
            
        }

        public void ChangeAnimation(string animationName) 
        {
            if (animations.ContainsKey(animationName)) 
            {
                sourceRectangles = animations[animationName];
                Debug.WriteLine($"Changed Animation to: {animationName}");
            }
        }

        public void Add(string animationName,List<Rectangle> rectangles)
        {
            animations.Add(animationName, rectangles);
        }

        public void Start()
        {
            active = true;
        }

        public void Stop()
        {
            active = false;
        }
        #endregion

    }
}
