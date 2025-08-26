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
        static BlockID NOTGate = 31;
        static BlockID ANDGate = 32;
        static BlockID NANDGate = 33;
        static BlockID ORGate = 34;
        static BlockID NORGate = 35;
        static BlockID XORGate = 36;

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
            if (block == 31) lvl.PhysicsHandlers[NOTGate] = TriggerNOT;
            else if (block == 32) lvl.PhysicsHandlers[ANDGate] = TriggerAND;
            else if (block == 33) lvl.PhysicsHandlers[NANDGate] = TriggerNAND;
            else if (block == 34) lvl.PhysicsHandlers[ORGate] = TriggerOR;
            else if (block == 35) lvl.PhysicsHandlers[NORGate] = TriggerNOR;
            else if (block == 36) lvl.PhysicsHandlers[XORGate] = TriggerXOR;
            else return;
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

            if ((input1 == on && input2 != on && input3 != on && input4 != on) ||
                (input1 != on && input2 == on && input3 != on && input4 != on) ||
                (input1 != on && input2 != on && input3 == on && input4 != on) ||
                (input1 != on && input2 != on && input3 != on && input4 == on))
                lvl.Blockchange(x, (ushort)(y + 1), z, on);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, off);
        }
    }
}