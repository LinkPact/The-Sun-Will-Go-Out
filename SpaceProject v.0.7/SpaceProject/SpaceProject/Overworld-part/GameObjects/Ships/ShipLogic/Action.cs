using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ShipAction
    {
        public virtual void Update(GameTime gameTime) { }
        public bool Finished;
    }

    public class CompositeAction : ShipAction
    {
        public void Add(ShipAction a)
        {
            Actions.Add(a);
        }
        protected List<ShipAction> Actions;
    }

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
}
