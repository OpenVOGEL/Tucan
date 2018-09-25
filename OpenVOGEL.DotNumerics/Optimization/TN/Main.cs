#region Translated by Jose Antonio De Santiago-Castillo.

//Translated by Jose Antonio De Santiago-Castillo.
//E-mail:JAntonioDeSantiago@gmail.com
//Website: www.DotNumerics.com
//
//Fortran to C# Translation.
//Translated by:
//F2CSharp Version 0.72 (Dicember 7, 2009)
//Code Optimizations: , assignment operator, for-loop: array indexes
//
#endregion

using System;
using DotNumerics.FortranLibrary;

namespace DotNumerics.Optimization.TruncatedNewton
{
    public sealed class MainProgram
    {


        #region Dependencies
        
        public MAINCB maincb; public MAINC mainc; public MAINS mains; public MAINSB mainsb; public SFUN sfun; public TN tn; 
        public TNBC tnbc;public LMQN lmqn; public LMQNBC lmqnbc; public MONIT monit; public ZTIME ztime; public STPMAX stpmax; 
        public MODZ modz;public CNVTST cnvtst; public CRASH crash; public MODLNP modlnp; public NDIA3 ndia3; public NEGVEC negvec; 
        public LSOUT lsout;public STEP1 step1; public MCHPR1 mchpr1; public CHKUCP chkucp; public SETUCR setucr; 
        public GTIMS gtims;public MSOLVE msolve; public MSLV mslv; public SSBFGS ssbfgs; public INITPC initpc; 
        public INITP3 initp3;public SETPAR setpar; public LINDER linder; public GETPTC getptc; public DDOT ddot; 
        public DAXPY daxpy;public DNRM2 dnrm2; public DCOPY dcopy; public DXPY dxpy; 

        #endregion


        #region Declaracion de variables publicas
        
        CommonBlock SUBSCR = new CommonBlock(0, 15, 0, 0);

        #endregion


        public MainProgram()
        {

            #region Set Dependencies
            
            sfun = new SFUN();
            monit = new MONIT();
            ztime = new ZTIME();
            stpmax = new STPMAX();
            modz = new MODZ();
            cnvtst = new CNVTST();
            crash = new CRASH();
            negvec = new NEGVEC();
            lsout = new LSOUT();
            mchpr1 = new MCHPR1();
            ssbfgs = new SSBFGS();
            getptc = new GETPTC();
            ddot = new DDOT();
            daxpy = new DAXPY();
            dnrm2 = new DNRM2();
            dcopy = new DCOPY();
            dxpy = new DXPY();
            gtims = new GTIMS(SUBSCR);
            setpar = new SETPAR(SUBSCR);
            ndia3 = new NDIA3(ddot);
            step1 = new STEP1(mchpr1);
            chkucp = new CHKUCP(mchpr1, dnrm2);
            setucr = new SETUCR(ddot);
            mslv = new MSLV(ddot, ssbfgs);
            msolve = new MSOLVE(mslv, SUBSCR);
            initp3 = new INITP3(ddot, dcopy);
            initpc = new INITPC(initp3, SUBSCR);
            modlnp = new MODLNP(ddot, initpc, ztime, msolve, gtims, ndia3, daxpy, negvec, dcopy);
            linder = new LINDER(ddot, getptc, lsout, dcopy);
            lmqn = new LMQN(dnrm2, step1, ddot, setpar, chkucp, setucr, modlnp, dcopy, linder, dxpy
                            , SUBSCR);
            mainc = new MAINC(sfun, lmqn);
            tn = new TN(mchpr1, lmqn);
            mains = new MAINS(sfun, tn);
            lmqnbc = new LMQNBC(ddot, dnrm2, step1, crash, setpar, chkucp, setucr, ztime, monit, modlnp
                                , dcopy, stpmax, linder, modz, cnvtst, dxpy, SUBSCR);
            maincb = new MAINCB(sfun, lmqnbc);
            tnbc = new TNBC(mchpr1, lmqnbc);
            mainsb = new MAINSB(sfun, tnbc);

            #endregion

        }
    }

    #region Interface
    
    public interface ISFUN
    {
        void Run(int N, double[] X, int offset_x, ref double F, ref double[] G, int offset_g);
    }

    #endregion

}
