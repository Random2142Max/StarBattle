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

        private Texture2D _backgroundMenu;
        private List<IComponent> _gameComponents;

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
        
        // Хранение модификаторов
        Modifiers modifiers;
        // Счётчик времени перед спавном модификатора
        int modifierTimer = 0;

        // Временный модификатор
        Modifier _modifier;

        // Взрыв
        Explosians explosians;

        // Снаряды
        Bullets bullets;

        // Счётчик снарядов
        int DeltaTime = 0;

        // Старт игры
        bool IsStartGame = false;

        // Картинка корабля, как спрайт
        // Point spriteSize = new Point(1,1);

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
            IsMouseVisible = true;

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
            // Взрыв
            explosians = new Explosians(Content);

            // Фон игры
            backgroundGameWindow = new Rectangle(0, 0,
                textB.windowWidthSize,
                textB.windowHeightSize
                );
            
            // Переменные для уменьшения кода
            var centerPoint = textB.windowWidthSize / 2;
            // Начальные позиции
            var startUserPosition = new Vector2(
                centerPoint,
                textB.windowHeightSize
                );
            
            var mainBWP = textB.windowWidthSize / 2 - textB.Button.Width / 2;
            _backgroundMenu = textB.BackgroundMenu;

            // Кнопки
            var startButton = new Button(
                textB.Button,
                textB.FontText)
            {
                Position = new Vector2(mainBWP, 400),
                Text = "Start"
            };
            startButton.Click += StartButton_Click;

            var quitButton = new Button(
                textB.Button,
                textB.FontText)
            {
                Position = new Vector2(mainBWP, 700),
                Text = "Quit"
            };
            quitButton.Click += QuitButton_Click;

            // _gameComponents, Лист с кнопками
            _gameComponents = new List<IComponent>()
            {
                startButton,
                quitButton,
            };

            // Спавн метеоритов в кол-ве: 10 - 20
            RandomSpawnMeteors(meteors);
            // Создание кораблика для игрока
            userShips.AddUserShip();
            // Создание взрыва
            explosians.AddExplosian();
            
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            IsStartGame = true;
        }

        // Обновляет состояние игры, управляет ее логикой
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            // Игрок
            var userShip = userShips.First();
            // Вызрыв
            var explosian = explosians.First();
            
            // Обновление меню
            if (!IsStartGame)
                foreach (var component in _gameComponents)
                    component.Update(gameTime);

            // Вызов клавиатуры для провреки
            KeyboardState keyboardState = Keyboard.GetState();

            // Проверка на столкновения
            foreach (var ship in userShips)
                ship.Update(keyboardState, ship);

            // Создание снарядов
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (DeltaTime == 0)
                {
                    // Создание снарядов
                    bullets.AddBullet(userShip);
                    foreach (var bullet in bullets)
                    {
                        bullet.Update(gameTime, bullet, DeltaTime, meteors, explosian);
                        DeltaTime = bullet.DeltaTime;
                    }
                    // Удаление снарядов
                    foreach (var e in bullets)
                        if (e ==  null)
                            bullets.Remove(e);
                    
                }
                else
                {
                    DeltaTime--;
                }
            }

            // Спавн рандомного модификаторы
            if (modifierTimer == 250)
            {
                if (_modifier == null)
                {
                    _modifier = modifiers.CreateModifier(Content);
                    RandomPositionModifier(_modifier);
                }
                else
                    _modifier.Position.Y += _modifier.Speed;
            }
            else
                modifierTimer++;

            // Удаление модификатора при достижении границы
            if (CollideWallsModifier())
            {
                ResetModifier();
            }

            // Проверка на столкновение модификатора и игрока
            if (CollideUserModifier())
            {
                switch (_modifier.TypeModifier)
                {
                    case 0:
                        userShip.Speed += 1f;
                        break;
                    case 1:
                        userShip.Speed -= 1f;
                        break;
                    case 2:
                        if (userShip.FireRate != 0)
                            userShip.FireRate -= 7;
                        break;
                    case 3:
                        userShip.FireRate += 7;
                        break;
                }
                ResetModifier();
            }

            // Движение метеорита
            foreach (var meteor in meteors)
                meteor.Update(meteor, userShip, explosian);

            // Движение снарядов игрока
            foreach (var bullet in bullets)
                bullet.Position.Y -= bullet.Speed;

            // End
            base.Update(gameTime);
        }

        // Выполняет отрисовку на экране
        protected override void Draw(GameTime gameTime)
        {
            // Размеры окна игры
            this._graphics.PreferredBackBufferWidth = textB.windowWidthSize;
            this._graphics.PreferredBackBufferHeight = textB.windowHeightSize;
            _graphics.ApplyChanges();
            //GraphicsDevice.Clear(_backgroundColor);// Синий фон

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (!IsStartGame)
            {
                //Фон игры
                _spriteBatch.Draw(
                    textB.BackgroundMenu,
                    backgroundGameWindow,
                    Color.White);
                // Меню
                foreach (var component in _gameComponents)
                    component.Draw(gameTime, _spriteBatch);
            }
            else
            {
                //Фон игры
                _spriteBatch.Draw(
                    textB.BackgroundSpace1,
                    backgroundGameWindow,
                    Color.White);

                foreach (var userShip in userShips)
                    userShip.Draw(_spriteBatch, userShip);

                // Создание временного модификатора
                if (_modifier != null)
                {
                    _spriteBatch.Draw(
                        _modifier.Texture,
                        _modifier.Position,
                        _modifier.Color);
                }

                // Метеориты
                foreach (var meteor in meteors)
                    meteor.Draw(_spriteBatch, meteor);

                // Взрыв
                foreach (var explosian in explosians)
                    explosian.Draw(_spriteBatch, explosian);

                // Выстрелы
                foreach (var bullet in bullets)
                    bullet.Draw(_spriteBatch, bullets);
            }
            
            _spriteBatch.End();

            // End
            base.Draw(gameTime);
        }

        void ResetModifier()
        {
            modifierTimer = 0;
            _modifier = null;
            modifiers.Delete();
        }

        // Рандомизация траектории падения модификатора
        void RandomPositionModifier(Modifier modifier)
        {
            var rndModifierPosition = new Random().Next(0,1980-modifier.Texture.Width);
            modifier.Position = new Vector2(rndModifierPosition,-modifier.Texture.Height);
        }

        // Рандомизация метеоритов
        void RandomSpawnMeteors(Meteors meteors)
        {
            // Спавн рандомного кол-ва метеоритов
            var rndMeteorsCount = new Random().Next(10, 31);
            for (int i = 0; i < rndMeteorsCount; i++)
            {
                // Рандомный множитель спавна по вертикале
                var rndMeteorsSpawnHeight = new Random().Next(1, 6);
                // Рандомный спавн метеорита по горизонтале
                var rndMeteorsPosition = new Random().Next(0, 1980 - textF.Meteor.Width);
                meteors.AddMeteor(
                    Content,
                    new Vector2(
                        rndMeteorsPosition,
                        -textF.Meteor.Height*rndMeteorsSpawnHeight
                        )
                    );
            }
        }

        protected bool CollideWallsModifier()
        {
            var result = false;
            if (_modifier != null)
                if ((_modifier.Position.Y + _modifier.Texture.Height) >= textB.windowHeightSize)
                    result = true;
            return result;
        }

        protected bool CollideUserModifier()
        {
            var result = false;
            var userShip = userShips.First;
            //var modifier = modifiers.First;
            Rectangle userShipRect = new Rectangle(
                    (int)userShip.Value.Position.X,
                    (int)userShip.Value.Position.Y,
                    userShip.Value.Texture.Width,
                    userShip.Value.Texture.Height);
            if (_modifier != null)
            {
                Rectangle modifierRect = new Rectangle(
                        (int)_modifier.Position.X,
                        (int)_modifier.Position.Y,
                        _modifier.Texture.Width,
                        _modifier.Texture.Height);
                result = userShipRect.Intersects(modifierRect);
            }
            return result;
        }
    }
}
