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
        


        List<List<int>> rectangleCoordinates = new List<List<int>>();

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
            Random rng = new Random();
            timer = 60 * 5;
            for (int i = 0; i < 44; i++) 
            {
                List<int> row = new List<int>();
                for (int j = 0; j < 44; j++)
                {
                    row.Add(rng.Next(0,2));
                }
                rectangleCoordinates.Add(row);
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
            float resolutionX =1+ GraphicsDevice.Viewport.Width / rectangleCoordinates[0].Count;
            float resolutionY =1+ GraphicsDevice.Viewport.Height / rectangleCoordinates.Count;

            for (int i = 0; i < rectangleCoordinates.Count; i++)
            {
                for(int j = 0; j < rectangleCoordinates.Count; j++) 
                {

                    if (rectangleCoordinates[i][j] == 1)
                    {
                        spriteBatch.Draw(whiteRectangle, new Rectangle(
                            (int)(j * resolutionX),
                            (int)(i * resolutionY),
                            2,
                            2),
                            Color.Green);
                    }
                    else
                    {
                        spriteBatch.Draw(whiteRectangle, new Rectangle(
                            (int)(j * resolutionX),
                            (int)(i * resolutionY),
                            2,
                            2),
                            Color.Red);
                    }

                }
            }
            MarchingSquares(resolutionX, resolutionY);
            spriteBatch.End(); 
            // TODO: Add your drawing code here
        }

        private double DegreesToRadians(double angle)
        {
            return (double) (Math.PI / 180) * angle;
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            // Calculate the length and angle of the line
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float angle = (float)Math.Atan2(dy, dx);
            float resolutionX = GraphicsDevice.Viewport.Width / rectangleCoordinates[0].Count;
            float resolutionY = GraphicsDevice.Viewport.Height / rectangleCoordinates.Count;

            // Draw the line using spriteBatch
            spriteBatch.Draw(line,
                             new Rectangle((int)start.X, (int)start.Y, (int)length, (int) 1), // 1-pixel height
                             null,
                             color,
                             angle,
                             new Vector2(0,0),
                             SpriteEffects.None,
                             0);
        }

        private void MarchingSquares(float resolutionX, float resolutionY)
        {

            for (int i = 0; i < rectangleCoordinates.Count - 1; i++)
            {
                for (int j = 0; j < rectangleCoordinates[i].Count - 1; j++)
                {
                    float x = j * resolutionX;
                    float y = i * resolutionY;
                    Vector2 a = new Vector2(x + resolutionX * 0.5f, y);
                    Vector2 b = new Vector2(x + resolutionX, y + resolutionY * 0.5f);
                    Vector2 c = new Vector2(x + resolutionX * 0.5f, y + resolutionY);
                    Vector2 d = new Vector2(x, y + resolutionY * 0.5f);
                    int state = GetState(rectangleCoordinates[i][j],
                                         rectangleCoordinates[i][j + 1],
                                         rectangleCoordinates[i + 1][j + 1],
                                         rectangleCoordinates[i + 1][j]);

                    switch (state)
                    {
                        case 0:
                            break;
                        case 1:
                            DrawLine(c, d, Color.White);
                            break;
                        case 2:
                            DrawLine(b, c, Color.White);
                            break;
                        case 3:
                            DrawLine(b, d, Color.White);
                            break;
                        case 4:
                            DrawLine(a, b, Color.White);
                            break;
                        case 5:
                            DrawLine(a, d, Color.White);
                            DrawLine(b, c, Color.White);
                            break;
                        case 6:
                            DrawLine(a, c, Color.White);
                            break;
                        case 7:
                            DrawLine(a, d, Color.White);
                            break;
                        case 8:
                            DrawLine(a, d, Color.White);
                            break;
                        case 9:
                            DrawLine(a, c, Color.White);
                            break;
                        case 10:
                            DrawLine(a, b, Color.White);
                            DrawLine(c, d, Color.White);
                            break;
                        case 11:
                            DrawLine(a, b, Color.White);
                            break;
                        case 12:
                            DrawLine(b, d, Color.White);
                            break;
                        case 13:
                            DrawLine(b, c, Color.White);
                            break;
                        case 14:
                            DrawLine(c, d, Color.White);
                            break;
                        case 15:
                            break;
                    }
                    
                }
            }
        }

        private int GetState(int a, int b, int c, int d) 
        {
            return a * 8 + b * 4 + c * 2 + d * 1;
        }
    }
}
