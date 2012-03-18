using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace towersmash
{
    class playerselect: MenuScreen
    {
        public playerselect()
            : base("Player Selection Screen")
        {
            player_selection = new int[4];
        }

        float border_size = .075f;
        float text_size = .05f;
        float character_size = .3f;
        float player_size = .1f;
        int[] player_selection;
        int character_number = towersmash.characters.Length;
        public override void HandleInput(InputState input)
        {
            //base.HandleInput(input);
            PlayerIndex junk;
            for (int i = 0; i < 4; i++)
            {
                //If the player hit A to join
                if (input.IsNewButtonPress(Buttons.A, (PlayerIndex)i, out junk) && !towersmash.players[i])
                {
                    towersmash.players[i] = true;
                }
                    //else if the player hit B to leave
                else if (input.IsNewButtonPress(Buttons.B, (PlayerIndex)i, out junk) && towersmash.players[i])
                {
                    towersmash.players[i] = false;
                }
                //If the player moves up the list
                if (towersmash.players[i] && input.IsNewButtonPress(Buttons.RightShoulder, (PlayerIndex)i, out junk))
                {
                    player_selection[i]++;
                    if (player_selection[i] > character_number)
                        player_selection[i] = 0;
                }
                //If the player moves down the list
                if (towersmash.players[i] && input.IsNewButtonPress(Buttons.LeftShoulder, (PlayerIndex)i, out junk))
                {
                    player_selection[i]--;
                    if (player_selection[i] < 0)
                        player_selection[i] = character_number;
                }
            }
            //If anyone wants to start the game
            if(input.IsNewButtonPress(Buttons.Start, null, out junk))
            {
                towersmash.updatenumberofplayers();
                if (towersmash.numberofplayers > 1)
                {
                    base.OnCancel(junk);
                    LoadingScreen.Load(ScreenManager, true, junk, new pvp(ScreenManager));
                }
                else
                {
                    MessageBoxScreen messagebox = new MessageBoxScreen("Must have at least 2 players ready for battle!");
                    ScreenManager.AddScreen(messagebox, null);
                }
            }
            //If anyone wants to go back to the main menu
            if (input.IsNewButtonPress(Buttons.Back, null, out junk))
            {
                base.OnCancel(junk);
            }

            //Quick hack for me on the keyboard
            if (input.IsNewKeyPress(Keys.Enter, null, out junk))
            {
                towersmash.numberofplayers = 2;
                towersmash.players[0] = true;
                towersmash.players[1] = true;
                towersmash.players[2] = false;
                towersmash.players[3] = false;
                towersmash.characters[0] = playertype.bashy;
                towersmash.characters[1] = playertype.shifty;
                base.OnCancel(junk);
                LoadingScreen.Load(ScreenManager, true, junk, new pvp(ScreenManager));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            //base.Draw(gameTime);
            spriteBatch.Begin();
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex playerindex = (PlayerIndex)i;
                Vector2 position = Vector2.Zero;
                //Determine the starting position to draw the gui
                switch (playerindex)
                {
                    case PlayerIndex.One:
                        position.X = towersmash.screen_width * .1f;
                        position.Y = towersmash.screen_height * .1f;
                        break;
                    case PlayerIndex.Two:
                        position.X = towersmash.screen_width - towersmash.screen_width * .40f;
                        position.Y = towersmash.screen_height * .1f;
                        break;
                    case PlayerIndex.Three:
                        position.X = towersmash.screen_width * .1f;
                        position.Y = towersmash.screen_height - towersmash.screen_height * .40f;
                        break;
                    case PlayerIndex.Four:
                        position.X = towersmash.screen_width - towersmash.screen_width * .40f;
                        position.Y = towersmash.screen_height - towersmash.screen_height * .40f;
                        break;
                    default:
                        position = Vector2.One;
                        break;
                }
                spriteBatch.DrawString(ScreenManager.Font, "Player: " + playerindex, position, Color.Yellow);
                position.Y += 50;
                //If the player has joined the game
                if (towersmash.players[i])
                {
                    StringBuilder text = new StringBuilder();
                    text.AppendLine("Press B to leave the game");
                    text.AppendLine("Your character is: ");
                    text.AppendLine(towersmash.characters[player_selection[i]].ToString());
                    spriteBatch.DrawString(ScreenManager.Font, text, position, Color.White);
                    //todo: at a later time when I have icons, put an icon here!
                }
                //else if the player hasn't joined the game
                else
                {
                    spriteBatch.DrawString(ScreenManager.Font, "Press A to join the game!", new Vector2(position.X, position.Y), Color.White);
                }
            }
            //draw back and start
            spriteBatch.DrawString(ScreenManager.Font, "Use the right and left shoulder buttons to change your character! \n Press Start to start the game! \n Press Back to go back to the main menu.", new Vector2(towersmash.screen_width * .1f, towersmash.screen_height - (towersmash.screen_height * .2f)), Color.Red);
            spriteBatch.End();
        }
    }
}