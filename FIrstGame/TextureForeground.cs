using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace FIrstGame
{
    public class TextureForeground
    {

        // Спрайты
        public Texture2D Meteor { get; set; } // Спрайт метеорита
        public Texture2D UserShip { get; set; } // Спрайт игрока
        public Texture2D Explosian { get; set; } // Спрайт взрыва
        public Texture2D Bullet { get; set; } // Спрайт снаряда
        public Texture2D Speed { get; set; } // Спрайт баффа скорости
        public Texture2D UnSpeed { get; set; } // Спрайт дебаффа скорости
        public Texture2D SpeedFire { get; set; } // Спрайт баффа скорострельности
        public Texture2D UnSpeedFire { get; set; } // Спрайт дебаффа скорострельности
        public TextureForeground(ContentManager contentManager)
        {
            UserShip = contentManager.Load<Texture2D>("Image/MainShip_Move_1");
            Meteor = contentManager.Load<Texture2D>("Image/Meteor");
            Explosian = contentManager.Load<Texture2D>("Image/Explosian");
            Bullet = contentManager.Load<Texture2D>("Image/Bullet");
            Speed = contentManager.Load<Texture2D>("ModifierImage/Speed");
            UnSpeed = contentManager.Load<Texture2D>("ModifierImage/UnSpeed");
            SpeedFire = contentManager.Load<Texture2D>("ModifierImage/SpeedFire");
            UnSpeedFire = contentManager.Load<Texture2D>("ModifierImage/UnSpeedFire");
        }
    }
}
