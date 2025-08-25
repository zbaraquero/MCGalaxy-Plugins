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

        static BlockID on = 155;    // oDoor_Green
        static BlockID off = 177;    // oDoor_Red
        static BlockID NOTGateBlock = 31;
        static BlockID ANDGateBlock = 32;
        static BlockID ORGateBlock = 33;
        static BlockID NANDGateBlock = 34;
        static BlockID NORGateBlock = 35;
        static BlockID XORGateBlock = 36;

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
                case 31:
                    lvl.PhysicsHandlers[NOTGateBlock] = TriggerNOT;
                    break;
                case 32:
                    lvl.PhysicsHandlers[ANDGateBlock] = TriggerAND;
                    break;
                case 33:
                    lvl.PhysicsHandlers[ORGateBlock] = TriggerOR;
                    break;
                case 34:
                    lvl.PhysicsHandlers[NANDGateBlock] = TriggerNAND;
                    break;
                case 35:
                    lvl.PhysicsHandlers[NORGateBlock] = TriggerNOR;
                    break;
                case 36:
                    lvl.PhysicsHandlers[XORGateBlock] = TriggerXOR;
                    break;
                default:
                    return;
            }
        }

        static void TriggerNOT(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == off || input2 == off || input3 == off || input4 == off)
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
        }

        static void TriggerAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == off || input2 == off || input3 == off || input4 == off)
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
        }

        static void TriggerOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == on || input2 == on || input3 == on || input4 == on)
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
        }

        static void TriggerNAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (!(input1 == off || input2 == off || input3 == off || input4 == off))
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
        }

        static void TriggerNOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (!(input1 == on || input2 == on || input3 == on || input4 == on))
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
        }

        static void TriggerXOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if ((input1 == on && input2 == off) || (input1 == on && input3 == off) || (input1 == on && input4 == off) ||
                (input2 == on && input1 == off) || (input2 == on && input3 == off) || (input2 == on && input4 == off) ||
                (input3 == on && input1 == off) || (input3 == on && input2 == off) || (input3 == on && input4 == off) ||
                (input4 == on && input1 == off) || (input4 == on && input2 == off) || (input4 == on && input3 == off))
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
        }
    }
}