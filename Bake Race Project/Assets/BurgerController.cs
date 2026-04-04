using UnityEngine;

public class BurgerController : MonoBehaviour
{
    private int step = 0;

    private addBuns addBottomBun;
    private addLettuce addLettuce;
    private addTomato addTomato;
    private addPatty addPatty;
    private addCheese addCheese;
    private addPickles addPickles;
    private addOnion addOnion;
    private addBuns addTopBun;

    void Start()
    {
        // Automatically grab all ingredient scripts attached to this GameObject
        addBottomBun = GetComponent<addBuns>();  // first bun
        addLettuce = GetComponent<addLettuce>();
        addTomato = GetComponent<addTomato>();
        addPatty = GetComponent<addPatty>();
        addCheese = GetComponent<addCheese>();
        addPickles = GetComponent<addPickles>();
        addOnion = GetComponent<addOnion>();
        addTopBun = GetComponent<addBuns>();    // last bun
    }

    void Update()
    {
        switch (step)
        {
            case 0:
                addBottomBun.TryAddIngredient();
                if (addBottomBun.IsAdded()) step++;
                break;
            case 1:
                addLettuce.TryAddIngredient();
                if (addLettuce.IsAdded()) step++;
                break;
            case 2:
                addTomato.TryAddIngredient();
                if (addTomato.IsAdded()) step++;
                break;
            case 3:
                addPatty.TryAddIngredient();
                if (addPatty.IsAdded()) step++;
                break;
            case 4:
                addCheese.TryAddIngredient();
                if (addCheese.IsAdded()) step++;
                break;
            case 5:
                addPickles.TryAddIngredient();
                if (addPickles.IsAdded()) step++;
                break;
            case 6:
                addOnion.TryAddIngredient();
                if (addOnion.IsAdded()) step++;
                break;
            case 7:
                addTopBun.TryAddIngredient();
                if (addTopBun.IsAdded()) step++;
                break;
        }
    }
}