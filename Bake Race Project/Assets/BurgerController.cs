using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BurgerController : MonoBehaviour
{
    private int step = 0;

    private addBottomBun addBottomBun;
    private addLettuce addLettuce;
    private addTomato addTomato;
    private addPatty addPatty;
    private addCheese addCheese;
    private addPickles addPickles;
    private addOnion addOnion;
    private addTopBun addTopBun;

    public GameObject notebook;
    public Material[] stepMaterials;
    private Renderer cubeRenderer;

    private bool isEnding = false; // prevents multiple triggers

    void Start()
    {
        addBottomBun = GetComponent<addBottomBun>();
        addLettuce = GetComponent<addLettuce>();
        addTomato = GetComponent<addTomato>();
        addPatty = GetComponent<addPatty>();
        addCheese = GetComponent<addCheese>();
        addPickles = GetComponent<addPickles>();
        addOnion = GetComponent<addOnion>();
        addTopBun = GetComponent<addTopBun>();

        if (notebook != null)
        {
            Transform cube = notebook.transform.Find("Cube");
            if (cube != null)
                cubeRenderer = cube.GetComponent<Renderer>();
        }
    }

    void Update()
    {
        switch (step)
        {
            case 0:
                addBottomBun.TryAddIngredient();
                if (addBottomBun.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 1:
                addLettuce.TryAddIngredient();
                if (addLettuce.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 2:
                addTomato.TryAddIngredient();
                if (addTomato.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 3:
                addPatty.TryAddIngredient();
                if (addPatty.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 4:
                addCheese.TryAddIngredient();
                if (addCheese.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 5:
                addPickles.TryAddIngredient();
                if (addPickles.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 6:
                addOnion.TryAddIngredient();
                if (addOnion.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;

            case 7:
                addTopBun.TryAddIngredient();
                if (addTopBun.IsAdded() && !isEnding)
                {
                    isEnding = true;
                    ChangeNotebookMaterial(step);
                    step++;
                    toEndScene();
                }
                break;
        }
    }

    private void ChangeNotebookMaterial(int stepIndex)
    {
        if (cubeRenderer != null && stepMaterials != null && stepIndex < stepMaterials.Length)
        {
            cubeRenderer.material = stepMaterials[stepIndex];
        }
    }

    private void toEndScene()
    {
        StartCoroutine(EndSceneAfterDelay());
    }

    private IEnumerator EndSceneAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("EndScene");
    }
}