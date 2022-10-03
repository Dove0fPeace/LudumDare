namespace Base_Components
{
    public interface IAbility
    {
        public void Use();
        public bool CanUse();

        public void InitiateAbility();
        
        public virtual Insects Insect => Insects.Generic;
    }
}