using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IToneStuff
{
    //int toneNum { get; }
    void SetTone(int toneSetter);
    void Destroy();
}