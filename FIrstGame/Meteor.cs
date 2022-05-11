using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FIrstGame
{
    // Описание класса Метеорит
    public class Meteor
    {
        public Texture2D Texture { get; set; } // Текстура метеорита
        public Vector2 Position; // Позиция метеорита
        public float Speed { get; set; } // Скорость меторита
        public Color Color = Color.White;// Цвет метеорита
        public Meteor(Texture2D _texture, Vector2 _position)
        {
            Texture = _texture;
            Position = _position;
            Speed = 1f;// Скорость метеорита по умолчанию
            Color = Color.White;// Цвет по умолчанию
        }
    }
    // Взаимодейтсвия с классом Метеорит
    class Meteors : LinkedList<Meteor>
    {
        public void AddMeteor(Texture2D texture, Vector2 position)
        {
            this.AddLast(new Meteor(texture, position));
        }
        public void DeleteMeteor()
        {
            this.RemoveFirst();
        }
    }
}
