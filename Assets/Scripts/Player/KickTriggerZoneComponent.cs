using Controller.Components.VitalitySystem;

namespace Controller.Player
{
    public class KickTriggerZoneComponent: EnemyTriggerComponent
    {
        public void DoKick()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy?.GetComponent<HealthComponent>();
                if (health != null)
                {
                    var force = (enemy.gameObject.transform.position - this.gameObject.transform.position).normalized;
                    health.DoKick(force);
                }
            }
        }
    }
}