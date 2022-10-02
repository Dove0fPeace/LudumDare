namespace Base_Components
{
    public interface IAbility
    {
        public void Use();
        
        public virtual Insects InsectType => Insects.Generic;
    }
}