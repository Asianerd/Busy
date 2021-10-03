using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Button
{
    public static List<Button> buttons = new List<Button>();
    public delegate void ButtonEvent();
    public event ButtonEvent PressEvent;

    public static Color neutralColor = Color.White;
    public static Color pressedColor = Color.Gray;

    Rectangle rect;
    Texture2D sprite;
    Vector2 origin;

    bool wasPressed;    // Pressed the previous frame
    bool isPressed;     // Pressed the current frame
    bool pressed;       // Pressed the current frame and mouse cursor is hovering
    public bool active; // True only during the frame the button is pressed and mouse cursor is hovering

    public Button(Texture2D _sprite, Vector2 position)
    {
        sprite = _sprite;
        origin = sprite.Bounds.Size.ToVector2() / 2;
        rect = new Rectangle(position.ToPoint(), (Vector2.One * 64).ToPoint());
        buttons.Add(this);
    }

    public void Update()
    {
        wasPressed = isPressed;
        isPressed = Main.mouseState.LeftButton == ButtonState.Pressed;
        pressed = false;
        active = false;

        if (rect.Contains(Main.cursor))
        {
            GetPressed();
        }
    }

    public void GetPressed()
    {
        pressed = isPressed;
        if (isPressed && !wasPressed)
        {
            active = true;
            if (PressEvent != null)
            {
                PressEvent();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
    {
        spriteBatch.Draw(sprite, position, null, pressed ? pressedColor : neutralColor, 0f, origin, scale, SpriteEffects.None, 0f);
    }


    #region Static
    public static void Initialize()
    {
        
    }
    #endregion
}