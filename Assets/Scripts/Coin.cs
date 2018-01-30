public class Coin : Attachment
{

	public override void OnTileReached()
	{
		++ScoreManager.Instance.coins;
		tile.attachments.Remove(this);
		Destroy(gameObject);
	}
}
