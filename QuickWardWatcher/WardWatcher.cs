using LeagueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickWardWatcher
{
    internal class WardWatcher
    {
        private WardSet _GameWards;
        private float _LastUpdateTime;

        public WardWatcher()
        {
            _GameWards = new WardSet();

            Game.OnGameStart += OnGameStart;
            Game.OnGameUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;
            
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

        private void OnGameUpdate(EventArgs args)
        {
            SearchForNewWards();
        }

        private void OnDraw(EventArgs args)
        {
            _GameWards.DrawAll();
        }

        private void SearchForNewWards()
        {

            //foreach (Obj_AI_Base obj in ObjectManager.Get<Obj_AI_Base>())
            //{
            //    if (obj.Name.ToLower().Contains("ward"))
            //    {
            //        Console.WriteLine(obj.Name);
            //    }
            //}

            Parallel.ForEach(ObjectManager.Get<LeagueSharp.Obj_AI_Base>(), obj =>
            {


                if (obj.IsEnemy)
                {
                
                    if (obj.Name == Ward.GREEN_WARD_NAME || 
                        obj.Name == Ward.PINK_WARD_NAME ||
                        obj.Name == Ward.TRINKET_WARD_NAME)
                    {

                        if (_GameWards.Add(new Ward(obj.SkinName, obj.Position, obj.NetworkId)))
                        {
                            Console.Out.WriteLine("Ward added (" + DateTime.Now + ")");
                            Console.Out.WriteLine("\tskin name - " + obj.SkinName);
                            Console.Out.WriteLine("\tposition - " + obj.Position);
                            Console.Out.WriteLine("\tgame time - " + Game.Time);
                            Console.Out.WriteLine("\tnetwork id - " + obj.NetworkId);
                            Console.Out.WriteLine("-------------------------------\n");
                        }
                    }
                }
            });

            if (Game.Time - _LastUpdateTime >= 0.5f)
            {
                _LastUpdateTime = Game.Time;
                _GameWards.Update();
            }
        }
    }
}