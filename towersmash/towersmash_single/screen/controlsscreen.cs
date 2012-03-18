using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace towersmash
{
    class controlsscreen: MenuScreen
    {
        public controlsscreen()
            : base("Controls")
        {
            IsPopup = false;
            MenuEntry resume = new MenuEntry("Return to Main Menu");
            resume.Selected += OnCancel;
            MenuEntries.Add(resume);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(towersmash.controls, new Vector2(450, 150), Color.White);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
