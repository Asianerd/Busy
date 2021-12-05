using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Clock
{
    #region Init, Content, Enable/Disable
    public static void LoadContent(Texture2D _sprite)
    {
        sprite = _sprite;
    }

    public static void Initialize()
    {
        ModuleManager.ResetModules += Disable;
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

    #region Start, Updates, Draws
    public static List<Clock> collection = new List<Clock>();
    public static Texture2D sprite;

    static void Start()
    {
        collection.Add(new Clock());
    }

    static void StaticUpdate()
    {
        foreach (Clock x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Clock x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    static double radianConstant = 2d * Math.PI;

    Vector2 position;
    float sRotation;
    float mRotation;
    float hRotation;

    Rectangle sRect;
    Rectangle mRect;
    Rectangle hRect;

    Clock()
    {
        Init(true);
    }

    void Init(bool trueinit = false)
    {
        position = Main.screen.Center.ToVector2();
        sRect = new Rectangle((int)position.X, (int)position.Y, 500, 5);
        mRect = new Rectangle((int)position.X, (int)position.Y, 300, 10);
        hRect = new Rectangle((int)position.X, (int)position.Y, 250, 10);
    }

    void Update()
    {
        var now = DateTime.Now;
        sRotation = (now.Second / 60f) * (float)(radianConstant);
        mRotation = (now.Minute / 60f) * (float)(radianConstant);
        hRotation = (now.Hour / 12f) * (float)(radianConstant + radianConstant);
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, sRect, null, Color.White, sRotation, Vector2.Zero, SpriteEffects.None, 0f);
        spriteBatch.Draw(sprite, mRect, null, Color.White, mRotation, Vector2.Zero, SpriteEffects.None, 0f);
        spriteBatch.Draw(sprite, hRect, null, Color.White, hRotation, Vector2.Zero, SpriteEffects.None, 0f);
        var now = DateTime.Now;
        spriteBatch.DrawString(UI.font, $"{now.Second} : {sRotation}\n{now.Minute} : {mRotation}", Vector2.Zero, Color.White);
    }
    #endregion
}
