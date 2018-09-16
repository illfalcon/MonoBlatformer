using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBlatformer
{
    public static class Camera
    {
        public static Vector2 Position;
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static int MaxX { get; set; }
        public static int MaxY { get; set; }
        public static int MinX { get; set; }
        public static int MinY { get; set; }
        
        public static void Initialize(Vector2 position, int height, int width, int maxX, int maxY, int minX, int minY)
        {
            Position = position;
            Height = height;
            Width = width;
            MaxX = maxX;
            MaxY = maxY;
            MinX = minX;
            MinY = minY;
        }
    }
}
