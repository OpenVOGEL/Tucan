
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_0( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_1( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_2( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_3( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_4( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0);

// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_0( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	for (int i = blockIdx.x; i < 1024; i += gridDim.x)
	{
		c[(i)] = a[(i)] + b[(i)];
	}
}
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_1( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	for (int i = blockIdx.x; i < aLen0; i += gridDim.x)
	{
		c[(i)] = a[(i)] + b[(i)];
	}
}
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_2( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	for (int i = blockIdx.x; i < bLen0; i += gridDim.x)
	{
		c[(i)] = a[(i)] + b[(i)];
	}
}
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_3( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	for (int i = blockIdx.x; i < cLen0; i += gridDim.x)
	{
		c[(i)] = a[(i)] + b[(i)];
	}
}
// CudafyExamples.Arrays.ArrayBasicIndexing
extern "C" __global__  void add_4( int* a, int aLen0,  int* b, int bLen0,  int* c, int cLen0)
{
	int i = blockIdx.x;
	int rank = 1;
	while (i < cLen0)
	{
		c[(i)] = a[(i)] + b[(i)];
		i += gridDim.x;
	}
}
