using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace towersmash
{
    public class simplegui
    {
        public simplegui(float width, float height, master master)
        {
            screen_width = width;
            screen_height = height;
            this._master = master;
        }

        master _master;
        SpriteFont font;
        Texture2D texture;
        
        float screen_width;
        float screen_height;
        float border;
        static float border_size = 0.05f;
        float separator;
        static float separator_size = 0.05f;
        float text;
        static float text_size = 0.0875f;
        double bar;
        static float bar_size = 0.1f;
        float gui_height;
        static float gui_height_size = 0.1f;
        static float gui_height_border = 0.1f;
        public static float gui_size = gui_height_size + gui_height_border;
        float gui_height_start;
        float number_of_elements = 6;
        int element_height;
        string[] elements = { "sprint", "shield", "melee", "sentry", "shoot", "lives" };
        bool[] played;
        public void loadcontent()
        {
            //Calculate positions of everything
            border = screen_width * border_size;
            separator = screen_width * separator_size;
            text = screen_width * text_size;
            bar = screen_width * bar_size;
            gui_height = screen_height * gui_height_size;
            element_height = (int)(gui_height / number_of_elements);
            gui_height_start = screen_height - gui_height - (screen_height * gui_height_border);
            font = _master.content.Load<SpriteFont>("guifont");
            texture = _master.content.Load<Texture2D>("guibar");
            played = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                played[i] = towersmash.players[i];
            }
        }

        float element_offset = 3.33f;
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.Additive);
            //Draw the bars
            Vector2 position = Vector2.Zero;            
            Color colorfont = Color.White;
            Color c_olor = Color.Red;
            position.X = border;
            position.Y = gui_height_start;
            spriteBatch.DrawString(font, _master.bullets.unitpool.CheckedOut.Count.ToString(), new Vector2( position.X, position.Y - 150), Color.White);
            //Loop through all four player boxes
            for (int z = 0; z < 4; z++)
            {
                if (towersmash.players[z])
                {
                    double width;
                    //Get the player
                    player _player = _master.players[z];
                    z++;
                    spriteBatch.DrawString(font, "Player #: " + z, position, Color.Yellow);
                    z--;
                    position.Y += element_height + element_offset;
                    //Draw lives
                    position.Y += element_height + element_offset;
                    spriteBatch.DrawString(font, "Player lives: " + _player.lives, position, Color.Gold);
                }
                else
                {
                    if (played[z])
                    {
                        z++;
                        spriteBatch.DrawString(font, "Player #: " + z, position, Color.Gold);
                        z--;
                        spriteBatch.DrawString(font, "is eliminated!", new Vector2(position.X, (position.Y + element_height)),
                            Color.Yellow);
                    }
                }
            //Else increment to next box
                z++;
                position.X = border + z * (screen_width * (text_size + bar_size + separator_size));
                z--;
                position.Y = gui_height_start;
            }
            spriteBatch.End();
        }
    }
}