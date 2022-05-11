using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FIrstGame
{
    public class TextureBackground
    {
        // Спрайты
        public Texture2D BackgroundSpace1 { get; set; } // Спрайт 1 фона
        public Texture2D BackgroundSpace2 { get; set; } // Спрайт 2 фона
        public Texture2D BackgroundSpace3 { get; set; } // Спрайт 3 фона
        public Texture2D BackgroundSpace4 { get; set; } // Спрайт 4 фона
        public Texture2D BackgroundSpace5 { get; set; } // Спрайт 5 фона

        public TextureBackground(ContentManager contentManager)
        {
            BackgroundSpace1 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace1");
            BackgroundSpace2 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace2");
            BackgroundSpace3 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace3");
            BackgroundSpace4 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace4");
            BackgroundSpace5 = contentManager.Load<Texture2D>("ImageBackground/BackgroundSpace5");
        }
    }
}