using static SadRogue.Primitives.Color;
using static CaveGame.SHutil;

namespace CaveGame;

public static class EnemyManager{

    /*
    public static bool TileOccupied(Enemy[] enemyList, int[] position, int[] playerPosition, bool[][] blockingTile){
        foreach (Enemy enemy in enemyList) {
            if(SHutil.Equals(position, enemy.Position)){
                return true;
            }
        }

        if(SHutil.Equals(position, playerPosition)){
            return true;
        }
        if(blockingTile[position[0]][position[1]] == true){
            return true;
        }

        return false;
    }
    */

    public static void TakeDamage(Enemy[] enemyList, int[] position, int x){
        foreach (Enemy enemy in enemyList) {
            if(SHutil.Equals(enemy.Position, position)){
                enemy.TakeDamage(x);
                break;
            }
        }
    }

    /*
    protected static void RemoveEnemy(Enemy[] enemyList, Enemy enemy){
        int index = -1;
        for (int i = 0; i < enemyList.Length; i++) {
            if(enemyList[i] == enemy){
                index = i;
                break;
            }
        }

        Enemy[] new_list = new Enemy[enemyList.Length - 1];

        int indexSkipper = 0;
        for (int i = 0; i < enemyList.Length; i++) {
            if(i == index){
                indexSkipper += 1;
            }
            else{
                new_list[i - indexSkipper] = enemyList[i];
            }
        }

        enemyList = new_list;
    }
    */

    public abstract class Enemy {

        public string Id = "";
        public ColoredGlyph Glyph = new ColoredGlyph();
        public string Name = "";
        public string Description = "";
        protected int MaxHealth;
        public int Health;
        public bool Blocking;
        public string State = "";

        protected int Speed;
        protected int Damage;

        public int[] Position;

        public void Turn(Player player){

        }

        public void TakeDamage(int x){
            Health -= x;
            if(Health <= 0){
                die();
            }
        }

        protected void die(){
            // do something
        }
    }
}