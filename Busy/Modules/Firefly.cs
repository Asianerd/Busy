using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Firefly
{
    #region Init, Content, Enable/Disable
    public static void LoadContent(Texture2D _sprite)
    {
        sprite = _sprite;
        origin = sprite.Bounds.Size.ToVector2() / 2;
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
    public static List<Firefly> collection = new List<Firefly>();
    public static Texture2D sprite;

    static void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            collection.Add(new Firefly());
        }
    }

    static void StaticUpdate()
    {
        foreach (Firefly x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Firefly x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    static Vector2 origin;
    static float speed = 1f;

    Vector2 position;
    float direction;    // Degrees : 0 - 360
    float target;       // Degrees : 0 - 360
    float radianDirection; // direction but in radians
    GameValue flicker;
    Color color;

    Vector2 incrementedVector;

    Firefly()
    {
        Init();
    }

    void Init()
    {
        position = new Vector2(
            Main.random.Next(0, Main.screen.Width),
            Main.random.Next(0, Main.screen.Height)
            );
        target = Main.random.Next(0, 360);
        flicker = new GameValue(0, 120, 1, Main.random.Next(0, 100));
        direction = Main.random.Next(0, 360);
    }

    void Update()
    {
        flicker.Regenerate();
        if (flicker.Percent() == 1)
        {
            flicker.AffectValue(0f);
        }

        float _c = MathF.Sin((float)flicker.Percent() * MathF.PI);
        color = new Color(_c, _c, _c, _c);


        float diff = direction - target;
        if (Math.Abs(diff) > 20f)
        {
            direction -= (direction - target) * 0.01f;
        }
        else
        {
            target = Main.random.Next(0, 360);
        }

        radianDirection = MathHelper.ToRadians(direction);

        incrementedVector.X = MathF.Cos(radianDirection) * speed;
        incrementedVector.Y = MathF.Sin(radianDirection) * speed;
        position += incrementedVector;


        if(position.X > Main.screen.Width)
        {
            position.X = 0;
        }
        else if(position.X < 0)
        {
            position.X = Main.screen.Width;
        }

        if(position.Y > Main.screen.Height)
        {
            position.Y = 0;
        }
        else if(position.Y < 0)
        {
            position.Y = Main.screen.Height;
        }
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, position, null, color, radianDirection, origin, 1f, SpriteEffects.None, 0f);
    }
    #endregion
}
