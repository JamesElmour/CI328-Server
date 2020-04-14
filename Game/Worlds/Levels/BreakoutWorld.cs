using PIGMServer.Game.Components;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Worlds.Levels
{
    public class BreakoutWorld : SubWorld
    {
        public BreakoutWorld Opponent { get; private set; }

        public BreakoutWorld(int index) : base(index)
        {

        }

        protected override void SetupSystems()
        {
            AddSystem(new PlayerSystem(Name));
            AddSystem(new MoveableSystem(Name));
            AddSystem(new ColliderSystem(Name));
            AddSystem(new BallSystem(Name));
            AddSystem(new BrickSystem(Name));
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
            PlayerSystem playerSystem = SystemManager.GetSystem<PlayerSystem>(Name);
            ColliderSystem colliderSystem = SystemManager.GetSystem<ColliderSystem>(Name);
            GameEntity entity = new GameEntity("Player", new Vector2(300, 680), "Player");
            Player player = new Player(entity);
            Collider collider = new Collider(entity, 256, 32, true);

            playerSystem.Add(player);
            colliderSystem.Add(collider);

            entity.Add(player);
            entity.Add(collider);

            Entities.Add(entity.Name, entity);
        }

        private void CreateBall()
        {
            BallSystem ballSystem = SystemManager.GetSystem<BallSystem>(Name);
            ColliderSystem colliderSystem = SystemManager.GetSystem<ColliderSystem>(Name);
            GameEntity entity = new GameEntity("Ball", new Vector2(300, 600), "Ball");
            Ball ball = new Ball(entity)
            {
                Direction = new Vector2(133, 34)
            };
            Collider collider = new Collider(entity, 32, 32, true);

            ballSystem.Add(ball);
            colliderSystem.Add(collider);

            entity.Add(ball);
            entity.Add(collider);

            Entities.Add(entity.Name, entity);
        }

        private void CreateBricks()
        {
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    Vector2 position = new Vector2((short)(x * 64 + 60), (short)(y * 16 + 420));
                    string name = "Brick_" + x + "_" + y;
                    GameEntity parent = new GameEntity(name, position, "Brick");
                    Collider collider = new Collider(parent, 64, 16, false);
                    Brick brick = new Brick(parent, x, y);

                    SystemManager.GetSystem<ColliderSystem>(Name).Add(collider);
                    SystemManager.GetSystem<BrickSystem>(Name).Add(brick);

                    parent.Add(brick);
                    parent.Add(collider);

                    Entities.Add(parent.Name, parent);
                }
            }
        }

        public void ChangeOpponent(BreakoutWorld world)
        {
            Opponent = world;
        }
    }
}
