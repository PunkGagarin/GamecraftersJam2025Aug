using UnityEngine;

[System.Serializable]
public class CustomKeyValue<K, V> {

    [SerializeField]
    private K _key;

    [SerializeField]
    private V _value;

    public CustomKeyValue() {
    }

    public CustomKeyValue(K key) {
        _key = key;
    }

    public CustomKeyValue(V value) {
        _value = value;
    }

    public CustomKeyValue(K key, V value) {
        _key = key;
        _value = value;
    }

    public K Key {
        get => _key;
        set => _key = value;
    }

    public V Value {
        get => _value;
        set => _value = value;
    }

    public void Deconstruct(out K key, out V value)
    {
        key = _key;
        value = _value;
    }
}