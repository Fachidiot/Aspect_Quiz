using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public InputMgr InputManager;
    public Button Q;
    public Button W;
    public Button E;
    public Button R;
    public Button T;
    public Button Y;
    public Button U;
    public Button I;
    public Button O;
    public Button P;
    public Button A;
    public Button S;
    public Button D;
    public Button F;
    public Button G;
    public Button H;
    public Button J;
    public Button K;
    public Button L;
    public Button Z;
    public Button X;
    public Button C;
    public Button V;
    public Button B;
    public Button N;
    public Button M;

    public void InputButton(string _input)
    {
        InputManager.KeyInput(_input);
        Debug.Log(_input);
    }
}
