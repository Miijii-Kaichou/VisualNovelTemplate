using UnityEngine;
using UnityEngine.UI;

public class CharacterModelHandler : MonoBehaviour
{
    private static CharacterModelHandler Instance;

    [SerializeField]
    Image characterImage;

    CharacterModel attachedCharacterModel;

    readonly Vector2 CENTER = new Vector2(0.5f, 0.5f);

    public CharacterModel AttachedCharacterModel => Instance.attachedCharacterModel;

    public bool HasCharacterModel
    {
        get
        {
            return attachedCharacterModel != null;
        }
    }

    [SerializeField]
    Expression[] currentExpressions;


    private void OnValidate()
    {
        characterImage = characterImage ?? GetComponent<Image>();
    }


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Give a character model to this handler
    /// </summary>
    /// <param name="characterModel"></param>
    public void InsertCharacterModel(CharacterModel characterModel)
    {
        attachedCharacterModel = characterModel;
        ImportExpressions();
    }

    /// <summary>
    /// Will actually create a sprite from the expressions Texture2D
    /// to then assign to the Image Component's sprite property field,
    /// thus displaying the image.
    /// </summary>
    /// <param name="characterSprite"></param>
    public void LoadImage(Texture2D characterSprite)
    {
        if (characterSprite == null) Resources.Load<Texture2D>("NullImage");

        Debug.Log("Loading Image");

        Sprite newSprite = Sprite.Create(
            characterSprite,
            new Rect(0f, 0f, characterSprite.width, characterSprite.height),
            CENTER,
            attachedCharacterModel.expressionPPU);

        characterImage.sprite = newSprite;
    }

    /// <summary>
    /// Import all avaliable expressions to the CharacterModelHandler
    /// </summary>
    void ImportExpressions()
    {
        int length = attachedCharacterModel.expressions.Length;
        currentExpressions = new Expression[length];
        for (int i = 0; i < attachedCharacterModel.expressions.Length; i++)
        {
            currentExpressions[i] = attachedCharacterModel.expressions[i];
        }
    }

    /// <summary>
    /// Loads an expression to the handler based on a name
    /// </summary>
    /// <param name="expressionName"></param>
    public void LoadExpression(string expressionName)
    {
        foreach(Expression expression in currentExpressions)
        {
            //At this point, naming conventions was already verfied from the ActiveCharacterSelector
            if (expression.expressionName.Contains(expressionName))
            {
                LoadImage(expression.texture);
            }
        }
    }
}