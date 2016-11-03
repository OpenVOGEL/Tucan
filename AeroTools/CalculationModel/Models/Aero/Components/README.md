##The Aero.Components namespace
This namespace contains the definitions of the primitives used in the panel method. These primitives work as basic bricks, each having a spacific shape and behavior.
The current primitives supported by VOGEL are:
- Finite vortex segments
- Triangular panels with constant dinstribution of doublet and source/sink intensity.
- Quadrilateral panels with constant dinstribution of doublet and source/sink intensity.

For the last two cases, the unit doublet and source/sink potential functions and the unit source/sink velocity has only been written analytically for flat regions. The evaluation of such functions for curved regions requires the use of numerical methods, making them very impratical. This is why we prefere working with flat panels.
Triangular panels are always flat, so they don't require special attention when evaluating the source/sink potential functions. They actually implement the exact analytical expressions presented in [1] for quadrilaterals, but letting aside the fourth side. General quadrilaterals, on the other hand, need to be projected in their mean normal direction when the source/sink potential and velocity is to be evaluated, which will inevitably produce some level of leakage.

The evaluation of the potential functions has been programmed in apart subroutines located in the PotentialFunctions.vb file.

### Bibliography
- [1]: Katz and Plotkin
