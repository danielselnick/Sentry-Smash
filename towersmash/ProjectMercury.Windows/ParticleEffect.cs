/*  
 Copyright © 2009 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using ProjectMercury.Emitters;
    
#if WINDOWS
    [TypeConverter("ProjectMercury.Design.ParticleEffectTypeConverter, Projectmercury.Design")]
#endif
    public class ParticleEffect : EmitterCollection
    {
        private string _name;

        /// <summary>
        /// Gets or sets the name of the ParticleEffect.
        /// </summary>
        /// <value>The name.</value>
        [ContentSerializer(Optional = true)]
        public string Name
        {
            get { return this._name; }
            set
            {
                Guard.ArgumentNullOrEmpty("Name", value);

                if (this.Name != value)
                {
                    this._name = value;

                    this.OnNameChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Occurs when name of the ParticleEffect has been changed.
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Raises the <see cref="E:NameChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnNameChanged(EventArgs e)
        {
            if (this.NameChanged != null)
                this.NameChanged(this, e);
        }

        /// <summary>
        /// Gets or sets the author of the ParticleEffect.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public string Author;

        /// <summary>
        /// Gets or sets the description of the ParticleEffect.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public string Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        public ParticleEffect()
        {
            this.Name = "Particle Effect";
        }

        /// <summary>
        /// Returns a deep copy of the ParticleEffect.
        /// </summary>
        public virtual ParticleEffect DeepCopy()
        {
            ParticleEffect effect = new ParticleEffect
            {
                Author = this.Author,
                Description = this.Description,
                Name = this.Name
            };

            foreach (Emitter emitter in this)
                effect.Add(emitter.DeepCopy());

            return effect;
        }

        /// <summary>
        /// Triggers the ParticleEffect at the specified position.
        /// </summary>
        public virtual void Trigger(Vector2 position)
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Trigger(ref position);
        }

        /// <summary>
        /// Triggers the ParticleEffect at the specified position.
        /// </summary>
        public virtual void Trigger(ref Vector2 position)
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Trigger(ref position);
        }

        /// <summary>
        /// Initialises all Emitters within the ParticleEffect.
        /// </summary>
        public virtual void Initialise()
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Initialise();
        }

        /// <summary>
        /// Terminates all Emitters within the ParticleEffect with immediate effect.
        /// </summary>
        public virtual void Terminate()
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Terminate();
        }

        /// <summary>
        /// Loads content required by Emitters within the ParticleEffect.
        /// </summary>
        public virtual void LoadContent(ContentManager content)
        {
            for (int i = 0; i < this.Count; i++)
                this[i].LoadContent(content);
        }

        /// <summary>
        /// Updates all Emitters within the ParticleEffect.
        /// </summary>
        /// <param name="totalSeconds">Total game time in whole and fractional seconds.</param>
        /// <param name="deltaSeconds">Elapsed frame time in whole and fractional seconds.</param>
        public virtual void Update(float deltaSeconds)
        {
            for (int i = 0; i < this.Count; i++)
                this[i].Update(deltaSeconds);
        }

        /// <summary>
        /// Starts updating the Particle effect asynchronously.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed frame time in whole and fractional seconds.</param>
        /// <param name="callback">An AyncCallback method that will be invoked when the update completes.</param>
        public IAsyncResult BeginUpdate(float deltaSeconds, AsyncCallback callback)
        {
            UpdateAsyncResult asyncResult = new UpdateAsyncResult(callback, null);

            ThreadPool.QueueUserWorkItem((r) =>
            {
                UpdateAsyncResult result = (UpdateAsyncResult)r;

                this.Update(deltaSeconds);

                result.OnCompleted();

            }, asyncResult);

            return asyncResult;
        }

        /// <summary>
        /// Blocks until the update operation is finished.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        public void EndUpdate(IAsyncResult asyncResult)
        {
            using (UpdateAsyncResult result = asyncResult as UpdateAsyncResult)
            {
                result.AsyncWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Updates all Emitters within the ParticleEffect.
        /// </summary>
        /// <param name="deltaSeconds">Elapsed frame time in whole and fractional seconds.</param>
        [Obsolete("Use Update(deltaSeconds) instead.", false)]
        public virtual void Update(float totalSeconds, float deltaSeconds)
        {
            this.Update(deltaSeconds);
        }

        /// <summary>
        /// Gets the total number of active Particles in the ParticleEffect.
        /// </summary>
        public int ActiveParticlesCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < base.Count; i++)
                    count += base[i].ActiveParticlesCount;

                return count;
            }
        }

        private class UpdateAsyncResult : IAsyncResult, IDisposable
        {
            /// <summary>
            /// Gets or sets the async callback.
            /// </summary>
            /// <value>The async callback.</value>
            public AsyncCallback Callback { get; private set; }

            /// <summary>
            /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
            /// </summary>
            /// <returns>
            /// A user-defined object that qualifies or contains information about an asynchronous operation.
            /// </returns>
            public object AsyncState { get; private set; }

            /// <summary>
            /// Gets or sets the manual reset event.
            /// </summary>
            /// <value>The manual reset event.</value>
            public ManualResetEvent ManualResetEvent { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is disposed.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is disposed; otherwise, <c>false</c>.
            /// </value>
            public bool IsDisposed { get; private set; }

            /// <summary>
            /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </returns>
            public WaitHandle AsyncWaitHandle
            {
                get { return this.ManualResetEvent; }
            }

            /// <summary>
            /// Occurs when this instance is disposed.
            /// </summary>
            public event EventHandler Disposed;

            /// <summary>
            /// Raises the <see cref="E:Disposed"/> event.
            /// </summary>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            protected virtual void OnDisposed(EventArgs e)
            {
                if (this.Disposed != null)
                    this.Disposed(this, e);
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (!this.IsDisposed)
                {
                    this.Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }

            /// <summary>
            /// Releases unmanaged and - optionally - managed resources
            /// </summary>
            /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected virtual void Dispose(bool disposing)
            {
                try
                {
                    if (disposing)
                    {
                        this.ManualResetEvent.Close();
                        this.ManualResetEvent = null;

                        this.AsyncState = null;

                        this.Callback = null;

                        this.OnDisposed(EventArgs.Empty);
                    }
                }
                finally
                {
                    this.IsDisposed = true;
                }
            }

            /// <summary>
            /// Releases unmanaged resources and performs other cleanup operations before the
            /// <see cref="UpdateAsyncResult"/> is reclaimed by garbage collection.
            /// </summary>
            ~UpdateAsyncResult()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="UpdateAsyncResult"/> class.
            /// </summary>
            /// <param name="callback">The callback.</param>
            /// <param name="state">The state.</param>
            public UpdateAsyncResult(AsyncCallback callback, object state)
            {
                this.Callback = callback;
                this.AsyncState = state;
                this.ManualResetEvent = new ManualResetEvent(false);
            }

            /// <summary>
            /// Called when the asynchronous operation has completed.
            /// </summary>
            public virtual void OnCompleted()
            {
                this.ManualResetEvent.Set();

                if (this.Callback != null)
                    this.Callback(this);
            }

            /// <summary>
            /// Gets a value that indicates whether the asynchronous operation completed synchronously.
            /// </summary>
            /// <value></value>
            /// <returns>true if the asynchronous operation completed synchronously; otherwise, false.
            /// </returns>
            public bool CompletedSynchronously
            {
                get { return false; }
            }

            /// <summary>
            /// Gets a value that indicates whether the asynchronous operation has completed.
            /// </summary>
            /// <value></value>
            /// <returns>true if the operation is complete; otherwise, false.
            /// </returns>
            public bool IsCompleted
            {
                get { return this.ManualResetEvent.WaitOne(0, false); }
            }
        }
    }
}