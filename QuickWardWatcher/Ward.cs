using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using LeagueSharp;

namespace QuickWardWatcher
{
    public class Ward
    {
        private const float WARD_INDICATOR_RADIUS = 80.0f;

        public const string GREEN_WARD_NAME = "SightWard";
        public const float GREEN_WARD_LENGTH = 180.0f;
        public const string PINK_WARD_NAME = "VisionWard";
        public const float PINK_WARD_LENGTH = float.MaxValue;
        public const string TRINKET_WARD_NAME = "YellowTrinket";
        public const float TRINKET_WARD_LENGTH = 60.0f;

        public Vector3 Position { get; private set; }

        public float[] TextPos { get; private set; }

        public float AliveTo { get; private set; }

        public int NetworkId { get; private set; }

        private System.Drawing.Color DrawColor { get; set; }

        public Ward(string skinName, Vector3 position, int networkId)
        {
            Position = position;


            if (skinName == GREEN_WARD_NAME)
            {
                AliveTo = Game.Time + GREEN_WARD_LENGTH;
                DrawColor = System.Drawing.Color.Green;
            } else if(skinName == PINK_WARD_NAME)
            {
                AliveTo = PINK_WARD_LENGTH; // Pinks may last forever.
                DrawColor = System.Drawing.Color.DeepPink;
            }
            else if (skinName == TRINKET_WARD_NAME)
            {
                AliveTo = Game.Time + TRINKET_WARD_LENGTH;
                DrawColor = System.Drawing.Color.Yellow;
            }

            NetworkId = networkId;
        }

        public void Draw()
        {
            //Drawing.DrawCircle(Position, WARD_INDICATOR_RADIUS, System.Drawing.Color.Red);

            TextPos = Drawing.WorldToScreen(Position);
            Drawing.DrawCircle(Position, 80.0f, DrawColor);
            
            if (AliveTo != float.MaxValue)
            {
                Drawing.DrawText(TextPos[0] - 15.0f, TextPos[1] - 10.0f, System.Drawing.Color.White, String.Format("{0}", (int)(AliveTo - Game.Time)));
            }
            
        }

        public override bool Equals(object obj)
        {
            Ward wardItem = obj as Ward;

            return wardItem.NetworkId == this.NetworkId;
        }

        public bool CheckRemove() {

            if (AliveTo == float.MaxValue &&
                ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(NetworkId) != null &&
                ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(NetworkId).IsDead)
            {
                AliveTo = Game.Time - 1.0f;
            }

            // Hold the object for a while after it's death
            // to ensure we don't re-add it while its getting reaped.
            return  (Game.Time >= AliveTo + 5.0f);
        }

        public override int GetHashCode()
        {
            return NetworkId;
        }
    }
}
