using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIrstGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        // Текстуры фона игры
        TextureBackground textB;
        // Текстуры игрового поля игры
        TextureForeground textF;
        // Корабль игрока
        UserShips userShips;
        // Метеориты
        Meteors meteors = new Meteors();
        //Point meteorSize; // Размер српайта метеорита
        // Модификатор
        Modifiers modifiers;
        int modifierTimer = 0;
        // Временный модификатор
        //Modifier _modifier;
        // Взрыв
        Vector2 DefaultExplosianPosition; // Позиция метеорита по умлочанию
        Vector2 explosianPosition; // Позиция взрыва
        // Снаряды
        Bullets bullets;
        // Счётчик снарядов
        int deltaTime = 0;

        // Картинка корабля, как спрайт
        // Point spriteSize = new Point(1,1);

        // Фон игры
        int windowWidthSize = 1920;
        int windowHeightSize = 1025;
        Rectangle backgroundGameWindow;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Частота кадров
            TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 10);
        }

        // Выполняет начальную инициализацию игры
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        // Загружает ресурсы игры
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Текстуры
            textB = new TextureBackground(Content);
            textF = new TextureForeground(Content);
            // Кораблик
            userShips = new UserShips(Content);
            // Снарядик
            bullets = new Bullets(Content);
            // Модификаторы
            modifiers = new Modifiers();
            // Модификатор
            //_modifier = new Modifier(Content);

            // Фон игры
            backgroundGameWindow = new Rectangle(0, 0,
                windowWidthSize,
                windowHeightSize
                );
            // Переменные для уменьшения кода
            var centerPoint = windowWidthSize / 2;
            // Начальные позиции
            var startUserPosition = new Vector2(
                centerPoint,
                windowHeightSize
                );
            DefaultExplosianPosition = new Vector2(
                0,
                -textF.Explosian.Height
                );
            // Спавн метеоритов в кол-ве до 10
            RandomSpawnMeteors(meteors);

            userShips.AddUserShip(textF.UserShip);

            explosianPosition = DefaultExplosianPosition;

        }

        // Обновляет состояние игры, управляет ее логикой
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Движения игрока
            KeyboardState keyboardState = Keyboard.GetState();
            var userShip = userShips.First;
            modifierTimer++;

            if (keyboardState.IsKeyDown(Keys.Up) & CheckingForWallsUserShip())
                userShip.Value.Position.Y -= userShip.Value.Speed;
            if (keyboardState.IsKeyDown(Keys.Down) & CheckingForWallsUserShip())
                userShip.Value.Position.Y += userShip.Value.Speed;
            if (keyboardState.IsKeyDown(Keys.Right) & CheckingForWallsUserShip())
                userShip.Value.Position.X += userShip.Value.Speed;
            if (keyboardState.IsKeyDown(Keys.Left) & CheckingForWallsUserShip())
                userShip.Value.Position.X -= userShip.Value.Speed;
            
            // Спавн рандомного модификаторы
            if (modifierTimer == 250)
            {
                modifiers.CreateModifier(Content);
                RandomPositionModifier(modifiers.GetModifier());
                //modifiers.CreateModifier(Content);
                //var modifier = modifiers.First;
                //RandomPositionModifier(modifier.Value);
            }
            // Доделать обработку падения текстур
            if (CheckingForWallsModifier())
            {
                modifierTimer = 0;
                modifiers.FirstOrDefault();
            }

            // Проверка на столкновение
            if (CollideUserModifier())
            {
                switch (modifiers.First().TypeModifier)
                {
                    case 0:
                        userShip.Value.Speed += 1f;
                        break;
                    case 1:
                        userShip.Value.Speed -= 1f;
                        break;
                }
            }
            if (!CollideUserMeteor())
            {
                // Движение метеорита
                foreach (var meteor in meteors)
                    meteor.Position.Y += meteor.Speed;
            }
            if (!CollideBulletMeteor())
            {
                // Тут движение снаряда выпущенного от игрока
                foreach (var bullet in bullets)
                {
                    bullet.Position.Y -= bullet.Speed;
                }
            }

            // Создание снарядов
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (deltaTime == 0)
                {
                    bullets.AddBullet(textF.Bullet);
                    var bullet = bullets.LastOrDefault();
                    bullet.Position = new Vector2(
                        userShip.Value.Position.X + (textF.UserShip.Width / 2 - textF.Bullet.Width / 2),
                        userShip.Value.Position.Y - textF.Bullet.Height);
                    deltaTime += 25;
                }
                else
                {
                    deltaTime--;
                }
            }

            // End
            base.Update(gameTime);
        }

        // Выполняет отрисовку на экране
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);// Синий фон
            // Размеры окна игры
            this._graphics.PreferredBackBufferWidth = windowWidthSize;
            this._graphics.PreferredBackBufferHeight = windowHeightSize;
            _graphics.ApplyChanges();

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // Фон игры
            _spriteBatch.Draw(
                textB.BackgroundSpace1,
                backgroundGameWindow,
                Color.White);

            // Корабль игрока
            var userShip = userShips.First;
            _spriteBatch.Draw(
                userShip.Value.Texture,
                userShip.Value.Position,
                userShip.Value.Color);

            // Создание временного модификатора
            if (modifiers.Count != 0)
            {
                _modifier = modifiers.First();
                _spriteBatch.Draw(
                    _modifier.Texture,
                    _modifier.Position,
                    _modifier.Color);
            }

            // Метеориты
            foreach (var meteor in meteors)
                _spriteBatch.Draw(
                    meteor.Texture,
                    meteor.Position,
                    meteor.Color);

            // Взрыв
            _spriteBatch.Draw(
                textF.Explosian,
                explosianPosition,
                Color.White);

            // Снаряд
            foreach (var bullet in bullets)
            {
                _spriteBatch.Draw(
                    bullet.Texture,
                    bullet.Position,
                    bullet.Color);
            }

            _spriteBatch.End();

            // End
            base.Draw(gameTime);
        }
        // Рандомизация траектории падения модификатора
        void RandomPositionModifier(Modifier modifier)
        {
            var rnd = new Random();
            var rndModifierPosition = rnd.Next(0,1980-modifier.Texture.Width);
            modifier.Position = new Vector2(rndModifierPosition,-modifier.Texture.Height);
        }

        // Рандомизация метеоритов
        void RandomSpawnMeteors(Meteors meteors)
        {
            var rnd = new Random();
            var rndMeteorsCount = rnd.Next(1, 11);
            for (int i = 0; i < rndMeteorsCount; i++)
            {
                var rnd1 = new Random();
                var rndMeteorsPosition = rnd1.Next(0, 1980 - textF.Meteor.Width);
                meteors.AddMeteor(
                    textF.Meteor,
                    new Vector2(
                        rndMeteorsPosition,
                        -textF.Meteor.Height
                        )
                    );
            }
        }
        protected bool CheckingForWallsModifier()
        {
            var result = false;
            var modifier = modifiers.First;
            if (modifier != null)
                if ((modifier.Value.Position.Y + modifier.Value.Texture.Height) >= windowHeightSize)
                    result = true;
            return result;
        }

        // Проверка на наличие стен у коробля игрока
        public bool CheckingForWallsUserShip()
        {
            var userShip = userShips.First;
            bool IsNextToWall = true;
            if (userShip.Value.Position.X < 0)
            {
                IsNextToWall = false;
                userShip.Value.Position.X = 0;
            }
            if (userShip.Value.Position.Y < 0)
            {
                IsNextToWall = false;
                userShip.Value.Position.Y = 0;
            }
            if ((userShip.Value.Position.X + textF.UserShip.Width) > windowWidthSize)
            {
                IsNextToWall = false;
                userShip.Value.Position.X = windowWidthSize - textF.UserShip.Width;
            }
            if ((userShip.Value.Position.Y + textF.UserShip.Height) > windowHeightSize)
            {
                IsNextToWall = false;
                userShip.Value.Position.Y = windowHeightSize - textF.UserShip.Height;
            }
            return IsNextToWall;
        }
        protected bool CollideUserModifier()
        {
            var result = false;
            var userShip = userShips.First;
            var modifier = modifiers.First;
            Rectangle userShipRect = new Rectangle(
                    (int)userShip.Value.Position.X,
                    (int)userShip.Value.Position.Y,
                    userShip.Value.Texture.Width,
                    userShip.Value.Texture.Height);
            if (modifier != null)
            {
                Rectangle modifierRect = new Rectangle(
                        (int)modifier.Value.Position.X,
                        (int)modifier.Value.Position.Y,
                        modifier.Value.Texture.Width,
                        modifier.Value.Texture.Height);
                result = userShipRect.Intersects(modifierRect);
            }
            return result;
        }
        protected bool CollideUserMeteor()
        {
            var result = false;
            var userShip = userShips.First;
            Rectangle userShipRect = new Rectangle(
                    (int)userShip.Value.Position.X,
                    (int)userShip.Value.Position.Y,
                    userShip.Value.Texture.Width,
                    userShip.Value.Texture.Height);
            foreach (var meteor in meteors)
            {
                Rectangle meteorRect = new Rectangle(
                    (int)meteor.Position.X,
                    (int)meteor.Position.Y,
                    meteor.Texture.Width,
                    meteor.Texture.Height);
                result = userShipRect.Intersects(meteorRect);
                if (result == true)
                {
                    userShip.Value.Speed = 0;
                    explosianPosition = userShip.Value.Position;
                    meteor.Speed = 0;
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
        protected bool CollideBulletMeteor()
        {
            var result = false;
            Bullet newBullet = null;
            //var delay = 0; прерывание
            foreach (var meteor in meteors)
            {
                Rectangle meteorRect = new Rectangle(
                        (int)meteor.Position.X,
                        (int)meteor.Position.Y,
                        textF.Meteor.Width,
                        textF.Meteor.Height
                    );
                foreach (var bullet in bullets)
                {
                    Rectangle bulletRect = new Rectangle(
                            (int)bullet.Position.X,
                            (int)bullet.Position.Y,
                            textF.Bullet.Width,
                            textF.Bullet.Height
                        );
                    result = bulletRect.Intersects(meteorRect);
                    if (result == true)
                    {
                        // Сохранение удаляемого снаряда
                        newBullet = bullet;
                        // Взрыв с переопределением позиции метеорита
                        explosianPosition = new Vector2(
                            meteor.Position.X - textF.Meteor.Width / 2,
                            meteor.Position.Y);
                        meteor.Position = NewMeteorPosition(meteor);
                        // Прерывания для отображения взрыва
                        //if (delay == 0)
                        //    delay += 25;
                        //else
                        //    delay--;
                        //if (delay == 5)
                        explosianPosition = DefaultExplosianPosition;
                    }
                }
            }
            // Удаление снаряда
            bullets.Remove(newBullet);

            return result;
        }
    }
}
