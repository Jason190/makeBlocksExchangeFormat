using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makeBlocksExchangeFormat
{
    class POSone
    {
        string name;
        double longitude, latitude, height, pitch, roll, yaw,heading;
        int state;
        string imagezs, image1, image2, image3, image4;

        public string Name { get => name; set => name = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Height { get => height; set => height = value; }
        public double Pitch { get => pitch; set => pitch = value; }
        public double Roll { get => roll; set => roll = value; }
        public double Yaw { get => yaw; set => yaw = value; }
        public double Heading { get => heading; set => heading = value; }
        public int State { get => state; set => state = value; }
        public string Imagezs { get => imagezs; set => imagezs = value; }
        public string Image1 { get => image1; set => image1 = value; }
        public string Image2 { get => image2; set => image2 = value; }
        public string Image3 { get => image3; set => image3 = value; }
        public string Image4 { get => image4; set => image4 = value; }

    }
}
