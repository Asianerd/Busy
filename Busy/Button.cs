using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class Button
{
    public static List<Button> collection = new List<Button>();

    public static Button F11;
    public static Button Reset;

    Keys key;

    bool isPressed;
    bool wasPressed;
    public bool active;

    public Button(Keys _key)
    {
        key = _key;
        collection.Add(this);
    }

    public void Update()
    {
        wasPressed = isPressed;
        isPressed = Main.keyboardState.IsKeyDown(key);

        active = !wasPressed && isPressed;
    }

    public static void Initalize()
    {
        F11 = new Button(Keys.F11);
        Reset = new Button(Keys.R);
    }
}
