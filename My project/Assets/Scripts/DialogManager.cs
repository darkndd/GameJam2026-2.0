using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DialogData
{
    public List<string> lines;
    public List<string> opciones;
}

[System.Serializable]
public class DialogContainer
{
    public DialogData dialog1;
    public DialogData dialog2;
    // si quieres más diálogos fijos, los añades aquí
}

[System.Serializable]
public class DialogEntry
{
    public string key;
    public DialogData value;
}

public class Opcion 
{
    public string text;
    public string nextDialog;
}

public class DialogManager : MonoBehaviour
{
    [SerializeField] public TextAsset dialogJsonFile;
    [SerializeField] public TextMeshProUGUI dialogText;
    [SerializeField] public Transform opcionesContainer;
    [SerializeField] public Button opcionButtonPrefab;
    [SerializeField] public int indexLineaActual = 0;
    [SerializeField] public Button continuarButton;

    private Dictionary<string, DialogData> dialogs = new Dictionary<string, DialogData>();
    private string currentDialogKey = "";

    private void Start()
    {
        LoadDialogs();

        // PLACEHOLDER: Mostrar el primer diálogo disponible al iniciar
        DisplayDialog("dialog1");
    }

    private void LoadDialogs()
    {
        if (dialogJsonFile == null)
        {
            Debug.LogError("dialogJsonFile es NULL en el inspector");
            return;
        }

        Debug.Log("Texto del JSON bruto:\n" + dialogJsonFile.text);

        try
        {
            // Aquí NO envolvemos en {"dialogs": ...}
            DialogContainer container = JsonUtility.FromJson<DialogContainer>(dialogJsonFile.text);

            if (container == null)
            {
                Debug.LogError("container es NULL (FromJson falló)");
                return;
            }

            dialogs = new Dictionary<string, DialogData>();

            if (container.dialog1 != null)
                dialogs["dialog1"] = container.dialog1;

            if (container.dialog2 != null)
                dialogs["dialog2"] = container.dialog2;

            Debug.Log($"Se cargaron {dialogs.Count} diálogos");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al parsear JSON: {e.Message}\n{e.StackTrace}");
        }
    }

    public void DisplayDialog(string dialogKey)
    {
        if (!dialogs.ContainsKey(dialogKey))
        {
            Debug.LogWarning($"Diálogo '{dialogKey}' no encontrado");
            return;
        }

        if (indexLineaActual == 0){
            continuarButton.gameObject.SetActive(true);
            foreach (Transform child in opcionesContainer)
            Destroy(child.gameObject);
        }

        currentDialogKey = dialogKey;
        DialogData dialog = dialogs[dialogKey];

        // Mostrar líneas de diálogo
        dialogText.text = dialog.lines[indexLineaActual];
        ++indexLineaActual;

        // Si se acabaron las líneas, mostrar opciones de dialogo
        if (indexLineaActual >= dialog.lines.Count)
        {
            List<Opcion> opcionesList = new List<Opcion>();

            for(int i = 0; i < dialog.opciones.Count; i++)
            {
                Opcion opcion = new Opcion();
                opcion.text = dialog.opciones[i].text;
                opcion.nextDialog = dialog.opciones[i].nextDialog;
                opcionesList.Add(opcion);
            }

            DisplayOpciones(opcionesList);
            indexLineaActual = 0;
            continuarButton.gameObject.SetActive(false);
        }
    }

    private void DisplayOpciones(List<Opcion> opciones)
    {
        foreach (Transform child in opcionesContainer)
            Destroy(child.gameObject);

        Debug.Log($"Mostrando {opciones.Count} opciones para el diálogo '{currentDialogKey}'");

        for (int i = 0; i < opciones.Count; i++)
        {
            Button newButton = Instantiate(opcionButtonPrefab, opcionesContainer);
            newButton.gameObject.SetActive(true);
            newButton.image.enabled = true;
            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.enabled = true;
            buttonComponent.interactable = true;

            // Forzar parámetros de UI visibles
            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero;
            rt.anchoredPosition = new Vector2(100, -50 * i); // separación vertical
            rt.sizeDelta = new Vector2(200, 8);

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.enabled = true;
            if (buttonText != null)
            {
                buttonText.text = opciones[i].text;
                buttonText.color = Color.white;

                newButton.onClick.RemoveAllListeners();
                newButton.onClick.AddListener(() => DisplayDialog(opciones[i].nextDialog));
            }
        }
    }

    private void OnOpcionSelected(int index, string opcion)
    {
        Debug.Log($"Opción seleccionada: [{index}] {opcion} en '{currentDialogKey}'");
    }

    public List<string> GetAvailableDialogs()
    {
        return new List<string>(dialogs.Keys);
    }

    public DialogData GetDialog(string dialogKey)
    {
        return dialogs.ContainsKey(dialogKey) ? dialogs[dialogKey] : null;
    }

    // Clase temporal para parsear el JSON correctamente
    [System.Serializable]
    private class TempDialogData
    {
        public DialogEntry[] dialogs;
    }

    public void nextLine()
    {
        DisplayDialog(currentDialogKey);
    }
}
