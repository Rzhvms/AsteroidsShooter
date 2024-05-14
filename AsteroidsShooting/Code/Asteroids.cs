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

        public static int GetRandomCount(int minValue, int maxValue) => Rand.Next(minValue, maxValue);

        public static void SpaceShipFire()
        {
            FireShoots.Add(new FireShot(SpaceShip.GetFirePosition));
        }

        public static void Initialize(SpriteBatch spriteBatch, int width, int height)
        {
            SpriteBatch = spriteBatch;
            Width = width;
            Height = height;
            Stars = new Stars[100];
            for (var i = 0; i < Stars.Length; i++)
                Stars[i] = new Stars(new Vector2(-Rand.Next(1, 10), 0));
            SpaceShip = new SpaceShip(new Vector2(0, Height / 2 - 75));
        }

        public static void Draw()
        {
            foreach (var star in Stars)
                star.Draw();
            foreach(var fire in FireShoots)
                fire.Draw();
            SpaceShip.Draw();
        }

        public static void Update()
        {
            foreach (var star in Stars)
                star.Update();
            for (var i = 0; i < FireShoots.Count; i++)
            {
                FireShoots[i].Update();
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
        Vector2 Position;
        Vector2 Direction;
        Color Color;

        public static Texture2D Texture2D;

        public Stars(Vector2 Position, Vector2 Direction)
        {
            this.Position = Position;
            this.Direction = Direction;
        }

        public Stars(Vector2 Direction)
        {
            this.Direction = Direction;
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
        Vector2 Position;
        const int SpaceShipSpeed = 5;
        Color Color = Color.White;
        public static Texture2D Texture2D;

        public SpaceShip(Vector2 Position) => this.Position = Position;

        public Vector2 GetFirePosition => new(Position.X + 75, Position.Y + 15);

        public void MoveUp()
        {
            if (this.Position.Y > 0)
                this.Position.Y -= SpaceShipSpeed;
        }

        public void MoveDown()
        {
            if (this.Position.Y < Asteroids.Height - Texture2D.Height)
                this.Position.Y += SpaceShipSpeed;
        }

        public void MoveLeft()
        {
            if (this.Position.X > 0)
                this.Position.X -= SpaceShipSpeed;
        }

        public void MoveRight()
        {
            if (this.Position.X < Asteroids.Width - Texture2D.Width)
                this.Position.X += SpaceShipSpeed;
        }

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, Color);

    }

    public class FireShot
    {
        Vector2 Position;
        Vector2 Direction;
        const int FireSpeed = 7;
        Color Color = Color.White;

        public static Texture2D Texture2D;

        public FireShot(Vector2 Position)
        {
            this.Position = Position;
            this.Direction = new Vector2(FireSpeed, 0);
        }

        public bool Hidden => Position.X > Asteroids.Width;
        public void Update()
        {
            if (Position.X <= Asteroids.Width)
                Position += Direction;
        }

        public void Draw() => Asteroids.SpriteBatch.Draw(Texture2D, Position, Color);
    }
}
