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
    public class Shooter : IEnemie, IDrawObject, IColisionObject, IcanBeHurt
    {
        // this enemy should try to move towards the player if it is in vision range
        // then it should try to shoot the player
        // first make it, then try to avoid asteroids

        static int id = 0;
        public EnemyManager EnemyManager { get; set; }
        public PlayerObject target;
        public string Name { get; set; }
        public PhysicsDataS PhysicsData { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Center { get; set; }
        public float Rotation { get; set; }
        public List<Vector2> CollisonShape { get; set; }
        public bool Static { get; set; }
        private bool isDead = false;
        private bool isInRange = false;

        //fake, stolen from Mine
        private static TextureAtlasRectangle texture = new TextureAtlasRectangle(new Vector2(0, 0), new Vector2(100, 100), new Vector2(1, 2), new Texture("resources/Enemies/Mine.png"), "Mine");

        public Shooter(Vector2 position, EnemyManager enemyManager)
        {
            Position = position;
            EnemyManager = enemyManager;
            Name = "Searcher" + id++;
            Static = false;
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
            /* 
         AnimatedExposion.newExplosion(Position, explosiondistance*2);
         foreach (var colisionObject in ColisionSystem.getinRadius( Position, explosiondistance, false,true))
         {
             if (colisionObject is IcanBeHurt)
             {
                 ((IcanBeHurt)colisionObject).applyDamage( (int) (50f * (1 - (Position - colisionObject.Position).Length / explosiondistance)));
             }
         } */
        }

        private void Behaviour()
        {
            // think about it later
            // should check if player is in range and then engage

            //if in range it should try to shoot the target
            return;
        }

        private void Movement(Vector2 input)
        {
            // WEEE
        }

        private void shoot()
        {
            // pew pew
        }
    }

}
