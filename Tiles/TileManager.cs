using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bravos.Tiles
{

    public class TileManager
    {
        private List<TileMap> Maps;
        private Vector Position;
        public int BlockSize { get; set; }
        private double Tween;
        public Dictionary<String, int> blocks = null;

        public int[,] BlockMap { get; set; }

        // bounds
        private int Xmin;
        private int Xmax;
        private int Ymin;
        private int Ymax;

        public TileManager(String path)
        {
            Tween = 0.7;
            
            AddTileMaps(path);
            Position = new Vector(0, 0);
            SetPosition(Position.X, Position.Y);
        }

        private void AddTileMaps(String path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            string[] data = new string[10];
            Maps = new List<TileMap>();

            // pentru a lua tilesize-ul
            XmlNodeList list = doc.GetElementsByTagName("tileset");
            XmlNode node = list.Item(0);

            XmlElement eElement = (XmlElement)node;
            int tileSize = Int32.Parse(eElement.GetAttribute("tilewidth"));

            // Pentru a lua latimea si inaltimea mapei
            list = doc.GetElementsByTagName("map");
            node = list.Item(0);
            eElement = (XmlElement)node;
            int width = Int32.Parse(eElement.GetAttribute("width"));
            int height = Int32.Parse(eElement.GetAttribute("height"));

            Xmin = GamePanel.WIDTH * GamePanel.SCALE - width * tileSize * GamePanel.SCALE;
            Xmax = 0;
            Ymin = GamePanel.HEIGHT * GamePanel.SCALE - height * tileSize * GamePanel.SCALE;
            Ymax = 0;
            BlockSize = tileSize* GamePanel.SCALE;
            // pentru a lua fisierul tileset
            list = doc.GetElementsByTagName("image");
            node = list.Item(0);
            eElement = (XmlElement)node;
            String TileSetPath = eElement.GetAttribute("source");

            // loading maps

            list = doc.GetElementsByTagName("layer");
            int layers = list.Count;

            for (int i = 0; i < layers; i++)
            {
                node = list.Item(i);
                eElement = (XmlElement)node;
                data[i] = eElement.GetElementsByTagName("data").Item(0).InnerText;

                switch (i)
                {
                    case 0:
                        Maps.Add(new TileMap(tileSize, width, height, Image.FromFile(TileSetPath), data[i], 0, this));
                        break;
                    case 1:
                        Maps.Add(new TileMap(tileSize, width, height, Image.FromFile(TileSetPath), data[i], 1, this));
                        break;

                }
            }

            foreach (TileMap map in Maps)
            {
                if(map.type == 1)
                    this.blocks = map.GetBlocks();
            }


        }
        public void Render(Graphics g)
        {
            foreach (TileMap map in Maps)
            {
                map.Render(g);
            }
        }


        public void SetPosition(double x, double y)
        {
            this.Position.X += (x - this.Position.X) * Tween;
            this.Position.Y += (y - this.Position.Y) * Tween;

            fixBounds();

            foreach (TileMap map in Maps)
            {
                map.SetPosition(x, y);
            }

        }

        private void fixBounds()
        {
            if (Position.X < Xmin) Position.X = Xmin;
            if (Position.Y < Ymin) Position.Y = Ymin;
            if (Position.X > Xmax) Position.X = Xmax;
            if (Position.Y > Ymax) Position.Y = Ymax;
        }

        public Vector GetPosition()
        {
            return Position;
        }

        public Dictionary<String, int> GetBlocks()
        {
            return blocks;
        }

    }

}
