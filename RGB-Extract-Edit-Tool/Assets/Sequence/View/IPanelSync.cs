using UnityEngine;

public interface IPanelSync
{
    /// <summary>
    /// 업데이트된 Data를 UI에 반영할 때 (1 : n)
    /// </summary>
    /// <param name="owner">Apply를 발생시킨 ui는 제외</param>
    void Sync(IPanelSync owner);

    void RegistSyncEvent();
    void UnregistSyncEvent();
}
