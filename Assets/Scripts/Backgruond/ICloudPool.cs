public interface ICloudPool
{
        // Вернуть облако в пул
        void ReturnToPool(CloudMove cloud);
    
        // Получить X-координату, при которой облако исчезает
        float GetDespawnX();
}