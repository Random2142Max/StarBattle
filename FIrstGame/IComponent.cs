using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FIrstGame
{
    public interface IComponent
    {
        virtual void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            throw new NotImplementedException();
        }
        virtual void Draw(SpriteBatch _spriteBatch, UserShips userShips)
        {
            throw new NotImplementedException();
        }

        virtual void Draw(SpriteBatch _spriteBatch, Bullets bullets)
        {
            throw new NotImplementedException();
        }
        virtual void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        virtual void Update(GameTime gameTime, UserShip userShip)
        {
            throw new NotImplementedException();
        }
        virtual void Update(GameTime gameTime, Bullets bullets)
        {
            throw new NotImplementedException();
        }
    }
}