using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class ModuleManager
{
    public static Dictionary<ModuleType, ModuleData> moduleDataCollection = new Dictionary<ModuleType, ModuleData>();
    public static List<ModuleType> moduleTypeCollection = new List<ModuleType>();
    public static ModuleType activeModule;

    public delegate void ModuleEvent();
    public static event ModuleEvent ActiveModuleChange;
    public static event ModuleEvent ResetModules;

    public static void Initialize()
    {
        moduleTypeCollection = Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>().ToList();
        activeModule = ModuleType.Lantern;
        UpdateActiveModule();

        Main.UpdateEvent += Update;
        ActiveModuleChange += UpdateActiveModule;

        #region Module initialization
        Rain.Initialize();
        Ripple.Initialize();
        Lantern.Initialize();
        Firefly.Initialize();
        DVD.Initialize();
        #endregion
    }

    public static void LoadContent(Dictionary<ModuleType, ModuleData> _moduleData)
    {
        moduleDataCollection = _moduleData;
    }

    public static void UpdateActiveModule()
    {
        Main.backgroundColor = moduleDataCollection[activeModule].backgroundColor;

        if(ResetModules != null)
        {
            ResetModules();
        }
        Main.ModuleUpdateEvent = null;
        Main.ModuleDrawEvent = null;
        switch(activeModule)
        {
            default:
                break;
            case ModuleType.Rain:
                Rain.Enable();
                break;
            case ModuleType.Ripple:
                Ripple.Enable();
                break;
            case ModuleType.Lantern:
                Lantern.Enable();
                break;
            case ModuleType.Fireflies:
                Firefly.Enable();
                break;
            case ModuleType.DVD:
                DVD.Enable();
                break;
        }
    }

    public static void Update()
    {
        foreach(ModuleType x in moduleTypeCollection)
        {
            if(UI.buttons[x].active)
            {
                if (x != activeModule)
                {
                    activeModule = x;
                    if (ActiveModuleChange != null)
                    {
                        ActiveModuleChange();
                    }
                    break;
                }
            }
        }
    }

    public enum ModuleType
    {
        Rain,
        Ripple,
        Lantern,
        Fireflies,
        DVD
    }

    public class ModuleData
    {
        public Color backgroundColor;
        public Texture2D logo;

        public ModuleData(Color _backgroundColor, Texture2D _logo)
        {
            backgroundColor = _backgroundColor;
            logo = _logo;
        }
    }
}
