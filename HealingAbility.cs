interface IHealingAbility
{
    int Range { get; }

    float Effectiveness { get; }

    void Heal(Human target);
}
