using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIrstGame
{
    public class Button : IComponent
    {
        #region Fields
        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private SpriteFont _font;
        private bool _isMovering;
        private Texture2D _texture;
        #endregion

        #region Properties
        public event EventHandler Click;
        public bool Clicked { get; set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    _texture.Width,
                    _texture.Height);
            }
        }
        public string Text { get; set; }
        #endregion

        #region Methods
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Yellow;
            
        }

        //public override
        void IComponent.Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            var colour = Color.White;

            if (_isMovering)
                colour = Color.Gray;
            _spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                _spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        //public override
        void IComponent.Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(
                _currentMouse.X,
                _currentMouse.Y,
                1,
                1);

            _isMovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isMovering = true;

                if (_currentMouse.LeftButton == ButtonState.Pressed)
                //&& _currentMouse.LeftButton == ButtonState.Released
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
