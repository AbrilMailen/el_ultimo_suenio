// Enemigo cuerpo a cuerpo: persigue y muerde.
// También sirve para el Bicharraco de pesadilla: solo cambia el EnemyData.
public class Bicharraco : Enemy
{
    protected override void PerformAttack()
    {
        // La mordida resta segundos al DreamTimer de la niña
        TargetDamageable?.TakeDamage(Data.Damage);
    }
}
