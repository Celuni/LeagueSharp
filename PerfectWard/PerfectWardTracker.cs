using LeagueSharp;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PerfectWard
{
    class PerfectWardTracker
    {
        private const int VK_M = 77;
        const int WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101, WM_CHAR = 0x0102, WM_SYSKEYDOWN = 0x0104, WM_SYSKEYUP = 0x0105;

        public PerfectWardTracker()
        {
            Game.OnGameStart += OnGameStart;
            Game.OnWndProc += OnWndProc;
            Drawing.OnDraw += OnDraw;
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
 	        Console.Out.WriteLine("Captured M press!!!");
        }

        unsafe void OnWndProc(WndEventArgs args)
        {
            if (args.Msg == WM_KEYDOWN)
            {
                if (args.WParam == VK_M)
                {
                    Vector3? nearestWard = Ward.FindNearestWardSpot(ObjectManager.Player.Position);

                    if (nearestWard != null)
                    {
                         SpellSlot wardSpellSlot = Ward.FindWardSpellSlot(ObjectManager.Player);
                         
                         if (wardSpellSlot != SpellSlot.Unknown)
                         {
                             ObjectManager.Player.Spellbook.CastSpell(wardSpellSlot, (Vector3)nearestWard);
                         } 
                    }
                }
            }
        }

        private void OnGameStart(EventArgs args)
        {
            Game.PrintChat(
                string.Format(
                    "{0} v{1} loaded.",
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version
                    )
                );
        }

        private void OnDraw(EventArgs args)
        {
            Ward.DrawWardSpots();
        }
    }
}
