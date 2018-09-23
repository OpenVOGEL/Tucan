//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using DotNumerics_Samples.Harness;

using DotNumerics.ODE;

namespace DotNumerics_Samples.Samples
{
    [Title("Differential Equations")]
    [Prefix("ODE")]
    class DifferentialEquations : SchemaInformationBasedSample
    {
        #region ODEs

        [Category("ODEs")]
        [Title("OdeAdamsMoulton")]
        [Description("The OdeAdamsMoulton class solves an initial-value problem for nonstiff ordinary differential equations using the Adams-Moulton method.")]
        public void ODEAdamsMoulton()
        {
            //Arenstorf orbit
            //The Arenstorf orbits are closed trajectories of the restricted three-body problem. 
            //Two bodies of masses m and (1-m) moving in a circular rotation, and a third body of 
            //negligible mass moving in the same plane (such as a satellite-earth-moon system.) 

            //using DotNumerics.ODE

            OdeFunction fun = new OdeFunction(ArenstorfOrbit);
            OdeAdamsMoulton odeAdams = new OdeAdamsMoulton(fun, 4);

            odeAdams.RelTol = 1.0E-7;
            odeAdams.AbsTol = 1.0E-7;

            double x0 = 0;
            double[] y0 = new double[4];
            y0[0] = 0.994E0;
            y0[1] = 0.0E0;
            y0[2] = 0.0E0;
            y0[3] = -2.00158510637908252240537862224E0;

            odeAdams.SetInitialValues(x0, y0);

            double[] y;
            double x = 0;
            for (int i = 0; i < 10; i++)
            {
                x = 2 * i;
                y = odeAdams.Solve(x);
                ObjectDumper.Write("X = " + x.ToString() + "  Y1 = " + y[0].ToString() + " Y2 = " + y[1].ToString());
            }

            //private double[] ArenstorfOrbit(double t, double[] y)
            //{
            //    double[] ydot = new double[4];
            //    double amu = 0; double amup = 0; double r1 = 0; double r2 = 0;
            //    double a = 0.012277471E0;
            //    double b = 1 - a;
            //    amu = a;
            //    amup = b;
            //    ydot[0] = y[2];
            //    ydot[1] = y[3];
            //    r1 = Math.Pow(y[0] + amu, 2) + Math.Pow(y[1], 2);
            //    r1 *= Math.Sqrt(r1);
            //    r2 = Math.Pow(y[0] - amup, 2) + Math.Pow(y[1], 2);
            //    r2 *= Math.Sqrt(r2);
            //    ydot[2] = y[0] + 2 * y[3] - amup * (y[0] + amu) / r1 - amu * (y[0] - amup) / r2;
            //    ydot[3] = y[1] - 2 * y[2] - amup * y[1] / r1 - amu * y[1] / r2;
            //    return ydot;
            //}
        }


        [Category("ODEs")]
        [Title("OdeExplicitRungeKutta45")]
        [Description("The OdeExplicitRungeKutta45 class solves an initial-value problem for nonstiff ordinary differential equations using the explicit Runge-Kutta method of order (4)5.")]
        public void ODEExplicitRungeKutta45()
        {
            //Arenstorf orbit
            //The Arenstorf orbits are closed trajectories of the restricted three-body problem. 
            //Two bodies of masses m and (1-m) moving in a circular rotation, and a third body of 
            //negligible mass moving in the same plane (such as a satellite-earth-moon system.) 

            //using DotNumerics.ODE

            OdeFunction fun = new OdeFunction(ArenstorfOrbit);
            OdeExplicitRungeKutta45 RK45 = new OdeExplicitRungeKutta45(fun, 4);

            RK45.RelTol = 1.0E-7;
            RK45.AbsTol = 1.0E-7;

            double x0 = 0;
            double[] y0 = new double[4];
            y0[0] = 0.994E0;
            y0[1] = 0.0E0;
            y0[2] = 0.0E0;
            y0[3] = -2.00158510637908252240537862224E0;

            RK45.SetInitialValues(x0, y0);

            double[,] y;
            double[] x = new double[10];
            for (int i = 0; i < 10; i++) x[i] = 2 * i;

            y = RK45.Solve(y0, x);

            for (int i = 0; i < 10; i++)
            {
                ObjectDumper.Write("X = " + x[i].ToString() + "  Y1 = " + y[i, 1].ToString() + " Y2 = " + y[i, 2].ToString());
            }

            //private double[] ArenstorfOrbit(double t, double[] y)
            //{
            //    double[] ydot = new double[4];
            //    double amu = 0; double amup = 0; double r1 = 0; double r2 = 0;
            //    double a = 0.012277471E0;
            //    double b = 1 - a;
            //    amu = a;
            //    amup = b;
            //    ydot[0] = y[2];
            //    ydot[1] = y[3];
            //    r1 = Math.Pow(y[0] + amu, 2) + Math.Pow(y[1], 2);
            //    r1 *= Math.Sqrt(r1);
            //    r2 = Math.Pow(y[0] - amup, 2) + Math.Pow(y[1], 2);
            //    r2 *= Math.Sqrt(r2);
            //    ydot[2] = y[0] + 2 * y[3] - amup * (y[0] + amu) / r1 - amu * (y[0] - amup) / r2;
            //    ydot[3] = y[1] - 2 * y[2] - amup * y[1] / r1 - amu * y[1] / r2;
            //    return ydot;
            //}

        }

        private double[] ArenstorfOrbit(double t, double[] y)
        {
            double[] ydot = new double[4];
            double amu = 0; double amup = 0; double r1 = 0; double r2 = 0;
            double a = 0.012277471E0;
            double b = 1 - a;
            amu = a;
            amup = b;
            ydot[0] = y[2];
            ydot[1] = y[3];
            r1 = Math.Pow(y[0] + amu, 2) + Math.Pow(y[1], 2);
            r1 *= Math.Sqrt(r1);
            r2 = Math.Pow(y[0] - amup, 2) + Math.Pow(y[1], 2);
            r2 *= Math.Sqrt(r2);
            ydot[2] = y[0] + 2 * y[3] - amup * (y[0] + amu) / r1 - amu * (y[0] - amup) / r2;
            ydot[3] = y[1] - 2 * y[2] - amup * y[1] / r1 - amu * y[1] / r2;
            return ydot;
        }

        #endregion


        #region Stiff ODEs

        [Category("Stiff ODEs")]
        [Title("OdeGearsBDF")]
        [Description("The OdeGearsBDF class solves an initial-value problem for stiff ordinary differential equations using the Gear’s BDF method.")]
        public void ODEDGearsBDF()
        {
            // Problem from chemical kinetics. Consists of the following three rate equations:
            //     dy1/dt = -.04*y1 + 1.e4*y2*y3
            //     dy2/dt = .04*y1 - 1.e4*y2*y3 - 3.e7*y2**2
            //     dy3/dt = 3.e7*y2**2
            // on the interval from t = 0.0 to t = 4.e10, with initial conditions
            // y1 = 1.0, y2 = y3 = 0.  The problem is stiff.

            //using DotNumerics.ODE

            double[] y = new double[3];
            y[0] = 1.0;
            y[1] = 0.0;
            y[2] = 0.0;

            OdeFunction YDot = new OdeFunction(RateEquations);
            OdeGearsBDF bdf = new OdeGearsBDF(YDot, 3);
            bdf.SetInitialValues(0, y);

            bdf.RelTolArray[0] = 1.0E-4;
            bdf.RelTolArray[1] = 1.0E-4;
            bdf.RelTolArray[2] = 1.0E-4;

            bdf.AbsTolArray[0] = 1.0E-8;
            bdf.AbsTolArray[1] = 1.0E-14;
            bdf.AbsTolArray[2] = 1.0E-6;

            double TOut = 0.4;
            for (int IOUT = 1; IOUT <= 12; IOUT++)
            {
                y = bdf.Solve(TOut);
                ObjectDumper.Write("t = " + TOut.ToString() + "  y = " + y[0].ToString() + "  " + y[1].ToString() + "  " + y[2].ToString());
                TOut = TOut * 10.0;
            }

        //private double[] RateEquations(double t, double[] y)
        //{
        //    double[] dydt = new double[3];
        //    dydt[0] = -.04E0 * y[0] + 1.0E4 * y[1] * y[2];
        //    dydt[2] = 3.0E7 * y[1] * y[1];
        //    dydt[1] = -dydt[0] - dydt[2];
        //    return dydt;
        //}
        }

        [Category("Stiff ODEs")]
        [Title("OdeGearsBDF with Jacobian")]
        [Description("The OdeGearsBDF class solves an initial-value problem for stiff ordinary differential equations using the Gear’s BDF method.")]
        public void ODEGearsBDFJacobian()
        {
            // Problem from chemical kinetics. Consists of the following three rate equations:
            //     dy1/dt = -.04*y1 + 1.e4*y2*y3
            //     dy2/dt = .04*y1 - 1.e4*y2*y3 - 3.e7*y2**2
            //     dy3/dt = 3.e7*y2**2
            // on the interval from t = 0.0 to t = 4.e10, with initial conditions
            // y1 = 1.0, y2 = y3 = 0.  The problem is stiff.

            //using DotNumerics.ODE

            double[] y = new double[3];
            y[0] = 1.0;
            y[1] = 0.0;
            y[2] = 0.0;

            OdeFunction YDot = new OdeFunction(RateEquations);
            OdeJacobian jac = new OdeJacobian(JacRateEquations);
            OdeGearsBDF bdf = new OdeGearsBDF(YDot, jac, 3);
            bdf.SetInitialValues(0, y);

            bdf.RelTolArray[0] = 1.0E-4;
            bdf.RelTolArray[1] = 1.0E-4;
            bdf.RelTolArray[2] = 1.0E-4;

            bdf.AbsTolArray[0] = 1.0E-8;
            bdf.AbsTolArray[1] = 1.0E-14;
            bdf.AbsTolArray[2] = 1.0E-6;

            double TOut = 0.4;
            for (int IOUT = 1; IOUT <= 12; IOUT++)
            {
                y = bdf.Solve(TOut);
                ObjectDumper.Write("t = " + TOut.ToString() + "  y = " + y[0].ToString() + "  " + y[1].ToString() + "  " + y[2].ToString());
                TOut = TOut * 10.0;
            }

            //private double[] RateEquations(double t, double[] y)
            //{
            //    double[] dydt = new double[3];
            //    dydt[0] = -.04E0 * y[0] + 1.0E4 * y[1] * y[2];
            //    dydt[2] = 3.0E7 * y[1] * y[1];
            //    dydt[1] = -dydt[0] - dydt[2];
            //    return dydt;
            //}

            //public double[,] JacRateEquations(double t, double[] y)
            //{
            //    double[,] jacobian = new double[y.Length, y.Length];
            //    jacobian[0, 0] = -.04E0;
            //    jacobian[0, 1] = 1.0E4 * y[2];
            //    jacobian[0, 2] = 1.0E4 * y[1];
            //    jacobian[1, 0] = .04E0;
            //    jacobian[1, 2] = -jacobian[0, 2];
            //    jacobian[2, 1] = 6.0E7 * y[1];
            //    jacobian[1, 1] = -jacobian[0, 1] - jacobian[2, 1];
            //    return jacobian;
            //}
        }

        private double[] RateEquations(double t, double[] y)
        {
            double[] dydt = new double[3];
            dydt[0] = -.04E0 * y[0] + 1.0E4 * y[1] * y[2];
            dydt[2] = 3.0E7 * y[1] * y[1];
            dydt[1] = -dydt[0] - dydt[2];
            return dydt;
        }
        public double[,] JacRateEquations(double t, double[] y)
        {
            double[,] jacobian = new double[y.Length, y.Length];
            jacobian[0, 0] = -.04E0;
            jacobian[0, 1] = 1.0E4 * y[2];
            jacobian[0, 2] = 1.0E4 * y[1];
            jacobian[1, 0] = .04E0;
            jacobian[1, 2] = -jacobian[0, 2];
            jacobian[2, 1] = 6.0E7 * y[1];
            jacobian[1, 1] = -jacobian[0, 1] - jacobian[2, 1];
            return jacobian;
        }

        [Category("Stiff ODEs")]
        [Title("OdeImplicitRungeKutta5")]
        [Description("The OdeImplicitRungeKutta5 class solves an initial-value problem for stiff ordinary differential equations using the implicit Runge-Kutta method of order 5.")]
        public void ODEImplicitRungeKutta5()
        {
            //VAN DER POL'S EQUATION
            //In this example the OdeImplicitRungeKutta5 class is used to solve the van der Pol equation:
            //y''-mu*(1-y^2)*y'+y=0
            //If y0=y and y1=y' then:
            //y'0 = y1,                                  y0(0)= 2
            //y'1 = ((1 - y0 ^2) * y1 - y0)/rpar,        y1(0)=-0.66
           
            //using DotNumerics.ODE

            OdeFunction YDot = new OdeFunction(VPol);
            OdeImplicitRungeKutta5 rungeKutta = new OdeImplicitRungeKutta5(YDot, 2);
            
            double[,] sol;
            double[] y0 = new double[2];
            y0[0] = 2.0E0;
            y0[1] = -0.66E0;

            double x0 = 0;
            double xf = 2;

            sol = rungeKutta.Solve(y0, x0, 0.2, xf);

            for (int i = 0; i < sol.GetLength(0); i++)
            {
                ObjectDumper.Write("X = " + sol[i, 0].ToString() + "  Y1 = " + sol[i, 1].ToString() + " Y2 = " + sol[i, 2].ToString());
            }


            //private double[] VPol(double T, double[] Y)
            //{
            //    double[] ydot = new double[2];           
            //    double rpar = 1.0E-6;
            //    ydot[0] = Y[1];
            //    ydot[1] = ((1 - Math.Pow(Y[0], 2)) * Y[1] - Y[0]) / rpar;
            //    return ydot;
            //}
        }

        [Category("Stiff ODEs")]
        [Title("OdeImplicitRungeKutta5 with Jacobian")]
        [Description("The OdeImplicitRungeKutta5 class solves an initial-value problem for stiff ordinary differential equations using the implicit Runge-Kutta method of order 5.")]
        public void ODEImplicitRungeKutta5Jacobian()
        {
            //VAN DER POL'S EQUATION
            //In this example the OdeImplicitRungeKutta5 class is used to solve the van der Pol equation:
            //y''-mu*(1-y^2)*y'+y=0
            //If y0=y and y1=y' then:
            //y'0 = y1,                                  y0(0)= 2
            //y'1 = ((1 - y0 ^2) * y1 - y0)/rpar,        y1(0)=-0.66

            //using DotNumerics.ODE

            OdeFunction YDot = new OdeFunction(VPol);
            OdeJacobian Jac = new OdeJacobian(JacVPol);

            OdeImplicitRungeKutta5 rungeKutta = new OdeImplicitRungeKutta5(YDot, Jac, 2);

            double[,] sol;
            double[] y0 = new double[2];
            y0[0] = 2.0E0;
            y0[1] = -0.66E0;

            double x0 = 0;
            double xf = 2;

            sol = rungeKutta.Solve(y0, x0, 0.2, xf);

            for (int i = 0; i < sol.GetLength(0); i++)
            {
                ObjectDumper.Write("X = " + sol[i, 0].ToString() + "  Y1 = " + sol[i, 1].ToString() + " Y2 = " + sol[i, 2].ToString());
            }


            //private double[] VPol(double T, double[] Y)
            //{
            //    double[] ydot = new double[2];           
            //    double rpar = 1.0E-6;
            //    ydot[0] = Y[1];
            //    ydot[1] = ((1 - Math.Pow(Y[0], 2)) * Y[1] - Y[0]) / rpar;
            //    return ydot;
            //}

            //public double[,] JacVPol(double T, double[] Y)
            //{
            //    double[,] jacobian = new double[2, 2];
            //    double rpar = 1.0E-6;
            //    jacobian[0, 0] = 0.0E0;
            //    jacobian[0, 1] = 1.0E0;
            //    jacobian[1, 0] = (-2.0E0 * Y[0] * Y[1] - 1.0E0) / rpar;
            //    jacobian[1, 1] = (1.0E0 - Math.Pow(Y[0], 2)) / rpar;
            //    return jacobian;
            //}
        }


        private double[] VPol(double T, double[] Y)
        {
            double[] ydot = new double[2];           
            double rpar = 1.0E-6;
            ydot[0] = Y[1];
            ydot[1] = ((1 - Math.Pow(Y[0], 2)) * Y[1] - Y[0]) / rpar;
            return ydot;
        }

        public double[,] JacVPol(double T, double[] Y)
        {
            double[,] jacobian = new double[2, 2];
            double rpar = 1.0E-6;
            jacobian[0, 0] = 0.0E0;
            jacobian[0, 1] = 1.0E0;
            jacobian[1, 0] = (-2.0E0 * Y[0] * Y[1] - 1.0E0) / rpar;
            jacobian[1, 1] = (1.0E0 - Math.Pow(Y[0], 2)) / rpar;
            return jacobian;
        }

        [Category("SchemaInformation")]
        [Title("Query - Views")]
        [Description("This sample lists all views in the schema.")]
        public void DesignTime2()
        {

        }

        [Category("SchemaInformation")]
        [Title("Query - Functions")]
        [Description("This sample lists all functions in the schema.")]
        public void DesignTime3()
        {

        }

 
        #endregion
    }

}
