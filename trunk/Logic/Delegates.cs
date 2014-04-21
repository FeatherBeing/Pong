using PongController;

namespace PongController
{
    public delegate void PlayerWonHandler(Player winner);
    public delegate void CollisionHandler(CollisionType collisionType);
}