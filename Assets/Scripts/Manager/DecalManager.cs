using System;
using UnityEngine;

namespace Manager
{
    public class DecalManager : MonoBehaviour
    {
        private  GameObject[] _decals = new GameObject[30];
        
        [SerializeField]
        public GameObject Decal;

        private  int pointer = 0;

        private static DecalManager _instance;

        public static DecalManager Instance => _instance;

        private void Start()
        {
            if(_instance != null) Destroy(this);
            
            _instance = this;

            for (var i = 0; i < _decals.Length; i++)
            {
                _decals[i] = Instantiate(Decal, new Vector3(0, -100, 0), Quaternion.identity);
            }
        }

        public void PlaceDefaultDecal(Vector3 position, Quaternion rotation)
        {
            if (pointer >= _decals.Length)
            {
                pointer = 0;
            }
            
            _decals[pointer].transform.SetPositionAndRotation(position, rotation);
                
            pointer++;
        }
        
        
    }
}