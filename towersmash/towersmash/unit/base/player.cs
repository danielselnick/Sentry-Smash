#define MELEEENABLED

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics.Collisions;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace towersmash
{ 
    
    public class player: gunit
    {
        public static Color player1color = Color.Red;
        public static Color player2color = Color.Yellow; 
        public static Color player3color = Color.Orange;
        public static Color player4color = Color.Turquoise;
       
        /// <summary>
        /// The players weapon which the player may use to fre at the neemy players
        /// </summary>
        public weapon pweapon;
        /// <summary>
        /// The players tower the player may use to deploy around the field
        /// </summary>
        public tower ptower;
        /// <summary>
        /// Players movespeed
        /// Used to apply a force to the player times the movement direction
        /// </summary>
        public float movespeed = 150000;
        private double _towerspawnrate = 1000;
        public const float sprintmodifier = 33;
        private float _sprintmodifier = sprintmodifier;
        public double shieldtimer = 0;
        public double shieldtime = 2000;
        public bool isShield = false;
        public double shieldcooldowntimer = 0;
        public double shieldcooldown = 5000;
        public bool isShieldcooldown = false;
        public Texture2D shieldtexture;
        public double sprinttimer = 0;
        public double sprinttime = 750;
        public bool isSprintcooldown = false;
        public bool isMeleecooldown = false;
        public double meleetimer = 0;
        public double meleetime = 333;
        bool meleedraw = false;
        int meleedrawcount = 0;
        public int lives = 5;
        private AABB aabb;
        Vector2 location = new Vector2();
        Stack<Iid> indexes = new Stack<Iid>();

        public double towerspawnrate
        {
            get { return _towerspawnrate; }
            set
            {
                _towerspawnrate = value;
                base.interval = value;
            }
        }
        
        public float getSpritModifier()
        {
            return _sprintmodifier;
        }

        public void modifySprint(float modifier)
        {
            _sprintmodifier *= modifier;
        }

        public bool isTower
        {
            get { return (base.timer < base.interval); }
        }

        public player()
            : base()
        {
            //create the pickups for the player
            pweapon = new w_machinegun();
            ptower = new t_machinegun();
            //room for more different custom pickups for a later time
        }

        public void reset()
        {
            body.ResetDynamics();
            base.reset();
            this.shieldcooldowntimer = 0;
            this.shieldtimer = 0;
            this.sprinttimer = 0;
            this.timer = 0;
            isShield = false;
            isSprintcooldown = false;
            isShieldcooldown = false;
            meleetimer = 0;
            isMeleecooldown = false;
        }
       
        public void handleinput(InputState input)
        {
            PlayerIndex playerindex = (PlayerIndex)(playernumber -1);
            Vector2 movementdirection = input.CurrentGamePadStates[(int)playerindex].ThumbSticks.Left;
            Vector2 aimdirection = input.CurrentGamePadStates[(int)playerindex].ThumbSticks.Right;
            //this is good
            movementdirection.Y = -movementdirection.Y;
            this.move(movementdirection);

            //If the player points the right joystick at something, then we shoot in that direction
            if (aimdirection.X > .1f || aimdirection.Y > .1f || aimdirection.X < -.1f || aimdirection.Y < -.1f)
            {
                shoot(aimdirection);                
            }
            //If the player hits the right trigger, then we create a tower based on the tower spawn timer interval
            if (input.CurrentGamePadStates[(int)playerindex].Triggers.Right > .5f && input.CurrentGamePadStates[(int)playerindex].Triggers.Left > .5f)
            {
                aimdirection.Y = -aimdirection.Y;
                if (aimdirection != Vector2.Zero)
                    createtower(aimdirection);
                else if(movementdirection != Vector2.Zero)
                    createtower(movementdirection);                   
            }
            
            //If the player presses down on the left thumbstick, then they will do a quick sprint
            if (input.CurrentGamePadStates[(int)playerindex].IsButtonDown(Buttons.LeftStick) && input.LastGamePadStates[(int)playerindex].IsButtonUp(Buttons.LeftStick))
            {
                sprint(movementdirection);
            }

            #if MELEEENABLED
//If the player presses down on the right thumbstick, then they will melee the opposing player
            if (input.CurrentGamePadStates[(int)playerindex].IsButtonDown(Buttons.RightStick) && input.LastGamePadStates[(int)playerindex].IsButtonUp(Buttons.RightStick))
            {
                meleeattack();                
            }
#endif
        }

        public void createtower()
        {
            if(istimer)
            master.createtower(this.position, this.ptower);
        }

        public void createtower(Vector2 aimdirection)
        {
            if (istimer)
                master.createtower(this.position, this.ptower, aimdirection);
        }

        public void shoot(Vector2 aimdirection)
        {
            aimdirection.Y = -aimdirection.Y;
            Vector2 locationOfTarget = Vector2.Zero;
            locationOfTarget.X += this.position.X + aimdirection.X;
            locationOfTarget.Y += this.position.Y + aimdirection.Y;
            //master.ShootBullet(pweapon.GetBullet, this.position, aimdirection);
            pweapon.Shoot(this.position, aimdirection);
        }
#if SHIELDENABLED
        private void shield()
        {
            if (!isShieldcooldown)
                isShield = true;
        }
#endif

        private void sprint(Vector2 movementdirection)
        {
            if (!isSprintcooldown)
            {
                this.move(movementdirection * _sprintmodifier);
                isSprintcooldown = true;
                sprinttimer = 0;
            }
            //else do nothing
        }

        #if MELEEENABLED
private void meleeattack()
        {
            if (!isMeleecooldown)
            {
                aabb.Position = this.geom.Position;

                foreach (player _player in master.players.unitpool)
                {
                    if (AABB.Intersect(ref this.aabb, ref _player.geom.AABB))
                    {
                        indexes.Push(_player.id);
                    }
                }
                foreach (bullet _bullet in master.bullets.unitpool)
                {
                    if (AABB.Intersect(ref this.aabb, ref _bullet.geom.AABB))
                    {
                        indexes.Push(_bullet.id);
                    }
                }
                while (indexes.Count > 0)
                {
                    Iid id = indexes.Pop();
                    if(id.UnitID == UnitID.bullet)
                    {
                        bullet _bullet = master.bullets[id.NodeID];
                        Vector2 position = _bullet.body.Position;
                        Vector2 center = _bullet.geom.Position;
                        Vector2 direction = Vector2.Subtract(center, this.aabb.Position );
                        direction.Normalize();
                        direction *= 1500; 
                        _bullet.body.ResetDynamics();
                        _bullet.body.Position = position;                                              
                        master.bullets.unitpool[id.NodeID].body.ApplyForce(direction);                    
                    }
                    else if (id.UnitID == UnitID.player && id.NodeID != this.id.NodeID)
                        {
                            player player = master.players[id.NodeID];
                            location = Vector2.Normalize(Vector2.Subtract(player.geom.Position, this.geom.Position));
                            player.addPush(3333f);
                            player.body.ApplyForce(location * player.getPush * 3333);
                        }
                }
                isMeleecooldown = true;
                meleetimer = 0;
                meleedraw = true;
                meleedrawcount = 0;
            }
        }
#endif

        public void move(Vector2 direction)
        {
            body.ApplyForce(direction * movespeed);
        }

        public void pushplayer(Vector2 direction, int newpush)
        {
            base.addPush(newpush);
            direction.Normalize();
            body.ApplyForce(direction * base.getPush);
        }

        public override void loadcontent(master master)
        {
            base.textureName = "player";
            shieldtexture = master.content.Load<Texture2D>("shield");
            base.loadcontent(master);
            base.playernumber = base.id.NodeID + 1;
            if (geom.OnCollision == null)
            {
                this.geom.OnCollision += this.oncollide;
            }
            
            //Load the players weapon
            pweapon.loadcontent(master);
            pweapon.playernumber = this.playernumber;
            //Load the players tower
            ptower.loadcontent(master);
            ptower.playernumber = this.playernumber;
            ptower.t_weapon.modifyfirerate(2);
            //custom physics values for the player
            body.IsQuadraticDragEnabled = true;
            body.Mass = 25;
            body.LinearDragCoefficient = body.Mass * 2f;
            body.QuadraticDragCoefficient = body.Mass * .85f;
            body.RotationalDragCoefficient = 1;            
            geom.CollisionGroup = playernumber;
            base.interval = _towerspawnrate;
            Vector2 vec0 = Vector2.Zero;
            Vector2 size = new Vector2(this.texture.Width * 3, this.texture.Height * 3);
            aabb = new AABB(ref vec0, ref size);
        }

        public override void update(GameTime gameTime)
        {
            //update the weapon
            pweapon.update(gameTime, this.geom.AABB.Max);
            //updateshield(gameTime); 
            base.update(gameTime);
            updateshield(gameTime);
            //if (sprinttimer > sprinttime)
            //    isSprintcooldown = false;
            //else
            //    sprinttimer += gameTime.ElapsedGameTime.Milliseconds;
        }

        
        private void updateshield(GameTime gametime)
        {
#if SHIELDENABLED
            //If the shield is on
            if (isShield)
            {
                shieldtimer += gametime.ElapsedGameTime.TotalMilliseconds;
                if (shieldtimer > shieldtime)
                {
                    isShield = false;
                    isShieldcooldown = true;
                    shieldtimer = 0;
                }
            }
            else if (isShieldcooldown)
            {
                shieldcooldowntimer += gametime.ElapsedGameTime.TotalMilliseconds;
                if (shieldcooldowntimer > shieldcooldown)
                {
                    isShieldcooldown = false;
                    shieldcooldowntimer = 0;
                    shieldtimer = 0;
                }
            }
#endif
            if (isSprintcooldown)
            {
                sprinttimer += gametime.ElapsedGameTime.TotalMilliseconds;
                if (sprinttimer > sprinttime)
                {
                    isSprintcooldown = false;
                    sprinttimer = 0;
                }
            }

            if (isMeleecooldown)
                if (meleetimer > meleetime)
                {
                    isMeleecooldown = false;
                    meleetimer = 0;
                }
                else
                    meleetimer += gametime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void draw(SpriteBatch spriteBatch)
        {

            #if SHIELDENABLED
            //Draw the shield if it's on
            if(isShield)
            spriteBatch.Draw(shieldtexture, new Vector2(this.position.X - 10, this.position.Y - 10), Color.White);
#endif

            #if MELEEENABLED
            if (meleedraw)
            {
                spriteBatch.Draw(shieldtexture, new Rectangle(
                    (int)this.position.X - this.texture.Width,
                    (int)this.position.Y - this.texture.Height,
                    this.texture.Width * 3, 
                    this.texture.Height * 3), Color.Goldenrod);
                meleedrawcount++;
                if (meleedrawcount > 3)
                {
                    meleedraw = false;
                    meleedrawcount = 0;
                }
            }
#endif
            //Draw the base on top of it
            base.draw(spriteBatch);
        }

        Vertices verts = new Vertices();
        public bool oncollide(Geom geom1, Geom geom2, ContactList contactList)
        {
            Iid g1tag = (Iid)geom1.Tag;
            Iid g2tag = (Iid)geom2.Tag;
            //Geom 2 tag was never created, we don't know the type, so just return true
           
            //make sure this unit is the player
            if (g1tag.UnitID == UnitID.player)
            {
                //Go through the different types of unit ids
                switch (g2tag.UnitID)
                {
                    case UnitID.bullet:
                        //take damage from the bullet
                        //apply force
                        if (!isShield)
                        {
                            bullet bullet = master.bullets[g2tag.NodeID];
                            this.addPush(bullet.getPush);
                            //todo: optimize this for the inline
                            this.body.ApplyForce(bullet.body.LinearVelocity * this.getPush);
                            master.triggerparticles("explosion", bullet.geom.Position);                           
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case UnitID.tower:                    
                        return true;
                    case UnitID.player:
                        return false;
                    default:
                        return true;
                }
            }
            else
                return false;
        }
    }
}
