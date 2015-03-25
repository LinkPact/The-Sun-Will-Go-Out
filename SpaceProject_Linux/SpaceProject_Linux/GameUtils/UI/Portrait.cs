using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public enum PortraitID
    {
        None,
        // Central characters
        Sair,
        Ai,
        Rok,
        Ente,
        Berr,
        RebelLeader,
        AllianceCommander,

        // Generic characters
        AllianceCaptain,
        RebelTroopLeader,
        AlliancePilot,
        RebelPilot,
        CommonCitizen
    }

    public class Portrait
    {
        private static Sprite portraitSpriteSheet;
        private static PortraitID lastPortraitID;

        private Sprite sprite;
        public Sprite Sprite { get { return sprite; } }

        public Portrait(PortraitID id)
        {
            sprite = GetPortrait(id);
        }

        public static void InitializePortraitSpriteSheet(Game1 game)
        {
            portraitSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites\\PortraitSpriteSheet"), null);
        }

        private static Sprite GetPortrait(PortraitID portrait)
        {
            Rectangle sourceRect;

            switch (portrait)
            {
                case PortraitID.Sair:
                    sourceRect = new Rectangle(0, 0, 149, 192);
                    break;

                case PortraitID.Ai:
                    sourceRect = new Rectangle(154, 0, 149, 192);
                    break;

                case PortraitID.Rok:
                    sourceRect = new Rectangle(308, 0, 149, 192);
                    break;

                case PortraitID.Ente:
                    sourceRect = new Rectangle(462, 0, 149, 192);
                    break;

                case PortraitID.Berr:
                    sourceRect = new Rectangle(0, 197, 149, 192);
                    break;

                case PortraitID.RebelLeader:
                    sourceRect = new Rectangle(154, 197, 149, 192);
                    break;

                case PortraitID.AllianceCommander:
                    sourceRect = new Rectangle(308, 197, 149, 192);
                    break;

                case PortraitID.AllianceCaptain:
                    sourceRect = new Rectangle(462, 197, 149, 192);
                    break;

                case PortraitID.RebelTroopLeader:
                    sourceRect = new Rectangle(0, 394, 149, 192);
                    break;

                case PortraitID.AlliancePilot:
                    sourceRect = new Rectangle(154, 394, 149, 192);
                    break;

                case PortraitID.RebelPilot:
                    sourceRect = new Rectangle(308, 394, 149, 192);
                    break;

                case PortraitID.CommonCitizen:
                    sourceRect = new Rectangle(462, 394, 149, 192);
                    break;

                default:
                    throw new ArgumentException("Invalid Portrait ID.");
            }

            return new Sprite(portraitSpriteSheet.Texture, sourceRect);
        }

        public static PortraitID GetPortraitIDFromString(String str)
        {
            if (str.Contains("[Berr]"))
            {
                lastPortraitID = PortraitID.Berr;
                return PortraitID.Berr;
            }

            else if (str.Contains("[Ai]"))
            {
                lastPortraitID = PortraitID.Ai;
                return PortraitID.Ai;
            }

            else if (str.Contains("[SAIR]"))
            {
                lastPortraitID = PortraitID.Sair;
                return PortraitID.Sair;
            }

            else if (str.Contains("[Captain]"))
            {
                lastPortraitID = PortraitID.AllianceCaptain;
                return PortraitID.AllianceCaptain;
            }

            else if (str.Contains("[Commander]"))
            {
                lastPortraitID = PortraitID.AllianceCommander;
                return PortraitID.AllianceCommander;
            }

            else if (str.Contains("[Rebel Pilot]"))
            {
                lastPortraitID = PortraitID.RebelTroopLeader;
                return PortraitID.RebelTroopLeader;
            }

            else if (str.Contains("[Rok]"))
            {
                lastPortraitID = PortraitID.Rok;
                return PortraitID.Rok;
            }

            else if (str.Contains("[Trainer]"))
            {
                lastPortraitID = PortraitID.AllianceCommander;
                return PortraitID.AllianceCommander;
            }

            else if (str.Contains("[Alliance man]"))
            {
                lastPortraitID = PortraitID.AllianceCaptain;
                return PortraitID.AllianceCaptain;
            }

            else if (str.Contains("[Alliance]"))
            {
                lastPortraitID = PortraitID.AlliancePilot;
                return PortraitID.AlliancePilot;
            }

            else if (str.Contains("[Debt collector]"))
            {
                lastPortraitID = PortraitID.Berr;
                return PortraitID.Berr;
            }

            else if (str.Contains("[Desperate man]"))
            {
                lastPortraitID = PortraitID.CommonCitizen;
                return PortraitID.CommonCitizen;
            }

            else if (str.Contains("[Mineral researcher]"))
            {
                lastPortraitID = PortraitID.RebelTroopLeader;
                return PortraitID.RebelTroopLeader;
            }

            else if (str.Contains("[Concerned citizen]"))
            {
                lastPortraitID = PortraitID.CommonCitizen;
                return PortraitID.CommonCitizen;
            }

            else if (str.Contains("[Colony doctor]"))
            {
                lastPortraitID = PortraitID.Ente;
                return PortraitID.Ente;
            }

            else if (str.Contains("[Crazy scientist]"))
            {
                lastPortraitID = PortraitID.Ente;
                return PortraitID.Ente;
            }

            else if (str.Contains("\""))
            {
                return lastPortraitID;
            }

            return PortraitID.None;
        }

        public static PortraitID GetRandomRumorPortrait()
        {
            return PortraitID.None;
        }
    }
}
