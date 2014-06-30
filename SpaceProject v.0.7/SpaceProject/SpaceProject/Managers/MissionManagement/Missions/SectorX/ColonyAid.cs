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

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
        }

        public override void Initialize()
        {
            base.Initialize();

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

            if (progress == 0 
                && missionHelper.IsPlayerOnStation("Lavis Station"))
            {
                if (ShipInventoryManager.ShipItems.Contains(medicalSupplies))
                {
                    missionHelper.ShowEvent(0);
                    progress = 1;
                    ObjectiveIndex = 1;
                    ShipInventoryManager.RemoveItem(medicalSupplies);
                }

                else
                {
                    missionHelper.ShowEvent(1);
                    MissionManager.MarkMissionAsFailed(this.MissionName);
                }
            }

            if (progress == 1
                && missionHelper.IsPlayerOnStation("Fotrun Station I"))
            {
                MissionManager.MarkMissionAsCompleted(this.MissionName);
            }
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
