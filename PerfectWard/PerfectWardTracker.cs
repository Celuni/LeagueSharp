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
    internal class PerfectWardTracker
    {
        private const int VK_LBUTTON = 1;
        private const int WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101, WM_CHAR = 0x0102, WM_SYSKEYDOWN = 0x0104, WM_SYSKEYUP = 0x0105, WM_MOUSEDOWN = 0x201;
        private WardSpot _PutSafeWard;

        public PerfectWardTracker()
        {
            Game.OnGameStart += OnGameStart;
            Game.OnWndProc += OnWndProc;
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += OnDraw; 
        }

        void Game_OnGameUpdate(EventArgs args)
        {
            if (_PutSafeWard != null)
            {
                if (Math.Sqrt(Math.Pow(_PutSafeWard.ClickPosition.X - ObjectManager.Player.Position.X, 2) + Math.Pow(_PutSafeWard.ClickPosition.Y - ObjectManager.Player.Position.Y, 2)) <= 650.0)
                {
                    SpellSlot wardSpellSlot = Ward.FindWardSpellSlot(ObjectManager.Player);

                    if (wardSpellSlot != SpellSlot.Unknown)
                    {
                        ObjectManager.Player.Spellbook.CastSpell(wardSpellSlot, (Vector3)_PutSafeWard.ClickPosition);
                    }
                    _PutSafeWard = null;
                }
            }
        }

        public void OnWndProc(WndEventArgs args)
        {
            if (args.Msg == WM_MOUSEDOWN)
            {
                if (args.WParam == VK_LBUTTON)
                {
                    Vector3? nearestWard = Ward.FindNearestWardSpot(Drawing.ScreenToWorld(Game.CursorPos.X, Game.CursorPos.Y));

                    if (nearestWard != null)
                    {
                         SpellSlot wardSpellSlot = Ward.FindWardSpellSlot(ObjectManager.Player);
                         
                         if (wardSpellSlot != SpellSlot.Unknown)
                         {
                             ObjectManager.Player.Spellbook.CastSpell(wardSpellSlot, (Vector3)nearestWard);
                         } 
                    }

                    WardSpot nearestSafeWard = Ward.FindNearestSafeWardSpot(Drawing.ScreenToWorld(Game.CursorPos.X, Game.CursorPos.Y));

                    if (nearestSafeWard != null)
                    {
                        SpellSlot wardSpellSlot = Ward.FindWardSpellSlot(ObjectManager.Player);

                        if (wardSpellSlot != SpellSlot.Unknown)
                        {
                            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, nearestSafeWard.MagneticPosition);
                            _PutSafeWard = nearestSafeWard;
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
            Ward.DrawSafeWardSpots();
        }
    }
}
