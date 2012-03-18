using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerGames.FarseerPhysics.Collisions;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;

namespace towersmash
{    
    public class master
    {
        public unitm<bullet> bullets;
        public unitm<player> players;
        public unitm<tower> towers;
        public obstaclem obstacles;
        public PhysicsSimulator physics;
        public ContentManager content;
        public InputState input;
        public Game game;
        KeyboardState currentkeyboardstate, lastkeyboardstate;
        MouseState currentmousestate, lastmousestate;
        public static Texture2D texture;
        public static Texture2D texturebullet;
        public static Texture2D texturetower;
        particleeffects particles;
        

        public master(PhysicsSimulator physics, Game game)
        {
            //random = new Random();
            this.physics = physics;
            this.game = game;
            input = new InputState();
            bullets = new unitm<bullet>(300, UnitID.bullet, this);
            //pickups = new unitm<pickup>(12, UnitID.pickup, this);
            players = new unitm<player>(towersmash.maxnumplayers, UnitID.player, this);
            towers = new unitm<tower>(30, UnitID.tower, this);
            obstacles = new obstaclem(pvp.arenarect, this);
            particles = new particleeffects(this);
            
        }

        public void loadcontent(ContentManager content)
        {
            this.content = content;
            texture = content.Load<Texture2D>("default");
            texturebullet = content.Load<Texture2D>("bullet");
            bullets.loadcontent(this);
            players.loadcontent(this);
            for (int i = 0; i < towersmash.maxnumplayers; i++)
            {
                if (towersmash.players[i])
                {
                    Iid id = new Iid(i, UnitID.player);
                    switch (towersmash.characters[i])
                    {
                        case playertype.bashy:
                            this.players[i] = new p_bashy(id);
                            break;
                        case playertype.shifty:
                            this.players[i] = new p_shifty(id);
                            break;
                        case playertype.tanky:
                            this.players[i] = new p_tanky(id);
                            break;
                        default:
                            break;
                    }
                    this.players[i].loadcontent(this);                    
                    // NOTE: WARNING IMPROPER USAGE OF UNITPOOL HERE MAJOR HACK THIS SHOULD ONLY BE USED FOR THE PLAYERS NOTHING ELSE
                    players.unitpool.active[i] = true;
                    players.unitpool.CheckedOut.AddLast(i);
                    // END HACK
                    players[i].add(this.physics);
                }
            }
            towers.loadcontent(this);
            obstacles.loadcontent(this);
            if (!SoundManager.IsInitialized)
                SoundManager.Initialize(this.game);
            if (!SoundManager.IsLoaded)
            {
                SoundManager.LoadSound("bulletsound");
                //PUT MORE SOUNDS HERE
            }
            particles.loadcontent();
        }

        public void update(GameTime gameTime)
        {          
            input.Update();
            bullets.update(gameTime);
            players.update(gameTime);
            towers.update(gameTime);
            obstacles.update(gameTime);
            lastkeyboardstate = currentkeyboardstate;
            lastmousestate = currentmousestate;
            currentkeyboardstate = Keyboard.GetState();
            currentmousestate = Mouse.GetState();
            Vector2 direction = Vector2.Zero;
            if (currentkeyboardstate.IsKeyDown(Keys.Up))
            {
                direction.Y -= 1;
            }
            if (currentkeyboardstate.IsKeyDown(Keys.Down))
            {
                direction.Y += 1;
            }
            if (currentkeyboardstate.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }
            if (currentkeyboardstate.IsKeyDown(Keys.Left))
            {
                direction.X -= 1;
            }
            players[0].move(direction);
            if (currentkeyboardstate.IsKeyDown(Keys.NumPad0))
            {
                if (!players[0].isSprintcooldown)
                {
                    players[0].isSprintcooldown = true;
                    players[0].move(Vector2.One * 30);
                }
            }
            if (currentkeyboardstate.IsKeyDown(Keys.Z))
            {
                foreach (player _player in players.unitpool)
                {
                    ///_player.shoot(Vector2.Zero);
                    _player.createtower();
                }
            }
            if (currentmousestate.LeftButton == ButtonState.Pressed)
            {
                //players[0].shoot(new Vector2(currentmousestate.X, currentmousestate.Y));
                this.triggerparticles("effect", players[0].geom.AABB.Position);
            }
            particles.update(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {           
            players.draw(spriteBatch);
            towers.draw(spriteBatch);
            bullets.draw(spriteBatch);
            obstacles.draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Immediate, SaveStateMode.None, camera.transform);
            particles.draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None, camera.transform);
        }

        //private double pickuptimer = 0;
        //private int interval = 2000;
        //Random random;
        //Vector2 position = new Vector2();
        //public void createpickups(GameTime gameTime, Rectangle arenarect)
        //{
        //    if (pickups.unitpool.ActiveCount < 8)
        //    {
        //        //increment counter
        //        pickuptimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        //        //if we've gone past the interval
        //        if (pickuptimer > interval)
        //        {
        //            //reset the time
        //            pickuptimer = 0;
        //            interval = random.Next(1000, 3000);
        //            int i = random.Next(0, pickupdefinitions.unitpool.ActiveCount);
        //            //get a pickup to instantiate from the pool
        //            pickup pickupdefinition = pickupdefinitions[i];
        //            pickup newpickup = pickups.unitpool.Retrieve().Item;
        //            //clone the pick-up with a pre-existing definition
        //            newpickup.clone(pickupdefinition);
        //            //add to the physics simulator
        //            newpickup.add(physics);
        //            //create a random location which is on the field
        //            position.X = random.Next(arenarect.X + 100, arenarect.X + arenarect.Width - 100);
        //            position.Y = random.Next(arenarect.Y + 100, arenarect.Y + arenarect.Height - 100);
        //            //todo: maybe make sure a pick-up isn't too close to any player?
        //            newpickup.body.Position = position;
        //        }
        //    }
        //}

        public void remove(Iid id)
        {
            switch (id.UnitID)
            {
                case (UnitID.bullet):
                    bullets.remove(id);
                    break;
                case (UnitID.tower):
                    towers.remove(id);
                    break;
                case(UnitID.obstacle):
                    obstacles.remove(id);
                    break;
                case (UnitID.player):
                    players.remove(id);
                    towersmash.players[id.NodeID] = false;
                    towersmash.numberofplayers--;
                    //todo: impliment something cool
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void handleinput(InputState input)
        {
            foreach (player p in players.unitpool)
            {
                p.handleinput(input);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="newbullet">
        /// <remarks>Make sure that the newbullet has the correct playernumber</remarks></param>
        public void shootbullet(Vector2 origin, Vector2 destination, bullet newbullet)
        {
            bullet rbullet = bullets.unitpool.Retrieve().Item;
            rbullet.clone(newbullet);
            rbullet.body.Position = origin;
            rbullet.startinglocation = origin;
            rbullet.add(physics);
            Vector2 direction = Vector2.Normalize(Vector2.Subtract(destination, origin));
            rbullet.body.ApplyForce( direction * rbullet.getShootingSpeed());
            rbullet.rotationV2 = direction;
            triggerparticles("effect", origin);
            //SoundManager.PlaySound(rbullet.shootingsound);
        }

        public void ShootBullet(bullet bulletToClone, Vector2 startingLocation, Vector2 aimDirection)
        {
            bullet newBullet = bullets.unitpool.Retrieve().Item;
            newBullet.clone(bulletToClone);
            newBullet.position = startingLocation;
            newBullet.startinglocation = startingLocation;
            newBullet.add(physics);
            newBullet.body.ApplyForce(aimDirection * bulletToClone.getShootingSpeed());
            newBullet.rotationV2 = aimDirection;
            triggerparticles("effect", startingLocation);
        }
        
        /// <summary>
        /// Calculate the player space from world to screen coordinates
        /// </summary>
        /// <returns>A rectangle containing the values of a union of all the players</returns>
        /// 
        
            Rectangle rect1 = new Rectangle();
              Rectangle rect2 = new Rectangle();
        public Rectangle playerspace()
        {
            //Rectangles used for iteration
            //Number of active players
            int count = players.unitpool.ActiveCount;
            //Get the first player
            player player = players.unitpool.Get(players.unitpool.CheckedOut.First.Value).Item;
            //Set the first rectangle equal to that player
            rect1.X = (int)(player.body.Position.X);
            rect1.Y = (int)(player.body.Position.Y);
            rect1.Width = (int)(player.geom.AABB.Width);
            rect1.Height = (int)(player.geom.AABB.Height);
            
            foreach(int i in players.unitpool.CheckedOut)
            {
                //Get the second player
                player = players.unitpool[i];
                //Set the second rectangle equal to that player
                rect2.X = (int)(player.body.Position.X);
                rect2.Y = (int)(player.body.Position.Y);
                rect2.Width = (int)(player.geom.AABB.Width);
                rect2.Height = (int)(player.geom.AABB.Height);
                //Do a union of the two players rectangles
                //Set the rectangle 1 equal to the result for the next iteration
                rect1 = math.union(rect1, rect2);                
            }
            return rect1;
        }
        
        public void createtower(Vector2 destination, tower ptower)
        {
            tower _tower;
            //get a fresh tower
            _tower = towers.unitpool.Retrieve().Item;
            _tower.clone(ptower);
            _tower.position = destination;
            _tower.add(physics);            
        }

        public void createtower(Vector2 destination, tower ptower, Vector2 direction)
        {
            tower _tower;
            _tower = towers.unitpool.Retrieve().Item;
            _tower.clone(ptower);
            _tower.position = destination;
            _tower.target = destination + direction;
        }

        public void triggerparticles(string particlename, Vector2 position)
        {
            particles.trigger(particlename, position);           
        }
    }
}
