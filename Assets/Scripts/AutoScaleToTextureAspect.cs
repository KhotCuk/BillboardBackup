using UnityEngine;

[ExecuteInEditMode] // biar bisa lihat perubahan langsung di Editor
public class AutoScaleToTextureAspect : MonoBehaviour
{
    [Header("Texture Referensi")]
    [Tooltip("Drag texture PNG/JPG ke sini")]
    public Texture2D texture;

    [Header("Pengaturan Scale")]
    [Tooltip("Berapa pixel per 1 unit scale (semakin besar semakin kecil objek)")]
    public float pixelsPerUnit = 100f;

    [Tooltip("Gunakan sumbu Z untuk tinggi (3D) atau Y (2D/Sprite)")]
    public bool useZForHeight = true;

    [Tooltip("Jika centang, hanya update saat texture berubah (hemat performa)")]
    public bool updateOnlyOnChange = true;

    private Texture2D lastTexture;

    private void Update()
    {
        if (texture == null) return;

        // Hanya update kalau texture baru atau diinginkan
        if (updateOnlyOnChange && texture == lastTexture) return;
        lastTexture = texture;

        if (texture.width <= 0 || texture.height <= 0) return;

        // Hitung rasio aspect (lebar / tinggi)
        float aspect = (float)texture.width / texture.height;

        // Hitung ukuran base (1 unit = pixelsPerUnit pixel)
        float baseHeight = 1f; // tinggi default 1 unit
        float baseWidth = baseHeight * aspect;

        // Scale akhir
        Vector3 newScale = new Vector3(baseWidth, baseHeight, 1f);

        if (useZForHeight)
        {
            // 3D: X = lebar, Z = tinggi, Y = 1 (ketebalan)
            newScale = new Vector3(baseWidth, 1f, baseHeight);
        }
        else
        {
            // 2D/Sprite: X = lebar, Y = tinggi, Z = 1
            newScale = new Vector3(baseWidth, baseHeight, 1f);
        }

        // Terapkan ke transform
        transform.localScale = newScale;

        Debug.Log($"Objek di-scale sesuai texture: {texture.width}x{texture.height} → aspect {aspect:F3} → scale {newScale}");
    }

    // Opsional: panggil manual kalau texture diganti via script
    public void RefreshScale()
    {
        lastTexture = null; // paksa update di frame berikutnya
    }
}