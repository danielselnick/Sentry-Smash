using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;

namespace towersmash
{
    class playerjoin: MenuScreen
    {
        MenuEntry[] players;
        public playerjoin()
            : base("Player Select Screen")
        {
            MenuEntry back = new MenuEntry("Return to main menu");
            MenuEntry start = new MenuEntry("Start the game");
            back.Selected += OnCancel;
            start.Selected += new EventHandler<PlayerIndexEventArgs>(start_Selected);
            MenuEntries.Add(start);
            players = new MenuEntry[towersmash.maxnumplayers];
            for(int i = 0; i < towersmash.maxnumplayers; i++)
            {
                players[i] = new MenuEntry(string.Empty);
                players[i].Selected += new EventHandler<PlayerIndexEventArgs>(player_Selected);
                MenuEntries.Add(players[i]);
            }
            SetText();
            MenuEntries.Add(back);
        }

        void SetText()
        {
            int temp;
            for (int i = 0; i < towersmash.maxnumplayers; i++)
            {
                temp = i;
                temp++;
                if (towersmash.players[i])
                {
                    players[i].Text = "Player " + temp + " has joined the battle!";
                }
                else
                {
                    players[i].Text = "Player " + temp + " needs to join the battle!";
                }
            }
        }

        void start_Selected(object sender, PlayerIndexEventArgs e)
        {
            towersmash.updatenumberofplayers();
            //if (counter > 1)
            {
                base.OnCancel(sender, e);
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new pvp(ScreenManager));
            }
            //else
            //{
            //    MessageBoxScreen messagebox = new MessageBoxScreen("Must have at least 2 players ready for battle!");
            //    ScreenManager.AddScreen(messagebox, null);
            //}
        }
        int i = 0;
        void player_Selected(object sender, PlayerIndexEventArgs e)
        {
            //this is the correct way
            //towersmash.players[(int)e.PlayerIndex] = !towersmash.players[(int)e.PlayerIndex];
            //need to be able to test out code solo, this is broken code to be used only for testing
            towersmash.players[i] = true;
            i++;
            SetText();
        }
    }
}
