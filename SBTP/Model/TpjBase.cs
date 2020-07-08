using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class TpjBase
    {
        private string mc;
        private string dw;
        private string tyrq;
        private double nw;
        private double ny;
        private double nj;
        private string xn;
        private string sxq;
        private double jg;
        private string bz;
        private int zt;

        public string Mc { get => mc; set => mc = value; }
        public string Dw { get => dw; set => dw = value; }
        public string Tyrq { get => tyrq; set => tyrq = value; }
        public double Nw { get => nw; set => nw = value; }
        public double Ny { get => ny; set => ny = value; }
        public double Nj { get => nj; set => nj = value; }
        public string Xn { get => xn; set => xn = value; }
        public string Sxq { get => sxq; set => sxq = value; }
        public double Jg { get => jg; set => jg = value; }
        public string Bz { get => bz; set => bz = value; }
        public int Zt { get => zt; set => zt = value; }
    }

    public class Yttpj : TpjBase
    {
        string cn;
        string zn;
        string gjl;

        public string Cn { get => cn; set => cn = value; }
        public string Zn { get => zn; set => zn = value; }
        public string Gjl { get => gjl; set => gjl = value; }
    }
    public class Kltpj : TpjBase
    {
        double cpsj;
        double cpbs;
        double zpbs;
        double pzsj;
        double kyqd;
        double bsb;
        double txml;

        public double Cpsj { get => cpsj; set => cpsj = value; }
        public double Cpbs { get => cpbs; set => cpbs = value; }
        public double Zpbs { get => zpbs; set => zpbs = value; }
        public double Pzsj { get => pzsj; set => pzsj = value; }
        public double Kyqd { get => kyqd; set => kyqd = value; }
        public double Bsb { get => bsb; set => bsb = value; }
        public double Txml { get => txml; set => txml = value; }
    }
}
