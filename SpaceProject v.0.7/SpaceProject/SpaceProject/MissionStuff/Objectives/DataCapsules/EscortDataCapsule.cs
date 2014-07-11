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

        private String shipIntroductionText;
        public String ShipIntroductionText { get { return shipIntroductionText; } set { shipIntroductionText = value; } }

        private String attackStartText;
        public String AttackStartText { get { return attackStartText; } set { attackStartText = value; } }

        private int shipToDefendHP;
        public int ShipToDefendHP { get { return shipToDefendHP; } set { shipToDefendHP = value; } }

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

        private List<String> levels;
        public List<String> Levels { get { return levels; } set { levels = value; } }

        public EscortDataCapsule(OverworldShip shipToDefend, String shipIntroductionText, List<OverworldShip> enemyShips,
            Vector2 startingPoint, List<String> levels, int enemyAttackStartTime, int enemyAttackFrequency)
        {
            Setup(shipToDefend, shipIntroductionText, enemyShips, startingPoint, levels, enemyAttackStartTime, enemyAttackFrequency);

            allyShips = new List<OverworldShip>();
            attackStartText = "";
            enemyMessages = new List<String>();
            shipToDefendHP = 2000;
        }

        public EscortDataCapsule(OverworldShip shipToDefend, String shipIntroductionText, List<OverworldShip> enemyShips,
            List<String> enemyMessages, List<OverworldShip> allyShips, Vector2 startingPoint, String attackStartText,
            int enemyAttackStartTime, int enemyAttackFrequency, int shipToDefendHP, List<String> levels)
        {
            Setup(shipToDefend, shipIntroductionText, enemyShips, startingPoint, levels, enemyAttackStartTime, enemyAttackFrequency);

            this.allyShips = allyShips;
            this.attackStartText = attackStartText;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;
            this.shipToDefendHP = shipToDefendHP;
            this.enemyMessages = enemyMessages;

            if (this.allyShips == null)
            {
                this.allyShips = new List<OverworldShip>();
            }

            if (this.attackStartText == null)
            {
                this.attackStartText = "";
            }

            if (this.enemyMessages == null)
            {
                this.enemyMessages = new List<String>();
            }
        }

        private void Setup(OverworldShip shipToDefend, String shipToDefendText, List<OverworldShip> enemyShips,
            Vector2 startingPoint, List<String> levels, int enemyAttackStartTime, int enemyAttackFrequency)
        {
            this.shipToDefend = shipToDefend;
            this.shipIntroductionText = shipToDefendText;
            this.enemyShips = enemyShips;
            this.startingPoint = startingPoint;
            this.levels = levels;
            this.enemyAttackStartTime = enemyAttackStartTime;
            this.enemyAttackFrequency = enemyAttackFrequency;

            if (this.shipIntroductionText == null)
            {
                this.shipIntroductionText = "";
            }

            if (this.enemyShips == null)
            {
                this.enemyShips = new List<OverworldShip>();
            }

            if (this.levels == null)
            {
                this.levels = new List<String>();
            }

            if (this.attackStartText == null)
            {
                this.attackStartText = "";
            }
        }
    }
}
