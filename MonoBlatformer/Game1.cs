using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBlatformer.Helpers;
using MonoBlatformer.MapThings;
using MonoBlatformer.Objects;

namespace MonoBlatformer
{
    public class Game1 : Game
    {
        const int SCREENWIDTH = 384, SCREENHEIGHT = 216;
        const bool FULLSCREEN = false;

        //SOURCE RECTANGLES
        Rectangle screenRect, desktopRect;

        //RENDERTARGETS
        RenderTarget2D MainTarget;                          
        // render to a standard target and fit it to the desktop resolution
        static public int screenW, screenH;
        //to know current resolution

        GraphicsDeviceManager graphics;
        PresentationParameters pp;
        SpriteBatch spriteBatch;

        Map map;
        Character character;

        public Game1()
        {
            // Set a display mode that is windowed but is the same as the desktop's current resolution (don't show a border)...
            // This is done instead of using true fullscreen mode since some firewalls will crash the computer in true fullscreen mode
            int initial_screen_width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10;
            int initial_screen_height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 10;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = initial_screen_width,
                PreferredBackBufferHeight = initial_screen_height,
                IsFullScreen = false,
                PreferredDepthStencilFormat = DepthFormat.Depth16
            };
            Window.IsBorderless = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            MainTarget = new RenderTarget2D(GraphicsDevice, SCREENWIDTH, SCREENHEIGHT);
            pp = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;
            screenW = MainTarget.Width;
            screenH = MainTarget.Height;
            desktopRect = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            screenRect = new Rectangle(0, 0, screenW, screenH);

            map = new Map();
            character = new Character();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D tileSet = Content.Load<Texture2D>("jungletileset");
            map.Initialize(100, 14, 16, 16, tileSet);
            Camera.Initialize(Vector2.Zero, SCREENHEIGHT, SCREENWIDTH, map.TileWidth * map.Width - SCREENWIDTH, map.TileHeight * map.Height - SCREENHEIGHT, 0, 8);

            Texture2D jump = Content.Load<Texture2D>("Player/jump");
            Texture2D run = Content.Load<Texture2D>("Player/runsheet");
            Texture2D land = Content.Load<Texture2D>("Player/landing");
            Texture2D idle = Content.Load<Texture2D>("Player/idlesheet");
            Texture2D fly = Content.Load<Texture2D>("Player/flysheet");
            Texture2D ledge = Content.Load<Texture2D>("Player/ledgesheet");
            Animation jumpAnimation = new Animation();
            jumpAnimation.Initialize(jump, 1, 60, 1, Color.White, 17, 34, false);
            Animation runAnimation = new Animation();
            runAnimation.Initialize(run, 1, 60, 8, Color.White, 21, 33, true);
            Animation flyAnimation = new Animation();
            flyAnimation.Initialize(fly, 1, 60, 2, Color.White, 20, 35, true);
            Animation idleAnimation = new Animation();
            idleAnimation.Initialize(idle, 1, 60, 12, Color.White, 19, 34, true);
            Animation landAnimation = new Animation();
            landAnimation.Initialize(land, 1, 60, 1, Color.White, 20, 35, false);
            Animation ledgeAnimation = new Animation();
            ledgeAnimation.Initialize(ledge, 1, 100, 6, Color.White, 20, 40, false);
            character.Initialize(new Vector2(100, 100), 21, 35, map, idleAnimation, runAnimation, flyAnimation, jumpAnimation, landAnimation, ledgeAnimation, 3, 10);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            character.Update(gameTime);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(MainTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);
            map.Draw(spriteBatch);
            character.Draw(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, desktopRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
