using UnityEngine;

public class FPSController : MonoBehaviour
{
    void Awake()
    {
        // ----------------------------------------------------
        // Configuração para PC (Windows, Mac, Linux Standalone)
        // ----------------------------------------------------
        #if UNITY_STANDALONE
            // -1 = Ilimitado (Deixa rodar o máximo que a GPU aguentar)
            // 60 = Taxa fixa de quadros suave
            Application.targetFrameRate = 60; 
            QualitySettings.vSyncCount = 0; // Desativa V-Sync
        
        // ----------------------------------------------------
        // Configuração para Mobile (Android e iOS)
        // ----------------------------------------------------
        #elif UNITY_ANDROID || UNITY_IOS
            Application.targetFrameRate = 30; // Prioriza bateria e aquecimento
            QualitySettings.vSyncCount = 0; 
        
        // ----------------------------------------------------
        // Configuração Padrão (Fallback)
        // ----------------------------------------------------
        #else
            Application.targetFrameRate = 60; 
        #endif
    }
}