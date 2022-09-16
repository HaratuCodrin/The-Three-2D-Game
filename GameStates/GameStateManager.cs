using Bravos.GameStates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bravos
{
     public class GameStateManager
    {
        private int currentState;
        private GameState state;
        private static GameStateManager gsm_instance = null;
        public const int MENU = 0;
        public const int PLAYSTATE = 1;
        public const int LEVEL1 = 2;
        public const int PRACTICE = 4;
        private int nextState;

        public Dictionary<String, int> PlayerData;


        public const int CODRIN = 1;
        public const int DAN = 2;
        public const int COSMIN = 3;

        // for transitions
        private int curtainState;
        private float curtainWidth;
        private float curtainHeight;
        private float tween = 0.03f;
        private float curtainProgress;

        private SolidBrush curtainBrush = new SolidBrush(Color.Black);

       private GameStateManager()
        {
            SetState(MENU);
            
            curtainProgress = tween;
            nextState = -1;
        }

        public static GameStateManager GetInstance()
        {
            if (gsm_instance == null) return new GameStateManager();
            else return gsm_instance;
        }

        public void SetState(int targetState)
        {
            currentState = targetState;
            switch(targetState)
            {
                case MENU:
                    state = new MenuState(this);                   
                    break;
                case DAN:
                     state = new Level1(this, "Dan");
                    break;
                case CODRIN:
                    state = new Level1(this, "Codrin");
                    break;
                case COSMIN:
                    state = new Level1(this, "Cosmin");
                    break;
                case PRACTICE:
                    state = new Lobby(this);
                    break;


            }
        }

        public void Update()
        {
            state.Update();
            switch (curtainState)
            {
                case 0:
                    break;
                case 1:
                    curtainProgress += tween;
                    curtainWidth = (float)GamePanel.GetSize().X * curtainProgress;
                    curtainHeight = (float)GamePanel.GetSize().Y * curtainProgress;
                    if (curtainWidth >= (float)1.5 * GamePanel.GetSize().X && curtainHeight >= (float)1.5 * GamePanel.GetSize().Y)
                    {
                        curtainState = 2;
                        if(nextState != -1)
                        {
                            SetState(nextState);
                            nextState = -1;
                        }
                    }
                    break;
                case 2:
                    curtainProgress -= tween;
                    curtainWidth = (float)GamePanel.GetSize().X * curtainProgress;
                    curtainHeight = (float)GamePanel.GetSize().Y * curtainProgress;
                    if (curtainWidth <= 2 && curtainHeight <= 2)
                        curtainState = 0;
                    break;
            }
        }

        public void Render(Graphics g)
        {
            state.Render(g);
            switch(curtainState)
            {
                case 0:
                    
                    break;
                case 1:
                case 2:
                    g.FillEllipse(curtainBrush, (float)GamePanel.GetCenter().X - curtainWidth/2, (float)GamePanel.GetCenter().Y - curtainHeight/2, curtainWidth, curtainHeight);
                    break;
            }
        }


        public void CurtainCall()
        {
            curtainState = 1;
        }

        public void CurtainCall(int nextState)
        {
            curtainState = 1;
            this.nextState = nextState;
        }


        public void KeyPressed(KeyEventArgs e)
        {
            state.KeyPressed(e);
            
        }

        public void KeyReleased(KeyEventArgs e)
        {
            state.KeyReleased(e);
        }

        public int GetCurrentState() => currentState; 
        public void SetCurrentState(int state) => currentState = state;



    }

   
}
