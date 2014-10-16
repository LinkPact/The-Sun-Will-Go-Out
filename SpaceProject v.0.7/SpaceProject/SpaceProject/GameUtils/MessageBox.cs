using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public enum MessageState
    {
        Invisible,
        Message,
        Tutorial,
        Menu,
        Inventory,
        YesNo,
        SelectionMenu,
        Map
    }

    public class MessageBox
    {
        private List<String> textStorage;
        private List<String> optionStorage;
        private List<System.Action> actionStorage;
        private int popupDelay;

        #region variables

        private Game1 Game;
        private Sprite spriteSheet;
        private Sprite tutorialBackdrop;
        //private SpriteFont font;

        private MessageState messageState;

        private Vector2 textBoxPos;
        private Vector2 textPos;

        private List<string> textBuffer;
        private string confirmString;

        private List<string> menuOptions;
        private List<System.Action> menuActions;
        private List<Item> inventoryRef;
        private List<Item> throwList;
        private int cursorIndex;
        private int currentIndexMax;

        //Map related 
        private List<GameObjectOverworld> objectsOnMap;
        private float scaleX;
        private float scaleY;

        //private ItemComparison itemComp;
        private int holdTimer;

        //Variables only related to the inventory popup
        #region inventoryVariables

        private int cursorColumn;
        private int pickedColumn;
        private int pickedIndex;
        private bool isPicked;

        #endregion

        int tempTimer;

        bool displayOnReturn = false;

        #endregion

        #region Properties

        public MessageState MessageState { get { return messageState; } set { messageState = value; } }

        public bool TextBufferEmpty { get { return (textBuffer.Count <= 0 && textStorage.Count <= 0); } private set { ; } } 

        #endregion

        public MessageBox(Game1 Game, Sprite SpriteSheet)
        {
            this.spriteSheet = SpriteSheet.GetSubSprite(new Rectangle(0, 58, 200, 150));
            this.Game = Game;
            //font = Game.fontManager.GetFont(14);
        }

        public void Initialize()
        {
            textBuffer = new List<string>();
            menuOptions = new List<string>();
            menuActions = new List<System.Action>();

            inventoryRef = ShipInventoryManager.ShipItems;

            holdTimer = Game.HoldKeyTreshold;

            displayOnReturn = false;

            textStorage = new List<String>();
            optionStorage = new List<String>();
            actionStorage = new List<System.Action>();
        }

        //Call this method, feed in a string and the message will appear on screen 
        public void DisplayMessage(string txt)
        {
            messageState = MessageState.Message;

            Game1.Paused = true;

            if (!txt.Contains('#'))
            {
                textBuffer.Add(txt);
            }
            else
            {
                List<String> tempList = SplitHashTagText(txt);
                foreach (String str in tempList)
                {
                    textBuffer.Add(str);
                }
            }

            tempTimer = 5;
        }

        public void DisplayMessage(string txt, int delay)
        {
            textStorage = new List<String>();
            textStorage.Add(txt);

            popupDelay = delay;
        }

        //Same as above, but with more than one message. The messages will be displayed one by one, proceeding
        //when pressing the actionkey
        public void DisplayMessage(List<string> txtList)
        {
            foreach (string str in txtList)
            {
                DisplayMessage(str);
            }
        }

        public void DisplayMessage(List<string> txtList, int delay)
        {
            textStorage = txtList;
            popupDelay = delay;
        }

        public void DisplayTutorialMessage(string txt)
        {

        }

        public void DisplayTutorialMessage(string txt, Sprite image)
        {

        }

        //Display a map of the system in a pop-up
        public void DisplayMap(List<GameObjectOverworld> objectsInOverworld)
        {
            messageState = MessageState.Map;

            Game1.Paused = true;

            scaleX = OverworldState.OVERWORLD_WIDTH / Game.Window.ClientBounds.Width;
            scaleY = OverworldState.OVERWORLD_HEIGHT / Game.Window.ClientBounds.Height;
            objectsOnMap = new List<GameObjectOverworld>();
            foreach (GameObjectOverworld obj in objectsInOverworld)
            {
                if (obj is ImmobileSpaceObject || obj is System1Star)
                    objectsOnMap.Add(obj);
            }

            tempTimer = 5;
        }

        //Displays the "Which-item-to-trash"-menu
        public void DisplayTrashMenu(List<Item> trash)
        {
            cursorIndex = 0;
            cursorColumn = 0;

            messageState = MessageState.Inventory;
            Game1.Paused = true;

            //Fills up the menuOption list with throw-items
            menuOptions.Clear();
            foreach (Item item in trash)
                menuOptions.Add(item.Name);

            menuOptions.Add("Finish");

            throwList = trash;

            currentIndexMax = menuOptions.Count;

            tempTimer = 5;
        }

        //Displays the "overmenu"
        public void DisplayMenu()
        {
            if (!displayOnReturn)
                cursorIndex = 0;

            messageState = MessageState.Menu;

            Game1.Paused = true;

            menuOptions.Clear();

            if (GameStateManager.currentState != "ShooterState")
            {
                menuOptions.Add("Help");
                menuOptions.Add("Ship Inventory");
                menuOptions.Add("Missions Screen");
                menuOptions.Add("Options");
                menuOptions.Add("Save");
                menuOptions.Add("Exit Game");
                menuOptions.Add("Return To Game");
            }

            else
            {
                menuOptions.Add("Restart Level");
                menuOptions.Add("Give Up Level");
                menuOptions.Add("Exit Game");
                menuOptions.Add("Return To Game");
            }

            currentIndexMax = menuOptions.Count;

            tempTimer = 5;
        }

        public void DisplaySelectionMenu(string message, List<String> options)
        {
            messageState = MessageState.SelectionMenu;
            Game1.Paused = true;
            textBuffer.Add(message);
            tempTimer = 5;

            menuOptions.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                menuOptions.Add(options[i]);
            }
        }

        public void DisplaySelectionMenu(string message, List<String> options, List<System.Action> actions)
        {
            // TODO: Make more general
            if (MissionManager.MissionEventBuffer.Count > 0)
            {
                textStorage.Clear();
                textStorage.Add(message);
                optionStorage = options;
                actionStorage = actions;
                return;
            }

            messageState = MessageState.SelectionMenu;
            Game1.Paused = true;
            textBuffer.Add(message);
            tempTimer = 5;

            menuOptions.Clear();

            if (options.Count > actions.Count)
            {
                throw new ArgumentException("Actions needed for each menu option.");
            }

            for (int i = 0; i < options.Count; i++)
            {
                menuOptions.Add(options[i]);
                menuActions.Add(actions[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            confirmString = "Press 'Enter' to continue...";

            if (actionStorage.Count > 0)
            {
                if (MissionManager.MissionEventBuffer.Count <= 0
                    && Game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
                {
                    DisplaySelectionMenu(textStorage[0], optionStorage, actionStorage);

                    textStorage.Clear();
                    optionStorage.Clear();
                    actionStorage.Clear();
                }
            }

            if (popupDelay > 0)
            {
                popupDelay -= gameTime.ElapsedGameTime.Milliseconds;

                if (popupDelay <= 1)
                {
                    DisplayMessage(textStorage);
                    popupDelay = -1000;
                    textStorage.Clear();
                }
            }

            if (messageState == MessageState.Inventory)
                InventoryCursorControls(gameTime);

            else if (messageState != MessageState.Invisible)
            {
                ButtonControls(gameTime);
                if (GameStateManager.currentState == "OptionsMenuState")
                {
                    MouseControls();
                }
            }

            //Makes the messagebox invisible when pressing the esc or back if it's displaying the menu
            if (messageState == MessageState.Menu)
            {
                if (tempTimer < 0 && ControlManager.CheckPress(RebindableKeys.Pause) ||
                    ControlManager.CheckPress(RebindableKeys.Action2) && messageState != MessageState.Inventory)
                {
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                }
                else if (tempTimer < 0 && ControlManager.CheckPress(RebindableKeys.Pause) ||
                    ControlManager.CheckPress(RebindableKeys.Action2) && messageState == MessageState.Inventory
                    && !isPicked)
                {
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                }
            }

            //displays the overmenu automaticly when returning from the inventory or mission screen 
            if (displayOnReturn == true)
            {
                if (GameStateManager.currentState.Equals("OverworldState") ||
                GameStateManager.currentState.Equals("System1State") ||
                GameStateManager.currentState.Equals("System2State") ||
                GameStateManager.currentState.Equals("System2State"))
                {
                    DisplayMenu();
                    displayOnReturn = false;
                }
            }

            tempTimer--;
        }

        //The different actions that happens when you move items in different ways.
        //(In the inventory popup)
        private void InventoryAction()
        {
            if (cursorColumn == 0 && pickedColumn == 0 && cursorIndex < throwList.Count)
            {
                int pos1 = pickedIndex;
                int pos2 = cursorIndex;

                Item tempItem1 = throwList[pos1];
                Item tempItem2 = throwList[pos2];

                throwList.RemoveAt(pos1);
                throwList.Insert(pos1, tempItem2);
                throwList.RemoveAt(pos2);
                throwList.Insert(pos2, tempItem1);

                menuOptions.Clear();

                foreach (Item item in throwList)
                    menuOptions.Add(item.Name);

                menuOptions.Add("Finish");
            }
            else if (pickedColumn == 0 && cursorColumn != 0)
            {
                int pos1 = pickedIndex;
                int pos2;

                if (cursorColumn == 1) pos2 = cursorIndex;
                else pos2 = cursorIndex + 14;

                Item tempItem1 = throwList[pos1];
                Item tempItem2 = ShipInventoryManager.ShipItems[pos2];

                throwList.RemoveAt(pos1);
                throwList.Insert(pos1, tempItem2);
                ShipInventoryManager.RemoveItemAt(pos2);
                ShipInventoryManager.InsertItem(pos2, tempItem1);

                menuOptions.Clear();

                foreach (Item item in throwList)
                    menuOptions.Add(item.Name);

                menuOptions.Add("Finish");
            }
            else if (pickedColumn != 0 && cursorColumn == 0)
            {
                int pos1;
                int pos2 = cursorIndex;

                if (pickedColumn == 1) pos1 = pickedIndex;
                else pos1 = pickedIndex + 14;

                Item tempItem1 = ShipInventoryManager.ShipItems[pos1];
                Item tempItem2 = throwList[pos2];

                throwList.RemoveAt(pos2);
                throwList.Insert(pos2, tempItem1);
                ShipInventoryManager.RemoveItemAt(pos1);
                ShipInventoryManager.InsertItem(pos1, tempItem2);

                menuOptions.Clear();

                foreach (Item item in throwList)
                    menuOptions.Add(item.Name);

                menuOptions.Add("Finish");
            }
            else if (cursorColumn != 0 && pickedColumn != 0)
            {
                int position1;
                int position2;

                if (pickedColumn == 1) position1 = pickedIndex;
                else position1 = pickedIndex + 14;

                if (cursorColumn == 1) position2 = cursorIndex;
                else position2 = cursorIndex + 14;

                ShipInventoryManager.SwitchItems(position1, position2);
            }
        }

        //Control over the inventory.
        private void InventoryCursorControls(GameTime gameTime)
        {
            if (ControlManager.CheckPress(RebindableKeys.Action2))
            {
                isPicked = false;
            }

            if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                if (!isPicked)
                {
                    if (!(cursorColumn == 0 && cursorIndex == throwList.Count))
                    {
                        isPicked = true;
                        pickedIndex = cursorIndex;
                        pickedColumn = cursorColumn;
                    }
                    else if (cursorColumn == 0 && cursorIndex == throwList.Count)
                    {
                        Game1.Paused = false;
                        messageState = MessageState.Invisible;
                    }
                }
                else
                {
                    isPicked = false;

                    //Prevents InventoryAction from attempting to move the finish-slot.
                    if (!(cursorColumn == 0 && cursorIndex == throwList.Count))
                        //LOGIC - THIS IS WHERE THE MAGIC HAPPENS
                        InventoryAction();
                }
            }

            if (ControlManager.CheckPress(RebindableKeys.Down))
            {
                cursorIndex++;
                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Down))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    cursorIndex++;
                    holdTimer = Game.ScrollSpeedFast;
                }
            }

            else if (ControlManager.CheckPress(RebindableKeys.Up))
            {
                cursorIndex--;
                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    cursorIndex--;
                    holdTimer = Game.ScrollSpeedFast;
                }
            }

            if (ControlManager.CheckPress(RebindableKeys.Right) && cursorColumn < 2)
            {
                cursorColumn++;

                if (cursorColumn == 1)
                {
                    if (inventoryRef.Count <= 14)
                        currentIndexMax = inventoryRef.Count - 1;
                    else
                        currentIndexMax = 14;
                }
                else if (cursorColumn == 2)
                {
                    currentIndexMax = inventoryRef.Count - 14;
                }

                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Right) && cursorColumn < 2)
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    cursorColumn++;

                    if (cursorColumn == 1)
                    {
                        if (inventoryRef.Count <= 14)
                            currentIndexMax = inventoryRef.Count - 1;
                        else
                            currentIndexMax = 14;
                    }
                    else if (cursorColumn == 2)
                    {
                        currentIndexMax = inventoryRef.Count - 14;
                    }

                    holdTimer = Game.ScrollSpeedSlow;
                }
            }

            if (ControlManager.CheckPress(RebindableKeys.Left) && cursorColumn > 0)
            {
                cursorColumn--;

                if (cursorColumn == 0)
                    currentIndexMax = menuOptions.Count;
                else if (cursorColumn == 1)
                {
                    if (inventoryRef.Count <= 14)
                        currentIndexMax = inventoryRef.Count - 1;
                    else
                        currentIndexMax = 14;
                }

                holdTimer = Game.HoldKeyTreshold;
            }

            else if (ControlManager.CheckHold(RebindableKeys.Left) && cursorColumn > 0)
            {
                holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (holdTimer <= 0)
                {
                    cursorColumn--;

                    if (cursorColumn == 0)
                        currentIndexMax = menuOptions.Count;
                    else if (cursorColumn == 1)
                    {
                        if (inventoryRef.Count <= 14)
                            currentIndexMax = inventoryRef.Count - 1;
                        else
                            currentIndexMax = 14;
                    }

                    holdTimer = Game.ScrollSpeedSlow;
                }
            }


            if (cursorIndex > currentIndexMax - 1)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                    cursorIndex = 0;
                else
                    cursorIndex = currentIndexMax - 1;
            }

            else if (cursorIndex < 0)
            {
                if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                    cursorIndex = currentIndexMax - 1;
                else
                    cursorIndex = 0;
            }
        }

        private void ButtonControls(GameTime gameTime)
        {
            if (messageState == MessageState.Menu)
            {
                if (ControlManager.CheckPress(RebindableKeys.Right))
                {
                    cursorIndex++;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Right))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex++;
                        holdTimer = Game.ScrollSpeedSlow;
                    }
                }

                else if (ControlManager.CheckPress(RebindableKeys.Left))
                {
                    cursorIndex--;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Left))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex--;
                        holdTimer = Game.ScrollSpeedSlow;
                    }
                }

                if (cursorIndex > menuOptions.Count - 1)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Right))
                        cursorIndex = 0;
                    else cursorIndex = menuOptions.Count - 1;
                }

                else if (cursorIndex < 0)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Left))
                        cursorIndex = menuOptions.Count - 1;
                    else
                        cursorIndex = 0;
                }
            }

            else if (messageState == MessageState.SelectionMenu)
            {
                if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    cursorIndex--;
                }

                else if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    cursorIndex++;
                }

                if (cursorIndex > menuOptions.Count - 1)
                {
                    cursorIndex = 0;
                }

                else if (cursorIndex < 0)
                {
                    cursorIndex = menuOptions.Count - 1;
                }
            }

            if (messageState == MessageState.Map)
            {
                if ((ControlManager.CheckKeypress(Keys.LeftShift)
                    || ControlManager.CheckKeypress(Keys.RightShift)
                    || ControlManager.CheckKeypress(Keys.N))
                    && tempTimer <= 0)
                {
                    HideMap();
                }
            }

            if (((ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeypress(Keys.Enter))
                || (GameStateManager.currentState == "MainMenuState" && ControlManager.IsLeftMouseButtonClicked() && messageState == MessageState.Message)
                && tempTimer <= 0))
                ButtonActions();
        }

        private void MouseControls()
        {
            Vector2 pos;
            float posAc = 5;
            if (GameStateManager.currentState == "OverworldState")
                pos = new Vector2(Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2,
                                  Game.camera.cameraPos.Y - Game.Window.ClientBounds.Height / 2);
            else
                pos = Vector2.Zero;

            if (messageState == MessageState.Menu)
            {
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (MathFunctions.IsMouseOverText(Game.fontManager.GetFont(14), menuOptions[i],
                        new Vector2(pos.X + posAc, pos.Y + 5), pos))
                    {
                        if (ControlManager.IsLeftMouseButtonClicked())
                        {
                            ButtonActions();
                        }
                        cursorIndex = i;
                        break;
                    }
                    posAc += Game.fontManager.GetFont(14).MeasureString(menuOptions[i]).X + 15;
                }
            }

            else if (messageState == MessageState.Message)
            {
                if (ControlManager.IsLeftMouseButtonClicked())
                {
                    ButtonActions();
                }
            }

            else if (messageState == MessageState.SelectionMenu)
            {
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (MathFunctions.IsMouseOverText(Game.fontManager.GetFont(14), menuOptions[i],
                        new Vector2(textPos.X - 15, textPos.Y + 30 + (i * 30) - 3),
                        pos))
                    {
                        cursorIndex = i;
                        if (ControlManager.IsLeftMouseButtonClicked())
                        {
                            ButtonActions();
                        }
                        break;
                    }
                }
            }
        }

        //Called when atempting to go back
        private void HideMessage()
        {
            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message
            if (messageState == MessageState.Message && tempTimer < 0)
            {
                textBuffer.Remove(textBuffer[0]);

                if (textBuffer.Count <= 0)
                {
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                }
            }
        }

        private void HideMap()
        {
            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message
            if (messageState == MessageState.Map && tempTimer < 0)
            {
                Game1.Paused = false;
                messageState = MessageState.Invisible;
            }
        }

        private void ButtonActions()
        {
            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message
            if (messageState == MessageState.Message && tempTimer < 0)
            {
                HideMessage();
            }

            //checks which option the user selects and executes it.  
            else if (messageState == MessageState.Menu && tempTimer < 0)
            {
                switch (menuOptions[cursorIndex])
                {
                    case "Ship Inventory":
                        Game.stateManager.ChangeState("ShipManagerState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Missions Screen":
                        Game.stateManager.ChangeState("MissionScreenState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Exit Game":
                        if (GameStateManager.currentState.Equals("OverworldState"))
                        {
                            DisplaySelectionMenu("What do you want to do?",
                                new List<string> { "Save and exit to menu", "Save and exit to desktop", "Exit to menu without saving",
                                "Exit to desktop without saving", "Cancel"});
                        }

                        else if (GameStateManager.currentState.Equals("ShooterState"))
                        {
                            DisplaySelectionMenu("What do you want to do? You cannot save during combat.",
                                new List<string> { "Exit to menu without saving", "Exit to desktop without saving", "Cancel" });
                        }
                        break;

                    case "Options":
                        Game.stateManager.ChangeState("OptionsMenuState");
                        Game.menuBGController.SetBackdropPosition(new Vector2(-903, -101));
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Save":
                        Game.Save();
                        break;

                    case "Help":
                        Game.stateManager.ChangeState("HelpScreenState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Return To Game":
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Restart Level":
                        Game.stateManager.shooterState.CurrentLevel.ResetLevel();
                        Game.stateManager.shooterState.Initialize();
                        Game.stateManager.shooterState.CurrentLevel.Initialize();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Give Up Level":
                        Game.stateManager.shooterState.CurrentLevel.GiveUpLevel();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Exit Level":
                        Game.stateManager.shooterState.CurrentLevel.ReturnToPreviousScreen();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;
                }
            }

            else if (messageState == MessageState.SelectionMenu && tempTimer < 0)
            {
                if (menuActions.Count <= 0)
                {
                    switch (menuOptions[cursorIndex].ToLower())
                    {
                        case "save and exit to menu":
                        case "save and restart":
                            Game.GameStarted = false;
                            textBuffer.Remove(textBuffer[0]);
                            messageState = MessageState.Invisible;
                            Game1.Paused = false;
                            Game.Save();
                            Game.Restart();
                            break;

                        case "save and exit to desktop":
                            Game.Save();
                            Game.Exit();
                            break;

                        case "exit to menu without saving":
                        case "restart":
                            Game.GameStarted = false;
                            textBuffer.Remove(textBuffer[0]);
                            messageState = MessageState.Invisible;
                            Game1.Paused = false;
                            Game.Restart();
                            break;

                        case "exit to desktop without saving":
                            Game.Exit();
                            break;

                        case "cancel":
                            textBuffer.Remove(textBuffer[0]);
                            Game1.Paused = false;
                            messageState = MessageState.Invisible;

                            if (GameStateManager.currentState.Equals("OverworldState"))
                            {
                                DisplayMenu();
                                cursorIndex = 5;
                            }
                            break;

                    }
                }

                else
                {
                    menuActions[cursorIndex].Invoke();
                    textBuffer.Remove(textBuffer[0]);
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                    menuActions.Clear();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GameStateManager.currentState == "OverworldState")
            {
                textBoxPos = new Vector2(Game.camera.cameraPos.X, Game.camera.cameraPos.Y);
                textPos = new Vector2(Game.camera.cameraPos.X, Game.camera.cameraPos.Y);
            }

            else
            {
                textBoxPos = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                textPos = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            }

            if (messageState == MessageState.Inventory)
            {
                //Displays a larger sized combo-box when working with inventory throwing.
                spriteBatch.Draw(spriteSheet.Texture, textBoxPos, spriteSheet.SourceRectangle, Color.White,
                             0.0f, new Vector2(0, 0),
                             1.0f, SpriteEffects.None, 0.95f);
                spriteBatch.Draw(spriteSheet.Texture, textBoxPos, spriteSheet.SourceRectangle, Color.White,
                             0.0f, new Vector2(spriteSheet.SourceRectangle.Value.Width, spriteSheet.SourceRectangle.Value.Height),
                             1.0f, SpriteEffects.None, 0.95f);
                spriteBatch.Draw(spriteSheet.Texture, textBoxPos, spriteSheet.SourceRectangle, Color.White,
                                 0.0f, new Vector2(0, spriteSheet.SourceRectangle.Value.Height),
                                 1.0f, SpriteEffects.None, 0.95f);
                spriteBatch.Draw(spriteSheet.Texture, textBoxPos, spriteSheet.SourceRectangle, Color.White,
                                 0.0f, new Vector2(spriteSheet.SourceRectangle.Value.Width, 0),
                                 1.0f, SpriteEffects.None, 0.95f);
            }

            switch (messageState)
            {
                case MessageState.Message:
                    {
                        spriteBatch.Draw(spriteSheet.Texture,
                             textBoxPos,
                             spriteSheet.SourceRectangle,
                             new Color(255, 255, 255, 185),
                             0.0f,
                             new Vector2(spriteSheet.SourceRectangle.Value.Width / 2,
                                         spriteSheet.SourceRectangle.Value.Height / 2),
                             1.5f,
                             SpriteEffects.None,
                             0.95f);

                        String text = TextUtils.WordWrap(Game.fontManager.GetFont(14),
                                                    textBuffer[0], (int)Math.Round((spriteSheet.SourceRectangle.Value.Width * 1.45) - 25, 0));

                        spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                text,
                                                new Vector2(textPos.X - 130,
                                                            textPos.Y - 98) + Game.fontManager.FontOffset,
                                                Game.fontManager.FontColor,
                                                0f,
                                                Vector2.Zero,
                                                1f,
                                                SpriteEffects.None,
                                                1f);

                        spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                               confirmString,
                                               new Vector2(textPos.X,
                                                           textPos.Y + 90) + Game.fontManager.FontOffset,
                                               Game.fontManager.FontColor,
                                               0f,
                                               Game.fontManager.GetFont(14).MeasureString(confirmString) / 2,
                                               1f,
                                               SpriteEffects.None,
                                               1f);
                    }
                    break;

                case MessageState.SelectionMenu:
                    {
                        spriteBatch.Draw(spriteSheet.Texture,
                             textBoxPos,
                             spriteSheet.SourceRectangle,
                             new Color(255, 255, 255, 185),
                             0.0f,
                             new Vector2(spriteSheet.SourceRectangle.Value.Width / 2,
                                         spriteSheet.SourceRectangle.Value.Height / 2),
                             1.5f,
                             SpriteEffects.None,
                             0.95f);

                        String text = TextUtils.WordWrap(Game.fontManager.GetFont(14),
                                                    textBuffer[0], (int)Math.Round((spriteSheet.SourceRectangle.Value.Width * 1.45) - 25, 0));

                        spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                text,
                                                new Vector2(textPos.X - 130,
                                                            textPos.Y - 98) + Game.fontManager.FontOffset,
                                                Game.fontManager.FontColor,
                                                0f,
                                                Vector2.Zero,
                                                1f,
                                                SpriteEffects.None,
                                                1f);

                        int numberOfMenuOptions = menuOptions.Count;
                        int ySpacing = 30;

                        if (numberOfMenuOptions == 5)
                        {
                            ySpacing = -30;
                        }

                        //loops through the menu options and colors the selected one red
                        for (int i = 0; i < menuOptions.Count; i++)
                        {
                            if (i == cursorIndex)
                                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                     menuOptions[i],
                                                     new Vector2(textPos.X,
                                                            textPos.Y + ySpacing + (i * 30)) + Game.fontManager.FontOffset,
                                                     Color.Red,
                                                     0f,
                                                     Game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                                                     1f,
                                                     SpriteEffects.None,
                                                     1f);

                            else
                                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                         menuOptions[i],
                                                         new Vector2(textPos.X,
                                                            textPos.Y + ySpacing + (i * 30)) + Game.fontManager.FontOffset,
                                                         Game.fontManager.FontColor,
                                                         0f,
                                                         Game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                                                         1f,
                                                         SpriteEffects.None,
                                                         1f);

                        }
                    }
                    break;

                case MessageState.Menu:
                    {
                        Vector2 pos;
                        if (GameStateManager.currentState == "OverworldState")
                            pos = new Vector2(Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2,
                                              Game.camera.cameraPos.Y - Game.Window.ClientBounds.Height / 2);


                        else
                            pos = Vector2.Zero;

                        spriteBatch.Draw(spriteSheet.Texture,
                             pos,
                             spriteSheet.SourceRectangle,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width, .15f),
                             SpriteEffects.None,
                             0.95f);


                        float XposAcc = 5;

                        //loops through the menu options and colors the selected one red
                        for (int i = 0; i < menuOptions.Count; i++)
                        {
                            if (i == cursorIndex)
                                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                     menuOptions[i],
                                                     new Vector2(pos.X + XposAcc, pos.Y) + Game.fontManager.FontOffset,
                                                     Color.Red,
                                                     0f,
                                                     Vector2.Zero,
                                                     1f,
                                                     SpriteEffects.None,
                                                     1f);

                            else
                                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                                         menuOptions[i],
                                                         new Vector2(pos.X + XposAcc, pos.Y) + Game.fontManager.FontOffset,
                                                         Game.fontManager.FontColor,
                                                         0f,
                                                         Vector2.Zero,
                                                         1f,
                                                         SpriteEffects.None,
                                                         1f);

                            XposAcc += Game.fontManager.GetFont(14).MeasureString(menuOptions[i]).X + 15;
                        }

                    }
                    break;
                case MessageState.Map:
                    spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2((int)Game.camera.Position.X /*- Game.Window.ClientBounds.Width / 2*/, (int)Game.camera.Position.Y /*- Game.Window.ClientBounds.Height / 2*/),
                             spriteSheet.SourceRectangle,
                             new Color(255, 255, 255, 220),
                             0.0f,
                             new Vector2(spriteSheet.SourceRectangle.Value.Width / 2,
                                         spriteSheet.SourceRectangle.Value.Height / 2),
                             3.6f,
                             SpriteEffects.None,
                             0.95f);

                    spriteBatch.Draw(Game.player.sprite.Texture,
                        new Vector2(Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2 + (Game.player.position.X / scaleX), Game.camera.cameraPos.Y - Game.Window.ClientBounds.Height / 2 + (Game.player.position.Y / scaleY)),
                        Game.player.sprite.SourceRectangle,
                        Color.Beige,
                        0.0f,
                        new Vector2(Game.player.sprite.SourceRectangle.Value.Width / 2, Game.player.sprite.SourceRectangle.Value.Height / 2),
                        0.5f,
                        SpriteEffects.None,
                        0.99f
                        );

                    foreach (GameObjectOverworld obj in objectsOnMap)
                    {
                        spriteBatch.Draw(obj.sprite.Texture,
                            new Vector2(Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2 + (obj.position.X / scaleX), Game.camera.cameraPos.Y - Game.Window.ClientBounds.Height / 2 + (obj.position.Y / scaleY)),
                            obj.sprite.SourceRectangle,
                            Color.White,
                            0.0f,
                            new Vector2(obj.sprite.SourceRectangle.Value.Width / 2, obj.sprite.SourceRectangle.Value.Height / 2),
                            0.15f,
                            SpriteEffects.None,
                            0.98f);

                        if (obj is Planet)
                        {
                            spriteBatch.DrawString(Game.fontManager.GetFont(14),
                                obj.name,
                                new Vector2(Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2 + (obj.position.X / scaleX), Game.camera.cameraPos.Y - Game.Window.ClientBounds.Height / 2 + (obj.position.Y / scaleY) - 40),
                                Game.fontManager.FontColor,
                                0f,
                                Vector2.Zero,
                                1f,
                                SpriteEffects.None,
                                0.98f);
                        }
                    }
                    break;

                case MessageState.Inventory:
                    {
                        //Displays information about the active weapon
                        if (cursorColumn == 0 && cursorIndex < throwList.Count)
                        {
                            throwList[cursorIndex].DisplayInfo(spriteBatch, Game.fontManager.GetFont(14), new Vector2(textPos.X - 200, textPos.Y - 280), Game.fontManager.FontColor);
                        }
                        else
                        {
                            int invPos;
                            if (cursorColumn == 1) invPos = cursorIndex;
                            else invPos = cursorIndex + 14;

                            ShipInventoryManager.ShipItems[invPos].DisplayInfo(spriteBatch, Game.fontManager.GetFont(14), new Vector2(textPos.X - 200, textPos.Y - 280), Game.fontManager.FontColor);
                        }

                        //loops through the menu options and colors the selected one red
                        for (int i = 0; i < menuOptions.Count - 1; i++)
                        {
                            if (i == cursorIndex && cursorColumn == 0)
                                spriteBatch.DrawString(Game.fontManager.GetFont(14), menuOptions[i], new Vector2(textPos.X - 125, textPos.Y - 130 + (i * 20)) + Game.fontManager.FontOffset, Color.Red,
                                    0f, Game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2, 1f, SpriteEffects.None, 1f);
                            else if (isPicked && i == pickedIndex && pickedColumn == 0)
                                spriteBatch.DrawString(Game.fontManager.GetFont(14), menuOptions[i], new Vector2(textPos.X - 125, textPos.Y - 130 + (i * 20)) + Game.fontManager.FontOffset, Color.Yellow,
                                    0f, Game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2, 1f, SpriteEffects.None, 1f);
                            else
                                spriteBatch.DrawString(Game.fontManager.GetFont(14), menuOptions[i], new Vector2(textPos.X - 125, textPos.Y - 130 + (i * 20)) + Game.fontManager.FontOffset, Game.fontManager.FontColor,
                                    0f, Game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2, 1f, SpriteEffects.None, 1f);
                        }

                        int lastIndex = menuOptions.Count - 1;

                        //Displays the "finish" line
                        if (cursorIndex == lastIndex && cursorColumn == 0)
                            spriteBatch.DrawString(Game.fontManager.GetFont(14), menuOptions[lastIndex], new Vector2(textPos.X - 125, textPos.Y - 15) + Game.fontManager.FontOffset, Color.Red,
                                0f, Game.fontManager.GetFont(14).MeasureString(menuOptions[lastIndex]) / 2, 1f, SpriteEffects.None, 1f);
                        else
                            spriteBatch.DrawString(Game.fontManager.GetFont(14), menuOptions[lastIndex], new Vector2(textPos.X - 125, textPos.Y - 15) + Game.fontManager.FontOffset, Game.fontManager.FontColor,
                                0f, Game.fontManager.GetFont(14).MeasureString(menuOptions[lastIndex]) / 2, 1f, SpriteEffects.None, 1f);

                        //Draws the inventory if the count is maximum 14 items.
                        if (inventoryRef.Count <= 14)
                        {
                            for (int n = 0; n < inventoryRef.Count; n++)
                            {
                                if (n == cursorIndex && cursorColumn == 1)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Red,
                                                0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else if (isPicked && n == pickedIndex && pickedColumn == 1)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Yellow,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Game.fontManager.FontColor,
                                            0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                            }
                        }
                        //Draws the inventory if it has more slots than 14.
                        else
                        {
                            for (int n = 0; n < 14; n++)
                            {
                                if (n == cursorIndex && cursorColumn == 1)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Red,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else if (isPicked && n == pickedIndex && pickedColumn == 1)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Yellow,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n].Name, new Vector2(textPos.X, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Game.fontManager.FontColor,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n].Name) / 2, 1f, SpriteEffects.None, 1f);
                            }

                            for (int n = 0; n < inventoryRef.Count - 14; n++)
                            {
                                if (n == cursorIndex && cursorColumn == 2)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n + 14].Name, new Vector2(textPos.X + 125, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Red,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n + 14].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else if (isPicked && n == pickedIndex && pickedColumn == 2)
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n + 14].Name, new Vector2(textPos.X + 125, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Color.Yellow,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n + 14].Name) / 2, 1f, SpriteEffects.None, 1f);
                                else
                                    spriteBatch.DrawString(Game.fontManager.GetFont(14), inventoryRef[n + 14].Name, new Vector2(textPos.X + 125, textPos.Y - 130 + (n * 20)) + Game.fontManager.FontOffset, Game.fontManager.FontColor,
                                        0f, Game.fontManager.GetFont(14).MeasureString(inventoryRef[n + 14].Name) / 2, 1f, SpriteEffects.None, 1f);
                            }
                        }
                    }
                    break;
            }

        }

        private List<String> SplitHashTagText(String text)
        {
            List<String> tempList = new List<String>();

            String[] split = text.Split('#');
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
                tempList.Add(split[i]);
            }

            return tempList;
        }
    }
}
