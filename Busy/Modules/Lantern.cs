using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

public class Lantern
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
    public static List<Lantern> collection = new List<Lantern>();
    public static Texture2D sprite;

    static void Start()
    {
        int amount = 100;

        List<float> depths = new List<float>();
        for (int i = 0; i < amount; i++)
        {
            depths.Add(Main.random.Next(50, 100) / 100f);
        }
        depths = depths.OrderBy(n=>n).ToList();

        for (int i = 0; i < amount; i++)
        {
            collection.Add(new Lantern(depths[i]));
        }
    }

    static void StaticUpdate()
    {
        foreach (Lantern x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Lantern x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    Vector2 position;
    Vector2 incrementedVector;
    float rotation;
    float depth;
    Color tint;

    Lantern(float _depth)
    {
        depth = _depth;
        Init(true);
    }

    void Init(bool trueinit = false)
    {
        if(trueinit)
        {
            position = new Vector2(
                Main.random.Next(-100, Main.screen.Width + 100),
                Main.random.Next(-100, Main.screen.Height + 100)
                );
            tint = new Color(depth, depth, depth, 1f);
        }

        rotation = Main.random.Next(0, 5) + 30 - 90;
        float direction = MathHelper.ToRadians(rotation);
        rotation = MathHelper.ToRadians(rotation - 30 + 90);
        incrementedVector = new Vector2(
            MathF.Cos(direction) * depth,
            MathF.Sin(direction) * depth
            );
    }

    void Update()
    {
        position += incrementedVector;

        if (position.X >= (Main.screen.Width + 100))
        {
            position.X = -100;
        }
        else if (position.X <= -100)
        {
            position.X = Main.screen.Width + 100;
        }

        if (position.Y >= (Main.screen.Height + 100))
        {
            position.Y = -100;
        }
        else if (position.Y <= -100)
        {
            position.Y = Main.screen.Height + 100;
        }
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, position, null, tint, rotation, sprite.Bounds.Center.ToVector2(), depth, SpriteEffects.None, 1f);
    }
    #endregion
}
