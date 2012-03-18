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
    using Emitters;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines a Renderer which uses hardware point sprites to render Particles.
    /// </summary>
    public sealed class PointSpriteRenderer : Renderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointSpriteRenderer"/> class.
        /// </summary>
        public PointSpriteRenderer() : base()
        {
            this.ShaderContentPath = "Content\\PointSprite";
        }

        private VertexDeclaration VertexDeclaration;
        private Effect PointSpriteEffect;
        private EffectPass EffectPass;
        private EffectParameter TextureParameter;
        private EffectParameter WorldParam;
        private EffectParameter ViewParam;
        private EffectParameter ProjectionParam;
        private EffectParameter TransformParam;
        private Matrix World;
        private Matrix View;
        private Matrix Projection;

        /// <summary>
        /// The path to the PointSprite.fx shader in your content project.
        /// </summary>
        public string ShaderContentPath;

        /// <summary>
        /// Disposes any unmanaged resources being used by this instance.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.VertexDeclaration != null)
                    this.VertexDeclaration.Dispose();

                if (this.PointSpriteEffect != null)
                    this.PointSpriteEffect.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Loads any content that is needed by the renderer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown if the GraphicsDeviceService has not been set.</exception>
        /// <exception cref="Microsoft.Xna.Framework.Content.ContentLoadException">Thrown if the content item
        /// defined in the ShaderContentPath property could not be loaded.</exception>
        public override void LoadContent(ContentManager content)
        {
            Guard.ArgumentNull("content", content);
            Guard.IsTrue(base.GraphicsDeviceService == null, "GraphicsDeviceService property has not been initialised with a valid value.");

            this.VertexDeclaration = new VertexDeclaration(base.GraphicsDeviceService.GraphicsDevice, Particle.VertexElements);

            try
            {
                this.PointSpriteEffect = content.Load<Effect>(this.ShaderContentPath);
            }
            catch (ContentLoadException e)
            {
                throw new ContentLoadException("Could not locate PointSprite.fx shader, please check the ShaderContentPath property against your content project.", e);
            }

            this.EffectPass = this.PointSpriteEffect.Techniques[0].Passes[0];
            this.TextureParameter = this.PointSpriteEffect.Parameters["SpriteTexture"];
            this.WorldParam = this.PointSpriteEffect.Parameters["World"];
            this.ViewParam = this.PointSpriteEffect.Parameters["View"];
            this.ProjectionParam = this.PointSpriteEffect.Parameters["Projection"];
            this.TransformParam = this.PointSpriteEffect.Parameters["WorldTransform"];

            if (this.WorldParam == null || this.ViewParam == null || this.ProjectionParam == null || this.TransformParam == null)
                throw new Exception("Unable to find shader parameters, your .fx file may be out of date.");

            // Create a WVP offset for the shader...
            this.World = Matrix.Identity;

            this.View = new Matrix(
                    1.0f, 0.0f, 0.0f, 0.0f,
                    0.0f, -1.0f, 0.0f, 0.0f,
                    0.0f, 0.0f, -1.0f, 0.0f,
                    0.0f, 0.0f, 0.0f, 1.0f);

            this.Projection = Matrix.CreateOrthographicOffCenter(0, base.GraphicsDeviceService.GraphicsDevice.Viewport.Width, -this.GraphicsDeviceService.GraphicsDevice.Viewport.Height, 0, 0, 1);
        }

        /// <summary>
        /// Renders the specified Emitter, applying the specified transformation offset.
        /// </summary>
        public override void RenderEmitter(Emitter emitter, ref Matrix transform)
        {
            Guard.ArgumentNull("emitter", emitter);
            Guard.IsTrue(base.GraphicsDeviceService == null, "GraphicsDeviceService property has not been initialised with a valid value.");

            if (emitter.BlendMode == BlendMode.None)
                return;

            if (emitter.ParticleTexture != null && emitter.ActiveParticlesCount > 0)
            {
                this.WorldParam.SetValue(this.World);
                this.ViewParam.SetValue(this.View);
                this.ProjectionParam.SetValue(this.Projection);
                this.TransformParam.SetValue(transform);

                // Use the Emitter Particle texture...
                this.TextureParameter.SetValue(emitter.ParticleTexture);

                // Set graphics device properties for rendering...
                GraphicsDevice device = base.GraphicsDeviceService.GraphicsDevice;
                {
                    device.VertexDeclaration = this.VertexDeclaration;
                    device.RenderState.PointSpriteEnable = true;
                    device.RenderState.AlphaBlendEnable = true;
                    device.RenderState.DepthBufferWriteEnable = false;
                }

                // Set render state for the emitter...
                this.SetRenderState(emitter);

                this.PointSpriteEffect.Begin(SaveStateMode.None);
                this.EffectPass.Begin();
#if XBOX
                // On the XBox we have to split the particle array into more manageable chunks.
                if (emitter.ActiveParticlesCount > 9000)
                {
                    for (int i = 0; i < emitter.ActiveParticlesCount; i += 9000)
                    {
                        int remaining = (emitter.ActiveParticlesCount - i < 9000 ? emitter.ActiveParticlesCount - i : 9000);

                        device.DrawUserPrimitives<Particle>(PrimitiveType.PointList, emitter.Particles, i, remaining);
                    }
                }
                else
                {
                    device.DrawUserPrimitives<Particle>(PrimitiveType.PointList, emitter.Particles, 0, emitter.ActiveParticlesCount);
                }
#elif WINDOWS
                device.DrawUserPrimitives<Particle>(PrimitiveType.PointList, emitter.Particles, 0, emitter.ActiveParticlesCount);
#endif
                this.EffectPass.End();
                this.PointSpriteEffect.End();
            }
        }
    }
}