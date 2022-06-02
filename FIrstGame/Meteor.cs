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
    // Описание класса Метеорит
    public class Meteor : IComponent
    {
        #region Properties
        public Texture2D Texture { get; set; } // Текстура метеорита
        public Vector2 Position; // Позиция метеорита
        public float Speed { get; set; } // Скорость меторита
        public Color Color = Color.White;// Цвет метеорита

        // Св-ва для обработки событий
        TextureBackground textB;
        TextureForeground textF;
        public Meteor(ContentManager contentManager,Vector2 _position)
        {
            textF = new TextureForeground(contentManager);
            textB = new TextureBackground(contentManager);
            Texture = textF.Meteor;
            Position = _position;
            Speed = 1f;// Скорость метеорита по умолчанию
            Color = Color.White;// Цвет по умолчанию
        }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Texture.Width,
                    Texture.Height);
            }
        }
        #endregion

        #region Methods

        public void Update(Meteor meteor, UserShip userShip, Explosian explosian)
        {
            // Создание метеоритов
            if (!CollideUserMeteor(meteor, userShip, explosian))
            {
                // Движение метеорита
                meteor.Position.Y += meteor.Speed;
            }
            // Переопределения места метеорита, если с ним ничего не случилось
            CollideMeteorEnd(meteor);
        }
        public void Draw(SpriteBatch _spriteBatch, Meteor meteor)
        {
            // Отрисовка метеоритов
            _spriteBatch.Draw(
                meteor.Texture,
                meteor.Position,
                meteor.Color);
        }
        protected bool CollideUserMeteor(Meteor meteor, UserShip userShip, Explosian explosian)
        {
            var result = false;
            result = userShip.Rectangle.Intersects(meteor.Rectangle);
            if (result == true)
            {
                userShip.Speed = 0;
                // Взрыв поверх игрока
                explosian.Update(userShip.Position);
                meteor.Speed = 0;
            }
            return result;
        }
        protected bool CollideMeteorEnd(Meteor meteor)
        {
            var result = false;
            if (meteor.Position.Y == textB.windowHeightSize)
            {
                RandomSpawnMeteor(meteor);
                result = true;
            }
            return result;
        }
        void RandomSpawnMeteor(Meteor meteor)
        {
            var rnd = new Random().Next(0, textB.windowWidthSize - meteor.Texture.Width);
            meteor.Position = new Vector2(rnd, -meteor.Texture.Height);
        }
        public Vector2 NewMeteorPosition(Meteor meteor)
        {
            var rnd = new Random();
            var newMeteorWidthPosition = rnd.Next(0, 1980 - meteor.Texture.Width);
            var newMeteorPosition = new Vector2(newMeteorWidthPosition, -textF.Meteor.Height);
            return meteor.Position = newMeteorPosition;
        }
        #endregion
    }
    // Взаимодейтсвия с классом Метеорит
    public class Meteors : LinkedList<Meteor>
    {
        public void AddMeteor(ContentManager contentManager, Vector2 position)
        {
            this.AddLast(new Meteor(contentManager,position));
        }
        public void DeleteMeteor()
        {
            this.RemoveFirst();
        }
    }
}
