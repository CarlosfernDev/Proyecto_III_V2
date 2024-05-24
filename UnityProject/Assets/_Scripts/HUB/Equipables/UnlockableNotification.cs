using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnlockableNotification : MonoBehaviour
{
    public List<SUnlockable> Unlockables;

    public Animator _NotificationAnimator;
    public TMP_Text _NameText;
    public Image _sprite;

    private Queue<SUnlockable> notificationQueue = new Queue<SUnlockable>();
    bool CheckingQueue = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            AddNotification(1);
    }

    public void AddNotification(int id)
    {
        Debug.Log("Esta la id del logro " + id);
        AddNotification(Unlockables[id]);
    }

    public void AddNotification(SUnlockable value)
    {
        if (value.IsUnlocked) return;
        value.IsUnlocked = true;
        SaveManager.SaveAllUnlockable();

        notificationQueue.Enqueue(value);
        if (!CheckingQueue)
        {
            StartCoroutine(DisplayNotificacions());
        }
    }

    IEnumerator DisplayNotificacions()
    {
        while(notificationQueue.Count > 0)
        {
            CheckingQueue = true;
            SUnlockable ActualUnlockable = notificationQueue.Dequeue();
            _NameText.text = ActualUnlockable.UnlockableName;
            _sprite.sprite = ActualUnlockable.UnlockableImageOn;
            _NotificationAnimator.SetTrigger("ActiveNotification");

            yield return new WaitForSeconds(0.1f);

            while (true)
            {
                if (_NotificationAnimator.GetCurrentAnimatorStateInfo(0).IsName("0"))
                    break;
                yield return null;
            }

            while (_NotificationAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
        }
        CheckingQueue = false;
        yield return null;
    }


}
