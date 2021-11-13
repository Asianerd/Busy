using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

class DVD
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
    public static List<DVD> collection = new List<DVD>();
    public static Texture2D sprite;

    static void Start()
    {
        collection.Add(new DVD());
    }

    static void StaticUpdate()
    {
        foreach(DVD x in collection)
        {
            x.Update();
        }
    }

    static void StaticDraw(SpriteBatch spriteBatch)
    {
        foreach (DVD x in collection)
        {
            x.Draw(spriteBatch);
        }
    }
    #endregion
    #endregion

    Vector2 position;
    Vector2 increment;
    Color currentColor;

    /*    \  |  /
     *     \ | /
     * ------+------
     *     / | \
     *    /  |  \
     */

    #region Public
    DVD()
    {
        Init(true);
    }

    void Init(bool trueinit = false)
    {
        if(trueinit)
        {
            position = new Vector2(Main.random.Next(0, Main.screen.Width), Main.random.Next(0, Main.screen.Height));
        }

        increment = new Vector2(
            Main.random.Next(0, 2) == 1 ? 1 : -1,
            Main.random.Next(0, 2) == 1 ? 1 : -1
            );
        ChangeColor();
    }

    void Update()
    {
        Vector2 nextPosition = position + (increment * 5);

        if (!Main.screen.Contains(nextPosition.ToPoint()))
        {
            ChangeColor();
            if (nextPosition.X >= Main.screen.Width)
            {
                increment.X = -1;
            }
            else if (nextPosition.X <= 0)
            {
                increment.X = 1;
            }

            if (nextPosition.Y >= Main.screen.Height)
            {
                increment.Y = -1;
            }
            else if (nextPosition.Y <= 0)
            {
                increment.Y = 1;
            }
        }
        position += increment * 5;
    }

    void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, position, null, currentColor, 0f, sprite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);
    }

    void ChangeColor()
    {
        Color[] _colorChoices = {
            new Color(0,0,255),
            new Color(0,255,0),
            new Color(0,255,255),
            new Color(255,0,0),
            new Color(255,0,255),
            new Color(255,255,0),
            new Color(255,255,255),
            };

        List<Color> _colors = _colorChoices.Where(n => n != currentColor).ToList();
        currentColor = _colors[Main.random.Next(0,_colors.Count())];
        
    }
    #endregion
}
