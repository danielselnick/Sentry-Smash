/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System.Runtime.InteropServices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        static Particle()
        {
            // Position vertex element...
            VertexElement positionElement       = new VertexElement();
            positionElement.VertexElementFormat = VertexElementFormat.Vector2;
            positionElement.VertexElementUsage  = VertexElementUsage.Position;

            // Scale vertex element...
            VertexElement scaleElement          = new VertexElement();
            scaleElement.Offset                 = 8;
            scaleElement.VertexElementFormat    = VertexElementFormat.Single;
            scaleElement.VertexElementUsage     = VertexElementUsage.PointSize;

            // Rotation vertex element...
            VertexElement rotationElement       = new VertexElement();
            rotationElement.Offset              = 12;
            rotationElement.VertexElementFormat = VertexElementFormat.Single;
            rotationElement.VertexElementUsage  = VertexElementUsage.TextureCoordinate;

            // Color vertex element...
            VertexElement colourElement         = new VertexElement();
            colourElement.Offset                = 16;
            colourElement.VertexElementFormat   = VertexElementFormat.Vector4;
            colourElement.VertexElementUsage    = VertexElementUsage.Color;

            // Vertex element array...
            Particle.VertexElements = new VertexElement[]
            {
                positionElement,
                scaleElement,
                rotationElement,
                colourElement
            };
        }

        /// <summary>
        /// Contains the vertex element data for a Particle.
        /// </summary>
        static public readonly VertexElement[] VertexElements;

        /// <summary>
        /// Gets the size of a Particle structure in bytes.
        /// </summary>
        static public int SizeInBytes { get { return 56; } }

        // Members used by the shader to draw the particles...
        public Vector2 Position;
        public float Scale;
        public float Rotation;
        public Vector4 Colour;

        // Members needed only for the simulation...
        public Vector2 Momentum;
        public Vector2 Velocity;
        public float Inception;
        public float Age;

        /// <summary>
        /// Applies a force to the particle.
        /// </summary>
        /// <param name="force">A vector describing the force and direction.</param>
        public void ApplyForce(ref Vector2 force)
        {
            this.Velocity.X += force.X;
            this.Velocity.Y += force.Y;
        }

        /// <summary>
        /// Updates the particle.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed seconds since the last update.</param>
        internal void Update(float elapsedSeconds)
        {
            // Add velocity to momentum...
            this.Momentum.X += this.Velocity.X;
            this.Momentum.Y += this.Velocity.Y;

            // Set velocity back to zero...
            this.Velocity.X = this.Velocity.Y = 0f;

            // Calculate momentum for this time-step...
            Vector2 deltaMomentum;

            deltaMomentum.X = this.Momentum.X * elapsedSeconds;
            deltaMomentum.Y = this.Momentum.Y * elapsedSeconds;

            // Add momentum to the particles Position...
            this.Position.X += deltaMomentum.X;
            this.Position.Y += deltaMomentum.Y;
        }
    }
}