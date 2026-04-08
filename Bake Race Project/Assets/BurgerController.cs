using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BurgerController : MonoBehaviour
{
    private int step = 0;               // used to track which step of the recipe we are on

    private addBottomBun addBottomBun;  // bottom bun script declaration
    private addLettuce addLettuce;      // lettuce script declaration
    private addTomato addTomato;        // tomato script declaration
    private addPatty addPatty;          // patty script declaration
    private addCheese addCheese;        // cheese script declaration
    private addPickles addPickles;      // pickles script declaration
    private addOnion addOnion;          // onion script declaration
    private addTopBun addTopBun;        // top bun script declaration

    public GameObject notebook;         // the cookbook
    public Material[] stepMaterials;    // array for holding each material -- each material is a step in the recipe
    private Renderer cubeRenderer;      // renderer for cookbook

    private bool isEnding = false; // prevents multiple triggers

    void Start()
    {
        // obtaining the corresponding script to each ingredient object
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
                cubeRenderer = cube.GetComponent<Renderer>();       // obtaining the cookbook renderer
        }
    }

    void Update()
    {
        switch (step)                               // switch statement for controlling each step of the recipe process
        {
            case 0:                                 // bottom bun
                addBottomBun.TryAddIngredient();    // add bottom bun to burger + remove bottom bun from users hand
                if (addBottomBun.IsAdded())         // if bottom bun added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 1:                                 // lettuce
                addLettuce.TryAddIngredient();      // add lettuce to burger + remove lettuce from users hand
                if (addLettuce.IsAdded())           // if lettuce added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 2:                                 // tomato
                addTomato.TryAddIngredient();       // add tomato to burger + remove tomato from users hand
                if (addTomato.IsAdded())            // if tomato added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 3:                                 // patty
                addPatty.TryAddIngredient();        // add patty to burger + remove patty from users hand
                if (addPatty.IsAdded())             // if patty added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 4:                                 // cheese
                addCheese.TryAddIngredient();       // add cheese to burger + remove cheese from users hand
                if (addCheese.IsAdded())            // if cheese added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 5:                                 // pickles
                addPickles.TryAddIngredient();      // add pickles to burger + remove cheese from users hand
                if (addPickles.IsAdded())           // if pickles added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 6:                                 // onion
                addOnion.TryAddIngredient();        // add onion to burger + remove onion from users hand
                if (addOnion.IsAdded())             // if onion added successfully
                {
                    ChangeNotebookMaterial(step);   // update UI on cookbook
                    step++;                         // increment step
                }
                break;

            case 7:                                 // top bun
                addTopBun.TryAddIngredient();       // add top bun to burger + remove top bun from users hand
                if (addTopBun.IsAdded() && !isEnding) // if top bun added successfully & isEnding is false (aka ensuring this is the first & only time we will be here)
                {
                    isEnding = true;                // flag isEnding as true so this block isnt reached again
                    ChangeNotebookMaterial(step);   // cross off final step in cookbook
                    step++;                         // increment step for fun
                    toEndScene();                   // teleport user to end scene
                }
                break;
        }
    }

    // used for updating the material of the cookbook (crossing out completed steps)
    private void ChangeNotebookMaterial(int stepIndex)
    {
        if (cubeRenderer != null && stepMaterials != null && stepIndex < stepMaterials.Length)
        {
            cubeRenderer.material = stepMaterials[stepIndex];
        }
    }

    // function for teleporting user to end scene
    private void toEndScene()
    {
        StartCoroutine(EndSceneAfterDelay());
    }

    private IEnumerator EndSceneAfterDelay()    // called once user finishes assembling the burger
    {
        yield return new WaitForSeconds(5f);    // five second delay
        SceneManager.LoadScene("EndScene");     // loads end scene
    }
}