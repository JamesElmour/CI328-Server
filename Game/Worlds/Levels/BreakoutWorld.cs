using PIGMServer.Game.Components;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using PIGMServer.UI;

namespace PIGMServer.Game.Worlds.Levels
{
    /// <summary>
    /// Breakout Subworld.
    /// </summary>
    public class BreakoutWorld : SubWorld
    {
        public bool Dead = false;   // If this SubWorld is dead.

        public BreakoutWorld(int index) : base(index)
        {

        }

        /// <summary>
        /// Set up all the SubWorld's Systems.
        /// </summary>
        protected override void SetupSystems()
        {
            AddSystem(new PlayerSystem(this));
            AddSystem(new ColliderSystem(this));
            AddSystem(new BallSystem(this));
            AddSystem(new BrickSystem(this));
            AddSystem(new PowerUpSystem(this));
        }

        /// <summary>
        /// Create components for the world.
        /// </summary>
        protected override void SetupComponents()
        {
            CreatePlayer();
            CreateBall();
            CreateBricks();
        }

        /// <summary>
        /// Set up the main Player entity.
        /// </summary>
        private void CreatePlayer()
        {
            // Get Player and Collider systems.
            PlayerSystem playerSystem = GetSystem<PlayerSystem>();
            ColliderSystem colliderSystem = GetSystem<ColliderSystem>();

            // Create Player Entity and Components.
            GameEntity entity = new GameEntity("Player", new Vector2(300, 680), "Player");
            Player player = new Player(entity);
            Collider collider = new Collider(entity, 256, 32, true);

            // Add them to the systems.
            playerSystem.Add(player);
            colliderSystem.Add(collider);
        }

        /// <summary>
        /// Create the initial Ball.
        /// </summary>
        private void CreateBall()
        {
            // Get Ball and Collider systems.
            BallSystem ballSystem = GetSystem<BallSystem>();
            ColliderSystem colliderSystem = GetSystem<ColliderSystem>();

            // Create Ball Entity and Components.
            GameEntity entity = new GameEntity("Ball", new Vector2(300, 600), "Ball");
            Ball ball = new Ball(entity)
            {
                Direction = new Vector2(133, 34)
            };
            Collider collider = new Collider(entity, 32, 32, true);

            // Add them to the systems.
            ballSystem.Add(ball);
            colliderSystem.Add(collider);
        }

        /// <summary>
        /// Create the initial Bricks.
        /// </summary>
        private void CreateBricks()
        {
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    // Calculate Brick's position and name.
                    Vector2 position = new Vector2((short)(x * 64 + 60), (short)(y * 16 + 420));
                    string name = "Brick_" + x + "_" + y;

                    // Create Entity and Components at position with name.
                    GameEntity parent = new GameEntity(name, position, "Brick");
                    Collider collider = new Collider(parent, 64, 16, false);
                    Brick brick = new Brick(parent, x, y);

                    // Add them to the systems.
                    GetSystem<ColliderSystem>().Add(collider);
                    GetSystem<BrickSystem>().Add(brick);
                }
            }
        }

        /// <summary>
        /// Set the World's opponent to the given SubWorld.
        /// </summary>
        /// <param name="world">New opponent.</param>
        public void ChangeOpponent(BreakoutWorld world)
        {
            Opponent = world;
        }
    }
}
