using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
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
        CommonCitizen,
        Pai
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

                case PortraitID.Pai:
                    sourceRect = new Rectangle(618, 0, 149, 192);
                    break;

                default:
                    throw new ArgumentException("Invalid Portrait ID.");
            }

            return new Sprite(portraitSpriteSheet.Texture, sourceRect);
        }

        public static PortraitID GetPortraitIDFromString(String str)
        {
            string lowerCase = str.ToLower();

            if (lowerCase.Contains("[berr]")
                || lowerCase.Contains("{berr}"))
            {
                lastPortraitID = PortraitID.Berr;
                return PortraitID.Berr;
            }

            else if (lowerCase.Contains("[ai]")
                || lowerCase.Contains("{ai}"))
            {
                lastPortraitID = PortraitID.Ai;
                return PortraitID.Ai;
            }

            else if (lowerCase.Contains("[sair]")
                || lowerCase.Contains("{sair}"))
            {
                lastPortraitID = PortraitID.Sair;
                return PortraitID.Sair;
            }

            else if (lowerCase.Contains("[captain]")
                || lowerCase.Contains("{captain}"))
            {
                lastPortraitID = PortraitID.AllianceCaptain;
                return PortraitID.AllianceCaptain;
            }

            else if (lowerCase.Contains("[commander]")
                || lowerCase.Contains("{commander}"))
            {
                lastPortraitID = PortraitID.AllianceCommander;
                return PortraitID.AllianceCommander;
            }

            else if (lowerCase.Contains("[rebel pilot]")
                || lowerCase.Contains("{rebel pilot}"))
            {
                lastPortraitID = PortraitID.RebelTroopLeader;
                return PortraitID.RebelTroopLeader;
            }

            else if (lowerCase.Contains("[rok]")
                || lowerCase.Contains("{rok}"))
            {
                lastPortraitID = PortraitID.Rok;
                return PortraitID.Rok;
            }

            else if (lowerCase.Contains("[trainer]")
                || lowerCase.Contains("{trainer}"))
            {
                lastPortraitID = PortraitID.AllianceCommander;
                return PortraitID.AllianceCommander;
            }

            else if (lowerCase.Contains("[alliance man]")
                || lowerCase.Contains("{alliance man}"))
            {
                lastPortraitID = PortraitID.AllianceCaptain;
                return PortraitID.AllianceCaptain;
            }

            else if (lowerCase.Contains("[alliance]")
                || lowerCase.Contains("{alliance}"))
            {
                lastPortraitID = PortraitID.AlliancePilot;
                return PortraitID.AlliancePilot;
            }

            else if (lowerCase.Contains("[debt collector]")
                || lowerCase.Contains("{debt collector}"))
            {
                lastPortraitID = PortraitID.Berr;
                return PortraitID.Berr;
            }

            else if (lowerCase.Contains("[desperate man]")
                || lowerCase.Contains("{desperate man}"))
            {
                lastPortraitID = PortraitID.CommonCitizen;
                return PortraitID.CommonCitizen;
            }

            else if (lowerCase.Contains("[mineral researcher]")
                || lowerCase.Contains("{mineral researcher}"))
            {
                lastPortraitID = PortraitID.RebelTroopLeader;
                return PortraitID.RebelTroopLeader;
            }

            else if (lowerCase.Contains("[concerned citizen]")
                || lowerCase.Contains("{concerned citizen}"))
            {
                lastPortraitID = PortraitID.CommonCitizen;
                return PortraitID.CommonCitizen;
            }

            else if (lowerCase.Contains("[colony doctor]")
                || lowerCase.Contains("{colony doctor}"))
            {
                lastPortraitID = PortraitID.Ente;
                return PortraitID.Ente;
            }

            else if (lowerCase.Contains("[crazy scientist]")
                || lowerCase.Contains("{crazy scientist}"))
            {
                lastPortraitID = PortraitID.Ente;
                return PortraitID.Ente;
            }

            else if (lowerCase.Contains("[squad member 1]")
                || lowerCase.Contains("{squad member 1}"))
            {
                lastPortraitID = PortraitID.AlliancePilot;
                return PortraitID.AlliancePilot;
            }

            else if (lowerCase.Contains("[squad member 2]")
                || lowerCase.Contains("{squad member 2}"))
            {
                lastPortraitID = PortraitID.AllianceCaptain;
                return PortraitID.AllianceCaptain;
            }

            else if (lowerCase.Contains("[pai]")
                || lowerCase.Contains("{pai}"))
            {
                lastPortraitID = PortraitID.Pai;
                return PortraitID.Pai;
            }

            else if (lowerCase.Contains("\""))
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
