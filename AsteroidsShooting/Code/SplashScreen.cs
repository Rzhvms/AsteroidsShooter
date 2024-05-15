using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsShooting
{
    public static class SplashScreen
    {
        public static Texture2D Background;
        public static SpriteFont Font;
        static int Visibility = 0;
        static Color Color;
        static Vector2 TextPosition = new(190, 200);

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font, "Space Shooter", TextPosition, Color);
        }

        public static void UpdateColor()
        {
            Color = Color.FromNonPremultiplied(255, 255, 255, Visibility % 256);
            Visibility++;
        }
    }
}