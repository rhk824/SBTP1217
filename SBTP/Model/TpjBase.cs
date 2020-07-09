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

        public override string ToString()
        {
            return cn.ToString() + " " + zn.ToString() + " " + gjl.ToString();
        }
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

    public class Tpjyy
    {
        string jh;
        string qk;
        string csrq;
        double wd;
        double khd;
        string sjd;
        string qyfs;
        double tpsj;
        double zyhd;
        double csbhd;
        double tchd;
        double tcsjc;
        double ltfx;
        double sjhd;
        double tstl;
        double zstl;
        double yqd;
        double kxd;
        double khbj;
        double bj;
        double tgsl;
        double tgjl;
        double txsbl;
        string ymc;
        string gmc;
        string yyl;
        double ynd;
        double gyl;
        double gnd;
        double glj;
        double sgts;
        double ylsf;
        double jxsj;
        double hssj;
        double xjfd;
        double yxq;
        double zy;
        string bz;
        int zt;

        public string Jh { get => jh; set => jh = value; }
        public string Qk { get => qk; set => qk = value; }
        public string Csrq { get => csrq; set => csrq = value; }
        public double Wd { get => wd; set => wd = value; }
        public double Khd { get => khd; set => khd = value; }
        public string Sjd { get => sjd; set => sjd = value; }
        public string Qyfs { get => qyfs; set => qyfs = value; }
        public double Tpsj { get => tpsj; set => tpsj = value; }
        public double Zyhd { get => zyhd; set => zyhd = value; }
        public double Csbhd { get => csbhd; set => csbhd = value; }
        public double Tchd { get => tchd; set => tchd = value; }
        public double Tcsjc { get => tcsjc; set => tcsjc = value; }
        public double Ltfx { get => ltfx; set => ltfx = value; }
        public double Sjhd { get => sjhd; set => sjhd = value; }
        public double Tstl { get => tstl; set => tstl = value; }
        public double Zstl { get => zstl; set => zstl = value; }
        public double Yqd { get => yqd; set => yqd = value; }
        public double Kxd { get => kxd; set => kxd = value; }
        public double Khbj { get => khbj; set => khbj = value; }
        public double Bj { get => bj; set => bj = value; }
        public double Tgsl { get => tgsl; set => tgsl = value; }
        public double Tgjl { get => tgjl; set => tgjl = value; }
        public double Txsbl { get => txsbl; set => txsbl = value; }
        public string Ymc { get => ymc; set => ymc = value; }
        public string Gmc { get => gmc; set => gmc = value; }
        public string Yyl { get => yyl; set => yyl = value; }
        public double Ynd { get => ynd; set => ynd = value; }
        public double Gyl { get => gyl; set => gyl = value; }
        public double Gnd { get => gnd; set => gnd = value; }
        public double Glj { get => glj; set => glj = value; }
        public double Sgts { get => sgts; set => sgts = value; }
        public double Ylsf { get => ylsf; set => ylsf = value; }
        public double Jxsj { get => jxsj; set => jxsj = value; }
        public double Hssj { get => hssj; set => hssj = value; }
        public double Xjfd { get => xjfd; set => xjfd = value; }
        public double Yxq { get => yxq; set => yxq = value; }
        public double Zy { get => zy; set => zy = value; }
        public string Bz { get => bz; set => bz = value; }
        public int Zt { get => zt; set => zt = value; }
    }

}
