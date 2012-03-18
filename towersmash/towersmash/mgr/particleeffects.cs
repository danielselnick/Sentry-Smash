using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMercury.Emitters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProjectMercury.Renderers;
using ProjectMercury;

namespace towersmash
{
    public class particleeffects
    {
        Dictionary<string, ParticleEffect> _particles;
        master master;
        ParticleEffect _particleEffect;
        SpriteBatchRenderer renderer;
        List<string> particleeffectslist = new List<string> {
            "effect", "explosion" };
        public particleeffects(master _master)
        {
            this.master = _master;
            _particles = new Dictionary<string, ParticleEffect>();
            renderer = new SpriteBatchRenderer();
        }

        void initialize()
        {
            _particleEffect.Initialise();
        }

        public void loadcontent()
        {
            ParticleEffect newParticleEffect;
            string directory = "particleeffects\\";
            string location;
            foreach (string effect in particleeffectslist)
            {
                location = directory + effect;
                newParticleEffect = master.content.Load<ParticleEffect>(location);
                newParticleEffect.Initialise();
                newParticleEffect.LoadContent(master.content);
                _particles.Add(effect, newParticleEffect);                
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleEffect p in _particles.Values)
            {
                renderer.RenderEffect(p, spriteBatch);
            }
        }

        public void update(GameTime gametime)
        {
            foreach (ParticleEffect p in _particles.Values)
            {
                p.Update((float)gametime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void trigger(string particleeffectname, Vector2 position)
        {
            ParticleEffect newtrigger;
            if (_particles.TryGetValue(particleeffectname, out newtrigger))
                newtrigger.Trigger(position);
            else
                throw new Exception();
        }
    }
}
