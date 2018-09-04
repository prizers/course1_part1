using System.Drawing;

namespace Digger
{
    public class CreatureAnimation
    {
        public CreatureCommand Command;
        public ICreature Creature;
        public Point Location;
        public Point TargetLogicalLocation;
    }
}