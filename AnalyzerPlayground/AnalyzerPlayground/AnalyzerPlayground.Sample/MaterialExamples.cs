#nullable disable
using UnityEngine;

namespace AnalyzerPlayground.Sample
{
    public class MaterialExamples : MonoBehaviour
    {
        private Material matOK;
        private Material matNG;

        private void Start()
        {
            // ok
            matOK = new Material();
            // Destroy していないため、RA0001 の警告が出ている
            matNG = new Material();
        }

        private void OnDisable()
        {
            Destroy(matOK);
        }
    }
}
