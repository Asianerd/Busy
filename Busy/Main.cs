using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static Random random = new Random();
    public static Rectangle screen = new Rectangle(0, 0, 1920, 1080);
    public static float FPS;
    public static Color backgroundColor;
    public static MouseState mouseState;
    public static Vector2 mousePosition;
    public static KeyboardState keyboardState;
    public static Point cursor;

    public delegate void MainEvents();
    public static MainEvents UpdateEvent;
    public static MainEvents ModuleUpdateEvent; // For modules

    public delegate void RenderEvent(SpriteBatch spriteBatch);
    public static RenderEvent DrawEvent;
    public static RenderEvent ModuleDrawEvent; // For modules

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = screen.Width;
        _graphics.PreferredBackBufferHeight = screen.Height;
        _graphics.IsFullScreen = true;

        InactiveSleepTime = new TimeSpan(0, 0, 0, 0, 0);

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += new EventHandler<EventArgs>(UpdateScreenSize);

        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        IsMouseVisible = false;

        ModuleManager.Initialize();
        UI.Initialize();
        MouseButton.Initialize();
        Button.Initalize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        string _logoPath = "Logos";
        ModuleManager.LoadContent(new Dictionary<ModuleManager.ModuleType, ModuleManager.ModuleData>() {
            { ModuleManager.ModuleType.Rain, new ModuleManager.ModuleData(new Color(0, 0, 50), Content.Load<Texture2D>($"{_logoPath}/rain")) },
            { ModuleManager.ModuleType.Ripple, new ModuleManager.ModuleData(new Color(20, 20, 60), Content.Load<Texture2D>($"{_logoPath}/ripple")) },
            { ModuleManager.ModuleType.Fireflies, new ModuleManager.ModuleData(new Color(0, 0, 20), Content.Load<Texture2D>($"{_logoPath}/firefly")) },
            { ModuleManager.ModuleType.Lantern, new ModuleManager.ModuleData(new Color(0, 0, 50), Content.Load<Texture2D>($"{_logoPath}/lantern")) },
            { ModuleManager.ModuleType.DVD, new ModuleManager.ModuleData(new Color(0, 0, 0), Content.Load<Texture2D>($"{_logoPath}/dvd")) },
            { ModuleManager.ModuleType.Star, new ModuleManager.ModuleData(new Color(0, 0, 40), Content.Load<Texture2D>($"{_logoPath}/star")) },
            { ModuleManager.ModuleType.Matrix, new ModuleManager.ModuleData(new Color(0, 0, 0), Content.Load<Texture2D>($"{_logoPath}/matrix")) },
            { ModuleManager.ModuleType.Clock, new ModuleManager.ModuleData(new Color(50, 50, 50), Content.Load<Texture2D>($"{_logoPath}/clock")) }
        });
        UI.LoadContent(Content.Load<Texture2D>("cursor"), Content.Load<Texture2D>("selectionBar"), Content.Load<Texture2D>("selectionCursor"), Content.Load<Texture2D>("moduleHighlight"), Content.Load<Texture2D>("darkOverlay"), Content.Load<SpriteFont>("font"));

        #region Module content loading
        Rain.LoadContent(Content.Load<Texture2D>("rain"));
        Ripple.LoadContent(Content.Load<Texture2D>("ripple"));
        Firefly.LoadContent(Content.Load<Texture2D>("firefly"));
        Lantern.LoadContent(Content.Load<Texture2D>("lantern"));
        DVD.LoadContent(Content.Load<Texture2D>("dvd"));
        Star.LoadContent(Content.Load<Texture2D>("star"));
        Matrix.LoadContent(Content.Load<SpriteFont>("matrix_font"));
        Clock.LoadContent(Content.Load<Texture2D>("clock_hand"));
        #endregion
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (Button x in Button.collection)
        {
            x.Update();
        }

        mouseState = Mouse.GetState();
        mousePosition = mouseState.Position.ToVector2();
        keyboardState = Keyboard.GetState();
        cursor = mouseState.Position;

        if(Button.F11.active)
        {
            _graphics.ToggleFullScreen();
        }

        if(Button.Reset.active)
        {
            ModuleManager.UpdateActiveModule();
        }

        if(UpdateEvent != null)
        {
            UpdateEvent();
        }
        if(ModuleUpdateEvent != null)
        {
            ModuleUpdateEvent();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);

        FPS = 1f / (float)gameTime.ElapsedGameTime.TotalSeconds;

        _spriteBatch.Begin();
        if (ModuleDrawEvent != null)
        {
            ModuleDrawEvent(_spriteBatch);
        }
        if (DrawEvent != null)
        {
            DrawEvent(_spriteBatch);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    void UpdateScreenSize(object sender, EventArgs a)
    {
        _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        screen = new Rectangle(0, 0,
            _graphics.PreferredBackBufferWidth,
            _graphics.PreferredBackBufferHeight);

        ModuleManager.UpdateActiveModule();
    }
}
