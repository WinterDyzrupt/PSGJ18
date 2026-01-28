namespace Common.Utils.Events
{
    using UnityEngine;
    using UnityEngine.Events;

    public class GameEventListener : MonoBehaviour
    {
        /// <summary>
        /// The specific type of event to respond to.
        /// </summary>
        public GameEvent Event;

        /// <summary>
        /// The function call to respond with.
        /// </summary>
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDestroy()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }

    }
}