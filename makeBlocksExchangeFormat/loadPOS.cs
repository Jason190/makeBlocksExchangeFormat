using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makeBlocksExchangeFormat
{
    class loadPOS
    {
        public List<POSone> loadPOSwzTxt(string path)
        {
            List<POSone> pos = new List<POSone>();
            try
            {
                StreamReader sr = new StreamReader(path);
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string holeLine = sr.ReadLine();
                    if (holeLine.Equals(string.Empty)) return null;
                    POSone POSone = new POSone();
                    string[] cells = holeLine.Split(',');
                    POSone.Name = cells[0];
                    POSone.Longitude = double.Parse(cells[2]);
                    POSone.Latitude = double.Parse(cells[3]);
                    POSone.Height = double.Parse(cells[4]);
                    POSone.Pitch = double.Parse(cells[5]);
                    POSone.Roll = double.Parse(cells[6]);
                    POSone.Heading = double.Parse(cells[7]);
                    POSone.State = int.Parse(cells[cells.Length - 1]);

                    pos.Add(POSone);
                }
                sr.Close();
            }
            catch
            {

            }
            return pos;
        }
    }
}
