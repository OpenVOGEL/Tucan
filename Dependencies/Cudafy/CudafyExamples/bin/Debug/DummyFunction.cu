extern "C" __global__ void  DummyFunction(int  *result)
{
    int  x = blockIdx.x;
    result[x] = ((result[x] * result[x]) * 1);
}

