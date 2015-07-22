using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Snodd fran forelasning
    public class Animation
    {
        #region declaration

        private List<Sprite> frames = new List<Sprite>();
        public int currentIndex { get; set; }
        public int lastTimeChanged { get; set; }

        public bool PlaysOnce = false;
        public bool HasEnded { get; private set; }

        public int LoopTime { get; set; }
        public int FrameTime { get { return LoopTime / frames.Count; } }
        public Sprite CurrentFrame { get { return frames[currentIndex]; } }

        #endregion

        public int Width
        {
            get
            {
                if (frames.Count > 0)
                {
                    if (frames[0].SourceRectangle != null)
                    {
                        return frames[0].SourceRectangle.Value.Width;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Height
        {
            get
            {
                if (frames.Count > 0)
                {
                    if (frames[0].SourceRectangle != null)
                    {
                        return frames[0].SourceRectangle.Value.Height;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        public Animation()
        {
            LoopTime = 0;
            currentIndex = 0;
            lastTimeChanged = 0;
        }

        public void AddFrame(Sprite frame)
        {
            frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (!HasEnded)
            {
                lastTimeChanged += gameTime.ElapsedGameTime.Milliseconds;
                if (lastTimeChanged >= FrameTime)
                {
                    currentIndex++;

                    if (currentIndex > frames.Count - 1)
                    {
                        if (!PlaysOnce)
                            currentIndex = 0;
                        else
                        {
                            HasEnded = true;
                            currentIndex -= 1;
                        }
                    }

                    lastTimeChanged = 0;
                }
            }
        }

    }
}
