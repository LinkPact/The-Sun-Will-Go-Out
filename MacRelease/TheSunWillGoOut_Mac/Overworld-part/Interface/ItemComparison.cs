using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
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
            equalSprite = spriteSheet.GetSubSprite(new Rectangle(63, 263, 9, 5));
            betterSprite = spriteSheet.GetSubSprite(new Rectangle(63, 269, 9, 8));
            worseSprite = spriteSheet.GetSubSprite(new Rectangle(63, 278, 9, 8));
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
        public void FindEquippedItem(string selectedItemKind, int slot = 1)
        {
            switch (selectedItemKind)
            { 
                case "Primary":
                    SetItem1(ShipInventoryManager.equippedPrimaryWeapons[slot - 1]);
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
            item1Kind = item1.Kind;

            switch (item1.Kind)
            {
                case "Primary":
                case "Secondary":
                case "EnergyCell":
                case "Shield":
                case "Plating":
                case "Empty":
                    SetStats(item1, item1Stats, item1HighIsBest);
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
            item2Kind = item2.Kind;

            switch (item2.Kind)
            {
                case "Primary":
                case "Secondary":
                case "EnergyCell":
                case "Shield":
                case "Plating":
                    SetStats(item2, item2Stats, item2HighIsBest); 
                    break;
            }
        }

        private void SetStats(Item item, List<double> stats, List<bool> highIsBestList)
        {
            presetCount = 2;

            stats.Add((double)item.Tier);
            highIsBestList.Add(true);

            if (item is ShipPart)
            {
                stats.Add((double)((ShipPart)item).Variety);
            }
            else
            {
                stats.Add(0);
            }

            highIsBestList.Add(true);
        }
    }
}
