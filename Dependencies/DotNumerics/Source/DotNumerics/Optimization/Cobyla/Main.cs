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
    public sealed class MainProgram
    {


        #region Dependencies
        
        public COBYLATEST cobylatest; public CALCFC calcfc; public COBYLA cobyla; public COBYLB cobylb; public TRSTLP trstlp; 

        #endregion


        #region Declaracion de variables publicas
        
        CommonBlock Default = new CommonBlock(0, 1, 0, 0);

        #endregion


        public MainProgram()
        {

            #region Set Dependencies
            
            trstlp = new TRSTLP();
            calcfc = new CALCFC(Default);
            cobylb = new COBYLB(calcfc, trstlp);
            cobyla = new COBYLA(cobylb);
            cobylatest = new COBYLATEST(cobyla, Default);

            #endregion

        }
    }
}
