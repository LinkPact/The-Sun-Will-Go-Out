using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    abstract public class ShipAction
    {
        public virtual void Update(GameTime gameTime) { }
        public bool Finished;
    }

    abstract public class CompositeAction : ShipAction
    {
        public void Add(ShipAction a)
        {
            Actions.Add(a);
        }
        protected List<ShipAction> Actions;
    }

    abstract public class ParallelAction : CompositeAction
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

    abstract public class SequentialAction : CompositeAction
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

    abstract public class PriorityAction : CompositeAction
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
}
