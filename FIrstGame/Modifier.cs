using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FIrstGame
{
    enum TypeModifiers
    {
        speed,
        unspeed
    }

    public class Modifier
    {
        public Texture2D Texture { get; set; } // Текстура 

        public Vector2 Position; // Позиция 

        public float Speed { get; set; } // Скорость модификатора

        public Color Color { get; set; } // Цвет

        public int TypeModifier { get; set; }// Тип модификатора
    }

    class Modifiers
    {
        Modifier Modifier { get; set; }

        public Modifier CreateModifier(ContentManager contentManager)
        {
            var textF = new TextureForeground(contentManager);

            // Рандомизатор модификатора
            Modifier = new Modifier()
            {
                Color = Color.White,// Цвет по умолчанию
                Speed = 5f,// Стандартная скорость
                TypeModifier = new Random().Next(0, Enum.GetNames(typeof(TypeModifiers)).Length)
            };

            switch (Modifier.TypeModifier)
            {
                case 0:
                    Modifier.Texture = textF.Speed;
                    break;
                case 1:
                    Modifier.Texture = textF.UnSpeed;
                    break;
            }

            return Modifier;
        }

        public Modifier GetModifier() => Modifier;

        public void Delete() => Modifier = null;
    }
}
