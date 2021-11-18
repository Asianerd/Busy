using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

class Matrix
{
    #region Init, Content, Enable/Disable
    public static void LoadContent(SpriteFont _font)
    {
        font = _font;
    }

    public static void Initialize()
    {
        ModuleManager.ResetModules += Disable;
        baseColor = new Color(0, 255, 70);
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
    public static List<Matrix> collection = new List<Matrix>();
    public static SpriteFont font;

    static void Start()
    {
        int amount = Main.screen.Width / spacing;
        int hAmount = Main.screen.Height / spacing;
        for (int i = 0; i < amount; i++)
        {
            collection.Add(new Matrix(new Vector2(
                spacing * i,
                Main.random.Next(0, hAmount) * spacing
                )));
        }
    }

    static void StaticUpdate()
    {
        Matrix[] copiedList = new Matrix[collection.Count];
        collection.CopyTo(copiedList);
        foreach (Matrix x in copiedList)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (Matrix x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion


    #region Public
    static Color baseColor;
    static char[] charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(n => n).ToArray();
    /*static char[] charList = "abcdefghijklmnopqrstuvwxyz".Select(n => n).ToArray();*/
    static int spacing = 16;
    Vector2 position;
    GameValue age;
    string character;
    int timer = 3; // Avoid using too many GameValues to save performance
    float depth;

    Matrix(Vector2 _position, float _depth = -10)
    {
        Init(_position, _depth);
    }

    void Init(Vector2 _position, float _depth)
    {
        position = _position;
        character = charList[Main.random.Next(0, charList.Length)].ToString();
        age = new GameValue(0, 75, -1);
        if (_depth == -10)
        {
            depth = Main.random.Next(5, 10) / 10f;
        }
        else
        {
            depth = _depth;
        }
    }

    void Update()
    {
        if (position.Y >= Main.screen.Height)
        {
            position.Y = Main.random.Next(-50, 0);
        }

        if (timer == 0)
        {
            collection.Add(new Matrix(new Vector2(position.X, position.Y + spacing), depth));
            timer = -10;
        }
        
        if(timer > 0)
        {
            timer--;
        }
        
        if (age.Percent() >= 0.95)
        {
            character = charList[Main.random.Next(0, charList.Length)].ToString();
        }
        age.Regenerate();

        if (age.Percent() <= 0)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        collection.Remove(this);
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, character, position, baseColor * (float)age.Percent() * depth);
    }
    #endregion
}
