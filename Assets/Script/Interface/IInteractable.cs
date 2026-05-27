using EANasir.Player;

namespace EANasir.Interface
{
	public interface IInteractable
	{
		/// <summary>
		/// Interact with object
		/// </summary>
		/// <returns>Copper to return after interaction</returns>
		public float Interact(PlayerController _player);
	}

}