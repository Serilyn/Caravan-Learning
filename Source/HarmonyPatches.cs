using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;

namespace Caravan_Learning
{
    public class HarmonyPatches : Mod
    {
        public HarmonyPatches(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("caravanLearning");
            harmony.PatchAll();
        }

        //Freeze learning decay
        [HarmonyPatch(typeof(Need_Learning),"IsFrozen", MethodType.Getter)]
        class Patch_IsFrozen
        {
            static void Postfix(ref bool __result, Pawn ___pawn)
            {
                if (___pawn.Map == null)
                    __result = true;
            }
        }

        //Allow our growth tier to increase on caravan
        [HarmonyPatch(typeof(Pawn_AgeTracker), "GrowthPointsPerDay", MethodType.Getter)]
        class Patch_NeedInterval
        {
            static void Postfix(ref float __result, Pawn_AgeTracker __instance, Pawn ___pawn)
            {
                Need_Learning learning = ___pawn.needs.learning;
                // It's okay if learning is frozen while on caravan so skip the suspended check.
                if (learning != null && ___pawn.Map == null)
                {
                    MethodInfo methodInfo = typeof(Pawn_AgeTracker).GetMethod("GrowthPointsPerDayAtLearningLevel", BindingFlags.NonPublic | BindingFlags.Instance);
                    var parameters = new object[] { learning.CurLevel };
                    __result = (float)methodInfo.Invoke(__instance, parameters);
                }
            }
        }

    }


}
