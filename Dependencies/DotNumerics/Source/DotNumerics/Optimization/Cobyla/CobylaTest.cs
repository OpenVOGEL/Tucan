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

namespace DotNumerics.Optimization.Cobyla
{

    #region The Class: COBYLATEST
    
    public class COBYLATEST
    {
    
        #region Dependencies
        
        COBYLA _cobyla; 
        #endregion
        #region Common variables
        
        #region Common Block: Default Declaration
        
        CommonBlock _default;
        Oint NPROB; 
        #endregion
        #endregion
        #region Variables
        
        const int NN = 10; const int MM = 15; const int LW = NN * (3 * NN + 2 * MM + 11) + 4 * MM + 6; 
        #endregion
        public COBYLATEST(COBYLA cobyla, CommonBlock Default)
        {
    
            #region Set Dependencies
            
            this._cobyla = cobyla; 
            #endregion
            #region Common varaible Initialization
            
            #region Common Block: Default Initialization
            
            this._default = Default;
            NPROB = Default.intData[0];
            #endregion
            #endregion
        }
    
        public COBYLATEST()
        {
    
            #region Initialization Common Blocks
            
            CommonBlock Default = new CommonBlock(0, 1, 0, 0);
            #endregion
            #region Dependencies (Initialization)
            
            TRSTLP trstlp = new TRSTLP();
            CALCFC calcfc = new CALCFC(Default);
            COBYLB cobylb = new COBYLB(calcfc, trstlp);
            COBYLA cobyla = new COBYLA(cobylb);
            #endregion
            #region Set Dependencies
            
            this._cobyla = cobyla; 
            #endregion
            #region Common varaible Initialization
            
            #region Common Block: Default Initialization
            
            this._default = Default;
            NPROB = Default.intData[0];
            #endregion
            #endregion
        }
        public void Run()
        {
            #region Implicit Variables
            
            double[] X = new double[NN]; int offset_x = 0; int o_x = -1; double[] XOPT = new double[NN]; int offset_xopt = 0; int o_xopt = -1; 
            double[] W = new double[LW]; int offset_w = 0; int o_w = -1;int[] IACT = new int[(MM+1)]; int offset_iact = 0; int o_iact = -1; 
            int N = 0;int M = 0; int I = 0; double RHOBEG = 0; double RHOEND = 0; int ICASE = 0; int IPRINT = 0; int MAXFUN = 0; 
            double TEMPA = 0;double TEMPB = 0; double TEMPC = 0; double TEMPD = 0; double TEMP = 0; 
            #endregion
            // C------------------------------------------------------------------------------
            // C     Main program of test problems in Report DAMTP 1992/NA5.
            // C------------------------------------------------------------------------------
            
            
            #region Body
            
            for (NPROB.v = 1; NPROB.v <= 10; NPROB.v++)
            {
                if (NPROB.v == 1)
                {
                    // C
                    // C     Minimization of a simple quadratic function of two variables.
                    // C
                    //ERROR-ERROR          PRINT 10;
                    N = 2;
                    M = 0;
                    XOPT[1 + o_xopt] =  - 1.0E0;
                    XOPT[2 + o_xopt] = 0.0E0;
                }
                else
                {
                    if (NPROB.v == 2)
                    {
                        // C
                        // C     Easy two dimensional minimization in unit circle.
                        // C
                        //ERROR-ERROR          PRINT 20;
                        N = 2;
                        M = 1;
                        XOPT[1 + o_xopt] = Math.Sqrt(0.5E0);
                        XOPT[2 + o_xopt] =  - XOPT[1 + o_xopt];
                    }
                    else
                    {
                        if (NPROB.v == 3)
                        {
                            // C
                            // C     Easy three dimensional minimization in ellipsoid.
                            // C
                            //ERROR-ERROR          PRINT 30;
                            N = 3;
                            M = 1;
                            XOPT[1 + o_xopt] = 1.0E0 / Math.Sqrt(3.0E0);
                            XOPT[2 + o_xopt] = 1.0E0 / Math.Sqrt(6.0E0);
                            XOPT[3 + o_xopt] =  - 1.0E0 / 3.0E0;
                        }
                        else
                        {
                            if (NPROB.v == 4)
                            {
                                // C
                                // C     Weak version of Rosenbrock's problem.
                                // C
                                //ERROR-ERROR          PRINT 40;
                                N = 2;
                                M = 0;
                                XOPT[1 + o_xopt] =  - 1.0E0;
                                XOPT[2 + o_xopt] = 1.0E0;
                            }
                            else
                            {
                                if (NPROB.v == 5)
                                {
                                    // C
                                    // C     Intermediate version of Rosenbrock's problem.
                                    // C
                                    //ERROR-ERROR          PRINT 50;
                                    N = 2;
                                    M = 0;
                                    XOPT[1 + o_xopt] =  - 1.0E0;
                                    XOPT[2 + o_xopt] = 1.0E0;
                                }
                                else
                                {
                                    if (NPROB.v == 6)
                                    {
                                        // C
                                        // C     This problem is taken from Fletcher's book Practical Methods of
                                        // C     Optimization and has the equation number (9.1.15).
                                        // C
                                        //ERROR-ERROR          PRINT 60;
                                        N = 2;
                                        M = 2;
                                        XOPT[1 + o_xopt] = Math.Sqrt(0.5E0);
                                        XOPT[2 + o_xopt] = XOPT[1 + o_xopt];
                                    }
                                    else
                                    {
                                        if (NPROB.v == 7)
                                        {
                                            // C
                                            // C     This problem is taken from Fletcher's book Practical Methods of
                                            // C     Optimization and has the equation number (14.4.2).
                                            // C
                                            //ERROR-ERROR          PRINT 70;
                                            N = 3;
                                            M = 3;
                                            XOPT[1 + o_xopt] = 0.0E0;
                                            XOPT[2 + o_xopt] =  - 3.0E0;
                                            XOPT[3 + o_xopt] =  - 3.0E0;
                                        }
                                        else
                                        {
                                            if (NPROB.v == 8)
                                            {
                                                // C
                                                // C     This problem is taken from page 66 of Hock and Schittkowski's book Test
                                                // C     Examples for Nonlinear Programming Codes. It is their test problem Number
                                                // C     43, and has the name Rosen-Suzuki.
                                                // C
                                                //ERROR-ERROR          PRINT 80;
                                                N = 4;
                                                M = 3;
                                                XOPT[1 + o_xopt] = 0.0E0;
                                                XOPT[2 + o_xopt] = 1.0E0;
                                                XOPT[3 + o_xopt] = 2.0E0;
                                                XOPT[4 + o_xopt] =  - 1.0E0;
                                            }
                                            else
                                            {
                                                if (NPROB.v == 9)
                                                {
                                                    // C
                                                    // C     This problem is taken from page 111 of Hock and Schittkowski's
                                                    // C     book Test Examples for Nonlinear Programming Codes. It is their
                                                    // C     test problem Number 100.
                                                    // C
                                                    //ERROR-ERROR          PRINT 90;
                                                    N = 7;
                                                    M = 4;
                                                    XOPT[1 + o_xopt] = 2.330499E0;
                                                    XOPT[2 + o_xopt] = 1.951372E0;
                                                    XOPT[3 + o_xopt] =  - 0.4775414E0;
                                                    XOPT[4 + o_xopt] = 4.365726E0;
                                                    XOPT[5 + o_xopt] =  - 0.624487E0;
                                                    XOPT[6 + o_xopt] = 1.038131E0;
                                                    XOPT[7 + o_xopt] = 1.594227E0;
                                                }
                                                else
                                                {
                                                    if (NPROB.v == 10)
                                                    {
                                                        // C
                                                        // C     This problem is taken from page 415 of Luenberger's book Applied
                                                        // C     Nonlinear Programming. It is to maximize the area of a hexagon of
                                                        // C     unit diameter.
                                                        // C
                                                        //ERROR-ERROR          PRINT 100;
                                                        N = 9;
                                                        M = 14;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                for (ICASE = 1; ICASE <= 2; ICASE++)
                {
                    for (I = 1; I <= N; I++)
                    {
                        X[I + o_x] = 1.0E0;
                    }
                    RHOBEG = 0.5E0;
                    RHOEND = 1.0E-6;
                    if (ICASE == 2) RHOEND = 1.0E-8;
                    IPRINT = 1;
                    MAXFUN = 3500;
                    this._cobyla.Run(N, M, ref X, offset_x, RHOBEG, RHOEND, IPRINT
                                     , ref MAXFUN, ref W, offset_w, ref IACT, offset_iact);
                    if (NPROB.v == 10)
                    {
                        TEMPA = X[1 + o_x] + X[3 + o_x] + X[5 + o_x] + X[7 + o_x];
                        TEMPB = X[2 + o_x] + X[4 + o_x] + X[6 + o_x] + X[8 + o_x];
                        TEMPC = 0.5E0 / Math.Sqrt(TEMPA * TEMPA + TEMPB * TEMPB);
                        TEMPD = TEMPC * Math.Sqrt(3.0E0);
                        XOPT[1 + o_xopt] = TEMPD * TEMPA + TEMPC * TEMPB;
                        XOPT[2 + o_xopt] = TEMPD * TEMPB - TEMPC * TEMPA;
                        XOPT[3 + o_xopt] = TEMPD * TEMPA - TEMPC * TEMPB;
                        XOPT[4 + o_xopt] = TEMPD * TEMPB + TEMPC * TEMPA;
                        for (I = 1; I <= 4; I++)
                        {
                            XOPT[I + 4 + o_xopt] = XOPT[I + o_xopt];
                        }
                    }
                    TEMP = 0.0E0;
                    for (I = 1; I <= N; I++)
                    {
                        TEMP += Math.Pow(X[I + o_x] - XOPT[I + o_xopt],2);
                    }
                    //ERROR-ERROR      PRINT 150, SQRT(TEMP);
                }
                //ERROR-ERROR      PRINT 170;
            }
            return;
            #endregion
        }
    }

    #endregion


    #region The Class: CALCFC
    
    // C------------------------------------------------------------------------------
    public class CALCFC
    {
    
        #region Common variables
        
        #region Common Block: Default Declaration
        
        CommonBlock _default;
        Oint NPROB; 
        #endregion
        #endregion
        public CALCFC(CommonBlock Default)
        {
    
            #region Common varaible Initialization
            
            #region Common Block: Default Initialization
            
            this._default = Default;
            NPROB = Default.intData[0];
            #endregion
            #endregion
        }
    
        public CALCFC()
        {
    
            #region Initialization Common Blocks
            
            CommonBlock Default = new CommonBlock(0, 1, 0, 0);
            #endregion
            #region Common varaible Initialization
            
            #region Common Block: Default Initialization
            
            this._default = Default;
            NPROB = Default.intData[0];
            #endregion
            #endregion
        }
        public void Run(int N, int M, double[] X, int offset_x, ref double F, ref double[] CON, int offset_con)
        {
            #region Array Index Correction
            
             int o_x = -1 + offset_x;  int o_con = -1 + offset_con; 
            #endregion
            #region Body
            
            if (NPROB.v == 1)
            {
                // C
                // C     Test problem 1 (Simple quadratic)
                // C     
                F = 10.0E0 * Math.Pow(X[1 + o_x] + 1.0E0,2) + Math.Pow(X[2 + o_x],2);
            }
            else
            {
                if (NPROB.v == 2)
                {
                    // C
                    // C    Test problem 2 (2D unit circle calculation)
                    // C
                    F = X[1 + o_x] * X[2 + o_x];
                    CON[1 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2);
                }
                else
                {
                    if (NPROB.v == 3)
                    {
                        // C
                        // C     Test problem 3 (3D ellipsoid calculation)
                        // C
                        F = X[1 + o_x] * X[2 + o_x] * X[3 + o_x];
                        CON[1 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x],2) - 2.0E0 * Math.Pow(X[2 + o_x],2) - 3.0E0 * Math.Pow(X[3 + o_x],2);
                    }
                    else
                    {
                        if (NPROB.v == 4)
                        {
                            // C
                            // C     Test problem 4 (Weak Rosenbrock)
                            // C
                            F = Math.Pow(Math.Pow(X[1 + o_x],2) - X[2 + o_x],2) + Math.Pow(1.0E0 + X[1 + o_x],2);
                        }
                        else
                        {
                            if (NPROB.v == 5)
                            {
                                // C
                                // C     Test problem 5 (Intermediate Rosenbrock)
                                // C
                                F = 10.0E0 * Math.Pow(Math.Pow(X[1 + o_x],2) - X[2 + o_x],2) + Math.Pow(1.0E0 + X[1 + o_x],2);
                            }
                            else
                            {
                                if (NPROB.v == 6)
                                {
                                    // C
                                    // C     Test problem 6 (Equation (9.1.15) in Fletcher's book)
                                    // C
                                    F =  - X[1 + o_x] - X[2 + o_x];
                                    CON[1 + o_con] = X[2 + o_x] - Math.Pow(X[1 + o_x],2);
                                    CON[2 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2);
                                }
                                else
                                {
                                    if (NPROB.v == 7)
                                    {
                                        // C
                                        // C     Test problem 7 (Equation (14.4.2) in Fletcher's book)
                                        // C
                                        F = X[3 + o_x];
                                        CON[1 + o_con] = 5.0E0 * X[1 + o_x] - X[2 + o_x] + X[3 + o_x];
                                        CON[2 + o_con] = X[3 + o_x] - Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2) - 4.0E0 * X[2 + o_x];
                                        CON[3 + o_con] = X[3 + o_x] - 5.0E0 * X[1 + o_x] - X[2 + o_x];
                                    }
                                    else
                                    {
                                        if (NPROB.v == 8)
                                        {
                                            // C
                                            // C     Test problem 8 (Rosen-Suzuki)
                                            // C
                                            F = Math.Pow(X[1 + o_x],2) + Math.Pow(X[2 + o_x],2) + 2.0 * Math.Pow(X[3 + o_x],2) + Math.Pow(X[4 + o_x],2) - 5.0E0 * X[1 + o_x] - 5.0E0 * X[2 + o_x] - 21.0E0 * X[3 + o_x] + 7.0E0 * X[4 + o_x];
                                            CON[1 + o_con] = 8.0E0 - Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2) - Math.Pow(X[3 + o_x],2) - Math.Pow(X[4 + o_x],2) - X[1 + o_x] + X[2 + o_x] - X[3 + o_x] + X[4 + o_x];
                                            CON[2 + o_con] = 10.0E0 - Math.Pow(X[1 + o_x],2) - 2.0E0 * Math.Pow(X[2 + o_x],2) - Math.Pow(X[3 + o_x],2) - 2.0E0 * Math.Pow(X[4 + o_x],2) + X[1 + o_x] + X[4 + o_x];
                                            CON[3 + o_con] = 5.0E0 - 2.0 * Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2) - Math.Pow(X[3 + o_x],2) - 2.0E0 * X[1 + o_x] + X[2 + o_x] + X[4 + o_x];
                                        }
                                        else
                                        {
                                            if (NPROB.v == 9)
                                            {
                                                // C
                                                // C     Test problem 9 (Hock and Schittkowski 100)
                                                // C
                                                F = Math.Pow(X[1 + o_x] - 10.0E0,2) + 5.0E0 * Math.Pow(X[2 + o_x] - 12.0E0,2) + Math.Pow(X[3 + o_x],4) + 3.0E0 * Math.Pow(X[4 + o_x] - 11.0E0,2) + 10.0E0 * Math.Pow(X[5 + o_x],6) + 7.0E0 * Math.Pow(X[6 + o_x],2) + Math.Pow(X[7 + o_x],4) - 4.0E0 * X[6 + o_x] * X[7 + o_x] - 10.0E0 * X[6 + o_x] - 8.0E0 * X[7 + o_x];
                                                CON[1 + o_con] = 127.0E0 - 2.0E0 * Math.Pow(X[1 + o_x],2) - 3.0E0 * Math.Pow(X[2 + o_x],4) - X[3 + o_x] - 4.0E0 * Math.Pow(X[4 + o_x],2) - 5.0E0 * X[5 + o_x];
                                                CON[2 + o_con] = 282.0E0 - 7.0E0 * X[1 + o_x] - 3.0E0 * X[2 + o_x] - 10.0E0 * Math.Pow(X[3 + o_x],2) - X[4 + o_x] + X[5 + o_x];
                                                CON[3 + o_con] = 196.0E0 - 23.0E0 * X[1 + o_x] - Math.Pow(X[2 + o_x],2) - 6.0E0 * Math.Pow(X[6 + o_x],2) + 8.0E0 * X[7 + o_x];
                                                CON[4 + o_con] =  - 4.0E0 * Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x],2) + 3.0E0 * X[1 + o_x] * X[2 + o_x] - 2.0E0 * Math.Pow(X[3 + o_x],2) - 5.0E0 * X[6 + o_x] + 11.0E0 * X[7 + o_x];
                                            }
                                            else
                                            {
                                                if (NPROB.v == 10)
                                                {
                                                    // C
                                                    // C     Test problem 10 (Hexagon area)
                                                    // C
                                                    F =  - 0.5E0 * (X[1 + o_x] * X[4 + o_x] - X[2 + o_x] * X[3 + o_x] + X[3 + o_x] * X[9 + o_x] - X[5 + o_x] * X[9 + o_x] + X[5 + o_x] * X[8 + o_x] - X[6 + o_x] * X[7 + o_x]);
                                                    CON[1 + o_con] = 1.0E0 - Math.Pow(X[3 + o_x],2) - Math.Pow(X[4 + o_x],2);
                                                    CON[2 + o_con] = 1.0E0 - Math.Pow(X[9 + o_x],2);
                                                    CON[3 + o_con] = 1.0E0 - Math.Pow(X[5 + o_x],2) - Math.Pow(X[6 + o_x],2);
                                                    CON[4 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x],2) - Math.Pow(X[2 + o_x] - X[9 + o_x],2);
                                                    CON[5 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x] - X[5 + o_x],2) - Math.Pow(X[2 + o_x] - X[6 + o_x],2);
                                                    CON[6 + o_con] = 1.0E0 - Math.Pow(X[1 + o_x] - X[7 + o_x],2) - Math.Pow(X[2 + o_x] - X[8 + o_x],2);
                                                    CON[7 + o_con] = 1.0E0 - Math.Pow(X[3 + o_x] - X[5 + o_x],2) - Math.Pow(X[4 + o_x] - X[6 + o_x],2);
                                                    CON[8 + o_con] = 1.0E0 - Math.Pow(X[3 + o_x] - X[7 + o_x],2) - Math.Pow(X[4 + o_x] - X[8 + o_x],2);
                                                    CON[9 + o_con] = 1.0E0 - Math.Pow(X[7 + o_x],2) - Math.Pow(X[8 + o_x] - X[9 + o_x],2);
                                                    CON[10 + o_con] = X[1 + o_x] * X[4 + o_x] - X[2 + o_x] * X[3 + o_x];
                                                    CON[11 + o_con] = X[3 + o_x] * X[9 + o_x];
                                                    CON[12 + o_con] =  - X[5 + o_x] * X[9 + o_x];
                                                    CON[13 + o_con] = X[5 + o_x] * X[8 + o_x] - X[6 + o_x] * X[7 + o_x];
                                                    CON[14 + o_con] = X[9 + o_x];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return;
            #endregion
        }
    }

    #endregion

}
