namespace CaveGame;

public static class SHutil
{
    public static int Random(int min, int max){
        return new Random().Next(min, max);
    }

    public static bool Equals(int[] a, int[] b){
        if(a.Length != b.Length){
            return false;
        }
        for (int i = 0; i < a.Length; i++) {
            if(a[i] != b[i]){
                return false;
            }
        }
        return true;
    }
}