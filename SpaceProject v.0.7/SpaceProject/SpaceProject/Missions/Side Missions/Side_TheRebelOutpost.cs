using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Side_TheRebelOutpost: Mission
    {
        private enum EventID
        {
            SairMessageToOutpost,
            OutpostFound,  
            Level1Completed,
            Level1Failed,
            BribeAccepted,
            SairMessageAfterAccept,
            BeforeLevel2,
            Level2Completed,
            Level2Failed,
            SairMessageAfterDecline,
            AcceptBribeCompletedText,
            DeclineBribeCompletedText
        }

        private readonly int SmallReward = 200;
        private readonly int BigReward = 600;

        private readonly Vector2 DummyCoordinatePosition = MathFunctions.CoordinateToPosition(new Vector2(975, 0));

        private readonly string level1 = "theRebelOutpost_1";
        private readonly string level2 = "theRebelOutpost_2";

        private Station fortrunStation;
        private RebelOutpostAsteroid rebelOutpost;
        private DummyCoordinateObject dummyCoordinateObject;

        private ShipPart rebelBribe;

        public Side_TheRebelOutpost(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            rebelBribe = new BasicLaserWeapon(Game, ItemVariety.low);

            fortrunStation = Game.stateManager.overworldState.GetStation("Fortrun Station I");

            dummyCoordinateObject = new DummyCoordinateObject(Game, Game.spriteSheetOverworld);
            dummyCoordinateObject.Initialize();
            dummyCoordinateObject.position = DummyCoordinatePosition;

            rebelOutpost = new RebelOutpostAsteroid(Game, Game.stateManager.overworldState.GetSectorX.GetSpriteSheet());
            rebelOutpost.Initialize();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            progress = 0;
            ObjectiveIndex = 0;

            Game.stateManager.overworldState.GetSectorX.AddGameObject(rebelOutpost);
            Game.stateManager.overworldState.GetSectorX.AddGameObject(dummyCoordinateObject);
        }

        public override void OnLoad()
        {
            if (MissionState == StateOfMission.Active)
            {
                Game.stateManager.overworldState.GetSectorX.AddGameObject(rebelOutpost);
                Game.stateManager.overworldState.GetSectorX.AddGameObject(dummyCoordinateObject);

                if (progress == 1)
                {
                    moneyReward = SmallReward;
                    CompletedText = GetEvent((int)EventID.AcceptBribeCompletedText).Text;
                }
                else if (progress == 2)
                {
                    moneyReward = BigReward;
                    CompletedText = GetEvent((int)EventID.DeclineBribeCompletedText).Text;
                }
            }
        }

        public override void OnFailed()
        {
            base.OnFailed();

            Game.stateManager.overworldState.GetSectorX.RemoveGameObject(rebelOutpost);
            Game.stateManager.overworldState.GetSectorX.RemoveGameObject(dummyCoordinateObject);
        }

        public override void OnReset()
        {
            SetDestinations();
            SetupObjectives();

            base.OnReset();

            ObjectiveIndex = 0;
            rebelOutpost.Activated = false;
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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

            AddDestination(dummyCoordinateObject, 4);
            AddDestination(fortrunStation, 3);
            AddDestination(rebelOutpost, 3);
            AddDestination(fortrunStation, 1);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            // Event: Message from Sair on the way to outpost
            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], 3000, 3000, PortraitID.Sair,
                GetEvent((int)EventID.SairMessageToOutpost).Text));

            // Objective: Find the outpost
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.OutpostFound), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return rebelOutpost.Activated; },
                delegate { return false; }));

            // Objective: Complete the first level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], level1, LevelStartCondition.TextCleared, 
                new EventTextCapsule(GetEvent((int)EventID.Level1Completed), GetEvent((int)EventID.Level1Failed), EventTextCanvas.MessageBox)));

            // Event: Rebels ask for mercy with bribe
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate 
                {
                    PopupHandler.DisplaySelectionMenu(String.Format("Accept the {0} from the rebels?", rebelBribe.Name),
                        new List<String>{"Yes", "No"},
                        new List<System.Action> { delegate { OnAcceptBribe(); },
                                                  delegate { OnDeclineBribe(); } });
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            // Branch 1 Event: Rebel message after accepting bribe
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.BribeAccepted), null, EventTextCanvas.MessageBox)));

            // Branch 1 Event: Message from Sair on the way back after accepting bribe
            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], 3000, 3000, PortraitID.Sair,
                GetEvent((int)EventID.SairMessageAfterAccept).Text));

            // Branch 1: Jump to final objective
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { ObjectiveIndex = 10; },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            // Branch 2 Event: Rebel message after declining bribe
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.BeforeLevel2), null, EventTextCanvas.MessageBox)));

            // Branch 2 Objective: Complete the second level
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], level2, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.Level2Completed), GetEvent((int)EventID.Level2Failed), EventTextCanvas.MessageBox)));

            // Branch 2 Event: Message from Sair on the way back after accepting bribe
            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], 3000, 3000, PortraitID.Sair,
                GetEvent((int)EventID.SairMessageAfterDecline).Text));

            // Objective: Return to Fortrun
            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0]));
        }

        private void OnAcceptBribe()
        {
            ShipInventoryManager.AddItem(rebelBribe);
            moneyReward = SmallReward;
            CompletedText = GetEvent((int)EventID.AcceptBribeCompletedText).Text;
            progress = 1;
        }

        private void OnDeclineBribe()
        {
            ObjectiveIndex = 7;
            moneyReward = BigReward;
            CompletedText = GetEvent((int)EventID.DeclineBribeCompletedText).Text;
            progress = 2;
        }
    }
}
