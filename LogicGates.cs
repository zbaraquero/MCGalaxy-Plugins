using System;
using MCGalaxy;
using MCGalaxy.Blocks;
using MCGalaxy.Events.LevelEvents;
using BlockID = System.UInt16;

namespace MCGalaxy
{
    public sealed class LogicGates : Plugin
    {
        public override string name { get { return "LogicGates"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override string creator { get { return "Nvzhnn"; } }
        public override bool LoadAtStartup { get { return true; } }

        static BlockID on = 155;    // oDoor_Green
        static BlockID off = 177;    // oDoor_Red
        static BlockID NOTGateBlock = Block.Orange;
        static BlockID ANDGateBlock = Block.Lime;

        public override void Load(bool startup)
        {
            OnBlockHandlersUpdatedEvent.Register(OnBlockHandlersUpdated, Priority.Low);
        }

        public override void Unload(bool shutdown)
        {
            OnBlockHandlersUpdatedEvent.Unregister(OnBlockHandlersUpdated);
        }

        static void OnBlockHandlersUpdated(Level lvl, BlockID block)
        {
            switch (block)
            {
                case Block.Orange:
                    lvl.PhysicsHandlers[NOTGateBlock] = TriggerNOT;
                    break;
                case Block.Lime:
                    lvl.PhysicsHandlers[ANDGateBlock] = TriggerAND;
                    break;
                default:
                    return;
            }
        }
        
        static void TriggerNOT(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;

            BlockID input = lvl.GetBlock((ushort)(x + 1), y, z);

            if (input == off)
            {
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            }
            else if (input == on)
            {
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
            }
        }

        static void TriggerAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;

            BlockID inputA = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID inputB = lvl.GetBlock((ushort)(x + 1), y, z);

            if ((inputA == off && inputB == off) || (inputA == off && inputB == on) || (inputA == on && inputB == off))
            {
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
            }
            else if (inputA == on && inputB == on)
            {
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            }
        }
    }
}