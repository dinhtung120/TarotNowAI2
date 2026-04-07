export async function compressAvatarImage(file: File): Promise<File> {
  const objectUrl = URL.createObjectURL(file);
  const image = new window.Image();

  try {
    await new Promise<void>((resolve, reject) => {
      image.onload = () => resolve();
      image.onerror = () => reject(new Error('Không thể đọc file ảnh.'));
      image.src = objectUrl;
    });

    const maxDimension = 1024;
    const ratio = Math.min(1, maxDimension / Math.max(image.width, image.height));
    const targetWidth = Math.max(1, Math.round(image.width * ratio));
    const targetHeight = Math.max(1, Math.round(image.height * ratio));

    const canvas = document.createElement('canvas');
    canvas.width = targetWidth;
    canvas.height = targetHeight;
    const context = canvas.getContext('2d');
    if (!context) {
      throw new Error('Không khởi tạo được bộ nén ảnh.');
    }

    context.drawImage(image, 0, 0, targetWidth, targetHeight);

    let blob = await new Promise<Blob | null>((resolve) => canvas.toBlob(resolve, 'image/avif', 0.8));
    let mimeType = 'image/avif';
    let fileExtension = 'avif';

    if (!blob) {
      blob = await new Promise<Blob | null>((resolve) => canvas.toBlob(resolve, 'image/webp', 0.85));
      mimeType = 'image/webp';
      fileExtension = 'webp';
    }

    if (!blob) {
      blob = await new Promise<Blob | null>((resolve) => canvas.toBlob(resolve, 'image/jpeg', 0.85));
      mimeType = 'image/jpeg';
      fileExtension = 'jpg';
    }

    if (!blob) {
      throw new Error('Không thể nén ảnh.');
    } 

    const newFileName = `avatar_${Date.now()}.${fileExtension}`;
    return new File([blob], newFileName, { type: mimeType });
  } finally {
    URL.revokeObjectURL(objectUrl);
  }
}
