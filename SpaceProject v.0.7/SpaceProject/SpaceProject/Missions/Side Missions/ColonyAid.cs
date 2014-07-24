using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ColonyAid : Mission
    {
        private enum EventID
        {
            LeaveSupplies = 0,
            SoldSupplies = 1
        }

        private MedicalSupplies medicalSupplies;

        MultipleShotWeapon regularPoweredWeapon;
        RegularEnergyCell regularCell;

        public ColonyAid(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
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

            objectives.Add(new ItemTransportObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Lavis Station"), medicalSupplies,
                new EventTextCapsule(GetEvent((int)EventID.LeaveSupplies), GetEvent((int)EventID.SoldSupplies),
                    EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Fotrun Station I")));

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

    }
}
