using System;
using System.Collections.Generic;
using MCGalaxy.Blocks;
using MCGalaxy.Events.LevelEvents;
using MCGalaxy.Tasks;
using BlockID = System.UInt16;

namespace MCGalaxy
{
    public sealed class LogicGates : Plugin
    {
        public override string name { get { return "LogicGates"; } }
        public override string MCGalaxy_Version { get { return "1.9.5.3"; } }
        public override string creator { get { return "Nvzhnn"; } }

        public static SchedulerTask task;

        static List<string> logicGatesLevels = new List<string>
        {
            "main", "nvzhnn"
        };

        static BlockID HIGH = 155;    // oDoor_Green
        static BlockID LOW = 177;    // oDoor_Red
        static BlockID CLK = 30;
        static BlockID NOT = 31;
        static BlockID AND = 32;
        static BlockID NAND = 33;
        static BlockID OR = 34;
        static BlockID NOR = 35;
        static BlockID XOR = 36;

        public override void Load(bool startup)
        {
            foreach (Level lvl in LevelInfo.Loaded.Items)
            {
                if (logicGatesLevels.Contains(lvl.name))
                {
                    RefreshPluginBlocks(lvl);
                    lvl.Config.PhysicsSpeed = 5;
                    lvl.PhysicsHandlers[NOT] = TriggerNAND; // could use TriggerNOR
                    lvl.PhysicsHandlers[AND] = TriggerAND;
                    lvl.PhysicsHandlers[NAND] = TriggerNAND;
                    lvl.PhysicsHandlers[OR] = TriggerOR;
                    lvl.PhysicsHandlers[NOR] = TriggerNOR;
                    lvl.PhysicsHandlers[XOR] = TriggerXOR;
                }
            }

            OnBlockHandlersUpdatedEvent.Register(OnBlockHandlersUpdated, Priority.Low);
            task = Server.MainScheduler.QueueRepeat(ToggleCLK, null, TimeSpan.FromMilliseconds(500));
        }

        public override void Unload(bool shutdown)
        {
            foreach (Level lvl in LevelInfo.Loaded.Items)
            {
                if (logicGatesLevels.Contains(lvl.name))
                {
                    lvl.Config.PhysicsSpeed = 250;
                    lvl.PhysicsHandlers[NOT] = null;
                    lvl.PhysicsHandlers[AND] = null;
                    lvl.PhysicsHandlers[NAND] = null;
                    lvl.PhysicsHandlers[OR] = null;
                    lvl.PhysicsHandlers[NOR] = null;
                    lvl.PhysicsHandlers[XOR] = null;
                }
            }

            OnBlockHandlersUpdatedEvent.Unregister(OnBlockHandlersUpdated);
            Server.MainScheduler.Cancel(task);
        }

        static void OnBlockHandlersUpdated(Level lvl, BlockID block)
        {
            if (block == NOT) lvl.PhysicsHandlers[block] = TriggerNAND; // could use TriggerNOR
            else if (block == AND) lvl.PhysicsHandlers[block] = TriggerAND;
            else if (block == NAND) lvl.PhysicsHandlers[block] = TriggerNAND;
            else if (block == OR) lvl.PhysicsHandlers[block] = TriggerOR;
            else if (block == NOR) lvl.PhysicsHandlers[block] = TriggerNOR;
            else if (block == XOR) lvl.PhysicsHandlers[block] = TriggerXOR;
        }

        void ToggleCLK(SchedulerTask task)
        {
            foreach (Level lvl in LevelInfo.Loaded.Items)
            {
                if (logicGatesLevels.Contains(lvl.name))
                {
                    for (ushort x = 0; x < lvl.Width; x++)
                        for (ushort y = 0; y < lvl.Height; y++)
                            for (ushort z = 0; z < lvl.Length; z++)
                            {
                                if (lvl.FastGetBlock(x, y, z) == CLK)
                                    if (lvl.FastGetBlock(x, (ushort)(y + 1), z) == LOW)
                                        lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
                                    else
                                        lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
                            }
                }
            }
        }

        static void TriggerAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID[] inputs = ReadInputs(lvl, x, y, z);

            if (inputs[0] == LOW || inputs[1] == LOW || inputs[2] == LOW || inputs[3] == LOW)
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
        }

        static void TriggerNAND(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID[] inputs = ReadInputs(lvl, x, y, z);

            if (inputs[0] == LOW || inputs[1] == LOW || inputs[2] == LOW || inputs[3] == LOW)
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static void TriggerOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID[] inputs = ReadInputs(lvl, x, y, z);

            if (inputs[0] == HIGH || inputs[1] == HIGH || inputs[2] == HIGH || inputs[3] == HIGH)
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static void TriggerNOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID[] inputs = ReadInputs(lvl, x, y, z);

            if (inputs[0] == HIGH || inputs[1] == HIGH || inputs[2] == HIGH || inputs[3] == HIGH)
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
        }

        static void TriggerXOR(Level lvl, ref PhysInfo C)
        {
            ushort x = C.X, y = C.Y, z = C.Z;
            BlockID[] inputs = ReadInputs(lvl, x, y, z);
            ushort highCount = 0;

            foreach (BlockID input in inputs)
                if (input == HIGH) highCount++;

            if (highCount % 2 == 1)
                lvl.Blockchange(x, (ushort)(y + 1), z, HIGH);
            else
                lvl.Blockchange(x, (ushort)(y + 1), z, LOW);
        }

        static BlockID[] ReadInputs(Level lvl, ushort x, ushort y, ushort z)
        {
            return new BlockID[]
            {
                lvl.FastGetBlock((ushort)(x - 1), y, z),
                lvl.FastGetBlock((ushort)(x + 1), y, z),
                lvl.FastGetBlock(x, y, (ushort)(z - 1)),
                lvl.FastGetBlock(x, y, (ushort)(z + 1)),
            };
        }

        void RefreshPluginBlocks(Level lvl)
        {
            for (ushort x = 0; x < lvl.Width; x++)
                for (ushort y = 0; y < lvl.Height; y++)
                    for (ushort z = 0; z < lvl.Length; z++)
                    {
                        BlockID b = lvl.FastGetBlock(x, y, z);

                        if (b == NOT || b == AND || b == NAND ||
                            b == OR || b == NOR || b == XOR)
                            lvl.AddCheck(lvl.PosToInt(x, y, z));
                    }
        }
    }
}