using Pong;

namespace Pong
{
    internal delegate void PlayerWonHandler(Player winner);
    internal delegate void TickEventHandler();
    internal delegate void CollisionHandler(CollisionType collisionType);
}