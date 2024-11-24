using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace AnimalMasterCaravan
{
    [StaticConstructorOnStartup]
    public static class AnimalMasterCaravanUtil
    {
        public static readonly Texture2D MasterIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/CaravanMaster");
        public static void DrawMasterIcon(Pawn pawn, Rect rect)
        {
            Log.Message("Drawing Texture");
            GUI.DrawTexture(rect, MasterIcon);
            if (Mouse.IsOver(rect))
            {
                string iconTooltipText = string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
                if (!iconTooltipText.NullOrEmpty())
                {
                    TooltipHandler.TipRegion(rect, iconTooltipText);
                }
            }
        }
    }

   
    [HarmonyPatch]
    public static class AnimalMasterCaravanPatch
    {
        
       
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(TransferableUIUtility), nameof(TransferableUIUtility.DoExtraIcons));
        }
        private static void Postfix(Transferable trad, Rect rect, ref float curX)
        {
            Log.Message("Postfix");
            if (!(trad.AnyThing is Pawn pawn))
            {
                return;
            }
           /* Log.Message(pawn.Label);
            Log.Message(pawn.RaceProps.Animal);
            Log.Message(pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond) == null);
            Log.Message(pawn.playerSettings.Master != null);*/
            if (pawn.RaceProps.Animal && pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond) == null && pawn.playerSettings != null && pawn.playerSettings.Master != null)
            {
                AnimalMasterCaravanUtil.DrawMasterIcon(pawn, new Rect(curX - 24f, (rect.height - 24f) / 2f, 24f, 24f));
                curX -= 24f;
            }
        }

    }
}
