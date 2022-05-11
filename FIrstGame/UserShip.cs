using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FIrstGame
{
    public class UserShip
    {
        public Vector2 Position; // Позиция игрока
        public Texture2D Texture { get; set; } // Текстура игрока
        //public Point shipSize; // Размер спрайта игрока
        public float Speed { get; set; }  // Скорость игрока

        public Color Color { get; set; }// Цвет 
        public UserShip(Texture2D _texture, ContentManager contentManager)
        {
            TextureForeground textF = new TextureForeground(contentManager);
            var monitor = new Vector2(
                System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width,
                System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
                );
            Position = new Vector2(monitor.X/2, monitor.Y - textF.UserShip.Height);
            Texture = textF.UserShip;
            Speed = 5f;
            Color = Color.White;
        }
    }

    public class UserShips : LinkedList<UserShip>
    {
        readonly ContentManager _contentManager;

        public UserShips(ContentManager contentManager)
        {
            _contentManager  = contentManager;
        }

        public void AddUserShip(Texture2D texture)
        {
            this.AddLast(new UserShip(texture,_contentManager));
        }
        public void RemoveUserShip()
        {
            this.RemoveLast();
        }
    }
}
