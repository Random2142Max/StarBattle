using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FIrstGame
{
    public class TextureBackground
    {

        // Фон игры
        public int windowWidthSize = 1920;
        public int windowHeightSize = 1025;
        // Спрайты
        public Texture2D BackgroundSpace1 { get; set; } // Спрайт 1 фона
        public Texture2D BackgroundSpace2 { get; set; } // Спрайт 2 фона
        public Texture2D BackgroundSpace3 { get; set; } // Спрайт 3 фона
        public Texture2D BackgroundSpace4 { get; set; } // Спрайт 4 фона
        public Texture2D BackgroundSpace5 { get; set; } // Спрайт 5 фона
        public Texture2D Button { get; set; } // Спрайт обводки кнопки
        public Texture2D BackgroundMenu { get; set; } // Спрайт фона главного меню
        public SpriteFont FontText { get; set; } // Текст для фона

        public TextureBackground(ContentManager contentManager)
        {
            // Спрайты
            BackgroundSpace1 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace1");
            BackgroundSpace2 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace2");
            BackgroundSpace3 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace3");
            BackgroundSpace4 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace4");
            BackgroundSpace5 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace5");
            Button = contentManager.Load<Texture2D>("ImageBackground/Button");
            BackgroundMenu = contentManager.Load<Texture2D>("ImageBackground/StarBattleBackground");
            // Текст
            FontText = contentManager.Load<SpriteFont>("Fonts/Font");
        }
    }
}