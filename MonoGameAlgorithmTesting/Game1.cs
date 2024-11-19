using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameAlgorithmTesting
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        int timer;
        const int RED = 1, GREEN = 2;
        int gameState = RED;
        Color backColor = Color.Red;
        Texture2D whiteRectangle;
        SpriteBatch spriteBatch;

        Texture2D line;
        float angleOfLine;
        int rectangleFactor = 8;
        int dotSize = 2;

        List<Vector2> rectangleCoordinates = new List<Vector2>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

        }

        protected override void Initialize()
        {
            line = new Texture2D(_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] { Color.White });
            angleOfLine = (float)(2 + MathHelper.Pi);

            timer = 60 * 5;
            for (int i = 0; i < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / rectangleFactor; i++) 
            {
                for (int j = 0; j < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / rectangleFactor; j++)
                {
                    rectangleCoordinates.Add(new Vector2(j * rectangleFactor, i * rectangleFactor));
                }
            }
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a 1px square rectangle texture that will be scaled to the
            // desired size and tinted the desired color at draw time
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            whiteRectangle.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            timer--;
            if (timer <= 0)
                Exit();

            switch(timer <= 30)
            {
                case true:
                    gameState = GREEN;
                    break;
                default:
                    gameState = RED;
                    break;
            }

            if(gameState == RED)
            {
                backColor = Color.Red;
            } else
            {
                backColor = Color.Lime;
            }

            angleOfLine += .01f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            
            spriteBatch.Begin();
            for (int i = 0; i < rectangleCoordinates.Count; i++)
            {
                Vector2 vec = rectangleCoordinates[i];
                spriteBatch.Draw(whiteRectangle, new Rectangle((int)vec.X, (int)vec.Y, dotSize, dotSize), 
                                Color.Chocolate);

                if(i < rectangleCoordinates.Count - 1)
                {
                    DrawLine(vec, rectangleCoordinates[i + 1], new Vector2(vec.X - rectangleCoordinates[i + 1].X, vec.Y - rectangleCoordinates[i + 1].Y), 0f, Color.White);
                }
            }



            spriteBatch.End(); 
            // TODO: Add your drawing code here
        }

        private double DegreesToRadians(double angle)
        {
            return (double) (Math.PI / 180) * angle;
        }

        private void DrawLine(Vector2 firstPosition, Vector2 secondPosition, Vector2 dimensions, float lineAngle, Color color)
        {
            float angle = (float) DegreesToRadians(Math.Atan2(secondPosition.X - firstPosition.X, secondPosition.Y - firstPosition.Y));
            spriteBatch.Draw(line, new Rectangle((int)firstPosition.X,(int)firstPosition.Y, (int)dimensions.X, (int)dimensions.Y), null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
