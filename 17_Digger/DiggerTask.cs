using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName() => "Terrain.png";
        public int GetDrawingPriority() => 999;
        public CreatureCommand Act(int x, int y) =>
            new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };
        public bool DeadInConflict(ICreature conflictedObject) =>
            conflictedObject is Player;
    }

    public class Player : ICreature
    {
        public string GetImageFileName() => "Digger.png";

        public int GetDrawingPriority() => 0;

        public CreatureCommand Act(int x, int y)
        {
            var cmd = new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };
            switch (Game.KeyPressed)
            {
                case System.Windows.Forms.Keys.Up:
                    cmd.DeltaY = -1;
                    break;
                case System.Windows.Forms.Keys.Down:
                    cmd.DeltaY = 1;
                    break;
                case System.Windows.Forms.Keys.Left:
                    cmd.DeltaX = -1;
                    break;
                case System.Windows.Forms.Keys.Right:
                    cmd.DeltaX = 1;
                    break;
                default:
                    break;
            }
            if (!CanWalkTo(x + cmd.DeltaX, y + cmd.DeltaY)) cmd.DeltaX = cmd.DeltaY = 0;
            else if (Game.Map.GetValue(x + cmd.DeltaX, y + cmd.DeltaY) is Gold) Game.Scores += 10;
            return cmd;
        }

        public bool DeadInConflict(ICreature conflictedObject) =>
            (conflictedObject is Sack) ||
            (conflictedObject is Monster);

        private bool CanWalkTo(int x, int y)
        {
            if (x < 0 || y < 0 || Game.MapWidth <= x || Game.MapHeight <= y) return false;
            var cell = Game.Map.GetValue(x, y);
            return !(cell is Sack);
        }
    }

    public class Sack : ICreature
    {
        public string GetImageFileName() => "Sack.png";
        public int GetDrawingPriority() => 10;

        public CreatureCommand Act(int x, int y)
        {
            var cmd = new CreatureCommand { DeltaX = 0, DeltaY = 1, TransformTo = this };

            if (CanFallTo(x + cmd.DeltaX, y + cmd.DeltaY))
            {
                ++FallingTime;
            }
            else
            {
                if (FallingTime > 1)
                {
                    cmd.TransformTo = new Gold();
                }
                FallingTime = 0;
                cmd.DeltaY = 0;
            }
            return cmd;
        }

        private bool CanFallTo(int x, int y)
        {
            if (x < 0 || y < 0 || Game.MapWidth <= x || Game.MapHeight <= y)
                return false;
            var cell = Game.Map.GetValue(x, y);
            return (cell == null) ||
                (IsFalling() && ((cell is Player) || (cell is Monster)));
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public bool IsFalling() => FallingTime > 0;

        int FallingTime = 0;
    }

    public class Gold : ICreature
    {
        public string GetImageFileName() => "Gold.png";
        public int GetDrawingPriority() => 10;
        public CreatureCommand Act(int x, int y) =>
            new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };
        public bool DeadInConflict(ICreature conflictedObject) => true;
    }

    public class Monster : ICreature
    {
        public string GetImageFileName() => "Monster.png";

        public int GetDrawingPriority() => 20;

        public CreatureCommand Act(int x, int y)
        {
            var cmd = new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };
            if (IsPlayerInSection(0, 0, x, Game.MapHeight) && CanWalkTo(x - 1, y))
                cmd.DeltaX = -1;
            else if (IsPlayerInSection(x + 1, 0, Game.MapWidth, Game.MapHeight) &&
                     CanWalkTo(x + 1, y))
                cmd.DeltaX = 1;
            else if (IsPlayerInSection(0, 0, Game.MapWidth, y) && CanWalkTo(x, y - 1))
                cmd.DeltaY = -1;
            else if (IsPlayerInSection(0, y + 1, Game.MapWidth, Game.MapHeight) &&
                     CanWalkTo(x, y + 1))
                cmd.DeltaY = 1;
            return cmd;
        }

        private bool IsPlayerInSection(int x0, int y0, int x1, int y1)
        {
            for (var x = x0; x < x1; ++x)
            {
                for (var y = y0; y < y1; ++y)
                {
                    if (Game.Map.GetValue(x, y) is Player) return true;
                }
            }
            return false;
        }

        private bool CanWalkTo(int x, int y)
        {
            if (x < 0 || y < 0 || Game.MapWidth <= x || Game.MapHeight <= y) return false;
            var cell = Game.Map.GetValue(x, y);
            return (cell == null) ||
                !((cell is Sack) || (cell is Monster) || (cell is Terrain));
        }

        public bool DeadInConflict(ICreature conflictedObject) =>
            (conflictedObject is Monster) ||
            ((conflictedObject is Sack) && (conflictedObject as Sack).IsFalling());
    }

}
