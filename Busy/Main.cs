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
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        ModuleManager.Initialize();
        UI.Initialize();
        Button.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        string _logoPath = "Logos";
        ModuleManager.LoadContent(new Dictionary<ModuleManager.ModuleType, ModuleManager.ModuleData>() {
            { ModuleManager.ModuleType.Rain, new ModuleManager.ModuleData(new Color(0, 0, 0), Content.Load<Texture2D>($"{_logoPath}/rain")) },
            { ModuleManager.ModuleType.Ripple, new ModuleManager.ModuleData(new Color(100, 100, 100), Content.Load<Texture2D>($"{_logoPath}/ripple")) },
        });
        UI.LoadContent(Content.Load<Texture2D>("selectionBar"), Content.Load<Texture2D>("selectionCursor"), Content.Load<Texture2D>("moduleHighlight"));

        #region Module content loading
        Rain.LoadContent(Content.Load<Texture2D>("rain"));
        #endregion
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        mouseState = Mouse.GetState();
        mousePosition = mouseState.Position.ToVector2();
        keyboardState = Keyboard.GetState();
        cursor = mouseState.Position;

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
}
