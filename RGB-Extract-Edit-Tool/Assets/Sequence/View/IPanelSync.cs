using UnityEngine;

public interface IPanelSync
{
    /// <summary>
    /// ������Ʈ�� Data�� UI�� �ݿ��� �� (1 : n)
    /// </summary>
    /// <param name="owner">Apply�� �߻���Ų ui�� ����</param>
    void Sync(IPanelSync owner);

    void RegistSyncEvent();
    void UnregistSyncEvent();
}
