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
    public class Bullet : IComponent
    {

        #region Properties
        public Texture2D Texture { get; set; } // Текстура 

        public Vector2 Position; // Позиция 
        public float Speed { get; set; } // Скорость 
        public Color Color { get; set; } // Цвет

        public int DeltaTime; // Счётчик для паузы между выстрелами
        // Скорострельность объявляется, для использовании за конструктором
        public int UserFireRate; // Скорострельность игрока

        // Св-ва для обработки событий
        TextureForeground textF;
        UserShip userShip;
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
        public Bullet(ContentManager contentManager, UserShip _userShip)
        {
            textF = new TextureForeground(contentManager);
            userShip = _userShip;
            Position = new Vector2(
                userShip.Position.X + (textF.UserShip.Width / 2 - textF.Bullet.Width / 2),
                userShip.Position.Y - textF.Bullet.Height);
            Texture = textF.Bullet;
            Speed = 5f;//_speed;
            UserFireRate = _userShip.FireRate;
            Color = Color.White;// Цвет по умолчанию
        }
        #region Methods
        // Обновление снарядов
        public void Update(GameTime gameTime, Bullet bullet, int _deltaTime, Meteors meteors, Explosian explosian)
        {
            // Задрежка перед следующим выстрелом
            DeltaTime += UserFireRate;
            // Проверка на столкновения
            if (CollideBulletMeteor(this, meteors, explosian))
            {
                // Удаление снаряда
                bullet = null;
                //...
            }
        }
        #warning Почему-то работает криво
        protected bool CollideBulletMeteor(Bullet bullet, Meteors meteors, Explosian explosian)
        {
            var result = false;
            var bulletRect = bullet.Rectangle;
            foreach (var meteor in meteors)
            {
                var meteorRect = meteor.Rectangle;
                result = bulletRect.Intersects(meteorRect);
                if (result == true)
                {
                    // Взрыв с переопределением позиции метеорита
                    explosian.ExplosianMeteor(meteor);
                    meteor.Position = NewMeteorPosition(meteor);
                    explosian.DefaultExplosianPosition();
                }
            }

            return result;
        }
        public Vector2 NewMeteorPosition(Meteor meteor)
        {
            var rnd = new Random();
            var newMeteorWidthPosition = rnd.Next(0, 1980 - meteor.Texture.Width);
            var newMeteorPosition = new Vector2(newMeteorWidthPosition, -textF.Meteor.Height);
            return meteor.Position = newMeteorPosition;
        }
        public void Draw(SpriteBatch _spriteBatch, Bullets bullets)
        {
            // Снаряд
            foreach (var bullet in bullets)
            {
                _spriteBatch.Draw(
                    bullet.Texture,
                    bullet.Position,
                    bullet.Color);
            }
        }

        #endregion
    }

    public class Bullets : LinkedList<Bullet>
    {
        readonly ContentManager _contentManager;

        public Bullets(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }
        public void AddBullet(UserShip userShip)
        {
            this.AddLast(new Bullet(_contentManager, userShip));
        }
        //public void DeleteBullet()
        //{
        //    this.RemoveFirst();
        //}
    }
}
