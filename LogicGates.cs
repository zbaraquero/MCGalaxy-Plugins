using System;
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

        static BlockID HIGH = 155;    // oDoor_Green
        static BlockID LOW = 177;    // oDoor_Red
        static BlockID CLK = 30;
        static BlockID NOT = 31;
        static BlockID AND = 32;
        static BlockID NAND = 33;
        static BlockID OR = 34;
        static BlockID NOR = 35;
        static BlockID XOR = 36;
        static ushort clkTicks = 0;
        const ushort ClkTickThreshold = 25;

        public override void Load(bool startup)
        {
            Level[] levels = LevelInfo.Loaded.Items;
            foreach (Level lvl in levels)
            {
                lvl.Config.PhysicsSpeed = 25;
                lvl.PhysicsHandlers[CLK] = UpdateCLK;
                lvl.PhysicsHandlers[NOT] = TriggerNOT;
                lvl.PhysicsHandlers[AND] = TriggerAND;
                lvl.PhysicsHandlers[NAND] = TriggerNAND;
                lvl.PhysicsHandlers[OR] = TriggerOR;
                lvl.PhysicsHandlers[NOR] = TriggerNOR;
                lvl.PhysicsHandlers[XOR] = TriggerXOR;
            }

            OnBlockHandlersUpdatedEvent.Register(OnBlockHandlersUpdated, Priority.Low);
        }

        public override void Unload(bool shutdown)
        {
            Level[] levels = LevelInfo.Loaded.Items;
            foreach (Level lvl in levels)
            {
                lvl.Config.PhysicsSpeed = 250;
                lvl.PhysicsHandlers[CLK] = null;
                lvl.PhysicsHandlers[NOT] = null;
                lvl.PhysicsHandlers[AND] = null;
                lvl.PhysicsHandlers[NAND] = null;
                lvl.PhysicsHandlers[OR] = null;
                lvl.PhysicsHandlers[NOR] = null;
                lvl.PhysicsHandlers[XOR] = null;
            }

            OnBlockHandlersUpdatedEvent.Unregister(OnBlockHandlersUpdated);
        }

        static void OnBlockHandlersUpdated(Level lvl, BlockID block)
        {
            if (block == CLK) lvl.PhysicsHandlers[block] = UpdateCLK;
            else if (block == NOT) lvl.PhysicsHandlers[block] = TriggerNOT;
            else if (block == AND) lvl.PhysicsHandlers[block] = TriggerAND;
            else if (block == NAND) lvl.PhysicsHandlers[block] = TriggerNAND;
            else if (block == OR) lvl.PhysicsHandlers[block] = TriggerOR;
            else if (block == NOR) lvl.PhysicsHandlers[block] = TriggerNOR;
            else if (block == XOR) lvl.PhysicsHandlers[block] = TriggerXOR;
        }

        static void UpdateCLK(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;

            if (clkTicks == ClkTickThreshold)
            {
                clkTicks = 0;
                if (lvl.GetBlock(x, (ushort)(y + 1), z) == LOW)
                    lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
                else
                    lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
            }
            else
                clkTicks++;
        }

        static void TriggerNOT(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == LOW || input2 == LOW || input3 == LOW || input4 == LOW)
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static void TriggerAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == LOW || input2 == LOW || input3 == LOW || input4 == LOW)
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
        }

        static void TriggerNAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (!(input1 == LOW || input2 == LOW || input3 == LOW || input4 == LOW))
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
        }

        static void TriggerOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (input1 == HIGH || input2 == HIGH || input3 == HIGH || input4 == HIGH)
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static void TriggerNOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if (!(input1 == HIGH || input2 == HIGH || input3 == HIGH || input4 == HIGH))
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static void TriggerXOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID input1 = lvl.GetBlock((ushort)(x - 1), y, z);
            BlockID input2 = lvl.GetBlock((ushort)(x + 1), y, z);
            BlockID input3 = lvl.GetBlock(x, y, (ushort)(z - 1));
            BlockID input4 = lvl.GetBlock(x, y, (ushort)(z + 1));

            if ((input1 == HIGH && input2 != HIGH && input3 != HIGH && input4 != HIGH) ||
                (input1 != HIGH && input2 == HIGH && input3 != HIGH && input4 != HIGH) ||
                (input1 != HIGH && input2 != HIGH && input3 == HIGH && input4 != HIGH) ||
                (input1 != HIGH && input2 != HIGH && input3 != HIGH && input4 == HIGH))
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }
    }
}