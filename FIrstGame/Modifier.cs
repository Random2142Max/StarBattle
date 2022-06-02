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
        Speed,
        UnSpeed,
        SpeedFire,
        UnSpeedFire
    }
    public class Modifier
    {
        public Texture2D Texture { get; set; } // Текстура 
        public Vector2 Position; // Позиция 
        public float Speed { get; set; } // Скорость модификатора
        public Color Color { get; set; } // Цвет
        public int TypeModifier { get; set; }// Тип модификатора
    }

    public class Modifiers
    {
        //readonly ContentManager _contentManager;
        Modifier Modifier { get; set; }
        public Modifier CreateModifier(ContentManager contentManager)
        {
            var textF = new TextureForeground(contentManager);
            
            // Рандомизатор модификатора
            Modifier = new Modifier()
            {
                Color = Color.White,// Цвет по умолчанию
                Speed = 2f,// Стандартная скорость
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
                case 2:
                    Modifier.Texture = textF.SpeedFire;
                    break;
                case 3:
                    Modifier.Texture = textF.UnSpeedFire;
                    break;
            }

            return Modifier;
        }
        public Modifier GetModifier() => Modifier;
        public void Delete() => Modifier = null;
    }
}
//public Modifier(ContentManager contentManager)
//{
//    var textF = new TextureForeground(contentManager);
//    var userShip = new UserShip(textF.UserShip, contentManager);
//    Color = Color.White;// Цвет по умолчанию
//    Speed = 5f;// Стандартная скорость
//    // Рандомизатор модификатора
//    var rnd = new Random();
//    var rndModifier = rnd.Next(0, (int)TypeModifiers.countTypes);
//    TypeModifier = rndModifier;
//    switch (rndModifier)
//    {
//        case 0:
//            Texture = textF.Speed;
//            break;
//        case 1:
//            Texture = textF.UnSpeed;
//            break;
//    }
//}
