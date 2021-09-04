using UnityEngine;
using UnityEngine.UI;

public class CharacterModelHandler : MonoBehaviour
{
    [SerializeField]
    Image characterImage;

    CharacterModel attachedCharacterModel;
    
    [SerializeField]
    Expression[] expressions;

    private void OnValidate()
    {
        characterImage = characterImage ?? GetComponent<Image>();
    }

    public void InsertCharacterModel(CharacterModel characterModel)
    {
        attachedCharacterModel = characterModel;
        ImportExpressions();
    }

    public void LoadImage(Texture2D characterSprite)
    {
        characterImage.sprite = Sprite.Create(
            characterSprite, 
            new Rect(0, 0, 0.5f, 0.5f),
            new Vector2(0.5f, 0.5f), 100f);

    }

    void ImportExpressions()
    {
        int length = attachedCharacterModel.expressions.Length;
        expressions = new Expression[length];
        for (int i = 0; i < attachedCharacterModel.expressions.Length; i++)
        {
            expressions[i] = attachedCharacterModel.expressions[i];
        }
    }

    public void LoadExpression(string expressionName)
    {
        foreach(Expression expression in expressions)
        {
            //Make sure the name
            if (expression.expressionName == attachedCharacterModel.characterName + "")
            {

            }
        }
    }
}