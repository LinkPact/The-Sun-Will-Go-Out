using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    enum ComparisonSymbol
    { 
        Equal,
        Better,
        Worse
    }

    //Class that compares item's stats and shows a visual representation of that comparison
    public class ItemComparison
    {
        private Game1 game;
        private Sprite spriteSheet;

        private int presetCount;

        private Item item1;
        private string item1Kind;
        private List<double> item1Stats = new List<double>();
        private List<bool> item1HighIsBest = new List<bool>();
        private List<ComparisonSymbol> item1Symbols = new List<ComparisonSymbol>();

        private Item item2;
        private string item2Kind;
        private List<double> item2Stats = new List<double>();
        private List<bool> item2HighIsBest = new List<bool>();
        private List<ComparisonSymbol> item2Symbols = new List<ComparisonSymbol>();

        public bool ShowSymbols = false;

        private bool okayToClearStats;
        public bool OkayToClearStats { get { return okayToClearStats; } set { okayToClearStats = value; } }

        private bool okayToClearSymbols;
        public bool OkayToClearSymbols { get { return okayToClearSymbols; } set { okayToClearSymbols = value; } }

        private bool compareComplete;

        private Sprite worseSprite;
        private Sprite betterSprite;
        private Sprite equalSprite;

        //Property that checks if CompareStats() can begin comparing items
        private bool ReadyToCompare
        {
            get
            {
                if (!compareComplete && item1Stats.Count > 0 && item2Stats.Count > 0)
                    return true;

                return false;
            }
        }

        public ItemComparison(Game1 Game, Sprite spriteSheet)
        {
            this.game = Game;
            this.spriteSheet = spriteSheet;
        }

        public ItemComparison(Game1 Game, Sprite spriteSheet, Rectangle equalSourceRect, Rectangle worseSourceRect,
            Rectangle betterSourceRect)
        {
            this.game = Game;
            this.spriteSheet = spriteSheet;
            equalSprite = spriteSheet.GetSubSprite(equalSourceRect);
            worseSprite = spriteSheet.GetSubSprite(worseSourceRect);
            betterSprite = spriteSheet.GetSubSprite(betterSourceRect);
        }

        public void Initialize()
        {
            worseSprite = spriteSheet.GetSubSprite(new Rectangle(216, 136, 9, 8));
            betterSprite = spriteSheet.GetSubSprite(new Rectangle(216, 127, 9, 8));
            equalSprite = spriteSheet.GetSubSprite(new Rectangle(216, 121, 9, 5));
        }

        //Checks for an okay from ReadyToCompare and then loops through 2 lists of stats
        //that it compares. This method should be called from the base's Update().
        public void CompareStats()
        {
            if (okayToClearSymbols)
            {
                item1Symbols.Clear();
                item2Symbols.Clear();
                okayToClearSymbols = false;
            }

            if (ReadyToCompare)
            {
                if(item1Stats.Count == item2Stats.Count)
                {
                    if(item1Kind == item2Kind || item1Kind == "Empty")
                    {
                        for (int i = 0; i < item1Stats.Count; i++)
                        {
                            if (i < presetCount)
                            {
                                if (item1HighIsBest[i])
                                {
                                    if (item1Stats[i] > item2Stats[i])
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Better);
                                        item2Symbols.Insert(i, ComparisonSymbol.Worse);
                                    }

                                    else if (item1Stats[i] < item2Stats[i])
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Worse);
                                        item2Symbols.Insert(i, ComparisonSymbol.Better);
                                    }

                                    else
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Equal);
                                        item2Symbols.Insert(i, ComparisonSymbol.Equal);
                                    }
                                }


                                else
                                {
                                    if (item1Stats[i] > item2Stats[i])
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Worse);
                                        item2Symbols.Insert(i, ComparisonSymbol.Better);
                                    }

                                    else if (item1Stats[i] < item2Stats[i])
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Better);
                                        item2Symbols.Insert(i, ComparisonSymbol.Worse);
                                    }

                                    else
                                    {
                                        item1Symbols.Insert(i, ComparisonSymbol.Equal);
                                        item2Symbols.Insert(i, ComparisonSymbol.Equal);
                                    }
                                }
                            }
                        }
                    }

                }

                compareComplete = true;
            }

            if (okayToClearStats)
            {
                item1Stats.Clear();
                item1HighIsBest.Clear();

                item2Stats.Clear();
                item2HighIsBest.Clear();

                okayToClearStats = false;
            }

        }

        //Loops through the list of symbols and draws them at the specified position.
        public void Draw(SpriteBatch spriteBatch, Vector2 pos, int spacing)
        {
            if (ShowSymbols)
            {
                //for (int i = 0; i < item1Symbols.Count; i++)
                //{
                //    switch (item1Symbols[i])
                //    {
                //        case ComparisonSymbol.Worse:
                //            spriteBatch.Draw(lowerSprite.Texture, new Vector2(pos.X, pos.Y + (i * 15)),
                //                lowerSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f,
                //                SpriteEffects.None, 0.95f);
                //            break;
                //
                //        case ComparisonSymbol.Better:
                //            spriteBatch.Draw(higherSprite.Texture, new Vector2(pos.X, pos.Y + (i * 15)),
                //                higherSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f,
                //                SpriteEffects.None, 0.95f);
                //            break;
                //
                //        case ComparisonSymbol.Equal:
                //            spriteBatch.Draw(equalSprite.Texture, new Vector2(pos.X, pos.Y + (i * 15)), 
                //                equalSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, 
                //                SpriteEffects.None, 0.95f);
                //            break;
                //    }
                //}

                for (int i = 0; i < item2Symbols.Count; i++)
                {
                    switch (item2Symbols[i])
                    {
                        case ComparisonSymbol.Worse:
                            spriteBatch.Draw(worseSprite.Texture, new Vector2(pos.X, pos.Y + (i * spacing)),
                                worseSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f,
                                SpriteEffects.None, 0.95f);
                            break;

                        case ComparisonSymbol.Better:
                            spriteBatch.Draw(betterSprite.Texture, new Vector2(pos.X, pos.Y + (i * spacing)),
                                betterSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f,
                                SpriteEffects.None, 0.95f);
                            break;

                        case ComparisonSymbol.Equal:
                            spriteBatch.Draw(equalSprite.Texture, new Vector2(pos.X, pos.Y + (i * spacing)),
                                equalSprite.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f,
                                SpriteEffects.None, 0.95f);
                            break;
                    }
                }
            }

        }

        //Checks for the selected item's kind (selectedItemKind) and sends in it's corresponding 
        //equipped item to FindItem1Stats
        public void FindEquippedItem(string selectedItemKind)
        {
            switch (selectedItemKind)
            { 
                case "Primary":
                    if (item2 != null)
                    {
                        SetItem1(ShipInventoryManager.equippedPrimaryWeapons[
                            ShipInventoryManager.GetEquippedItemIndexOfType(item2.Name)
                            ]);
                    }
                    else
                    {
                        SetItem1(ShipInventoryManager.equippedPrimaryWeapons[0]);
                    }
                    break;

                case "Secondary":
                    SetItem1(ShipInventoryManager.equippedSecondary);
                    break;

                case "EnergyCell":
                    SetItem1(ShipInventoryManager.equippedEnergyCell);
                    break;

                case "Shield":
                    SetItem1(ShipInventoryManager.equippedShield);
                    break;

                case "Plating":
                    SetItem1(ShipInventoryManager.equippedPlating);
                    break;

                case "Empty":
                    okayToClearSymbols = true;
                    break;

                default:
                    break;
            }
        }

        //Sets the first item to compare
        public void SetItem1(Item item1)
        {
            compareComplete = false;

            okayToClearStats = true;

            this.item1 = item1;

            switch (item1.Kind)
            {
                case "Primary":
                    presetCount = 4;
                    item1Kind = "Primary";

                    item1Stats.Add(Math.Round((double)((PlayerWeapon)item1).Damage, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)1000 / ((PlayerWeapon)item1).Delay, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)(((PlayerWeapon)item1).Speed * ((PlayerWeapon)item1).Duration), 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)(1000 * ((PlayerWeapon)item1).EnergyCost / ((PlayerWeapon)item1).Delay), 1));
                    item1HighIsBest.Add(false);
                    break;

                case "Secondary":
                    presetCount = 4;
                    item1Kind = "Secondary";

                    item1Stats.Add(Math.Round((double)((PlayerWeapon)item1).Damage, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)1000 / ((PlayerWeapon)item1).Delay, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)(((PlayerWeapon)item1).Speed * ((PlayerWeapon)item1).Duration), 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)(1000 * ((PlayerWeapon)item1).EnergyCost / ((PlayerWeapon)item1).Delay), 1));
                    item1HighIsBest.Add(false);
                    break;

                case "EnergyCell":
                    presetCount = 2;
                    item1Kind = "EnergyCell";

                    item1Stats.Add(Math.Round((double)((PlayerEnergyCell)item1).Capacity, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)((PlayerEnergyCell)item1).Recharge, 1));
                    item1HighIsBest.Add(true);
                    break;

                case "Shield":
                    presetCount = 2;
                    item1Kind = "Shield";

                    item1Stats.Add(Math.Round((double)((PlayerShield)item1).Capacity, 1));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)1000 / ((PlayerShield)item1).ConversionFactor, 1));
                    item1HighIsBest.Add(false);
                    break;

                case "Plating":
                    presetCount = 4;
                    item1Kind = "Plating";

                    item1Stats.Add(Math.Round((double)((PlayerPlating)item1).Armor, 0));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)((PlayerPlating)item1).Speed * 1000, 0));
                    item1HighIsBest.Add(true);

                    item1Stats.Add(Math.Round((double)((PlayerPlating)item1).PrimarySlots * 10, 0));
                    item1HighIsBest.Add(false);

                    break;

                case "Empty":
                    item1Kind = "Empty";

                    if (item1 is EmptyShield)
                    {
                        presetCount = 2;

                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                    }

                    else if (item1 is EmptyWeapon)
                    {
                        presetCount = 4;

                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                        item1Stats.Add(0);
                        item1HighIsBest.Add(true);
                    }
                    break;
            }
        }

        //Sets the second item to compare
        public void SetItem2(Item item2)
        {
            compareComplete = false;

            okayToClearStats = true;
            okayToClearSymbols = true;

            this.item2 = item2;

            switch (item2.Kind)
            {
                case "Primary":
                    item2Kind = "Primary";

                    item2Stats.Add(Math.Round((double)((PlayerWeapon)item2).Damage, 1));
                    item2HighIsBest.Add(true);
                        
                    item2Stats.Add(Math.Round((double)1000 / ((PlayerWeapon)item2).Delay, 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)(((PlayerWeapon)item2).Speed * ((PlayerWeapon)item2).Duration), 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)(1000 * ((PlayerWeapon)item2).EnergyCost / ((PlayerWeapon)item2).Delay), 1));
                    item2HighIsBest.Add(false);
                    
                    break;

                case "Secondary":
                    item2Kind = "Secondary";

                    item2Stats.Add(Math.Round((double)((PlayerWeapon)item2).Damage, 1));
                    item2HighIsBest.Add(true);
                        
                    item2Stats.Add(Math.Round((double)1000 / ((PlayerWeapon)item2).Delay, 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)(((PlayerWeapon)item2).Speed * ((PlayerWeapon)item2).Duration), 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)(1000 * ((PlayerWeapon)item2).EnergyCost / ((PlayerWeapon)item2).Delay), 1));
                    item2HighIsBest.Add(false);
                    
                    break;

                case "EnergyCell":
                    item2Kind = "EnergyCell";

                    item2Stats.Add(Math.Round((double)((PlayerEnergyCell)item2).Capacity, 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)((PlayerEnergyCell)item2).Recharge, 1));
                    item2HighIsBest.Add(true);
                    break;

                case "Shield":
                    item2Kind = "Shield";

                    item2Stats.Add(Math.Round((double)((PlayerShield)item2).Capacity, 1));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)1000 / ((PlayerShield)item2).ConversionFactor, 1));
                    item2HighIsBest.Add(false);
                    break;

                case "Plating":
                    item2Kind = "Plating";

                    item2Stats.Add(Math.Round((double)((PlayerPlating)item2).Armor, 0));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)((PlayerPlating)item2).Speed * 1000, 0));
                    item2HighIsBest.Add(true);

                    item2Stats.Add(Math.Round((double)((PlayerPlating)item2).PrimarySlots * 10, 0));
                    item2HighIsBest.Add(false);
                    break;

                case "Resource":
                    break;

            }
        }
    }
}
