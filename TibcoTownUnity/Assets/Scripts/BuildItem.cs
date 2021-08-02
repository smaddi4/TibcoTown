using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildItem : ObjectBehaviour {

    private const int SHOW_COINS_NUMBER = 3;
    private const float TIME_BETWEEN_COINS = .1f;

    [SerializeField]
    private SpriteRenderer buildImage;
	[SerializeField]
	private GameObject loadingObject;
	[SerializeField]
	private GameObject coinObjectPrefab;

    private int constructionPrice;
    private int constructionTime;
	private int profitValue;
    private int profitTime;

    private bool isNew;
    private bool isBuilded;
    private float currentCoinsTime;

	public void SetValues(Sprite sprite, int constructionPrice, int constructionTime, int profitValue, int profitTime)
	{
        this.buildImage.sprite = sprite;
        this.constructionPrice = constructionPrice;
		this.constructionTime = constructionTime;
		this.profitValue = profitValue;
		this.profitTime = profitTime;

		isNew = true;
		isBuilded = false;
		StartCoroutine(FixBuildTime());
		StartCoroutine(ContructionBuildTime());

	}

	/*public void Update()
	{
		if (isBuilded && !isPause)
		{
			currentCoinsTime += Time.deltaTime;
			if (currentCoinsTime > profitTime)
			{
				StartCoroutine(ShowCoins());
				currentCoinsTime = 0;
				GameManagerBehaviour.Instance.AddMoney(profitValue);
			}
		}
	}*/

    IEnumerator FixBuildTime ()
    {
        yield return new WaitForSeconds(.1f);
        isNew = false;
        //GameManagerBehaviour.Instance.RemoveMoney(constructionPrice);
    }

    IEnumerator ContructionBuildTime ()
    {
        yield return new WaitForSeconds(constructionTime);
		yield return new WaitUntil(() => !isPause);
        isBuilded = true;
        Destroy(loadingObject);

    }

    IEnumerator ShowCoins ()
    {
		for (int i = 0; i < SHOW_COINS_NUMBER; i++)
		{
            yield return new WaitUntil(() => !isPause);
            GameObject coin = Instantiate(coinObjectPrefab, transform.position, Quaternion.identity);
			coin.transform.SetParent(this.transform);
            yield return new WaitForSeconds(TIME_BETWEEN_COINS);
		}
    }

	public override void OnPause(bool pauseValue)
	{
		base.OnPause(pauseValue);
        if(!isBuilded){
            if(loadingObject != null){
                loadingObject.gameObject.SetActive(!pauseValue);  
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
	{
        if(isNew)
            Destroy(this.gameObject);	
    }

}
