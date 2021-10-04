using System;
using System.Collections.Generic;
using System.Text;

class Input
{
    public static Input debugInput = new Input();

    bool wasPressed = false;
    bool isPressed;
    public bool active;

    public void Update(bool pressed)
    {
        wasPressed = isPressed;
        isPressed = pressed;

        active = !wasPressed && isPressed;
    }
}
