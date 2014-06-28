using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class SetupCreature
    {
        public float speedFactor;
        public float HPFactor;
        public float shootDelayFactor;
        public Movement newMovement;

        public SetupCreature()
        {
            speedFactor = 1f;
            HPFactor = 1f;
            shootDelayFactor = 1f;
            newMovement = Movement.None;
        }

        public void SetSpeedFactor(float speedFactor)
        {
            this.speedFactor = speedFactor;
        }

        public void SetHPFactor(float HPFactor)
        {
            this.HPFactor = HPFactor;
        }

        public void SetShootDelayFactor(float delayFactor)
        {
            this.shootDelayFactor = delayFactor;
        }

        public void SetMovement(Movement movement)
        {
            newMovement = movement;
        }
    }
}
