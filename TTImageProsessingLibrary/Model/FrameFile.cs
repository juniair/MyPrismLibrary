using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTImageProsessingLibrary.Model
{
    public class FrameFile
    {
        public byte[] Source { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
    }
}
