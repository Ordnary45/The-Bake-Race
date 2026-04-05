using UnityEngine;

public class BurgerController : MonoBehaviour
{
    private int step = 0;                   // tracking which recipe step we are on. corresponds to switch statement

    private addBottomBun addBottomBun;      // ingredient declarations
    private addLettuce addLettuce;
    private addTomato addTomato;
    private addPatty addPatty;
    private addCheese addCheese;
    private addPickles addPickles;
    private addOnion addOnion;
    private addTopBun addTopBun;

    public GameObject notebook;
    public Material[] stepMaterials;        // used for holding each new notebook material
    private Renderer cubeRenderer;

    void Start()
    {
        // obtain each ingredient's corresponding script
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

    // switch statement for each step of the recipe
    void Update()
    {
        switch (step)
        {
            case 0:
                addBottomBun.TryAddIngredient();        // call upon ingredient's 'add' function in their corresponding script
                if (addBottomBun.IsAdded())
                {
                    ChangeNotebookMaterial(step);       // update notebook material (aka crossing out completed step)
                    step++;                             // incrementing step
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
                if (addTopBun.IsAdded())
                {
                    ChangeNotebookMaterial(step);
                    step++;
                }
                break;
        }
    }

    // updating the material of the notebook
    private void ChangeNotebookMaterial(int stepIndex)
    {
        if (cubeRenderer != null && stepMaterials != null && stepIndex < stepMaterials.Length)
        {
            cubeRenderer.material = stepMaterials[stepIndex];
        }
    }
}