﻿using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game.Modding;
using Game.SceneFlow;
using Game;
using Unity.Entities;
using Colossal.UI;
using System.IO;

namespace AssetUIShuffler
{
    public class Mod : IMod
    {
        public static string Name = "Asset UI Shuffler";
        public static string Version = "1.0.1";
        public static string Author = "StarQ";
        public static string uiHostName = "starq-asset-ui-shuffler";

        public static ILog log = LogManager.GetLogger($"{nameof(AssetUIShuffler)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        public static Setting m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                //log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            UIManager.defaultUISystem.AddHostLocation(uiHostName, Path.Combine(Path.GetDirectoryName(asset.path), ".Thumbs"), false);

            AssetDatabase.global.LoadSettings(nameof(AssetUIShuffler), m_Setting, new Setting(this));
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<UIShufflerSystem>();
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.PathwayPriorityDropdownVersion = 0;
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
            UIManager.defaultUISystem.RemoveHostLocation(uiHostName);
        }
    }
}
