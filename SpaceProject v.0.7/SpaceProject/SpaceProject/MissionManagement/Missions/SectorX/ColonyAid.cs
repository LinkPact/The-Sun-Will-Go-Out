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
        private MedicalSupplies medicalSupplies;

        MultipleShotWeapon regularPoweredWeapon;
        RegularEnergyCell regularCell;

        public ColonyAid(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[2, 1];

            medicalSupplies = new MedicalSupplies(this.Game);

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");  

            regularPoweredWeapon = new MultipleShotWeapon(Game, ItemVariety.high);
            RewardItems.Add(regularPoweredWeapon);

            regularCell = new RegularEnergyCell(Game, ItemVariety.high);
            RewardItems.Add(regularCell);

            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            ObjectiveDescriptions.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
        }

        public override void Initialize()
        {
            base.Initialize();

            requiresAvailableSlot = true;

            objectives.Add(new ItemTransportObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getStation("Lavis Station"), medicalSupplies,
                new EventTextCapsule(new List<String> { EventArray[0, 0] }, new List<String> { EventArray[1, 0] },
                    EventTextCanvas.BaseState)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.getStation("Fotrun Station I")));
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
