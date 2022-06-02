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
    public class Explosian : IComponent
    {

        #region Properties
        public Texture2D Texture { get; set; } // Текстура 

        public Vector2 Position; // Позиция 
        public Color Color { get; set; } // Цвет

        // Св-ва для обработки событий
        TextureForeground textF;
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
        public Explosian(ContentManager contentManager)
        {
            textF = new TextureForeground(contentManager);
            Texture = textF.Explosian;
            Position = new Vector2(0, -textF.Explosian.Height);
            Color = Color.White;// Цвет по умолчанию
        }
        #region Methods
        // Обновляет позицию взрыва при контакте
        public void Update(Vector2 newPosition)
        {
            this.Position = newPosition;
        }
        // Обновление позиции взрыва при контакте снарядов с метеоритом
        public void ExplosianMeteor(Meteor meteor)
        {
            this.Position = new Vector2(
                            meteor.Position.X - textF.Meteor.Width / 2,
                            meteor.Position.Y);
        }
        // Позиция взрыва по умолчанию
        public void DefaultExplosianPosition()
        {
            this.Position = new Vector2(0, -textF.Explosian.Height);
        }
        // Отрисовка взрыва
        public void Draw(SpriteBatch _spriteBatch, Explosian explosian)
        {
            _spriteBatch.Draw(
                explosian.Texture,
                explosian.Position,
                explosian.Color);
        }
        #endregion
    }
    public class Explosians : LinkedList<Explosian>
    {
        readonly ContentManager _contentManager;

        public Explosians(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        public void AddExplosian()
        {
            this.AddLast(new Explosian(_contentManager));
        }
        //public void DeleteExplosian()
        //{
        //    this.RemoveFirst();
        //}
    }
}
