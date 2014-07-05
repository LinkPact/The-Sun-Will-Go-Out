using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class EscortDataCapsule
    {
        private OverworldShip shipToDefend;
        public OverworldShip ShipToDefend { get { return shipToDefend; } set { shipToDefend = value; } }

        private List<String> shipToDefendText;
        public List<String> ShipToDefendText { get { return shipToDefendText; } set { shipToDefendText = value; } }

        private List<OverworldShip> enemyShips;
        public List<OverworldShip> EnemyShips { get { return enemyShips; } set { enemyShips = value; } }

        private List<String> enemyMessages;
        public List<String> EnemyMessages { get { return enemyMessages; } set { enemyMessages = value; } }

        private List<OverworldShip> allyShips;
        public List<OverworldShip> AllyShips { get { return allyShips; } set { allyShips = value; } }

        private Vector2 startingPoint;
        public Vector2 StartingPoint { get { return startingPoint; } set { startingPoint = value; } }

        private int enemyAttackStartTime;
        public int EnemyAttackStartTime { get { return enemyAttackStartTime; } set { enemyAttackStartTime = value; } }

        private int enemyAttackFrequency;
        public int EnemyAttackFrequency { get { return enemyAttackFrequency; } set { enemyAttackFrequency = value; } }

        public EscortDataCapsule(OverworldShip shipToDefend, List<String> shipToDefendText, List<OverworldShip> enemyShips,
            List<String> enemyMessages, List<OverworldShip> allyShips, Vector2 startingPoint,
            int enemyAttackStartTime, int enemyAttackFrequency)
        {
            this.shipToDefend = shipToDefend;
            this.shipToDefendText = shipToDefendText;
            this.enemyShips = enemyShips;
            this.enemyMessages = enemyMessages;
            this.allyShips = allyShips;
            this.startingPoint = startingPoint;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;

            if (this.enemyShips == null)
            {
                this.enemyShips = new List<OverworldShip>();
            }

            if (this.allyShips == null)
            {
                this.allyShips = new List<OverworldShip>();
            }

            if (this.enemyMessages == null)
            {
                this.enemyMessages = new List<String>();
            }
        }
    }
}
