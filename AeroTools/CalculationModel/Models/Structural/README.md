Note: this module is now mainly being developed at the UNC, and therefore some part of the documentation comes in Spanish. We will strive to translate these documents into English as soon as we can.

##Sobre la conexión aeroelástica de Open VOGEL (ES)

El modelo estructural está basado en el metodo de elementos finitos, implementado a travez de "nodos" y "elementos".
Estas dos entidades básicas están definidas en la biblioteca _OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library_.

###Nodos
_OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes.StructuralNode_

Los nodos representan puntos de conexión donde uno o más elementos confluyen. Cada nodo cuenta con una serie de propiedades que definen el estado local:
- _Position_: posición en coordenadas ortogonales globales (x, y, z).
- _Constrains_: condiciones de borde para cada grado de libertad, puede ser una restricción total o una toma de rigidez.
- _Load_: la carga nodal en coordenadas globales para cada grado de libertad (Fx, Fy, Fz, Mx, My, Mz).
- _Displacement_: el desplazamiento nodal en coordenadas ortoglobales (ux, uy, uz, rx, ry, rz).
- _Index_: sirve para localizar el nodo en el stack de nodos.

Los nodos tienen 6 grados de libertad y reciben la carga aplicada sobre la estructura.

###Elementos
_OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements_

Una estructura está formada por una nube de nodos interconectados por elementos, que en principio pueden ser de cualquier tipo (barra, placa, etc), mientras tengan una matriz de masa y una de rigidez que se puedan ensamblar en el sistema global. Estas propiedades y métodos están garantizados al implementar la interface `IFiniteElement`.
Actualmente tenemos una sola definición de elemento, el `BeamElement`, que es una barra de sección constante y solo dos nodos en los extremos.
Las propiedades más importantes del `BeamElement` son:

- _NodoA_ (implementa el nodo 0 de `IFiniteElement`)
- _NodoB_ (implementa el nodo 1 de `IFiniteElement`)
- _Section_: contiene las propiedades de la sección, tanto de rigidez como inercia.
- _Basis_: las direcciones locales en vectores ortonormales (u, v, w).
- _M_: Matriz de masa (12x12) (implementa M de IFiniteElement)
- _K_: Matriz de rigidez (12x12) (implementa K de IFiniteElement)

###Estructura
_OpenVOGEL.AeroTools.CalculationModel.Models.Structural.StructuralCore_

La clase `StructuralCore` es una definición que contiene todos los nodos y elementos de la estructura.
Este objeto se encarga de solicitar a los elementos de la estructura el ensamblaje de sus matrices locales, y con ellas de ensamblar el sistema global. También se encarga de generar la descomposición modal: ejecuta la descomposición (haciendo uso de la biblioteca de álgebra localizada en _MathTools_) y almacena los modos dinámicos con sus propiedades modales.
Actualmente, en esta clase hay que implementar la interface `IFiniteElement` para evitar el elemento BeamElement y asi genrar una definicion general.

###Linker
_OpenVOGEL.AeroTools.CalculationModel.Models.Structural_

En el archivo _Linker.vb_ se encuentran tres clases que se encargan de administrar la conexión entre la estructura y el modelo aerodinámico.
De añadir otro método de conexión aero-estructural, deberíamos crear una biblioteca similar.
La clase que administra toda la parte dinámica se conoce como `StructuralLink`. Su tarea es traspasar las cargas del modelo aerodinámico al estructural,
integrar las ecuaciones de movimiento, y traspasar el movimiento resultante de la estructura nuevamente al modelo aerodinámico.
En el desarrollo actual, la conexión tiene dos componentes básicos:

- `KinematicLink`: contiene un nodo estructural y una serie de nodos aerodinamicos.
- `MechanicLink`: contiene un elemento estructural y una serie de paneles delgados del modelo aerodinami-co.

La clase principal `StructuralLink` contiene dos colecciones, una de `KinematicLinks` y otra de `MechanicLinks`. 
Cuando se ejecuta el cálculo, se evalúa en cada paso las cargas en los paneles aerodinámicos. 
Luego se llama al método de cada `MechanicLink` para que reúna esas cargas y las concentre en los nodos estructurales del elemento estructural asociado.

Con las cargas en la estructura, el `StructuralLink` está en condiciones de integrar las ecuaciones de movimiento. En el desarrollo actual se integran las ecuaciones modales desacopladas para evitar tener que resolver un sistema de ODEs acopladas (algo que requiere un gran esfuerzo computacional).
Usar el método de descomposición modal tiene obviamente todas las limitaciones de un análisis lineal.

Lo que se hace entonces es calcular la carga modal (el trabajo virtual del estado de cargas sobre los desplazamientos virtuales representados por cada forma modal), y con eso los desplazamientos generalizados de cada modo.
Para integrar las ecuaciones se puede usar cualquier método. Por el momento solo se ha implementado Newmark porque es el más estabilidad numérica ha demostrado (el método de la diferencia central es, de suguro, mas que deficiente en ese sentido, por lo que ha sido eliminado).

Los desplazamientos generalizados se emplean mas tarde para recomponer el movimiento mediante una combinación lineal de las formas modales.
Una vez que se conoce el movimiento de cada nodo estructural, se llama a al método de cada `KinematicLink` para que transfiera ese movimiento a los nodos asociados del modelo estructural.

>####Nota 
>Como se puede ver, la manera en que está estructurada la conexión aeroelástica es bastante general, y es muy flexible en cuanto a:
>
>- La forma de transferir la carga a la estructura
>- La forma de integrar las ecuaciones diferenciales de movimiento
>- La forma de retomar el movimiento de la estructura a al modelo aerodinámico.
>
>Sería recomendable usar la estructura actual como una plantilla para las próximas implementaciones.

