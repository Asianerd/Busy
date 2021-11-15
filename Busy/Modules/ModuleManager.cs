using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class ModuleManager
{
    public static Dictionary<ModuleType, ModuleData> moduleDataCollection = new Dictionary<ModuleType, ModuleData>();
    public static Dictionary<ModuleType, Action> moduleEnablingCollection = new Dictionary<ModuleType, Action>();
    public static List<ModuleType> moduleTypeCollection = new List<ModuleType>();
    public static ModuleType activeModule;

    public delegate void ModuleEvent();
    public static event ModuleEvent ActiveModuleChange;
    public static event ModuleEvent ResetModules;

    /* Steps to add new module
     *  1. Add module in ModuleType enum
     *  2. Add module Enable function in UpdateActiveModule
     *  3. Add ModuleData in Main.LoadContent
     *  4. Add LoadContent in Main.LoadContent
     *  5. Add Initialize in ModuleManager.Initialize
     */

    public static void Initialize()
    {
        moduleTypeCollection = Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>().ToList();
        moduleEnablingCollection = new Dictionary<ModuleType, Action>()
        {
            { ModuleType.Rain, () => { Rain.Enable(); } },
            { ModuleType.Ripple, () => { Ripple.Enable(); } },
            { ModuleType.Lantern, () => { Star.Enable();Lantern.Enable(); } },
            { ModuleType.Fireflies, () => { Firefly.Enable(); } },
            { ModuleType.DVD, () => { DVD.Enable(); } },
            { ModuleType.Star, () => { Star.Enable(); } },
        };
        activeModule = ModuleType.Star;

        Main.UpdateEvent += Update;
        ActiveModuleChange += UpdateActiveModule;

        #region Module initialization
        Rain.Initialize();
        Ripple.Initialize();
        Lantern.Initialize();
        Firefly.Initialize();
        DVD.Initialize();
        Star.Initialize();
        #endregion

        UpdateActiveModule();
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
        moduleEnablingCollection[activeModule].Invoke();
/*        switch(activeModule)
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
        }*/
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
        DVD,
        Star
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
