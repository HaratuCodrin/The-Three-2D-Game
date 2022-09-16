using Bravos.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bravos
{
    public abstract class GameState
    {
        protected GameStateManager gsm;
        protected List<Gologan> collectables;
        public GameState(GameStateManager gsm)
        {
            this.gsm = gsm;
        }


        public abstract void Update();
        public abstract void Render(Graphics g);
        public abstract void KeyPressed(KeyEventArgs e);
        public abstract void KeyReleased(KeyEventArgs e);

        public abstract void Load();

    }

}