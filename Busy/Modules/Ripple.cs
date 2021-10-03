using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Ripple
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
    public static List<Ripple> collection = new List<Ripple>();
    public static Texture2D sprite;

    static void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            collection.Add(new Ripple());
        }
    }

    static void StaticUpdate()
    {
        foreach (Ripple x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Ripple x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    static Vector2 origin;

    Vector2 position;
    GameValue value;
    Color color;

    Ripple()
    {
        Init(true);
    }

    void Init(bool randomScale = false)
    {
        position = new Vector2(
            Main.random.Next(0, Main.screen.Width),
            Main.random.Next(0, Main.screen.Height)
            );
        value = new GameValue(0, 480, 1, randomScale ? Main.random.Next(0, 100) : 0);
    }

    void Update()
    {
        value.Regenerate();
        if(value.Percent() == 1)
        {
            Init();
        }

        float _c = MathF.Sin((float)value.Percent() * MathF.PI);
        color = new Color(_c, _c, _c, _c);
    }

    void Draw(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        spriteBatch.Draw(sprite, position, null, color, 0f, origin, (float)value.Percent(), SpriteEffects.None, 0f);
    }
    #endregion
}
