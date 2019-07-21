using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pandaros.API.Models;
using Pandaros.API.Research;
using Pandaros.API;
using Recipes;
using Science;
using Jobs;
using NPC;
using UnityEngine;

namespace Pandaros.JobBlocks.Jobs
{
    [ModLoader.ModManager]
    public static class FishingHolerRegister
    {
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, GameLoader.NAMESPACE + ".FishingHoleRegister.RegisterJobs")]
        [ModLoader.ModCallbackProvidesFor("create_savemanager")]
        [ModLoader.ModCallbackProvidesFor("pipliz.server.loadresearchables")]
        public static void RegisterJobs()
        {
            NPCType.AddSettings(new NPCTypeStandardSettings
            {
                keyName = GameLoader.NAMESPACE + ".Fisherman",
                printName = "Fishing Hole",
                maskColor1 = new Color32(175, 199, 237, 255),
                type = NPCTypeID.GetNextID()
            });

            ServerManager.BlockEntityCallbacks.RegisterEntityManager(new BlockJobManager<CraftingJobInstance>(new CraftingJobSettings(GameLoader.NAMESPACE + ".FishingHole", GameLoader.NAMESPACE + ".Fisherman")));
        }
    }

    public class FishingHoleTexture : CSTextureMapping
    {
        public override string name => GameLoader.NAMESPACE + ".FishingHole";
        public override string albedo => GameLoader.BLOCKS_ALBEDO_PATH + "FishingHole.png";
        public override string height => GameLoader.BLOCKS_HEIGHT_PATH + "FishingHole.png";
        public override string normal => GameLoader.BLOCKS_NORMAL_PATH + "FishingHole.png";
    }

    public class FishingHole : CSType
    {
        public override string name { get; set; } = GameLoader.NAMESPACE + ".FishingHole";
        public override string sideall { get; set; } = "planks";
        public override string sideyp { get; set; } = GameLoader.NAMESPACE + ".FishingHole";
        public override string icon { get; set; } = GameLoader.ICON_PATH + "FishingHole.png";
        public override string onPlaceAudio { get; set; } = GameLoader.NAMESPACE + ".Splash";
        public override string onRemoveAudio { get; set; } = GameLoader.NAMESPACE + ".Splash";
        public override List<string> categories { get; set; } = new List<string>()
        {
            "job"
        };
    }

    public class FishingHoleRecipe : ICSRecipe
    {
        public List<RecipeItem> requires => new List<RecipeItem>()
        {
            new RecipeItem(ColonyBuiltIn.ItemTypes.PLANKS.Id, 4),
            new RecipeItem(ColonyBuiltIn.ItemTypes.BUCKETWATER.Id, 4)
        };

        public List<RecipeResult> results => new List<RecipeResult>()
        {
            new RecipeResult(GameLoader.NAMESPACE + ".FishingHole")
        };

        public CraftPriority defaultPriority => CraftPriority.Medium;

        public bool isOptional => true;

        public int defaultLimit => 5;

        public string Job => ColonyBuiltIn.NpcTypes.CRAFTER;

        public string name => GameLoader.NAMESPACE + ".FishingHole";
    }

    public class FishingHoleScience : PandaResearch
    {
        public override string name => GameLoader.NAMESPACE + ".FishingHole";

        public override string IconDirectory => GameLoader.ICON_PATH;

        public override bool AddLevelToName => false;

        public override int NumberOfLevels => 1;

        public override int BaseIterationCount => 5;

        public override Dictionary<int, List<string>> Dependancies => new Dictionary<int, List<string>>()
        {
            {
                1,
                new List<string>()
                {
                    ColonyBuiltIn.Research.WATERGATHERER
                }
            }
        };

        public override Dictionary<int, List<InventoryItem>> RequiredItems => new Dictionary<int, List<InventoryItem>>()
        {
            {
                1,
                new List<InventoryItem>()
                {
                    new InventoryItem(ColonyBuiltIn.ItemTypes.PLANKS.Id, 5),
                    new InventoryItem(ColonyBuiltIn.ItemTypes.BUCKETWATER.Id, 5)
                }
            }
        };

        public override Dictionary<int, List<RecipeUnlock>> Unlocks => new Dictionary<int, List<RecipeUnlock>>()
        {
            {
                1,
                new List<RecipeUnlock>()
                {
                    new RecipeUnlock(GameLoader.NAMESPACE + ".FishingHole", ERecipeUnlockType.Recipe)
                }
            }
        };
    }
}
