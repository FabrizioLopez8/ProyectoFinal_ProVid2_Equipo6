using System;

public enum TipoElemento { Fuego, Agua, Planta } // tipo de bolsimon

[Serializable]
public class Bolsimon
{
    public string nombre;
    public int hp;
    public int hpMax;
    public int ataque;
    public bool escudo;
    public TipoElemento tipo;
    public bool quemado;        // recibe daño al inicio de su turno (habilidad de Fuego)
    public bool danioReducido;  // su proximo ataque hace menos daño (habilidad de Agua)
    public bool saltaTurno;     // pierde su proximo turno (habilidad de Planta)
}