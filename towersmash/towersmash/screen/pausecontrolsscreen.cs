using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace towersmash
{
    /// <summary>
    /// Screen that shows the player controls
    /// </summary>
    class pausecontrolsscreen : MenuScreen
    {
        public pausecontrolsscreen()
            : base("Controls Screen")
        {
            IsPopup = true;
            //Create menu entry
            MenuEntry resume = new MenuEntry("Return to game");

            // Hook up event handler
            resume.Selected += OnCancel;
            //resume.Selected += new EventHandler<PlayerIndexEventArgs>(resume_Selected);

            //Add the resume entry to the screen
            MenuEntries.Add(resume);
        }

        public void pausemenu(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw the controls onto the screen
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(towersmash.controls, new Vector2(450, 150), Color.White);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
