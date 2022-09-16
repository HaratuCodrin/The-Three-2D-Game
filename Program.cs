using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bravos
{
    class GameLauncher
    {
        public static void Main(string[] args)
        {
            GamePanel test = new GamePanel(new Vector(GamePanel.WIDTH * GamePanel.SCALE, GamePanel.HEIGHT * GamePanel.SCALE), "The Three");
        }
    }
}
