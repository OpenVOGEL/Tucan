
// CudafyModuleViewer.CUDACheck
extern "C" __global__  void TestKernelFunction( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);

// CudafyModuleViewer.CUDACheck
extern "C" __global__  void TestKernelFunction( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	int num = threadIdx.x + blockIdx.x * blockDim.x;
	c[(num)] = a[(num)] + b[(num)];
}
