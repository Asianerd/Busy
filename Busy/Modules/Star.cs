using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Star
{
    #region Init, Content, Enable/Disable
    public static void LoadContent(Texture2D _sprite)
    {
        sprite = _sprite;
    }

    public static void Initialize()
    {
        ModuleManager.ResetModules += Disable;
        InitializeOrigin();
    }

    public static void Enable()
    {
        Main.ModuleUpdateEvent += StaticUpdate;
        Main.ModuleDrawEvent += StaticDraw;
        Start();
    }

    public static void Disable()
    {
        collection.Clear();
    }

    public static void InitializeOrigin()
    {
        origin = new Vector2(
            Main.screen.Width * 1.8f,
            Main.screen.Height * 1.2f
            );
    }

    #region Start, Updates, Draws
    public static List<Star> collection = new List<Star>();
    public static Texture2D sprite;

    static void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            collection.Add(new Star());
        }
    }

    static void StaticUpdate()
    {
        foreach (Star x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Star x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    static Vector2 origin;
    Vector2 position;
    float distance;
    float direction;
    float intensity;
    float scale;
    float speed;
    Color color;

    Star()
    {
        Init(true);
    }

    void Init(bool trueinit = false)
    {
        if (trueinit)
        {
            int _i = 10000;
            direction = MathHelper.ToRadians(Main.random.Next(0, 360 * _i)/((float)_i));
            distance = Main.random.Next(0, (int)Vector2.Distance(origin, Vector2.Zero)); // Distance to top-left corner of screen
            intensity = Main.random.Next(5, 10) / 10f;
            scale = MathF.Pow(intensity, 14) + 0.5f;
            color = new Color(Vector3.One * intensity);
            speed = 0.0001f * intensity;

        }
    }

    void Update()
    {
        direction += speed;

        position.X = MathF.Cos(direction) * distance + origin.X;
        position.Y = MathF.Sin(direction) * distance + origin.Y;
    }

    void Draw(SpriteBatch spriteBatch)
    {
        if (Main.screen.Contains(position.ToPoint()))
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
    }
    #endregion
}
