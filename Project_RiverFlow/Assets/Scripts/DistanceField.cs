public class DistanceField
{
    private int[,] array;

    public DistanceField(int sizeX, int sizeY, int defaultValue) { 
        this.array = new int[sizeX,sizeY];
        for (int i = 0; i < sizeX; i++) {
            for (int j = 0; j < sizeY; j++) {
                this.array[i, j] = defaultValue;
            }
        }
    }
}
