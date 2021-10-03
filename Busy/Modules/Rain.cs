using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Rain
{
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

    #region Static
    public static List<Rain> collection = new List<Rain>();
    public static Texture2D sprite;

    static void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            collection.Add(new Rain());
        }
    }

    static void StaticUpdate()
    {
        foreach (Rain x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Rain x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion


    #region Public
    Vector2 position;
    Vector2 size;
    Color color;
    float depth;

    Rain()
    {
        Init();
    }

    void Init()
    {
        position = new Vector2(
            Main.random.Next(0, Main.screen.Width),
            Main.random.Next(0, 500) - 500
            );
        depth = Main.random.Next(12, 24);

        float _c = depth / 24;
        color = new Color(_c, _c, _c, 1);
        size = new Vector2(1, (int)depth);
    }

    void Update()
    {
        if ((position.X < 0) || (position.X > Main.screen.Width) ||
            (position.Y < -600) || (position.Y > Main.screen.Height))
        {
            Init();
            return;
        }

        position.Y += depth;
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
    }
    #endregion
}
