// Anything the player can hit with the light attack (for now, the dark hands).
// When Hamza's real player is ready, his attack just needs to call Hit() on
// anything it touches that has this interface.
public interface IHittable
{
    void Hit();
}
