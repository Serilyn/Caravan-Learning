using HarmonyLib;
using Verse;
using RimWorld;

namespace Caravan_Learning
{
    public class HarmonyPatches : Mod
    {
        public HarmonyPatches(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("caravanLearning");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Need_Learning),"IsFrozen", MethodType.Getter)]
        class Patch_IsFrozen
        {
            static void Postfix(ref bool __result, Pawn ___pawn)
            {
                if (___pawn.Map == null)
                    __result = true;
            }
        }

    }


}
