namespace PlayerStates
{
    using Godot;

    public struct SPlayerState
    {
        required public string Anim { get; init; }

        required public Vector2 Pos { get; init; }

        required public bool Flip { get; init; }
    }

}