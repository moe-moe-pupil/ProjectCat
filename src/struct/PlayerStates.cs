using Godot;

namespace PlayerState
{
    public struct BasicState
    {
        public string Anim;
        public Vector2 Pos;
        public bool Flip;
        public void setValues(Vector2 pos, string anim, bool flip)
        {
            Anim = anim;
            Pos = pos;
            Flip = flip;
        }
    }
}
