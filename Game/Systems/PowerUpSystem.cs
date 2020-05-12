using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using static PIGMServer.Game.Components.PowerUp;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// System which processes all PowerUps for ECS technique.
    /// </summary>
    public class PowerUpSystem : GameSystem<PowerUp>
    {
        public bool KeyPressed = false;                                     // Track if user has pressed key, activating PowerUp.
        public bool PowerUpInUse = false;                                   // If PowerUp is in-use.
        public bool HasPowerUp = false;                                     // If SubWorld has PowerUp ready.
        private List<GameEntity> SpawnedObjects = new List<GameEntity>();   // Track all spawned Entities by PowerUps.

        /// <summary>
        /// Create PowerUp system.
        /// </summary>
        /// <param name="world">Owning SubWorld.</param>
        public PowerUpSystem(SubWorld world) : base(world)
        { }

        // Dictionaries formatted for easy PowerUp choice depending on combo.
        private Dictionary<int, Dictionary<int, PowerUps>> PowerLookUps = new Dictionary<int, Dictionary<int, PowerUps>>()
        {
            {
                1, new Dictionary<int, PowerUps>()
                {
                    { 1, PowerUps.ExtraLife },
                    { 2, PowerUps.MultiBall },
                    { 3, PowerUps.SpeedBall }
                }
            },
            {
                2, new Dictionary<int, PowerUps>()
                {
                    { 1, PowerUps.RapidBall },
                    { 2, PowerUps.QuadBall },
                    { 3, PowerUps.ExtraLife }
                }
            }
        };

        /// <summary>
        /// Activate the currently held PowerUp.
        /// </summary>
        public void ActivatePowerUp()
        {
            KeyPressed = true;
        }

        /// <summary>
        /// Process the given PowerUp component.
        /// </summary>
        /// <param name="component">PowerUp component for processing.</param>
        /// <param name="deltaTime">Deltatime.</param>
        protected override void Process(PowerUp component, float deltaTime)
        {
            if (PowerUpInUse) // If there is a PowerUp in use...
                ProcessActivePowerUp(component, deltaTime); // ... Process the active PowerUp.
            else if (CanUsePowerUp()) // Otherwise, if can use PowerUp...
                UsePowerUp(component); // ... Use the PowerUp.
            
            if (KeyPressed) // If the use key is pressed...
                KeyPressed = false; // ... Unpress it.

        }

        /// <summary>
        /// Process the Given active PowerUp.
        /// </summary>
        /// <param name="powerUp">Active PowerUp.</param>
        /// <param name="deltaTime">DeltaTime.</param>
        void ProcessActivePowerUp(PowerUp powerUp, float deltaTime)
        {
            // Deduct DeltaTime from time remaining.
            powerUp.TimeTillEnd -= deltaTime;

            if(powerUp.TimeTillEnd <= 0) // If PowerUp has ended...
            {
                // Destroy PowerUp and end it.
                powerUp.Parent.Destroy = true;
                EndPowerUp();
            }
        }

        /// <summary>
        /// Reset game state to before activate PowerUp.
        /// </summary>
        void EndPowerUp()
        {
            // Reset ball speed and destroy spawned objects.
            World.Opponent.GetSystem<BallSystem>().SetSpeed(600);
            DestroySpawnedPowerUpObjects();

            // Set PowerUp not in use.
            PowerUpInUse = false;
        }

        /// <summary>
        /// Calculate if a PowerUp can be used at this given moment.
        /// </summary>
        /// <returns>If PowerUp can be used.</returns>
        bool CanUsePowerUp()
        {
            return !PowerUpInUse;
        }

        /// <summary>
        /// Use the given PowerUp.
        /// </summary>
        /// <param name="powerUp">PowerUp to use.</param>
        void UsePowerUp(PowerUp powerUp)
        {
            if (!powerUp.Used) // If PowerUp hasn't been used...
            {
                // Set PowerUp in use and set its timeout to 10 seconds.
                PowerUpInUse = true;
                powerUp.TimeTillEnd = 10;

                switch (powerUp.Type) // Find PowerUp's associated functionality.
                {
                    case PowerUps.MultiBall:
                        Multiball(1);
                        break;
                    case PowerUps.ExtraLife:
                        AddLife();
                        break;
                    case PowerUps.QuadBall:
                        Multiball(3);
                        break;
                    case PowerUps.SpeedBall:
                        Speedball(1000);
                        break;
                    case PowerUps.RapidBall:
                        Speedball(1400);
                        break;
                }

                // Destroy PowerUp and set it used.
                powerUp.Parent.Destroy = true;
                powerUp.Used = true;
            }
        }

        /// <summary>
        /// Add a life to the player.
        /// </summary>
        void AddLife()
        {
            World.GetSystem<PlayerSystem>().Get("Player").Lives++;
        }


        /// <summary>
        /// Spawn the given number of balls.
        /// </summary>
        /// <param name="balls">Balls to spawn.</param>
        void Multiball(int balls)
        {
            SpawnBalls(balls);
        }

        /// <summary>
        /// Set all active Ball speeds to the given speed.
        /// </summary>
        /// <param name="speed">Speed set Balls to.</param>
        void Speedball(int speed)
        {
            World.Opponent.GetSystem<BallSystem>().SetSpeed(speed);
        }

        /// <summary>
        /// Spawn the given count of Balls at random positions.
        /// </summary>
        /// <param name="count">Count of Balls to spawn</param>
        void SpawnBalls(int count)
        {
            // For each Ball to spawn.
            for(int i = 1; i < count + 1; i++)
            {
                // Get random X position.
                short xPos = (short)((new Random().NextDouble() - 0.5) * 720);

                // Create spawned Ball's Entity at random position.
                GameEntity ballEntity = new GameEntity("PowerUp_Ball_" + i);
                ballEntity.Position.x = xPos;
                ballEntity.Position.y = 600;

                // Create the new Ball with a set direction and ID.
                Ball ball = new Ball(ballEntity)
                {
                    Direction = new Vector2(150, 150),
                    ID = i
                };

                // Create Ball's Collider.
                Collider collider = new Collider(ballEntity, 32, 32, true);

                // Add Ball and Collider to world systems.
                World.Opponent.GetSystem<BallSystem>().Add(ball);
                World.Opponent.GetSystem<ColliderSystem>().Add(collider);

                // Add Ball to spawned objects list.
                SpawnedObjects.Add(ballEntity);
            }
        }

        /// <summary>
        /// Destroy call objects spawned by PowerUps.
        /// </summary>
        void DestroySpawnedPowerUpObjects()
        {
            // Foreach spawned object, destroy it.
            SpawnedObjects.ForEach((x) => x.Destroy = true);
        }

        /// <summary>
        /// Create a Message housing information for creation of PowerUp.
        /// </summary>
        /// <param name="p">Given PowerUp.</param>
        /// <returns>Message containing information on created PowerUp.</returns>
        public override Message Create(PowerUp p)
        {
            return new Message((int) SuperOps.PowerUp, ((int) p.Type) * 2 + 1);
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.PowerUp;
        }

        /// <summary>
        /// Create a PowerUp with the given combo.
        /// </summary>
        /// <param name="combo">Combo to create PowerUp from.</param>
        public void CreatePowerup(int combo)
        {
            // Get weighted combo for PowerUp pool selection.
            int weightedCombo = 0;

            if (combo >= 5)
            {
                if (combo <= 9)
                    weightedCombo = 1;
                else if (combo <= 15)
                    weightedCombo = 2;
                else
                    weightedCombo = 2;
            }


            if(weightedCombo > 0) // If weighted combo is above 0...
            {
                // ... Select a random PowerUp.
                int choice = new Random().Next(1, 3);
                PowerUps op = PowerLookUps[weightedCombo][choice];

                if (Components.Count == 0) // If there are no activate or waiting PowerUps...
                {
                    // Create the PowerUp.
                    GameEntity entity = new GameEntity("PowerUp_" + op.ToString() + Components.Count);
                    PowerUp pow = new PowerUp(entity, op)
                    {
                        Altered = true
                    };

                    Add(pow);
                }
            }

        }

        /// <summary>
        /// PowerUp doesn't return any alterations, only for creation.
        /// </summary>
        protected override Message GatherAlterations(PowerUp alteredComponent)
        {
            return null;
        }
    }
}
