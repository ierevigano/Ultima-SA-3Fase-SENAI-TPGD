using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Chaves de PlayerPrefs (Para salvar/carregar os dados)
    private const string QUALITY_KEY = "QualityLevel";
    private const string VOLUME_KEY = "MasterVolume";
    private const string RESOLUTION_INDEX_KEY = "ResolutionIndex";
    private const string FULLSCREEN_KEY = "Fullscreen";

    public AudioMixer mainAudioMixer;
    private const string MASTER_VOLUME_PARAM = "MasterVolume"; 

    private Resolution[] availableResolutions;
    public TMP_Dropdown resolutionDropdown; // Arraste seu Dropdown de Resolução aqui

    void Start()
    {
        // 1. Configura a lista de resoluções disponíveis
        availableResolutions = Screen.resolutions;
        
        // 2. Chama a função para carregar todas as configurações
        LoadSettings();
    }
    
    // =======================================================
    // # Carregar e Salvar
    // =======================================================

    private void LoadSettings()
    {
        // Carrega Qualidade
        int savedQuality = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());
        SetQuality(savedQuality);
        
        // Carrega Volume
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f); // Default 1.0 (100%)
        SetVolume(savedVolume);

        // Carrega Tela Cheia
        bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1; // Default 1 (True)
        Screen.fullScreen = isFullscreen;
        
        // Carrega Resolução (Assumindo que você configurou o Dropdown na Unity)
        int savedResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_INDEX_KEY, 0); 
        SetResolution(savedResolutionIndex); // Aplica a resolução salva
        
        // Atualiza a UI para refletir os valores carregados
        // Nota: Você precisará de referências aos seus sliders/dropdowns para isso.
        // Exemplo: if (resolutionDropdown != null) resolutionDropdown.value = savedResolutionIndex;
    }

    private void SaveSettings()
    {
        // Salva os dados no disco
        PlayerPrefs.Save();
        Debug.Log("Configurações salvas!");
    }

    // =======================================================
    // # Métodos de Ajuste (com Salvamento)
    // =======================================================

    // Chamado pelo Dropdown
    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < availableResolutions.Length)
        {
            Resolution resolution = availableResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            
            PlayerPrefs.SetInt(RESOLUTION_INDEX_KEY, resolutionIndex);
            SaveSettings();
        }
    }
    
    // Chamado pelo Toggle/Checkbox
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // PlayerPrefs só salva int/float/string. 1 para True, 0 para False.
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        SaveSettings();
    }
    
    // Chamado pelo Slider
    public void SetVolume(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        mainAudioMixer.SetFloat(MASTER_VOLUME_PARAM, db);
        
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        SaveSettings();
    }
    
    // Chamado pelo Dropdown
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        
        PlayerPrefs.SetInt(QUALITY_KEY, qualityIndex);
        SaveSettings();
    }
}