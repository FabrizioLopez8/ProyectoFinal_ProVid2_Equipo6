using System;

public enum TipoElemento { Fuego, Agua, Planta} //tipo de bolsimon
[Serializable]
public class Bolsimon
{
    public string nombre;
    public int hp;
    public int hpMax;
    public int ataque;
    public bool escudo;
    public TipoElemento tipo;
}