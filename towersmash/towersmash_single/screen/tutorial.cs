using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;

namespace towersmash.screen
{
    class tutorial: GameplayScreen
    {
        public tutorial(ScreenManager screenm)
            : base(screenm)
        {            
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }
    }
}
