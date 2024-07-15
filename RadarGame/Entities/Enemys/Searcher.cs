using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Engine;
using App.Engine.Template;
using Engine.graphics.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.others;
using RadarGame.Physics;

namespace RadarGame.Entities.Enemys
{
    public class Searcher : IEnemie, IDrawObject, IColisionObject, IcanBeHurt
    {
        // this enemy should try to move towards the player if it is in vision range
        // first make it, then try to avoid asteroids

        static int id = 0;
        public EnemyManager EnemyManager { get ; set ; }
        public PlayerObject target;
        public string Name { get ; set ; }
        public PhysicsDataS PhysicsData { get ; set ; }
        public Vector2 Position { get ; set ; }
        public Vector2 Center { get ; set ; }
        public float Rotation { get ; set ; }
        public List<Vector2> CollisonShape { get ; set ; }
        public bool Static { get ; set ; }
        private bool isDead = false;
        private bool isInRange = false;
        private Vector2 visionRangeMin;
        private Vector2 visionRangeMax;
        private float visionThreshold = 50f;  // maybe change
        private int direction = 0;

        private float explosiondistance = 500;
        //fake, stolen from Mine
        private static TextureAtlasRectangle texture = new TextureAtlasRectangle(new Vector2(0, 0), new Vector2(100, 100), new Vector2(1, 2), new Texture("resources/Enemies/Mine.png"), "Mine");

        public Searcher(Vector2 position, EnemyManager enemyManager)
        {
            Position = position;
            EnemyManager = enemyManager;
            Name = "Searcher" + id++;
            Static = false;
            visionRangeMin = new Vector2(0f, 1f);   // CHECK IF REALISTIC
            visionRangeMax = new Vector2(0f, visionThreshold);
            PlayerObject target = (PlayerObject)EntityManager.GetObject("Player");

            PhysicsData = new PhysicsDataS
            {
                Velocity = Vector2.Zero,
                Mass = 1f,
                Drag = 0.000f,
                Acceleration = Vector2.Zero,
                AngularAcceleration = 0f,
                AngularVelocity = 0.5f
            };
            CollisonShape = new List<Vector2>
            {
            new Vector2(-50, -50),
            new Vector2(50, -50),
            new Vector2(50, 50),
            new Vector2(-50, 50)
            };
        }

        public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
        {
            throw new NotImplementedException();

            if (target == null)
            {
                target = (PlayerObject)EntityManager.GetObject("Player");
                return;
            }
            // do stuff
            Behaviour();

            // do more stuff
        }

        public void Draw(List<View> surface)
        {
            throw new NotImplementedException();
        }

        public void OnColision(IColisionObject colidedObject)
        {
            throw new NotImplementedException();
            // explode();
            // explode like mine.cs, but without timer just boom
        }

        public bool applyDamage(int damage)
        {
            if (IsDead()) return false;
            // explode();
            return true;
        }

        public bool IsDead()
        {
            return isDead;
        }
        public bool IsInRange()
        {
            return isInRange;
        }

        public void onDeleted()
        {
            texture.Dispose();
        }

        private void explode()
        {
            if (IsDead()) return;
            isDead = true;
          
         AnimatedExposion.newExplosion(Position, explosiondistance*2);
         foreach (var colisionObject in ColisionSystem.getinRadius( Position, explosiondistance, false,true))
         {
             if (colisionObject is IcanBeHurt)
             {
                 ((IcanBeHurt)colisionObject).applyDamage( (int) (50f * (1 - (Position - colisionObject.Position).Length / explosiondistance)));
             }
         }
        }

        private void Behaviour()
        {
            Vector2 randomDirection = new Vector2(0, 0);

            // should check if player is in range and then engage
            if ((Vector2.Distance(target.Position, Position)) > visionThreshold) 
            {
                //spieler ist in vision, do move towards
                Movement(target.Position);
            } else
            {

                //check with raycast for asteroid infront
                IColisionObject? ifStuff = ColisionSystem.castRay(visionRangeMin, visionRangeMax, this);
                if (ifStuff != null)
                {
                    // asteroid is in raycast, avoid

                    randomDirection = new Vector2(1, -1); // noch nich fest, aber das is die richtung
                    
                } else
                {
                    // asteroid is not in raycast, move
                    randomDirection = new Vector2(0, 1); // noch nich fest, aber das is die richtung
                }

                Movement(randomDirection);
                
            }
            return;
        }

        private void Movement(Vector2 inputMovement)
        {
            // WEEEE

        }
    }
}
