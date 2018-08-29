using Controls;
using UnityEngine;

namespace Assets.Scripts
{
    public class PieCreator : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        private void Start()
        {
            for (var i = 1; i <= 20; i++)
            {
                var obj = Instantiate(prefab, new Vector3(0, i, 0), Quaternion.identity);
                obj.GetComponent<PieControl>().Weight = i;
            }
        }
    }
}
