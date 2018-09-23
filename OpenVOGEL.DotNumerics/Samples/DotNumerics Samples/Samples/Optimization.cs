//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Common;
using System.Threading;
using System.Text;
using DotNumerics_Samples.Harness;

using DotNumerics.Optimization;

namespace DotNumerics_Samples.Samples
{
    [Title("Optimization")]
    [Prefix("Optimization")]
    class Optimization: SchemaInformationBasedSample
    {
        #region Unconstrained Minimization
        [Category("Unconstrained Minimization")]
        [Title("Simplex method")]
        [Description("The Nelder-Mead Simplex method. ")]
        public void OptimizationSimplex()
        {
            //Rosenbrock banana function
            //f(a,b)=100*(b-a^2)^2+(1-a)^2
            //The minimum is at (1,1) and the  function value is 0.

            //using DotNumerics.Optimization;

            Simplex simplex = new Simplex();
            double[] initialGuess = new double[2];
            initialGuess[0] = 0.1;
            initialGuess[1] = 2;
            double[] minimum = simplex.ComputeMin(BananaFunction, initialGuess);

            ObjectDumper.Write("Simplex Method.");
            ObjectDumper.Write("a = " + minimum[0].ToString() + ",   b = " + minimum[1].ToString());

            //f(a,b) = 100*(b-a^2)^2 + (1-a)^2
            //private double BananaFunction(double[] x)
            //{
            //    double f = 0;
            //    f = 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + Math.Pow((1 - x[0]), 2);
            //    return f;
            //}

        }

        [Category("Unconstrained Minimization")]
        [Title("L-BFGS-B")]
        [Description("L-BFGS-B: Limited memory Broyden–Fletcher–Goldfarb–Shanno method.")]
        public void OptimizationLBFGSB()
        {
            //Rosenbrock banana function
            //f(a,b)=100*(b-a^2)^2+(1-a)^2
            //The minimum is at (1,1) and the  function value is 0.

            //using DotNumerics.Optimization;

            L_BFGS_B LBFGSB = new L_BFGS_B();
            double[] initialGuess = new double[2];
            initialGuess[0] = 0.1;
            initialGuess[1] = 2;
            double[] minimum = LBFGSB.ComputeMin(BananaFunction, BananaGradient, initialGuess);

            ObjectDumper.Write("Limited memory Broyden–Fletcher–Goldfarb–Shanno method.");
            ObjectDumper.Write("a = " + minimum[0].ToString() + ",   b = " + minimum[1].ToString());

            //f(a,b) = 100*(b-a^2)^2 + (1-a)^2
            //private double BananaFunction(double[] x)
            //{
            //    double f = 0;
            //    f = 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + Math.Pow((1 - x[0]), 2);
            //    return f;
            //}

            //double[] bananaGrad = new double[2];
            //private double[] BananaGradient(double[] x)
            //{
            //    this.bananaGrad[0] = 200 * (x[1] - x[0] * x[0]) * (-2 * x[0]) - 2 * (1 - x[0]);
            //    this.bananaGrad[1] = 200 * (x[1] - x[0] * x[0]);
            //    return bananaGrad;
            //}
        }


        [Category("Unconstrained Minimization")]
        [Title("TruncatedNewton")]
        [Description("Truncated Newton method.")]
        public void OptimizationTruncatedNewton()
        {
            //Rosenbrock banana function
            //f(a,b)=100*(b-a^2)^2+(1-a)^2
            //The minimum is at (1,1) and the  function value is 0.

            //using DotNumerics.Optimization;

            TruncatedNewton tNewton = new TruncatedNewton();
            double[] initialGuess = new double[2];
            initialGuess[0] = 0.1;
            initialGuess[1] = 2;
            double[] minimum = tNewton.ComputeMin(BananaFunction, BananaGradient, initialGuess);

            ObjectDumper.Write("Truncated Newton.");
            ObjectDumper.Write("a = " + minimum[0].ToString() + ",   b = " + minimum[1].ToString());

            //f(a,b) = 100*(b-a^2)^2 + (1-a)^2
            //private double BananaFunction(double[] x)
            //{
            //    double f = 0;
            //    f = 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + Math.Pow((1 - x[0]), 2);
            //    return f;
            //}

            //double[] bananaGrad = new double[2];
            //private double[] BananaGradient(double[] x)
            //{
            //    this.bananaGrad[0] = 200 * (x[1] - x[0] * x[0]) * (-2 * x[0]) - 2 * (1 - x[0]);
            //    this.bananaGrad[1] = 200 * (x[1] - x[0] * x[0]);
            //    return bananaGrad;
            //}
        }



        //f(a,b) = 100*(b-a^2)^2 + (1-a)^2
        private double BananaFunction(double[] x)
        {
            double f = 0;
            f = 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + Math.Pow((1 - x[0]), 2);
            return f;
        }

        double[] bananaGrad = new double[2];
        private double[] BananaGradient(double[] x)
        {
            this.bananaGrad[0] = 200 * (x[1] - x[0] * x[0]) * (-2 * x[0]) - 2 * (1 - x[0]);
            this.bananaGrad[1] = 200 * (x[1] - x[0] * x[0]);
            return bananaGrad;
        }


        #endregion

        #region Constrained Minimization

        [Category("Constrained Minimization")]
        [Title("Simplex method")]
        [Description("The Nelder-Mead Simplex method. ")]
        public void OptimizationSimplexConstrained()
        {
            //This example minimize the function
            //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
            //The minimum is at (0,1,2,3...,n) for the unconstrained case.

            //using DotNumerics.Optimization;

            Simplex simplex = new Simplex();
            int numVariables = 5;
            OptBoundVariable[] variables = new OptBoundVariable[numVariables];
            //Constrained Minimization on the interval (-10,10), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i] = new OptBoundVariable("x" + i.ToString(), -2, -10, 10);
            double[] minimum = simplex.ComputeMin(ObjetiveFunction, variables);

            ObjectDumper.Write("Simplex Method. Constrained Minimization on the interval (-10,10)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //Constrained Minimization on the interval (-10,3), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i].UpperBound = 3;
            minimum = simplex.ComputeMin(ObjetiveFunction, variables);

            ObjectDumper.Write("Simplex Method. Constrained Minimization on the interval (-10,3)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
            //private double ObjetiveFunction(double[] x)
            //{
            //    int numVariables = 5;
            //    double f = 0;
            //    for (int i = 0; i < numVariables; i++) f += Math.Pow(x[i] - i, 2);
            //    return f;
            //}
        }

        [Category("Constrained Minimization")]
        [Title("L-BFGS-B")]
        [Description("L-BFGS-B: Limited memory Broyden–Fletcher–Goldfarb–Shanno method.")]
        public void OptimizationLBFGSBConstrained()
        {
            //This example minimize the function
            //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
            //The minimum is at (0,1,2,3...,n) for the unconstrained case.

            //using DotNumerics.Optimization;

            L_BFGS_B LBFGSB = new L_BFGS_B();
            int numVariables = 5;
            OptBoundVariable[] variables = new OptBoundVariable[numVariables];
            //Constrained Minimization on the interval (-10,10), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i] = new OptBoundVariable("x" + i.ToString(), -2, -10, 10);
            double[] minimum = LBFGSB.ComputeMin(ObjetiveFunction, Gradient,variables);

            ObjectDumper.Write("L-BFGS-B Method. Constrained Minimization on the interval (-10,10)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //Constrained Minimization on the interval (-10,3), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i].UpperBound = 3;
            minimum = LBFGSB.ComputeMin(ObjetiveFunction, Gradient, variables);

            ObjectDumper.Write("L-BFGS-B Method. Constrained Minimization on the interval (-10,3)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
            //private double ObjetiveFunction(double[] x)
            //{
            //    int numVariables = 5;
            //    double f = 0;
            //    for (int i = 0; i < numVariables; i++) f += Math.Pow(x[i] - i, 2);
            //    return f;
            //}

            //private double[] Gradient(double[] x)
            //{
            //    int numVariables = 5;
            //    double[] grad = new double[x.Length];
            //    for (int i = 0; i < numVariables; i++) grad[i] = 2 * (x[i] - i);
            //    return grad;
            //}
        }


        [Category("Constrained Minimization")]
        [Title("TruncatedNewton")]
        [Description("Truncated Newton method.")]
        public void OptimizationTruncatedNewtonConstrained()
        {
            //This example minimize the function
            //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
            //The minimum is at (0,1,2,3...,n) for the unconstrained case.

            //using DotNumerics.Optimization;

            TruncatedNewton tNewton = new TruncatedNewton();
            int numVariables = 5;
            OptBoundVariable[] variables = new OptBoundVariable[numVariables];
            //Constrained Minimization on the interval (-10,10), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i] = new OptBoundVariable("x" + i.ToString(), -2, -10, 10);
            double[] minimum = tNewton.ComputeMin(ObjetiveFunction, Gradient, variables);

            ObjectDumper.Write("Truncated Newton Method. Constrained Minimization on the interval (-10,10)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //Constrained Minimization on the interval (-10,3), initial Guess=-2; 
            for (int i = 0; i < numVariables; i++) variables[i].UpperBound = 3;
            minimum = tNewton.ComputeMin(ObjetiveFunction, Gradient, variables);

            ObjectDumper.Write("Truncated Newton Method. Constrained Minimization on the interval (-10,3)");
            for (int i = 0; i < minimum.Length; i++) ObjectDumper.Write("x" + i.ToString() + " = " + minimum[i].ToString());

            //f(a,b) = 100*(b-a^2)^2 + (1-a)^2
            //private double BananaFunction(double[] x)
            //{
            //    double f = 0;
            //    f = 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + Math.Pow((1 - x[0]), 2);
            //    return f;
            //}

            //double[] bananaGrad = new double[2];
            //private double[] BananaGradient(double[] x)
            //{
            //    this.bananaGrad[0] = 200 * (x[1] - x[0] * x[0]) * (-2 * x[0]) - 2 * (1 - x[0]);
            //    this.bananaGrad[1] = 200 * (x[1] - x[0] * x[0]);
            //    return bananaGrad;
            //}
        }

        //f(x0,x2,...,xn)= (x0-0)^2+(x1-1)^2+...(xn-n)^2
        private double ObjetiveFunction(double[] x)
        {
            int numVariables = 5;
            double f = 0;
            for (int i = 0; i < numVariables; i++) f += Math.Pow(x[i] - i, 2);
            return f;
        }

        private double[] Gradient(double[] x)
        {
            int numVariables = 5;
            double[] grad = new double[x.Length];
            for (int i = 0; i < numVariables; i++) grad[i] = 2 * (x[i] - i);
            return grad;
        }


        #endregion



    }
}

