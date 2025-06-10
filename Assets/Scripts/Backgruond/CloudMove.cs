using System;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private float _speed;
    private float _despawnX; // Кэшируем значение, чтобы не запрашивать каждый кадр
    private ICloudPool _pool;

    // Инициализация (вызывается при создании или взятии из пула)
    public void Initialize(ICloudPool pool, float speed, float despawnX) 
    {
        if (pool == null) 
            throw new ArgumentNullException(nameof(pool), "Pool cannot be null!");

        this._pool = pool;
        this._speed = speed;
        this._despawnX = despawnX;
    }

    void Update() 
    {
        // Движение влево
        transform.Translate(Vector3.left * (_speed * Time.deltaTime));
        
        // Проверка на выход за экран
        if (transform.position.x < _despawnX) 
        {
            _pool.ReturnToPool(this);
        }
    }

    // Сброс позиции при повторном использовании
    public void ResetCloud(Vector3 position) 
    {
        transform.position = position;
    }
}

