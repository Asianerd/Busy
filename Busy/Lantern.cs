using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Lantern
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
        for (int i = 0; i < 20; i++)
        {
            collection.Add(new Lantern());
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
    Lantern()
    {
        Init(true);
    }

    void Init(bool randomScale = false)
    {

    }

    void Update()
    {

    }

    void Draw(SpriteBatch spriteBatch)
    {

    }
    #endregion
}
