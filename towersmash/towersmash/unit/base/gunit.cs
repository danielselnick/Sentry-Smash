using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework.Content;

namespace towersmash
{
    public class gunit: iunit
    {
        public Texture2D texture;
        public Color color;
        private Iid _id;
        private float _rotationf;
        public float mass = 50;
        public master master;
        public Geom geom;
        public Body body;
        public int hp = 100;
        public int dmg = 10;
        public const float push = 75;
        private float _push = push;
        public string name;
        public string textureName = "default";
        public double timer = 0;
        public double interval = 2000;
        private int _playernumber;
        private Vector2 _rotationv2;

        public gunit()
        { }

        
        public void load(ContentManager Content)
        {
            Vector2 origin;
            texture = Content.Load<Texture2D>(textureName);
            body = BodyFactory.Instance.CreateBody(1f, 1f);
            geom = GeomFactory.Instance.CreateRectangleGeom(body, texture.Width, texture.Height);           
            geom.Tag = _id;
        }

        
        public float getPush
        {
            get { return _push; }
        }
        public void modifyPush(float modify)
        { _push *= modify; }

        public void addPush(float addPushValue)
        { _push += addPushValue; }


        public void reset()
        {
            _push = push;
        }
        
        public int playernumber 
        {
            get { return _playernumber; } 
            set 
            {
                _playernumber = value;
                geom.CollisionGroup = _playernumber;
                switch (_playernumber)
                {
                    case(1):
                        color = player.player1color;
                        break;
                    case (2):
                        color = player.player2color;
                        break;
                    case (3):
                        color = player.player3color;
                        break;
                    case(4):
                        color = player.player4color;
                        break;
                    default:
                        color = Color.White;
                        break;
                }
            }
        }
        public float rotationf
        {
            get { return _rotationf; }
            set
            {
                _rotationf = value;
                _rotationv2 = math.radianstovector(value);
                _rotationv2.Normalize();
                body.Rotation = value;
            }
        }
        
        /// <summary>
        /// Which direction the unit is facing
        /// </summary>
        public Vector2 rotationV2
        {
            get { return _rotationv2; }
            set
            {
                _rotationv2 = value;
                _rotationv2.Normalize();
                _rotationf = math.vectortoradians(value);
                body.Rotation = _rotationf;
            }
        }        

        /// <summary>
        /// Gets and sets the body position
        /// </summary>
        public Vector2 position
        {
            get { return body.Position; }
            set { body.Position = value; }            
        }

        /// <summary>
        /// Adds the gunit to the physics simulation
        /// </summary>
        /// <param name="physics">The physics simulator to add the gunit to</param>
        public void add(PhysicsSimulator physics)
        {
                physics.Add(body);
                physics.Add(geom);
        }

        /// <summary>
        /// Remove the gunit from the given physics simulation
        /// </summary>
        /// <param name="physics">The physics simulator to be removed from the simulation</param>
        public void remove(PhysicsSimulator physics)
        {            
                physics.Remove(body);
                physics.Remove(geom);            
        }

        public virtual void loadcontent(master master)
        {
            this.master = master;
            load(master.content);
            this.playernumber = playernumber;
        }

        public virtual void update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            //this.rotationf = body.Rotation;
        }

        public bool istimer
        {
            get
            {
                if (timer > interval)
                {
                    timer = 0;
                    return true;
                }
                else
                    return false;
            }
        }

        public virtual void clone(gunit unit)
        {
            hp = unit.hp;
            dmg = unit.dmg;
            _push = unit.getPush;
            name = unit.name;
            textureName = unit.textureName;
            texture = unit.texture;
            cloner.clonebody(ref this.body, ref unit.body);
            body.ResetDynamics();
            cloner.clonegeom(ref this.geom, ref unit.geom);
            geom.Tag = this.id;
            playernumber = unit.playernumber;
            geom.CollisionGroup = playernumber;
            interval = unit.interval;
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height), null, color, _rotationf, Vector2.Zero, SpriteEffects.None, 0f);
            //master.GameGraphics.DrawPolygon(geom.WorldVertices, this.color);           
        }

        public Iid id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == null)
                    _id = value;
                else
                    throw new Exception("already set the id, can't change it!");
            }
        }
    }
}
