using System.Drawing;

namespace Bravos.Tiles
{
    public class Tile
    {
        public const int NORMAL = 0;
        public const int BLOCKED = 1;

        private int Type;

        private Image img;

        public Tile(Image img)
        {
            this.img = img;
        }

        public Tile(Image img, int type)
        {
            this.img = img;
            this.Type = type;
        }

        public Image GetImage()
        {
            return img;
        }
    }

}
