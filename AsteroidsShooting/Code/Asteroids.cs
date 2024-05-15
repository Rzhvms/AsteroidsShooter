using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AsteroidsShooting
{
    public class Asteroids
    {
        public static int Width, Height;
        public static Random Rand = new();
        public static SpriteBatch SpriteBatch;
        public static Stars[] Stars;
        public static SpaceShip SpaceShip;
        public static List<FireShot> FireShoots = new();
        public static List<Asteroid> AsteroidsList = new();

        public static int GetRandomCount(int minValue, int maxValue) => Rand.Next(minValue, maxValue);

        public static void SpaceShipFire() => FireShoots.Add(new FireShot(SpaceShip.GetFirePosition));

        public static void Initialize(SpriteBatch spriteBatch, int width, int height)
        {
            SpriteBatch = spriteBatch;
            Width = width;
            Height = height;
            Stars = new Stars[100];
            SpaceShip = new SpaceShip(new Vector2(0, Height / 2 - 75));

            for (var i = 0; i < Stars.Length; i++)
                Stars[i] = new Stars(new Vector2(-Rand.Next(1, 10), 0));

            for (var i = 0; i < 40; i++)
                AsteroidsList.Add(new Asteroid());
        }

        public static void Draw()
        {
            foreach (var star in Stars)
                star.Draw();

            foreach (var fire in FireShoots)
                fire.Draw();

            foreach (var asteroid in AsteroidsList)
                asteroid.Draw();

            SpaceShip.Draw();
        }

        public static void Update()
        {
            foreach (var star in Stars)
                star.Update();

            foreach (var asteroid in AsteroidsList)
                asteroid.Update();

            for (var i = 0; i < FireShoots.Count; i++)
            {
                FireShoots[i].Update();
                var crashAsteroid = FireShoots[i].Crash(AsteroidsList);

                if (crashAsteroid != null)
                {
                    AsteroidsList.Remove(crashAsteroid);
                    FireShoots.RemoveAt(i);
                    i--;
                    continue;
                }

                if (FireShoots[i].Hidden)
                {
                    FireShoots.RemoveAt(i);
                    i--;
                }
            }

        }
    }

    public class Stars
    {
        public static Texture2D Texture2D;
        Vector2 Position;
        Vector2 Direction;
        Color Color;

        public Stars(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Stars(Vector2 direction)
        {
            Direction = direction;
            RandomSet();
        }

        public void Update()
        {
            Position += Direction;

            if (Position.X < 0)
                RandomSet();
        }

        public void RandomSet()
        {
            Position = new Vector2(Asteroids.GetRandomCount(Asteroids.Width, Asteroids.Width + 300),
                Asteroids.GetRandomCount(0, Asteroids.Height));

            var colorSet = Asteroids.GetRandomCount(0, 256);
            Color = Color.FromNonPremultiplied(colorSet, colorSet, colorSet, colorSet);
        }

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, Color);
    }

    public class SpaceShip
    {
        public static Texture2D Texture2D;
        const int SpaceShipSpeed = 5;
        Color Color = Color.White;
        Vector2 Position;

        public SpaceShip(Vector2 position) => Position = position;

        public Vector2 GetFirePosition => new(Position.X + 75, Position.Y + 15);

        public void MoveUp()
        {
            if (Position.Y > 0)
                Position.Y -= SpaceShipSpeed;
        }

        public void MoveDown()
        {
            if (Position.Y < Asteroids.Height - Texture2D.Height)
                Position.Y += SpaceShipSpeed;
        }

        public void MoveLeft()
        {
            if (Position.X > 0)
                Position.X -= SpaceShipSpeed;
        }

        public void MoveRight()
        {
            if (Position.X < Asteroids.Width - Texture2D.Width)
                Position.X += SpaceShipSpeed;
        }

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, Color);
    }

    public class FireShot
    {
        public static Texture2D Texture2D;
        const int FireSpeed = 7;
        Color Color = Color.White;
        Vector2 Position;
        Vector2 Direction;

        public FireShot(Vector2 position)
        {
            Position = position;
            Direction = new Vector2(FireSpeed, 0);
        }

        public Asteroid Crash(List<Asteroid> asteroidsList)
        {
            foreach (var asteroid in asteroidsList)
                if (asteroid.IsIntersect(new Rectangle((int)Position.X, (int)Position.Y, Texture2D.Width, Texture2D.Height)))
                    return asteroid;

            return null;
        }

        public void Update()
        {
            if (Position.X <= Asteroids.Width)
                Position += Direction;
        }

        public bool Hidden => Position.X > Asteroids.Width;

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, Color);
    }

    public class Asteroid
    {
        public static Texture2D Texture2D;
        Color color = Color.White;
        Vector2 Position;
        Vector2 Direction;
        float Scale;
        float Rotation = 0;
        float RotationSpeed = 1;

        Vector2 Center => new(Texture2D.Width / 2, Texture2D.Height / 2);

        Point Size => new((int)(Texture2D.Width * Scale), (int)(Texture2D.Height * Scale));

        public Asteroid(Vector2 position, Vector2 direction, float scale,
            float rotation, float rotationSpeed)
        {
            Position = position;
            Direction = direction;
            Rotation = rotation;
            RotationSpeed = rotationSpeed;
            Scale = scale;
        }
        
        public bool IsIntersect(Rectangle rectangle) => rectangle.Intersects(new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y));

        public Asteroid() => RandomSet();

        public Asteroid(Vector2 direction)
        {
            Direction = direction;
            RandomSet();
        }

        public void Update()
        {
            Position += Direction;
            Rotation += RotationSpeed;
            if (Position.X < -Texture2D.Width * Scale)
                RandomSet();
        }

        public void RandomSet()
        {
            Position = new Vector2(Asteroids.GetRandomCount(Asteroids.Width, Asteroids.Width + 300),
                Asteroids.GetRandomCount(0, Asteroids.Height));
            Direction = new Vector2(-(float)Asteroids.Rand.NextDouble() * 2 + 0.1f, 0f);
            Scale = (float)Asteroids.Rand.NextDouble();
            RotationSpeed = (float)(Asteroids.Rand.NextDouble() - 0.5) / 4;

        }

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, null, color,
            Rotation, Center, Scale, SpriteEffects.None, 0);
    }
}