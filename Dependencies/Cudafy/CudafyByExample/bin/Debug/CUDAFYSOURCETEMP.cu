
// CudafyByExample.hist_gpu_shmem_atomics
extern "C" __global__  void histo_kernel( unsigned char* buffer, int bufferLen0, int size,  unsigned int* histo, int histoLen0);

// CudafyByExample.hist_gpu_shmem_atomics
extern "C" __global__  void histo_kernel( unsigned char* buffer, int bufferLen0, int size,  unsigned int* histo, int histoLen0)
{
	__shared__ unsigned int array[256];

	int arrayLen0 = 256;
	array[(threadIdx.x)] = 0u;
	__syncthreads();
	int i = threadIdx.x + blockIdx.x * blockDim.x;
	int num = blockDim.x * gridDim.x;
	while (i < size)
	{
		atomicAdd(&array[((int)buffer[(i)])], 1u);
		i += num;
	}
	__syncthreads();
	atomicAdd(&histo[(threadIdx.x)], array[(threadIdx.x)]);
}
