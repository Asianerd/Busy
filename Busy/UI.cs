using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

static class UI
{
    public static Dictionary<ModuleManager.ModuleType, Button> buttons = new Dictionary<ModuleManager.ModuleType, Button>();
    static Texture2D radialSelectionBar;
    static Texture2D selectionCursor;
    static Texture2D selectedModule;
    static Texture2D darkOverlay;
    static SpriteFont font;

    static GameValue menuValue;

    static bool showDebug = false;

    public static void Initialize()
    {
        Main.UpdateEvent += Update;
        Main.DrawEvent += Draw;

        menuValue = new GameValue(0, 120, 10, 0);

        float increment = MathHelper.ToRadians(360 / ModuleManager.moduleTypeCollection.Count);
        foreach (var item in ModuleManager.moduleTypeCollection.Select((value, i) => new { i, value }))
        {
            float degrees = increment * item.i;

            buttons.Add(item.value, new Button(ModuleManager.moduleDataCollection[item.value].logo, new Vector2(-500, -500)));
        }
    }

    public static void LoadContent(Texture2D _selectionBarSprite, Texture2D _selectionCursor, Texture2D _selectedModule, Texture2D _darkOverlay, SpriteFont _font)
    {
        radialSelectionBar = _selectionBarSprite;
        selectionCursor = _selectionCursor;
        selectedModule = _selectedModule;
        darkOverlay = _darkOverlay;

        font = _font;
    }

    public static void Update()
    {
        if (menuValue.Percent() == 1)
        {
            foreach (Button x in buttons.Values)
            {
                x.Update();
            }
        }

        if (Main.keyboardState.IsKeyDown(Keys.Tab))
        {
            menuValue.Regenerate();
        }
        else
        {
            menuValue.AffectValue(-10d);
        }

        Input.debugInput.Update(Main.keyboardState.IsKeyDown(Keys.F3));

        if(Input.debugInput.active)
        {
            showDebug = !showDebug;
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (menuValue.Percent() != 0)
        {
            Vector2 origin = Main.screen.Center.ToVector2();
            Vector2 renderedPosition = origin;
            float scale = (float)menuValue.Percent();
            float radiansToMouse = (float)Math.Atan2(Main.mousePosition.Y - origin.Y, Main.mousePosition.X - origin.X);
            float distance = 200f * scale;
            spriteBatch.Draw(darkOverlay, Main.screen, new Color(scale, scale, scale, scale));
            spriteBatch.Draw(radialSelectionBar, renderedPosition, null, Color.White, 0f, new Vector2(128, 128), scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(selectionCursor, renderedPosition, null, Color.White, radiansToMouse, new Vector2(128, 128), scale, SpriteEffects.None, 0f);

            float radianIncrement = MathHelper.ToRadians(360 / ModuleManager.moduleTypeCollection.Count);

            foreach(ModuleManager.ModuleType x in ModuleManager.moduleTypeCollection)
            {
                bool selected = false;

                float _radians = ((int)x * radianIncrement) + (radianIncrement / 2);
                float _max = _radians + (radianIncrement / 2);
                float _min = _radians - (radianIncrement / 2);
                if ((FullRadian(radiansToMouse) > _min) &&
                    (FullRadian(radiansToMouse) < _max))
                {
                    selected = true;
                    buttons[x].GetPressed();
                }

                renderedPosition = new Vector2(MathF.Cos(_radians) * distance, MathF.Sin(_radians) * distance) + origin;

                buttons[x].Draw(spriteBatch, renderedPosition, scale + (selected ? 0.2f : 0f));
                if(x == ModuleManager.activeModule)
                {
                    spriteBatch.Draw(selectedModule, renderedPosition, null, Color.White, 0f, Vector2.One * 32, scale + (selected ? 0.3f : 0.1f), SpriteEffects.None, 0f);
                }
            }
        }

        if (showDebug)
        {
            spriteBatch.DrawString(font, $"FPS : {Main.FPS}", Vector2.Zero, Color.White);
        }
    }

    static float FullRadian(float _radian)
    {
        if (_radian < 0)
        {
            return (float)((Math.PI - MathF.Abs(_radian)) + Math.PI);
        }
        else
        {
            return _radian;
        }
    }
}
