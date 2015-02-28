using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Side_ColonyAid : Mission
    {
        private enum EventID
        {
            LeaveSupplies = 0,
            SoldSupplies = 1
        }

        private MedicalSupplies medicalSupplies;

        MultipleShotWeapon regularPoweredWeapon;
        RegularEnergyCell regularCell;

        public Side_ColonyAid(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            medicalSupplies = new MedicalSupplies(this.Game);

            regularPoweredWeapon = new MultipleShotWeapon(Game, ItemVariety.high);
            RewardItems.Add(regularPoweredWeapon);

            regularCell = new RegularEnergyCell(Game, ItemVariety.high);
            RewardItems.Add(regularCell);


            requiresAvailableSlot = true;
        }

        public override void StartMission()
        {
            ShipInventoryManager.AddItem(medicalSupplies);
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            destinations.Add(Game.stateManager.overworldState.GetStation("Lavis Station"));
            destinations.Add(Game.stateManager.overworldState.GetStation("Fortrun Station I"));
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ItemTransportObjective(Game, this, ObjectiveDescriptions[0],
                destinations[0], medicalSupplies,
                new EventTextCapsule(GetEvent((int)EventID.LeaveSupplies), GetEvent((int)EventID.SoldSupplies),
                    EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                destinations[1]));
        }
    }
}
