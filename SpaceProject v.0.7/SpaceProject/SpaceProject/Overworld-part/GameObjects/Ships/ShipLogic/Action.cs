using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    abstract public class ShipAction
    {
        public virtual void Update(GameTime gameTime) { }
        public virtual void Reset() { }
        public bool Finished = false;
    }

    abstract public class CompositeAction : ShipAction
    {
        public void Add(ShipAction a)
        {
            Actions.Add(a);
        }
        protected List<ShipAction> Actions = new List<ShipAction>();
    }

    // Perform several Actions in parallel
    public class ParallelAction : CompositeAction
    {
        public override void Update(GameTime gameTime)
        {
            foreach (ShipAction a in Actions)
            {
                a.Update(gameTime);
            }
            Actions.RemoveAll(a => a.Finished);
            Finished = Actions.Count == 0;
        }
    }

    // Performs several Actions in sequense  
    public class SequentialAction : CompositeAction
    {
        public override void Update(GameTime gameTime)
        {
            if (Actions.Count > 0)
            {
                Actions[0].Update(gameTime);
                if (Actions[0].Finished)
                    Actions.RemoveAt(0);
            }
            Finished = Actions.Count == 0;
        }
    }

    // Chose which action to execute from a prioritised list
    public class PriorityAction : CompositeAction
    {
        public override void Update(GameTime gameTime)
        {
            foreach (ShipAction a in Actions)
            {
                a.Update(gameTime);
                if (a.Finished)
                    break;
            }
        }
    }

    // Create a sequence of Action that will be repeted over and over. 
    public class LoopAction : CompositeAction
    {
        private int loopindex;

        public LoopAction() { loopindex = 0; }

        public override void Update(GameTime gameTime)
        {
            if (Actions.Count != 0)
            {
                Actions[loopindex].Update(gameTime);
                if (Actions[loopindex].Finished)
                {
                    Actions[loopindex].Reset();
                    loopindex++;
                    if (loopindex > Actions.Count - 1)
                        loopindex = 0;
                }   
            }
        }
    }
}
