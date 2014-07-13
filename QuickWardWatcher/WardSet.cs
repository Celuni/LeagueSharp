using LeagueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickWardWatcher
{
    public class WardSet : HashSet<Ward>
    {
        public virtual bool Add(Ward ward) {
            return base.Add(ward);
        }

        public void Update()
        {
            this.RemoveWhere(ward => ward.CheckRemove()); 
        }

        public void DrawAll()
        {
            foreach(Ward ward in this)
            {
                if (Game.Time <= ward.AliveTo) // The set may contain wards that are dead
                {
                    ward.Draw();
                }
            }
        }
    }
}
