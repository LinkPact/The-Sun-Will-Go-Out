﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class MissionMenuState : MenuState
    {

        #region Mission Fields

        private List<Mission> availableMissions;

        private Mission activeMission;
        private Mission selectedMission;
        private Cursor missionCursor;

        private int missionCursorIndex;
        private int responseCursorIndex;

        #endregion

        #region Properties

        public List<Mission> AvailableMissions { get { return availableMissions; } }
        public Mission ActiveMission { get { return activeMission; } set { activeMission = value; } }

        public Mission SelectedMission { get { return selectedMission; } set { selectedMission = value; } }
        public Cursor MissionCursor { get { return missionCursor; } set { missionCursor = value; } }

        public int MissionCursorIndex { get { return missionCursorIndex; } set { missionCursorIndex = value; } }
        public int ResponseCursorIndex { get { return responseCursorIndex; } set { responseCursorIndex = value; } }

        #endregion

        public MissionMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
        }

        public override void Initialize()
        {

            #region Initialize Mission Fields

            availableMissions = new List<Mission>();

            missionCursor = new Cursor(this.Game, SpriteSheet, new Rectangle(201, 121, 14, 14), new Rectangle(201, 135, 14, 14));
            missionCursor.Initialize();

            missionCursorIndex = -1;

            #endregion

            confirmString = "Press 'Enter' to continue..";

            confirmStringPos = new Vector2((Game.Window.ClientBounds.Width * 2 / 3),
                                            Game.Window.ClientBounds.Height - BaseState.Game.fontManager.GetFont(14).MeasureString(confirmString).Y - 10);

            confirmStringOrigin = BaseState.Game.fontManager.GetFont(14).MeasureString(confirmString) / 2;
        }

        public override void OnEnter()
        {
            availableMissions = MissionManager.ReturnAvailableMissions(BaseState.GetBase().name);
            BaseStateManager.OverviewMenuState.ButtonMission.isSelected = true;
        }

        public override void OnLeave()
        {
            BaseStateManager.OverviewMenuState.ButtonMission.isSelected = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (BaseState.GetBase() != null)
                availableMissions = MissionManager.ReturnAvailableMissions(BaseState.GetBase().name);

            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission) ||
                BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                missionCursor.isActive = true;
                missionCursor.isVisible = true;
            }

            else
            {
                missionCursor.isActive = false;
                missionCursor.isVisible = false;
            }


            if (BaseStateManager.ButtonControl == ButtonControl.Mission)
            {
                //Moves button cursor right when pressing up. 
                if (ControlManager.CheckPress(RebindableKeys.Up))
                    missionCursorIndex--;

                //Moves button cursor left when pressing down
                else if (ControlManager.CheckPress(RebindableKeys.Down))
                    missionCursorIndex++;

                if (MissionCursorIndex > availableMissions.Count)
                    missionCursorIndex = 0;

                else if (MissionCursorIndex < 0)
                    missionCursorIndex = availableMissions.Count;

                if (availableMissions.Count > 0 && missionCursorIndex != availableMissions.Count)
                {
                    selectedMission = availableMissions[MissionCursorIndex];
                }
            }

            else if (BaseStateManager.ButtonControl == ButtonControl.Response)
            {
                //Moves button cursor right when pressing up. 
                if (ControlManager.CheckPress(RebindableKeys.Up))
                    responseCursorIndex--;

                //Moves button cursor left when pressing down
                else if (ControlManager.CheckPress(RebindableKeys.Down))
                    responseCursorIndex++;

                if (MissionManager.MissionResponseBuffer.Count <= 0)
                {
                    if (responseCursorIndex > 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = 1;
                }

                else
                {
                    if (responseCursorIndex > MissionManager.MissionResponseBuffer.Count - 1)
                        responseCursorIndex = 0;

                    else if (responseCursorIndex < 0)
                        responseCursorIndex = MissionManager.MissionResponseBuffer.Count - 1;
                }

            }

            base.Update(gameTime);
        }

        public void UpdateTextCursorPos()
        {
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
            {
                List<TextBox> textBoxes = BaseStateManager.TextBoxes;

                for (int i = 0; i < textBoxes.Count; i++)
                {
                    if (textBoxes[i].GetText().EndsWith(".."))
                    {
                        textBoxes.Remove(textBoxes[i]);
                    }
                }

                if (missionCursorIndex != availableMissions.Count)
                {
                    missionCursor.position.X = textBoxes[missionCursorIndex + 1].TextBoxRect.X - Game.fontManager.GetFont(16).MeasureString(availableMissions[missionCursorIndex].MissionName).X / 2 - 10;
                    missionCursor.position.Y = textBoxes[missionCursorIndex + 1].TextBoxRect.Y + 13;
                }
                else
                {
                    missionCursor.position.X = textBoxes[missionCursorIndex + 1].TextBoxRect.X - Game.fontManager.GetFont(16).MeasureString("Back").X / 2 - 10;
                    missionCursor.position.Y = textBoxes[missionCursorIndex + 1].TextBoxRect.Y + 13;
                }
            }

            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                if (MissionManager.MissionResponseBuffer.Count <= 0)
                {
                    switch (responseCursorIndex)
                    {
                        case 0:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(selectedMission.PosResponse).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle1.Y + 10;
                                break;
                            }

                        case 1:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(selectedMission.NegResponse).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle2.Y + 10;
                                break;
                            }
                    }
                }

                else
                {
                    switch (responseCursorIndex)
                    {
                        case 0:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle1.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[0]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle1.Y + 10;
                                break;
                            }

                        case 1:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle2.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[1]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle2.Y + 10;
                                break;
                            }

                        case 2:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle3.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[2]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle3.Y + 10;
                                break;
                            }

                        case 3:
                            {
                                missionCursor.position.X = BaseStateManager.ResponseRectangle4.X - BaseState.Game.fontManager.GetFont(14).MeasureString(MissionManager.MissionResponseBuffer[3]).X / 2 - 10;
                                missionCursor.position.Y = BaseStateManager.ResponseRectangle4.Y + 10;
                                break;
                            }
                    }
                }
            }
        }

        public override void ButtonActions()
        {   
            //Actions for pressing Ok-key in "SELECTMISSION STATE" 
            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Mission))
            {
                if (missionCursorIndex == availableMissions.Count)
                {
                    BaseStateManager.TextBoxes.Clear();
                    BaseStateManager.ChangeMenuSubState("Overview");
                }

                else
                {
                    DisplayMissionText();
                }
            }

            //Actions for pressing Ok-key in "MISSION STATE" 
            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Response))
            {
                if (MissionManager.MissionResponseBuffer.Count > 0)
                {
                    activeMission.MissionResponse = responseCursorIndex + 1;
                    activeMission.MissionLogic();
                    activeMission.MissionResponse = 0;
                    MissionEvent();
                }

                else
                {
                    if (responseCursorIndex == 0)
                    {
                        DisplayMissionAcceptText();
                    }

                    else if (responseCursorIndex == 1)
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        DisplayAvailableMissions(availableMissions);

                        BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                              new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                           ((Game.Window.ClientBounds.Height / 2) + 40) + 20 * (availableMissions.Count + 1),
                                             Game.Window.ClientBounds.Width - 20,
                              10),
                              true,
                              "Back"));

                        SelectMission();
                    }
                }
            }

            else if (BaseStateManager.ButtonControl.Equals(ButtonControl.Confirm))
            {
                if (selectedMission != null)
                {
                    if (selectedMission.AcceptIndex + 1 < selectedMission.AcceptText.Count<string>())
                    {
                        selectedMission.AcceptIndex++;
                        DisplayMissionAcceptText();
                    }
                    else
                    {
                        BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                        BaseStateManager.ChangeMenuSubState("Overview");
                    }
                }

                else
                {
                    BaseStateManager.ChangeMenuSubState("Overview");
                    BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                }
            }
        }

        public override void CursorActions()
        { 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            missionCursor.Draw(spriteBatch);

            if (BaseStateManager.ButtonControl.Equals(ButtonControl.Confirm))
            {
                //Draw confirm string
                spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                       confirmString,
                                       confirmStringPos + Game.fontManager.FontOffset,
                                       Game.fontManager.FontColor,
                                       .0f,
                                       confirmStringOrigin,
                                       1.0f,
                                       SpriteEffects.None,
                                       .75f);
            }
        }

        public void MissionEvent()
        {
            BaseStateManager.TextBoxes.Clear();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              BaseStateManager.LowerScreenRectangle,
                                                              false,
                                                              MissionManager.MissionEventBuffer[0]));

            MissionManager.MissionEventBuffer.Remove(MissionManager.MissionEventBuffer[0]);

            if (MissionManager.MissionEventBuffer.Count > 0)
                BaseStateManager.ButtonControl = ButtonControl.Confirm;

            else if (MissionManager.MissionResponseBuffer.Count <= 0)
                BaseStateManager.ButtonControl = ButtonControl.Confirm;

            else
            {
                for (int i = 0; i < MissionManager.MissionResponseBuffer.Count; i++)
                {
                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                          BaseStateManager.ResponseRectangles[i],
                                                          true,
                                                          TextUtils.WordWrap(BaseState.Game.fontManager.GetFont(14),
                                                                             MissionManager.MissionResponseBuffer[i],
                                                                             Game.Window.ClientBounds.Width * 2 / 3)
                                                          ));

                }

                BaseStateManager.ButtonControl = ButtonControl.Response;
            }
        }

        public void SelectMission()
        {
            MissionCursorIndex = 0;

            if (availableMissions.Count <= 0)
            {
                BaseStateManager.ActiveButton = BaseStateManager.AllButtons[BaseStateManager.ActiveButtonIndexX, BaseStateManager.ActiveButtonIndexY];
                BaseStateManager.ChangeMenuSubState("Overview");
            }

            else
            {
                if (missionCursorIndex != availableMissions.Count)
                {
                    SelectedMission = availableMissions[MissionCursorIndex];
                    BaseStateManager.ButtonControl = ButtonControl.Mission;
                }
            }
            BaseStateManager.ActiveMenuState.CursorActions();
        }

        public void DisplayMissionText()
        {
            BaseStateManager.TextBoxes.Clear();

            if (SelectedMission != null)
            {
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  BaseStateManager.LowerScreenRectangle,
                                                                  false,
                                                                  SelectedMission.MissionText));

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle1,
                                                      true,
                                                      SelectedMission.PosResponse));

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                      BaseStateManager.ResponseRectangle2,
                                                      true,
                                                      SelectedMission.NegResponse));
            }

            BaseStateManager.ButtonControl = ButtonControl.Response;
            ResponseCursorIndex = 0;

        }

        public void DisplayAvailableMissions(List<Mission> availableMissions)
        {
            BaseStateManager.TextBoxes.Clear();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                                                       (Game.Window.ClientBounds.Height / 2) + 10,
                                                                        Game.Window.ClientBounds.Width - 20,
                                                                        10),
                                                                        true,
                                                                        "Available Missions:" + "\n\n"));
            if (availableMissions.Count > 0)
            {
                for (int i = 0; i < availableMissions.Count; i++)
                {
                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16),
                                                                      new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                                                                   ((Game.Window.ClientBounds.Height / 2) + 40) + 20 * availableMissions.IndexOf(availableMissions[i]) + 1,
                                                                                     Game.Window.ClientBounds.Width - 20,
                                                                      10),
                                                                      true,
                                                                      availableMissions[i].MissionName));
                }
            }

            else
            {
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), new Rectangle((Game.Window.ClientBounds.Width * 2 / 3),
                                                            ((Game.Window.ClientBounds.Height / 2) + 60),
                                                            Game.Window.ClientBounds.Width - 20,
                                                            10),
                                                            true,
                                                            "<None>"));
            }
        }

        public void DisplayMissionAcceptText()
        {
            if (selectedMission.RequiresAvailableSlot)
            {
                if (ShipInventoryManager.HasAvailableSlot())
                {
                    BaseStateManager.TextBoxes.Clear();

                    MissionManager.MarkMissionAsActive(selectedMission.MissionName);

                    BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                          BaseStateManager.LowerScreenRectangle,
                                                                          false,
                                                                          selectedMission.AcceptText[selectedMission.AcceptIndex]));

                    missionCursorIndex = 0;

                    BaseStateManager.ButtonControl = ButtonControl.Confirm;
                }

                else
                    DisplayMissionAcceptFailedText();
            }

            else
            {
                BaseStateManager.TextBoxes.Clear();

                MissionManager.MarkMissionAsActive(selectedMission.MissionName);

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                      BaseStateManager.LowerScreenRectangle,
                                                                      false,
                                                                      selectedMission.AcceptText[selectedMission.AcceptIndex]));

                missionCursorIndex = 0;

                BaseStateManager.ButtonControl = ButtonControl.Confirm;
            }
        }

        public void DisplayMissionAcceptFailedText()
        {
            BaseStateManager.TextBoxes.Clear();

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              BaseStateManager.LowerScreenRectangle,
                                                              false,
                                                              selectedMission.AcceptFailedText));

            missionCursorIndex = 0;

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }

        public void DisplayMissionCompletedText()
        {
            Game.SaveOnEnterOverworld = true;

            BaseStateManager.TextBoxes.Clear();

            List<Mission> completedMissions = MissionManager.ReturnCompletedMissions(BaseState.GetBase().name);

            List<Item> rewardItems = completedMissions[0].RewardItems;

            if (rewardItems.Count > 0)
            {
                string rewardText = "";

                foreach (Item reward in rewardItems)
                {
                    rewardText += "\n" + reward.Name;
                }

                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  BaseStateManager.LowerScreenRectangle,
                                                                  false,
                                                                  completedMissions[0].CompletedText +
                                                                  "\n\n" +
                                                                  "You reward is: \n" +
                                                                  completedMissions[0].MoneyReward + " Rupees" +
                                                                  rewardText));
            }

            else
            {
                BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                  BaseStateManager.LowerScreenRectangle,
                                                                  false,
                                                                  completedMissions[0].CompletedText +
                                                                  "\n\n" +
                                                                  "You reward is: \n" +
                                                                  completedMissions[0].MoneyReward + " Rupees"));
            }

            MissionManager.MarkCompletedMissionAsDead(completedMissions[0].MissionName);

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }

        public void DisplayMissionFailedText()
        {
            BaseStateManager.TextBoxes.Clear();

            List<Mission> failedMissions = MissionManager.ReturnFailedMissions(BaseState.GetBase().name);

            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                              BaseStateManager.LowerScreenRectangle,
                                                              false,
                                                              failedMissions[0].FailedText));


            MissionManager.MarkFailedMissionAsDead(failedMissions[0].MissionName);

            BaseStateManager.ButtonControl = ButtonControl.Confirm;
        }
    }
}
