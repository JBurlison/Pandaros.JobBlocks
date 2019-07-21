using Pipliz.JSON;
using System;
using System.IO;
using System.Reflection;

namespace Pandaros.JobBlocks
{
    [ModLoader.ModManager]
    public static class GameLoader
    {
        public const string NAMESPACE = "Pandaros.JobBlocks";
        public static string MESH_PATH = "gamedata/mods/Pandaros/JobBlocks/Meshes/";
        public static string AUDIO_PATH = "gamedata/mods/Pandaros/JobBlocks/Audio/";
        public static string ICON_PATH = "gamedata/mods/Pandaros/JobBlocks/icons/";
        public static string BLOCKS_ALBEDO_PATH = "Textures/albedo/";
        public static string BLOCKS_EMISSIVE_PATH = "Textures/emissive/";
        public static string BLOCKS_HEIGHT_PATH = "Textures/height/";
        public static string BLOCKS_NORMAL_PATH = "Textures/normal/";
        public static string BLOCKS_NPC_PATH = "gamedata/mods/Pandaros/JobBlocks/Textures/npc/";
        public static string TEXTURE_FOLDER_PANDA = "Textures";
        public static string NPC_PATH = "gamedata/textures/materials/npc/";
        public static string MOD_FOLDER = @"gamedata/mods/Pandaros/JobBlocks";
        public static string MODS_FOLDER = @"";
        public static string GAMEDATA_FOLDER = @"";
        public static string GAME_ROOT = @"";
        public static string SAVE_LOC = "";
        public static readonly Version MOD_VER = new Version(0, 0, 1, 0);
        public static bool RUNNING { get; private set; }
        public static bool WorldLoaded { get; private set; }
        public static Colony StubColony { get; private set; }
        public static JSONNode ModInfo { get; private set; }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterSelectedWorld, NAMESPACE + ".AfterSelectedWorld")]
        public static void AfterSelectedWorld()
        {
            WorldLoaded = true;
            SAVE_LOC = GAMEDATA_FOLDER + "savegames/" + ServerManager.WorldName + "/";
            StubColony = Colony.CreateStub(-99998);
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnAssemblyLoaded, NAMESPACE + ".OnAssemblyLoaded")]
        public static void OnAssemblyLoaded(string path)
        {
            MOD_FOLDER = Path.GetDirectoryName(path);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            JobBlocksLogger.Log("Found mod in {0}", MOD_FOLDER);

            GAME_ROOT = path.Substring(0, path.IndexOf("gamedata")).Replace("/", "/");
            GAMEDATA_FOLDER = path.Substring(0, path.IndexOf("gamedata") + "gamedata".Length).Replace("/", "/") + "/";

            MODS_FOLDER = GAMEDATA_FOLDER + "mods/";
            ICON_PATH = Path.Combine(MOD_FOLDER, "icons").Replace("\\", "/") + "/";
            MESH_PATH = Path.Combine(MOD_FOLDER, "Meshes").Replace("\\", "/") + "/";
            AUDIO_PATH = Path.Combine(MOD_FOLDER, "Audio").Replace("\\", "/") + "/";
            TEXTURE_FOLDER_PANDA = Path.Combine(MOD_FOLDER, "Textures").Replace("\\", "/") + "/";
            BLOCKS_ALBEDO_PATH = Path.Combine(TEXTURE_FOLDER_PANDA, "albedo").Replace("\\", "/") + "/";
            BLOCKS_EMISSIVE_PATH = Path.Combine(TEXTURE_FOLDER_PANDA, "emissive").Replace("\\", "/") + "/";
            BLOCKS_HEIGHT_PATH = Path.Combine(TEXTURE_FOLDER_PANDA, "height").Replace("\\", "/") + "/";
            BLOCKS_NORMAL_PATH = Path.Combine(TEXTURE_FOLDER_PANDA, "normal").Replace("\\", "/") + "/";
            BLOCKS_NPC_PATH = Path.Combine(TEXTURE_FOLDER_PANDA, "npc").Replace("\\", "/") + "/";

            ModInfo = JSON.Deserialize(MOD_FOLDER + "/modInfo.json")[0];
        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            JobBlocksLogger.Log(args.Name);
            try
            {
                //if (args.Name.Contains("System.Xml.Linq"))
                //    return Assembly.LoadFile(MOD_FOLDER + "/System.Xml.Linq.dll");

            }
            catch (Exception ex)
            {
                JobBlocksLogger.LogError(ex);
            }

            return null;
        }


        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterStartup, NAMESPACE + ".AfterStartup")]
        public static void AfterStartup()
        {
            RUNNING = true;
           // CommandManager.RegisterCommand(new ChatHistory());
#if Debug
            ChatCommands.CommandManager.RegisterCommand(new Research.PandaResearchCommand());
#endif
        }

        [ModLoader.ModCallback(ModLoader.EModCallbackType.OnQuit, NAMESPACE + ".OnQuitLate")]
        public static void OnQuitLate()
        {
            RUNNING = false;
            WorldLoaded = false;
        }
    }
}
