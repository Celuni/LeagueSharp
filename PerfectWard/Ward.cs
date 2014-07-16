using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using LeagueSharp;

namespace PerfectWard
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

        private static List<Vector3> _WardSpots;
        private static List<WardSpot> _SafeWardSpots;

        public static List<Vector3> WardSpots
        {
            get
            {
                if(_WardSpots == null)
                {
                    InitializeWardSpots();
                }

                return _WardSpots;
            }
        }

        public static List<WardSpot> SafeWardSpots
        {
            get
            {
                if(_SafeWardSpots == null)
                {
                    InitializeSafeWardSpots();
                }

                return _SafeWardSpots;
            }
        }

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
            }
            else if (skinName == PINK_WARD_NAME)
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

        public bool CheckRemove()
        {

            if (AliveTo == float.MaxValue &&
                ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(NetworkId) != null &&
                ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(NetworkId).IsDead)
            {
                AliveTo = Game.Time - 1.0f;
            }

            // Hold the object for a while after it's death
            // to ensure we don't re-add it while its getting reaped.
            return (Game.Time >= AliveTo + 5.0f);
        }

        public override int GetHashCode()
        {
            return NetworkId;
        }

        private static void InitializeWardSpots()
        {
            _WardSpots = new List<Vector3>();

            _WardSpots.Add(new Vector3(2823.37f, 7617.03f, 55.03f));    // Blue Golem
            _WardSpots.Add(new Vector3(7422.0f, 46.53f, 3282.0f));      // Blue Lizard
            _WardSpots.Add(new Vector3(10148.0f, 44.41f, 2839.0f));     // Blue Tri Bush
            _WardSpots.Add(new Vector3(6269.0f, 42.51f, 4445.0f));      // Blue Pass Bush
            _WardSpots.Add(new Vector3(7151.64f, 51.67f, 4719.66f));    // Blue River Entrance
            _WardSpots.Add(new Vector3(4354.54f, 53.67f, 7079.51f));    // Blue Round Bush
            _WardSpots.Add(new Vector3(4728.0f, 51.29f, 8336.0f));      // Blue River Round Bush
            _WardSpots.Add(new Vector3(6762.52f, 55.68f, 2918.75f));    // Blue Split Push Bush

            _WardSpots.Add(new Vector3(11217.39f, 54.87f, 6841.89f));   // Purple Golem
            _WardSpots.Add(new Vector3(6610.35f, 54.45f, 11064.61f));   // Purple Lizard
            _WardSpots.Add(new Vector3(3883.0f, 39.87f, 11577.0f));     // Purple Tri Bush
            _WardSpots.Add(new Vector3(7775.0f, 43.14f, 10046.49f));    // Purple Pass Bush
            _WardSpots.Add(new Vector3(6867.68f, 57.01f, 9567.63f));    // Purple River Entrance
            _WardSpots.Add(new Vector3(9720.86f, 54.85f, 7501.50f));    // Purple Round Bush
            _WardSpots.Add(new Vector3(9233.13f, -44.63f, 6094.48f));   // Purple River Round Bush
            _WardSpots.Add(new Vector3(7282.69f, 52.59f, 1148992.53f)); // Purple Split Push Bush

            _WardSpots.Add(new Vector3(10180.18f, -62.32f, 4969.32f));  // Dragon
            _WardSpots.Add(new Vector3(8875.13f, -64.07f, 5390.57f));   // Dragon Bush
            _WardSpots.Add(new Vector3(3920.88f, -60.42f, 9477.78f));   // Baron
            _WardSpots.Add(new Vector3(5017.27f, -62.70f, 8954.09f));   // Baron Bush

            _WardSpots.Add(new Vector3(12731.25f, 50.32f, 9132.66f));   // Purple Bot T2
            _WardSpots.Add(new Vector3(12731.25f, 50.32f, 9132.66f));   // Purple Bot T2
            _WardSpots.Add(new Vector3(9260.02f, 54.62f, 8582.67f));    // Purple Mid T1

            _WardSpots.Add(new Vector3(4749.79f, 53.559f, 5890.76f));   // Blue Mid T1
            _WardSpots.Add(new Vector3(5983.58f, 52.99f, 1547.98f));    // Blue Bot T2
            _WardSpots.Add(new Vector3(1213.70f, 58.77f, 5324.73f));    // Blue Top T2
        }

        private static void InitializeSafeWardSpots()
        {
            _SafeWardSpots = new List<WardSpot>();

            // Dragon -> Tri Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(9695.0f, 43.02f, 3465.0f),
                                            new Vector3(9843.38f, 43.02f, 3125.16f),
                                            new Vector3(9946.10f, 43.02f, 3064.81f),
                                            new Vector3(9595.0f, 43.02f, 3665.0f)));
            // Nashor -> Tri Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(4346.10f, 36.62f, 10964.81f),
                                            new Vector3(4214.93f, 36.62f, 11202.01f),
                                            new Vector3(4146.10f, 36.62f, 11314.81f),
                                            new Vector3(4384.36f, 36.62f, 10680.41f)));

            // Blue Top -> Solo Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(2349.0f, 44.20f, 10387.0f),
                                            new Vector3(2257.97f, 44.20f, 10783.37f),
                                            new Vector3(2446.10f, 44.20f, 10914.81f),
                                            new Vector3(2311.0f, 44.20f, 10185.0f)));

            // Blue Mid -> round Bush // Inconsistent Placement
            _SafeWardSpots.Add(new WardSpot(new Vector3(4946.52f, 54.71f, 6474.56f),
                                            new Vector3(4891.98f, 53.62f, 6639.05f),
                                            new Vector3(4546.10f, 53.78f, 6864.81f),
                                            new Vector3(5217.0f, 54.95f, 6263.0f)));

            // Blue Mid -> River Lane Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(5528.96f, 45.64f, 7615.20f),
                                            new Vector3(5688.96f, 45.64f, 7825.20f),
                                            new Vector3(5796.10f, 45.64f, 7914.81f),
                                            new Vector3(5460.13f, 45.64f, 7469.77f)));

            // Blue Lizard -> Dragon Pass Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(7745.0f, 47.71f, 4065.0f),
                                            new Vector3(7927.65f, 47.71f, 4239.77f),
                                            new Vector3(8146.10f, 47.71f, 4414.81f),
                                            new Vector3(7645.0f, 47.71f, 4015.0f)));

            // Purple Mid -> Round Bush // Inconsistent Placement
            _SafeWardSpots.Add(new WardSpot(new Vector3(9057.0f, 45.73f, 8245.0f),
                                            new Vector3(9230.7f, 66.39f, 7892.22f),
                                            new Vector3(9446.10f, 54.66f, 7814.81f),
                                            new Vector3(8895.0f, 54.89f, 8313.0f)));

            // Purple Mid -> River Round Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(9025.78f, 46.27f, 6591.64f),
                                            new Vector3(9200.08f, 43.21f, 6425.05f),
                                            new Vector3(9396.10f, 23.72f, 6264.81f),
                                            new Vector3(8795.0f, 56.11f, 6815.0f)));

            // Purple Mid -> River Lane Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(8530.27f, 46.98f, 6637.38f),
                                            new Vector3(8539.27f, 46.98f, 6637.38f),
                                            new Vector3(8396.10f, 46.98f, 6464.81f),
                                            new Vector3(8779.17f, 46.98f, 6804.70f)));

            // Purple Bottom -> Solo Bush
            _SafeWardSpots.Add(new WardSpot(new Vector3(11889.0f, 42.84f, 4205.0f),
                                            new Vector3(11974.23f, 42.84f, 3807.21f),
                                            new Vector3(11646.10f, 42.84f, 3464.81f),
                                            new Vector3(11939.0f, 42.84f, 4255.0f)));

            // Purple Lizard -> Nashor Pass Bush // Inconsistent Placement
            _SafeWardSpots.Add(new WardSpot(new Vector3(6299.0f, 45.47f, 10377.75f),
                                            new Vector3(6030.24f, 54.29f, 10292.37f),
                                            new Vector3(5846.10f, 53.94f, 10164.81f),
                                            new Vector3(6447.0f, 54.63f, 10463.0f)));
        }

        public static Vector3? FindNearestWardSpot(Vector3 cursorPosition)
        {
            Console.Out.WriteLine("My position is !!!" + cursorPosition);

            foreach (Vector3 wardPosition in WardSpots)
            {

                double dist = Math.Sqrt(Math.Pow(wardPosition.X - cursorPosition.X, 2) +
                             Math.Pow(wardPosition.Y - cursorPosition.Y, 2) +
                             Math.Pow(wardPosition.Z - cursorPosition.Z, 2));

                //Console.Out.WriteLine("\t" + wardPosition + "(" + dist.ToString() + ")");

                if(dist <= 250.0) 
                {
                    return wardPosition;
                }
            }

            return null;
        }

        public static void DrawWardSpots()
        {
            Drawing.DrawCircle(ObjectManager.Player.Position, WARD_INDICATOR_RADIUS, System.Drawing.Color.Red);
            foreach (Vector3 wardPos in WardSpots)
            {
                System.Drawing.Color wardColor = (Math.Sqrt(Math.Pow(wardPos.X - Game.CursorPos.X, 2) +
                                     Math.Pow(wardPos.Y - Game.CursorPos.Y, 2) +
                                     Math.Pow(wardPos.Z - Game.CursorPos.Z, 2)) <= 250.0) ? System.Drawing.Color.Red : System.Drawing.Color.LightBlue;

                //Console.Out.WriteLine("Drawing ward at " + wardPos.ToString());

                //float[] pos = Drawing.WorldToScreen(wardPos);
                //Drawing.DrawText(pos[0], pos[1], System.Drawing.Color.White, "HELLLLLLLLLLLLLLO");
                Drawing.DrawCircle(wardPos, WARD_INDICATOR_RADIUS, System.Drawing.Color.Red);
            }
        }

        public static SpellSlot FindWardSpellSlot(Obj_AI_Hero player)
        {
            foreach(InventorySlot slot in player.InventoryItems)
            {
                Console.Out.WriteLine("\tWard Id for" + slot.Name + " is " + slot.Id + " at slot number " + slot.Slot);

                if (slot.Id == (ItemId)2044)
                {
                    return ConvertSlotNumberToSpellSlot(slot.Slot);
                    
                }
            }

            return SpellSlot.Unknown;
        }

        private static SpellSlot ConvertSlotNumberToSpellSlot(int slotNumber)
        {
            switch (slotNumber)
            {
                case 0:
                    return SpellSlot.Item1;

                case 1:
                    return SpellSlot.Item2;
                case 2:
                    return SpellSlot.Item3;
                case 3:
                    return SpellSlot.Item4;
                case 4:
                    return SpellSlot.Item5;
                case 5:
                    return SpellSlot.Item6;
                default:
                    return SpellSlot.Unknown;
            }
        }
    }
}
