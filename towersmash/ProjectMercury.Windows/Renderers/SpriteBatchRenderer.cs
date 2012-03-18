/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.Renderers
{
    using System;
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Emitters;

    /// <summary>
    /// Defines a Renderer which uses the standard XNA SpriteBatch class to render Particles.
    /// </summary>
    public sealed class SpriteBatchRenderer : Renderer
    {
        private SpriteBatch Batch;

        /// <summary>
        /// Disposes any unmanaged resources being used by the Renderer.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (this.Batch != null)
                    this.Batch.Dispose();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads any content required by the renderer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the GraphicsDeviceManager has not been set.</exception>
        public override void LoadContent(ContentManager content)
        {
            Guard.IsTrue(base.GraphicsDeviceService == null, "GraphicsDeviceService property has not been initialised with a valid value.");

            this.Batch = new SpriteBatch(base.GraphicsDeviceService.GraphicsDevice);
        }

        /// <summary>
        /// Renders the specified Emitter, applying the specified transformation offset.
        /// </summary>
        public override void RenderEmitter(Emitter emitter, ref Matrix transform)
        {
            Guard.ArgumentNull("emitter", emitter);
            Guard.IsTrue(this.Batch == null, "SpriteBatchRenderer is not ready! Did you forget to LoadContent?");

            if (emitter.BlendMode == BlendMode.None)
                return;

            if (emitter.ParticleTexture != null && emitter.ActiveParticlesCount > 0)
            {
                // Calculate the source rectangle and origin offset of the Particle texture...
                Rectangle source = new Rectangle(0, 0, emitter.ParticleTexture.Width, emitter.ParticleTexture.Height);
                Vector2 origin = new Vector2(source.Width / 2f, source.Height / 2f);

                this.Batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, transform);

                this.SetRenderState(emitter);

                for (int i = 0; i < emitter.ActiveParticlesCount; i++)
                {
                    Particle particle = emitter.Particles[i];

                    float scale = particle.Scale / emitter.ParticleTexture.Width;

                    this.Batch.Draw(emitter.ParticleTexture, particle.Position, source, new Color(particle.Colour), particle.Rotation, origin, scale, SpriteEffects.None, 0f);
                }

                this.Batch.End();
            }
        }

        /// <summary>
        /// Renders the specified ParticleEffect.
        /// </summary>
        public void RenderEffect(ParticleEffect effect, SpriteBatch spriteBatch)
        {
            Guard.ArgumentNull("effect", effect);
            Guard.ArgumentNull("spriteBatch", spriteBatch);

            for (int i = 0; i < effect.Count; i++)
                this.RenderEmitter(effect[i], spriteBatch);
        }

        /// <summary>
        /// Renders the specified Emitter.
        /// </summary>
        public void RenderEmitter(Emitter emitter, SpriteBatch spriteBatch)
        {
            Guard.ArgumentNull("emitter", emitter);
            Guard.ArgumentNull("spriteBatch", spriteBatch);
            float width = emitter.ParticleTexture.Width;
            float height = emitter.ParticleTexture.Height;
            if (emitter.BlendMode == BlendMode.None)
                return;

            if (emitter.ParticleTexture != null && emitter.ActiveParticlesCount > 0)
            {
                for (int i = 0; i < emitter.ActiveParticlesCount; i++)
                {
                    Particle particle = emitter.Particles[i];

                    float scale = particle.Scale / emitter.ParticleTexture.Width;

                    spriteBatch.Draw(emitter.ParticleTexture, particle.Position, new Rectangle(0, 0, (int)width, (int)height), new Color(particle.Colour), particle.Rotation, new Vector2(width / 2f, height / 2f), scale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}