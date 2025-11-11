using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float parallaxMultiplier = 0.5f;
        public bool infiniteHorizontal = true;
        public bool infiniteVertical = false;
    }

    [Tooltip("Put the farthest on the lowest Enum")]
    [Header("Parallax Layers")]
    [SerializeField] private ParallaxLayer[] layers;

    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform;

    private Vector3 lastCameraPosition;
    private Vector2[] startPositions;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        startPositions = new Vector2[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].layerTransform != null)
                startPositions[i] = layers[i].layerTransform.position;
        }
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].layerTransform == null) continue;

            // Apply parallax movement
            Vector3 newPosition = layers[i].layerTransform.position;
            newPosition.x += deltaMovement.x * layers[i].parallaxMultiplier;
            newPosition.y += deltaMovement.y * layers[i].parallaxMultiplier;

            layers[i].layerTransform.position = newPosition;

            // Infinite scrolling
            HandleInfiniteScrolling(i, deltaMovement);
        }

        lastCameraPosition = cameraTransform.position;
    }

    private void HandleInfiniteScrolling(int layerIndex, Vector3 deltaMovement)
    {
        ParallaxLayer layer = layers[layerIndex];
        SpriteRenderer spriteRenderer = layer.layerTransform.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        Sprite sprite = spriteRenderer.sprite;
        float textureUnitSizeX = sprite.texture.width / sprite.pixelsPerUnit;
        float textureUnitSizeY = sprite.texture.height / sprite.pixelsPerUnit;

        // Horizontal infinite scrolling
        if (layer.infiniteHorizontal && Mathf.Abs(deltaMovement.x) > 0)
        {
            if (Mathf.Abs(cameraTransform.position.x - layer.layerTransform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - layer.layerTransform.position.x) % textureUnitSizeX;
                Vector3 newPos = layer.layerTransform.position;
                newPos.x = cameraTransform.position.x + offsetPositionX;
                layer.layerTransform.position = newPos;
            }
        }

        // Vertical infinite scrolling
        if (layer.infiniteVertical && Mathf.Abs(deltaMovement.y) > 0)
        {
            if (Mathf.Abs(cameraTransform.position.y - layer.layerTransform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - layer.layerTransform.position.y) % textureUnitSizeY;
                Vector3 newPos = layer.layerTransform.position;
                newPos.y = cameraTransform.position.y + offsetPositionY;
                layer.layerTransform.position = newPos;
            }
        }
    }
}
