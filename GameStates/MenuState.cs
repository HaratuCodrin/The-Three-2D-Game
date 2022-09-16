using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace Bravos
{
    public class MenuState : GameState
    {
        private Background bg;
        private readonly string[] options =
        {
            /*"New Game",
            "Load Game",
            "Practice Tool",
            "Quit"*/
            "DAN",
            "COSMIN",
            "CODRIN",
            "PRACTICE",
            "Quit"

        };

        private readonly Color titleColor;
        private Color choiceColor;
        private readonly Font titleFont;

        private readonly Font font;
        private int currentChoice = 0;      

        public MenuState(GameStateManager gsm) : base(gsm)
        {
            bg = new Background("Images/menubg.gif", 40);
            bg.setVelocity(-0.5, 0);
            bg.setPosition(new Vector(0, 0));
            titleColor = Color.Blue;
            titleFont = new Font("Century Gothic", 36);
            font = new Font("Arial", 22);      
        }

        private void select()
        {
            switch(currentChoice)
            {
                /*case 0: // New Game
                    gsm.CurtainCall(GameStateManager.PRACTICE);
                    break;
                case 1: // Load Game
                    break;
                case 2: // Practice Tool
                    gsm.SetState(GameStateManager.PRACTICE);
                    break;
                case 3: // Quit
                    System.Environment.Exit(0);
                    break;*/
                case 0: 
                    gsm.CurtainCall(GameStateManager.DAN);
                    break;
                case 1:
                    gsm.CurtainCall(GameStateManager.COSMIN);
                    break;
                case 2: 
                    gsm.CurtainCall(GameStateManager.CODRIN);
                    break;
                case 3:
                    gsm.SetState(GameStateManager.PRACTICE);
                    break;
                case 4: 
                    System.Environment.Exit(0);
                    break;


            }
        }



        public override void Load()
        {
           
        }

        public override void Render(Graphics g)
        {
            // draw background
            bg.Render(g);

            // title
            g.DrawString(GamePanel.title, titleFont, new SolidBrush(titleColor), new Point(90*GamePanel.SCALE, 70*GamePanel.SCALE));

            // options
            for(int i = 0; i < options.Length; i++)
            {
                if (i == currentChoice)
                {
                    choiceColor = Color.Black;
                }
                else
                    choiceColor = Color.Red;

                g.DrawString(options[i], font, new SolidBrush(choiceColor) , new Point(125*GamePanel.SCALE, (140 + i * 15)*GamePanel.SCALE));
            }

        }

        public override void Update()
        {
            bg.Update();
        }

        public override void KeyPressed(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) 
            {
                GamePanel.sounds["menuSelect"].Play();
                select(); 
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                GamePanel.sounds["menuHover"].Play();
                currentChoice--;
                if (currentChoice == -1)
                {
                    currentChoice = options.Length - 1;
                }
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                GamePanel.sounds["menuHover"].Play();
                currentChoice++;
                if (currentChoice == options.Length)
                {
                    currentChoice = 0;
                }
            }


        }

        public override void KeyReleased(KeyEventArgs e)
        {
            
        }
    }

}