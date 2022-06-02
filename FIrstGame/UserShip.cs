using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FIrstGame
{
    public class UserShip : IComponent
    {
        #region Properties
        public Texture2D Texture { get; set; } // Текстура игрока
        public Color Color { get; set; } // Цвет
        public float Speed { get; set; }  // Скорость игрока

        public int FireRate; // Скорострельность игрока
        public Vector2 Position;// Позиция игрока

        // Св-ва для обработки событий
        TextureForeground textF;
        TextureBackground textB;
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
        public UserShip(ContentManager contentManager)
        {
            textF = new TextureForeground(contentManager);
            textB = new TextureBackground(contentManager);
            // monitor - используется для определения стартовой позиция игрока в начале игры, относительно окна
            var monitor = new Vector2(
                System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width,
                System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height
                );
            Position = new Vector2(monitor.X/2, monitor.Y - textF.UserShip.Height);
            Texture = textF.UserShip;
            Speed = 5f;
            FireRate = 35;
            Color = Color.White;
        }
        // override
        public void Update(KeyboardState keyboardState, UserShip userShip)
        {
            if (CollideWallsUserShip(userShip))
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                    userShip.Position.Y -= userShip.Speed;
                if (keyboardState.IsKeyDown(Keys.Down))
                    userShip.Position.Y += userShip.Speed;
                if (keyboardState.IsKeyDown(Keys.Right))
                    userShip.Position.X += userShip.Speed;
                if (keyboardState.IsKeyDown(Keys.Left))
                    userShip.Position.X -= userShip.Speed;
            }
        }
        // override
        public void Draw(SpriteBatch _spriteBatch, UserShip userShip)
        {
            // Корабль игрока
            _spriteBatch.Draw(
                userShip.Texture,
                userShip.Position,
                userShip.Color);
        }
        // Проверка на наличие стен у коробля игрока
        public bool CollideWallsUserShip(UserShip userShip)
        {
            bool IsNextToWall = true;

            if (userShip.Position.X < 0)
            {
                IsNextToWall = false;
                userShip.Position.X = 0;
            }
            if (userShip.Position.Y < 0)
            {
                IsNextToWall = false;
                userShip.Position.Y = 0;
            }
            if ((userShip.Position.X + userShip.Texture.Width) > textB.windowWidthSize)
            {
                IsNextToWall = false;
                userShip.Position.X = textB.windowWidthSize - userShip.Texture.Width;
            }
            if ((userShip.Position.Y + userShip.Texture.Height) > textB.windowHeightSize)
            {
                IsNextToWall = false;
                userShip.Position.Y = textB.windowHeightSize - userShip.Texture.Height;
            }
            return IsNextToWall;
        }
    }

    public class UserShips : LinkedList<UserShip>
    {
        readonly ContentManager _contentManager;

        public UserShips(ContentManager contentManager)
        {
            _contentManager  = contentManager;
        }

        public void AddUserShip()
        {
            this.AddLast(new UserShip(_contentManager));
        }
        public void RemoveUserShip()
        {
            this.RemoveLast();
        }
    }
}
