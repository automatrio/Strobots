using Godot;

namespace Multiplayer
{
    public struct SurrogateData
    {
        public SurrogateData(Vector3 position, int health = 0, int state = 0)
        {
            Position = position;
            Health = health;
            State = state;
        }
        public Vector3 Position { get; set; }
        public int Health { get; set; }
        public int State { get; set; }
    }
}