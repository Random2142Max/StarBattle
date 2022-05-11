using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace FIrstGame
{
    // Описание класса Bullet
    public class Bullet
    {
        public Texture2D Texture { get; set; } // Текстура 
        public Vector2 Position; // Позиция 
        public float Speed { get; set; } // Скорость 
        public Color Color { get; set; } // Цвет 
        public Bullet(Texture2D _texture, ContentManager contentManager)
        {
            var textF = new TextureForeground(contentManager);
            var userShip = new UserShip(textF.UserShip,contentManager);
            Position = new Vector2(
                userShip.Position.X / 2,
                userShip.Position.Y - textF.Bullet.Height);//_position;
            Texture = _texture;
            Speed = 5f;//_speed;
            Color = Color.White;// Цвет по умолчанию
        }
    }

    class Bullets : LinkedList<Bullet>
    {
        readonly ContentManager _contentManager;

        public Bullets(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        public void AddBullet(Texture2D texture)
        {
            this.AddLast(new Bullet(texture, _contentManager));
        }
        //public void DeleteBullet()
        //{
        //    this.RemoveFirst();
        //}
    }
}
